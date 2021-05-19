using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErucaCRM.Repository;
using ErucaCRM.Business.Interfaces;
using ErucaCRM.Repository.Infrastructure.Contract;
using ErucaCRM.Domain;
using ErucaCRM.Utility;
using System.Data;
namespace ErucaCRM.Business
{
    public class ContactBusiness : IContactBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ContactRepository contactRepository;
        private readonly AddressRepository addressRepository;
        private readonly UserRepository userRepository;
        private readonly TagRepository tagRepository;
        private readonly AccountContactRepository accountcontactRepository;
        private readonly LeadContactRepository leadContactRepository;
        private readonly ContactTagRepository contactTagRepository;

        public ContactBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            contactRepository = new ContactRepository(unitOfWork);
            addressRepository = new AddressRepository(unitOfWork);
            tagRepository = new TagRepository(unitOfWork);
            contactTagRepository = new ContactTagRepository(unitOfWork);
            accountcontactRepository = new AccountContactRepository(unitOfWork);
            leadContactRepository = new LeadContactRepository(unitOfWork);
            userRepository = new UserRepository(_unitOfWork);
        }

        public int AddContact(ContactModel contactModel)
        {

            Contact contact = new Contact();
            AutoMapper.Mapper.Map(contactModel, contact);
            contact.CreatedDate = DateTime.UtcNow;
            contact.ModifiedBy = contactModel.ModifiedBy;
            contact.CreatedBy = contactModel.CreatedBy;
            contact.ModifiedDate = DateTime.UtcNow;
            contact.Address = null;
            contact.User = null;

            contact.Address = ValidateAddress(contact.Address, contactModel.AddressModel);

            if (contactModel.AccountId > 0)
            {
                AccountContact accountcontact = new AccountContact();
                accountcontact.AccountId = contactModel.AccountId;

                accountcontact.CreatedBy = contactModel.CreatedBy;
                accountcontact.CreatedDate = DateTime.UtcNow;
                contact.AccountContacts.Add(accountcontact);
            }
            if (contactModel.LeadId > 0)
            {
                LeadContact leadcontact = new LeadContact();
                leadcontact.LeadId = contactModel.LeadId.Value;

                leadcontact.CreatedBy = contactModel.CreatedBy;
                leadcontact.CreatedDate = DateTime.UtcNow;
                contact.LeadContacts.Add(leadcontact);
            }
            contactRepository.Insert(contact);
            List<int> lisTagIds = new List<int>();

            if (!string.IsNullOrEmpty(contactModel.ContactTagIds))
            {
                string[] arrTagIds = contactModel.ContactTagIds.Split(',');
                int tagId = 0;
                for (int i = 0; i < arrTagIds.Length; i++)
                {
                    tagId = arrTagIds[i].Decrypt();

                    lisTagIds.Add(tagId);
                }

            }

            if (!string.IsNullOrEmpty(contactModel.NewTagNames))
            {
                List<int> lisNewAddedTagIds = new List<int>();
                lisNewAddedTagIds = AddTagsToSystem(contactModel.NewTagNames, contactModel.CompanyId, contactModel.CreatedBy);

                if (lisNewAddedTagIds.Count > 0)
                {
                    lisTagIds = lisTagIds.Concat(lisNewAddedTagIds).ToList();

                }
            }


            //Associate Tagids to contact
            if (lisTagIds.Count > 0)
            {
                ContactTag contactTag;
                for (int i = 0; i < lisTagIds.Count; i++)
                {
                    contactTag = new ContactTag();
                    contactTag.ContactId = contact.ContactId;
                    contactTag.TagId = lisTagIds[i];
                    contactTagRepository.Insert(contactTag);

                }


            }


            return contact.ContactId;

        }

        //       public List<TagModel> ListTopUsedContacTags(int companyId)
        //       {
        //           List<TagModel> listTagModel = new List<TagModel>();
        ////List<ContactTag>  obj= contactTagRepository.GetAll(x => x.Contact.CompanyId == companyId).ToList();
        ////           obj.Select(x=>x.Tag).GroupBy(x=>x.TagId).OrderBy(x=>x.Count()).Select

        //           var tags = tagRepository.GetAll(x => x.CompanyId == companyId).OrderBy(y => y.ContactTags.Count()).ToList();

        //       }

        public List<ContactBulkUploadModel> BulkInsertContact(DataTable ContactDataTable, int UserId, int CompanyId)
        {
            List<Contact> contactList = new List<Contact>();
            List<Contact> returnList = new List<Contact>();
            List<ContactBulkUploadModel> returnContactList = new List<ContactBulkUploadModel>();
            for (int i = 0; i < ContactDataTable.Rows.Count; i++)
            {


                Contact contactModel = new Contact();

                contactModel.EmailAddress = Convert.ToString(ContactDataTable.Rows[i]["EmailAddress"]);
                //contactModel.FirstName = Convert.ToString(ContactDataTable.Rows[i]["FirstName"]);
                //contactModel.LastName = Convert.ToString(ContactDataTable.Rows[i]["LastName"]);
                contactModel.FirstName = Convert.ToString(ContactDataTable.Rows[i]["Name"]);
                contactModel.Phone = Convert.ToString(ContactDataTable.Rows[i]["Phone"]);
                contactModel.Mobile = Convert.ToString(ContactDataTable.Rows[i]["Mobile"]);
                contactModel.ContactCompanyName = Convert.ToString(ContactDataTable.Rows[i]["CompanyName"]);
                contactModel.JobPosition = Convert.ToString(ContactDataTable.Rows[i]["JobPosition"]);
                contactModel.OwnerId = UserId;
                contactModel.CompanyId = CompanyId;
                contactModel.CreatedDate = System.DateTime.UtcNow;
                contactModel.CreatedBy = UserId;
                contactModel.ModifiedBy = UserId;
                contactModel.ModifiedDate = System.DateTime.UtcNow;
                if (string.IsNullOrEmpty(Convert.ToString(ContactDataTable.Rows[i]["Name"])))
                {
                    ContactBulkUploadModel bulkuploadmodel = new ContactBulkUploadModel();
                    AutoMapper.Mapper.Map(contactModel, bulkuploadmodel);
                    bulkuploadmodel.ErrorDescription = CommonFunctions.GetGlobalizedLabel(ErucaCRM.Utility.Constants.MODULE_CONTACT, "FirstNameRequired");
                    returnContactList.Add(bulkuploadmodel);

                }
                else
                    if (Convert.ToString(ContactDataTable.Rows[i]["Name"]).Length > 140)
                    {
                        ContactBulkUploadModel bulkuploadmodel = new ContactBulkUploadModel();
                        AutoMapper.Mapper.Map(contactModel, bulkuploadmodel);
                        bulkuploadmodel.ErrorDescription = CommonFunctions.GetGlobalizedLabel(ErucaCRM.Utility.Constants.MODULE_CONTACT, "FirstNameLengthNotValid"
    );
                        returnContactList.Add(bulkuploadmodel);

                    }

//                    else
//                        if (Convert.ToString(ContactDataTable.Rows[i]["LastName"]).Length > 140)
//                        {
//                            ContactBulkUploadModel bulkuploadmodel = new ContactBulkUploadModel();
//                            AutoMapper.Mapper.Map(contactModel, bulkuploadmodel);
//                            bulkuploadmodel.ErrorDescription = CommonFunctions.GetGlobalizedLabel(ErucaCRM.Utility.Constants.MODULE_CONTACT, "LastNameLengthNotValid"
//);
//                            returnContactList.Add(bulkuploadmodel);

//                        }
                        else
                            if (!Convert.ToString(ContactDataTable.Rows[i]["EmailAddress"]).IsValidEmailAddress() || Convert.ToString(ContactDataTable.Rows[i]["EmailAddress"]).Length > 140)
                            {
                                ContactBulkUploadModel bulkuploadmodel = new ContactBulkUploadModel();
                                AutoMapper.Mapper.Map(contactModel, bulkuploadmodel);
                                bulkuploadmodel.ErrorDescription = CommonFunctions.GetGlobalizedLabel(ErucaCRM.Utility.Constants.MODULE_CONTACT, "EmailNotValid"
        ); ;
                                returnContactList.Add(bulkuploadmodel);

                            }

                            else
                                if (Convert.ToString(ContactDataTable.Rows[i]["Phone"]).Length > 20)
                                {
                                    ContactBulkUploadModel bulkuploadmodel = new ContactBulkUploadModel();
                                    AutoMapper.Mapper.Map(contactModel, bulkuploadmodel);
                                    bulkuploadmodel.ErrorDescription = CommonFunctions.GetGlobalizedLabel(ErucaCRM.Utility.Constants.MODULE_CONTACT, "PhoneNumberNotValid"
            );
                                    returnContactList.Add(bulkuploadmodel);
                                }
                                else
                                    if (Convert.ToString(ContactDataTable.Rows[i]["CompanyName"]).Length > 80)
                                    {
                                        ContactBulkUploadModel bulkuploadmodel = new ContactBulkUploadModel();
                                        AutoMapper.Mapper.Map(contactModel, bulkuploadmodel);
                                        bulkuploadmodel.ErrorDescription = CommonFunctions.GetGlobalizedLabel(ErucaCRM.Utility.Constants.MODULE_CONTACT, "CompanyNameNotValid"

                );
                                        returnContactList.Add(bulkuploadmodel);
                                    }
                                    else
                                        if (Convert.ToString(ContactDataTable.Rows[i]["Mobile"]).Length > 20)
                                        {
                                            ContactBulkUploadModel bulkuploadmodel = new ContactBulkUploadModel();
                                            AutoMapper.Mapper.Map(contactModel, bulkuploadmodel);
                                            bulkuploadmodel.ErrorDescription = CommonFunctions.GetGlobalizedLabel(ErucaCRM.Utility.Constants.MODULE_CONTACT, "MobileNumberNotValid"
                    );
                                            returnContactList.Add(bulkuploadmodel);
                                        }
                                        else
                                        {
                                            contactList.Add(contactModel);
                                        }



            }

            contactRepository.InsertAll(contactList);
            return returnContactList;
        }

        public void DeleteFileFromServerAfterUpload(string FilePath)
        {
            if (System.IO.File.Exists(FilePath))
            {
                System.IO.File.Delete(FilePath);
            }


        }

        public void AddContact(List<ContactModel> contactModelList)
        {
            List<Contact> contactList = new List<Contact>();
            Contact contact = new Contact();
            contact.Address = null;
            contact.User = null;
        }

        private List<int> AddTagsToSystem(string newTagNames, int companyId, int userId)
        {
            List<int> lisNewTagIds = new List<int>();
            string[] arrNewTagName = newTagNames.Split(',');
            Tag tag = new Tag();

            for (int i = 0; i < arrNewTagName.Length; i++)
            {
                tag = new Tag();
                tag.TagName = arrNewTagName[i];
                tag.CompanyId = companyId;
                tag.CreatedBy = userId;
                tag.ModifiedBy = userId;
                tag.CreatedDate = DateTime.UtcNow;
                tag.ModifiedDate = DateTime.UtcNow;

                tagRepository.Insert(tag);
                lisNewTagIds.Add(tag.TagId);

            }



            return lisNewTagIds;

        }

        public void UpdateContact(ContactModel contactModel)
        {
            Contact contact = contactRepository.SingleOrDefault(u => u.ContactId == contactModel.ContactId && u.RecordDeleted == false);
            if (contact != null)
            {
                contactModel.ModifiedDate = DateTime.UtcNow;
                AutoMapper.Mapper.Map(contactModel, contact);

                if (contact.AddressId == null || contact.AddressId == 0) { contact.Address = null; }


                else if (contact.Address.CountryId == 0)
                    contact.Address.CountryId = null;




                contactRepository.Update(contact);
                List<int> listAllExistingTagIds = new List<int>();
                List<int> listNewTagIds = new List<int>();
                List<int> listContactAssociatedTagIds = new List<int>();
                List<int> listDeleteTagIds = new List<int>();
                listContactAssociatedTagIds = contact.ContactTags.Select(x => x.TagId).ToList();

                if (!string.IsNullOrEmpty(contactModel.ContactTagIds))
                {
                    string[] arrTagIds = contactModel.ContactTagIds.Split(',');
                    int tagId = 0;
                    for (int i = 0; i < arrTagIds.Length; i++)
                    {
                        tagId = arrTagIds[i].Decrypt();
                        if (listContactAssociatedTagIds.Where(x => x == tagId).Count() == 0)
                        {
                            listNewTagIds.Add(tagId);
                        }
                        else
                        {
                            listAllExistingTagIds.Add(tagId);
                        }
                    }

                }


                if (!string.IsNullOrEmpty(contactModel.NewTagNames))
                {
                    List<int> lisNewAddedTagIds = new List<int>();
                    lisNewAddedTagIds = AddTagsToSystem(contactModel.NewTagNames, contactModel.CompanyId, contactModel.CreatedBy);

                    if (lisNewAddedTagIds.Count > 0)
                    {
                        listNewTagIds = listNewTagIds.Concat(lisNewAddedTagIds).ToList();

                    }
                }
                for (int i = 0; i < listContactAssociatedTagIds.Count; i++)
                {
                    if (listAllExistingTagIds.Where(x => x == listContactAssociatedTagIds[i]).Count() == 0)
                    {
                        listDeleteTagIds.Add(listContactAssociatedTagIds[i]);
                    }

                }

                //Associate Tagids to contact
                if (listNewTagIds.Count > 0)
                {
                    ContactTag contactTag;
                    for (int i = 0; i < listNewTagIds.Count; i++)
                    {
                        contactTag = new ContactTag();
                        contactTag.ContactId = contact.ContactId;
                        contactTag.TagId = listNewTagIds[i];
                        contactTagRepository.Insert(contactTag);

                    }


                }

                if (listDeleteTagIds.Count > 0)
                    contactTagRepository.Delete(x => x.ContactId == contact.ContactId && listDeleteTagIds.Contains(x.TagId));


            }
        }

        public Boolean DeleteAccountContact(int contactId, int accountId, int userId)
        {

            accountcontactRepository.Delete(x => x.ContactId == contactId && x.AccountId == accountId);
            return true;

        }

        public Boolean DeleteLeadContact(int contactId, int leadId, int userId)
        {
            leadContactRepository.Delete(x => x.ContactId == contactId && x.LeadId == leadId);
            return true;

        }

        public Boolean DeleteContact(int contactId, int userId)
        {
            bool IsDeleted = false;
            IsDeleted = contactRepository.DeleteContact(contactId);
            return IsDeleted;
        }

        public List<TagModel> GetMostUsedContactTags(int companyId)
        {
            List<TagModel> lisTagModels = new List<TagModel>();

            List<Tag> listTags = tagRepository.GetAll(x => x.CompanyId == companyId && x.ContactTags.Count() > 0).OrderByDescending(y => y.ContactTags.Count()).Distinct().Skip(0).Take(5).ToList();

            AutoMapper.Mapper.Map(listTags, lisTagModels);

            return lisTagModels;


        }

        public ContactModel GetContactByContactId(int contactId)
        {
            ContactModel contactmodel = null;
            Contact contact = contactRepository.SingleOrDefault(u => u.ContactId == contactId && u.RecordDeleted == false);
            contactmodel = new ContactModel();
            AutoMapper.Mapper.Map(contact, contactmodel);
            AutoMapper.Mapper.Map(contact.Address, contactmodel.AddressModel);
            AutoMapper.Mapper.Map(contact.FileAttachments, contactmodel.FileAttachmentModels);
            if (contact.User != null)
            {
                contactmodel.OwnerName = contact.User.FirstName + " " + contact.User.LastName;
                // GetContactOwner((contact.OwnerId ?? 0));    
            }
            return contactmodel; 
        }

        private String GetContactOwner(int contactOwnerID)
        {
            string contactOwnerName = "";
            User contactOwnerUser = userRepository.SingleOrDefault(u => u.UserId == contactOwnerID && u.RecordDeleted == false);// userRepository.SingleOrDefault(x => x.UserId == contactOwnerID);
            contactOwnerName = (contactOwnerUser.FirstName + " " + contactOwnerUser.LastName);
            return contactOwnerName;
        }

        public List<ContactModel> GetTaggedContacts(int tagId, int companyId, int currentPage, Int16 pageSize, ref int totalRecords)
        {
            List<ContactModel> listContactModel = new List<ContactModel>();
            List<Contact> listContacts = new List<Contact>();


            totalRecords = contactRepository.Count(x => x.ContactTags.Where(c => c.TagId == tagId).Count() > 0 && x.CompanyId == companyId && x.RecordDeleted == false);

            listContacts = contactRepository.GetPagedRecords(x => x.ContactTags.Where(c => c.TagId == tagId).Count() > 0 && x.CompanyId == companyId && x.RecordDeleted == false, y => y.FirstName, currentPage, pageSize).ToList();


            AutoMapper.Mapper.Map(listContacts, listContactModel);

            return listContactModel;
        }

        public List<ContactModel> GetTaggedContactSearchByName(string tagSearchName, int companyId, int currentPage, Int16 pageSize, ref int totalRecords)
        {
            List<ContactModel> listContactModel = new List<ContactModel>();
            List<Contact> listContacts = new List<Contact>();


            totalRecords = contactRepository.Count(x => x.ContactTags.Where(c => c.Tag.TagName.ToLower() == tagSearchName.ToLower()).Count() > 0 && x.CompanyId == companyId && x.RecordDeleted == false);

            listContacts = contactRepository.GetPagedRecords(x => x.ContactTags.Where(c => c.Tag.TagName.ToLower() == tagSearchName.ToLower()).Count() > 0 && x.CompanyId == companyId && x.RecordDeleted == false, y => y.FirstName, currentPage, pageSize).ToList();


            AutoMapper.Mapper.Map(listContacts, listContactModel);

            return listContactModel;
        }

        public List<ContactModel> GetAllContactsByTagSearch(int companyId, int currentPage, Int16 pageSize, int currentOwnerID, string searchTags, ref int totalRecords)
        {
            List<ContactModel> listContactModel = new List<ContactModel>();
            List<Contact> listContacts = new List<Contact>();
            String[] arraySearchTags = searchTags.Split(',');
            IEnumerable<string> listSearchTags = arraySearchTags.Select(x => "," + x + ",");

            totalRecords = contactRepository.Count(x => x.CompanyId == companyId && x.RecordDeleted == false && x.Tags != null && listSearchTags.Any(y => ("," + x.Tags + ",").Contains(y)));

            listContacts = contactRepository.GetPagedRecords(x => x.CompanyId == companyId && x.RecordDeleted == false && x.Tags != null && listSearchTags.Any(y => ("," + x.Tags + ",").Contains(y)), y => y.CreatedDate, currentPage, pageSize).ToList();

            AutoMapper.Mapper.Map(listContacts, listContactModel);

            return listContactModel;
        }

        public List<ContactModel> GetAllContacts(int companyId, int currentPage, Int16 pageSize, int currentOwnerID,string tagName,int tagId, string filterBy, ref int totalRecords)
        {
            List<ContactModel> listContactModel = new List<ContactModel>();
           // List<SSP_NonAssociatedContactList_Result> listContacts = new List<SSP_NonAssociatedContactList_Result>();
            List<SSP_GetContactForLeadAccountContacts_Result> ContactListResult = contactRepository.GetContactsByUserId(currentOwnerID, companyId, tagName, tagId, filterBy, currentPage, pageSize, out totalRecords);
            AutoMapper.Mapper.Map(ContactListResult, listContactModel);

            //if (filterBy == "Allcontacts")
            //{
            //    totalRecords = contactRepository.Count(x => x.CompanyId == companyId && x.RecordDeleted == false);
            //    listContacts = contactRepository.GetPagedRecordsDecending(x => x.CompanyId == companyId && x.RecordDeleted == false, y => y.CreatedDate, currentPage, pageSize).ToList();
            //}
            //else if (filterBy == "Mycontacts")
            //{

            //    totalRecords = contactRepository.Count(x => x.CompanyId == companyId && x.RecordDeleted == false && x.OwnerId == currentOwnerID);
            //    listContacts = contactRepository.GetPagedRecordsDecending(x => x.CompanyId == companyId && x.RecordDeleted == false && x.OwnerId == currentOwnerID, y => y.CreatedDate, currentPage, pageSize).ToList();
            //}
            //else if (filterBy == "ThisWeekContacts")
            //{
            //    DateTime dateFrom = DateTime.Today.AddDays(-7);
            //    // dateFrom = 
            //    //DateTime dateTo = new DateTime();
            //    DateTime dateTo = DateTime.Today;
            //    totalRecords = contactRepository.Count(x => x.CompanyId == companyId && x.RecordDeleted == false && (x.CreatedDate.Value <= dateTo && x.CreatedDate.Value >= dateFrom));
            //    listContacts = contactRepository.GetPagedRecordsDecending(x => x.CompanyId == companyId && x.RecordDeleted == false && ((x.CreatedDate.Value <= dateTo && x.CreatedDate.Value >= dateFrom)), y => y.CreatedDate, currentPage, pageSize).ToList();
            //}
            //else if (filterBy == "LastWeekContacts")
            //{
            //    DateTime dateFrom = DateTime.Today.AddDays(-14);
            //    // dateFrom = 
            //    //DateTime dateTo = new DateTime();
            //    DateTime dateTo = DateTime.Today.AddDays(-7);
            //    totalRecords = contactRepository.Count(x => x.CompanyId == companyId && x.RecordDeleted == false && ((x.CreatedDate.Value <= dateTo && x.CreatedDate.Value >= dateFrom)));
            //    listContacts = contactRepository.GetPagedRecordsDecending(x => x.CompanyId == companyId && x.RecordDeleted == false && ((x.CreatedDate.Value <= dateTo && x.CreatedDate.Value >= dateFrom)), y => y.CreatedDate, currentPage, pageSize).ToList();
            //}



            //AutoMapper.Mapper.Map(listContacts, listContactModel);

            return listContactModel;
        }


        public List<ContactModel> NonAssociatedContactList(int companyId, int currentPage, Int16 pageSize, int currentOwnerID, string filterBy, int filterId, ref int totalRecords)
        {
            List<ContactModel> listContactModel = new List<ContactModel>();
            List<Contact> listContacts = new List<Contact>();

            List<SSP_NonAssociatedContactList_Result> NonAssociatedContact_Res = contactRepository.NonAssociatedContactList(companyId,currentPage,pageSize,currentOwnerID,filterBy,filterId,out totalRecords);
            AutoMapper.Mapper.Map(NonAssociatedContact_Res, listContactModel);
            //if (filterBy == "Allcontacts")
            //{
            //    totalRecords = contactRepository.Count(x => x.CompanyId == companyId && x.RecordDeleted == false && x.AccountContacts.Any(y => y.ContactId == x.ContactId && y.AccountId == filterId) == false);
            //    listContacts = contactRepository.GetPagedRecords(x => x.CompanyId == companyId && x.RecordDeleted == false && x.AccountContacts.Any(y => y.ContactId == x.ContactId && y.AccountId == filterId) == false, y => y.FirstName, currentPage, pageSize).ToList();
            //}
            //else if (filterBy == "LeadContacts")
            //{
            //    totalRecords = contactRepository.Count(x => x.CompanyId == companyId && x.RecordDeleted == false && x.LeadContacts.Any(y => y.ContactId == x.ContactId && y.LeadId == filterId) == false);
            //    listContacts = contactRepository.GetPagedRecords(x => x.CompanyId == companyId && x.RecordDeleted == false && x.LeadContacts.Any(y => y.ContactId == x.ContactId && y.LeadId == filterId) == false, y => y.FirstName, currentPage, pageSize).ToList();
            //}
           // AutoMapper.Mapper.Map(listContacts, listContactModel);

            return listContactModel;
        }


        public IList<ContactModel> GetContactsByOwnerIdAndCompanyID(int companyId, int ownerId,string tagName="",int tagId=0,int currentPage=1,int pagesize=0)
        {
            int totalrecords = 0;
            List<ContactModel> contactsModel = new List<ContactModel>();
            List<SSP_GetContactForLeadAccountContacts_Result> ContactLead_Result = contactRepository.GetContactsByUserId(ownerId,companyId,tagName,tagId,Enums.ContactList.GetAllMyContacts.ToString(),currentPage,pagesize,out totalrecords).ToList();

            //List<Contact> contacts = contactRepository.GetAll(l => l.CompanyId == companyId && l.OwnerId == ownerId).ToList();
            //List<Contact> contacts = contactRepository.GetAll(l => l.CompanyId == companyId && l.RecordDeleted == false).ToList();
          //  AutoMapper.Mapper.Map(contacts, contactsModel);
            AutoMapper.Mapper.Map(ContactLead_Result, contactsModel);
            // contactsModel.Insert(0, new ContactModel { ContactId = 0, FirstName = Constants.CULTURE_SPECIFIC_DROPDOWNS_SELECT_OPTION });
            return contactsModel;
        }

        private Address ValidateAddress(Address oldAddress, AddressModel newAddress)
        {
            Address objAddress = null;
            //if Address is already there then update the address;
            if (oldAddress != null && newAddress != null)
            {

                oldAddress.Street = newAddress.Street;
                oldAddress.City = newAddress.City;
                oldAddress.State = newAddress.State;
                oldAddress.Zipcode = newAddress.Zipcode;

                if (newAddress.CountryId.GetValueOrDefault() > 0)
                    oldAddress.CountryId = newAddress.CountryId.GetValueOrDefault();
                else
                    oldAddress.CountryId = null;
                return oldAddress;


            }
            else if (newAddress != null)
            {
                if (newAddress.Street != null || newAddress.City != null || newAddress.Zipcode != null || newAddress.State != null || newAddress.CountryId > 0)
                {
                    objAddress = new Address();

                    objAddress.Street = newAddress.Street;
                    objAddress.City = newAddress.City;
                    objAddress.State = newAddress.State;
                    objAddress.Zipcode = newAddress.Zipcode;

                    if (newAddress.CountryId.GetValueOrDefault() > 0)
                        objAddress.CountryId = newAddress.CountryId.GetValueOrDefault();
                    else
                        objAddress.CountryId = null;
                    return objAddress;
                }

            }

            return objAddress;
        }

        public List<AccountContactModel> GetContactByAccountId(int userId,int companyId, int accountId)
        {
            List<AccountContactModel> contactModelList = new List<AccountContactModel>();
            List<AccountContact> accountContactList = new List<AccountContact>();
            List<ContactModel> contactModellist = new List<ContactModel>();
          
                List<SSP_GetContactForLeadAccountContacts_Result> AccountContact_Result = accountcontactRepository.GetContactsByAccountId(userId, accountId, companyId, Enums.ContactList.AccountContacts.ToString()).ToList();
           
            //accountContactList = accountcontactRepository.GetAll(r => r.AccountId == accountId).ToList();
            //AutoMapper.Mapper.Map(accountContactList, contactModelList);
               // AutoMapper.Mapper.Map(AccountContact_Result, contactModel);
                //AutoMapper.Mapper.Map(AccountContact_Result, contactModelList);
                foreach (SSP_GetContactForLeadAccountContacts_Result item in AccountContact_Result)
                {
                    ContactModel contactModel = new ContactModel();
                    AccountContactModel accountcontactModel = new AccountContactModel();
                    AutoMapper.Mapper.Map(item, contactModel);
                    accountcontactModel.ContactId = contactModel.ContactId;
                    accountcontactModel.CreatedBy = contactModel.CreatedBy;
                    accountcontactModel.AccountId = contactModel.AccountId;
                    accountcontactModel.Contact = contactModel;
                    contactModelList.Add(accountcontactModel);
                }
          
            return contactModelList;

        }

        public List<ContactModel> GetContactByLeadId(int userId, int CompanyId, int LeadId, int CurrentPage, int pagesize,ref int totalrecord,string tagName="",int tagId=0)
        {
            List<ContactModel> contactModelList = new List<ContactModel>();
            List<SSP_GetContactForLeadAccountContacts_Result> ContactLead_Result = contactRepository.GetContactsByLeadId(userId, LeadId, Enums.ContactList.LeadContacts.ToString(), CompanyId, CurrentPage, pagesize,tagName,tagId, out totalrecord);


            //totalrecord = contactRepository.Count(x => x.CompanyId == CompanyId && (x.RecordDeleted == null || x.RecordDeleted == false)
            //    && x.LeadContacts.Any(y => y.LeadId == LeadId));

            //leadContactList = contactRepository.GetPagedRecords(x => x.CompanyId == CompanyId && x.RecordDeleted == false && x.LeadContacts.Any(y => y.LeadId == LeadId), y => y.FirstName, CurrentPage > 0 ? CurrentPage : 1, pagesize).ToList();


            AutoMapper.Mapper.Map(ContactLead_Result, contactModelList);

            //AutoMapper.Mapper.Map(ContactLead_Result, contactModelList);

            return contactModelList;
        }
        public List<ContactModel> GetAssociatedContactByLeadId(int userId, int leadId, int companyId, int currentPage, int pageSize, ref int totalrecords)
        {
            List<ContactModel> contactModelList = new List<ContactModel>();
            List<SSP_GetContactsByLeadId_Result> ContactLead_Result = contactRepository.GetAssociatedContactByLeadId(leadId, userId, companyId, currentPage, pageSize, out totalrecords);
            AutoMapper.Mapper.Map(ContactLead_Result, contactModelList);      

            return contactModelList;
        }


        public List<ContactModel> GetContactList(int UserId,int CompanyId,int filterId=0,int curruntPage=1,int pageSize=0,int tagId=0,string TagName="")
        {
            List<ContactModel> contactModelList = new List<ContactModel>();
            List<Contact> contactList = new List<Contact>();
            List<SSP_GetContactForLeadAccountContacts_Result> ContactListResult = contactRepository.GetContactsList(UserId, CompanyId, filterId, Enums.ContactList.GetAllMyContacts.ToString(), curruntPage, pageSize, tagId, TagName).ToList();
            AutoMapper.Mapper.Map(ContactListResult, contactModelList);
            //contactList = contactRepository.GetAll(r => r.CompanyId == CompanyId).ToList();
            //AutoMapper.Mapper.Map(contactList, contactModelList);
            return contactModelList;
        }

        public void AssociateLeadToContact(int leadId, int ContactId, int UserId)
        {
            LeadContact leadContact = new LeadContact();
            leadContact.LeadId = leadId;
            leadContact.ContactId = ContactId;
            leadContact.CreatedBy = UserId;
            leadContact.CreatedDate = DateTime.UtcNow;
            leadContactRepository.Insert(leadContact);
        }

        public void AssociateAccountToContact(List<AccountContactModel> accountcontactlist, int createdBy)
        {

            List<AccountContact> accountContactList = new List<AccountContact>();

            foreach (var accountcontact in accountcontactlist)
            {
                AccountContact acccontact = new AccountContact();
                acccontact.AccountId = accountcontact.AccountId;
                acccontact.ContactId = accountcontact.ContactId;
                acccontact.CreatedBy = createdBy;
                acccontact.CreatedDate = DateTime.UtcNow;
                accountContactList.Add(acccontact);
            }
            accountcontactRepository.InsertAll(accountContactList);
        }

        public void AssociateLeadToContact(List<LeadContactModel> leadContactModelList, int createdBy)
        {
            List<LeadContact> LeadContactList = new List<LeadContact>();
            foreach (LeadContactModel leadContactModel in leadContactModelList)
            {
                LeadContact acccontact = new LeadContact();
                acccontact.LeadId = leadContactModel.LeadId;
                acccontact.ContactId = leadContactModel.ContactId;
                acccontact.CreatedBy = createdBy;
                acccontact.CreatedDate = DateTime.UtcNow;
                LeadContactList.Add(acccontact);
            }
            leadContactRepository.InsertAll(LeadContactList);
        }

        public bool IsLeadContactExists(int companyId, int LeadId)
        {
            int totalRecords = contactRepository.Count(x => x.CompanyId == companyId && (x.RecordDeleted == null || x.RecordDeleted == false)
                  && x.LeadContacts.Any(y => y.LeadId == LeadId));
            return totalRecords > 0;
        }

    }


}
