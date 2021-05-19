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

namespace ErucaCRM.Business
{
    public class AccountBusiness : IAccountBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly AccountRespository accountRepository;
        private readonly AccountTypeRespository accountTypeRepository;
        private readonly TagRepository tagRepository;
        private readonly AccountTagRepository accountTagRepository;
        private readonly TaskItemRepository taskItemRepository;
        private readonly ContactRepository contactRepository;
        private readonly FileAttachmentRepository fileAttachmentRepository;
        private readonly SalesOrderRepository salesOrderRepository;
        private readonly AccountCaseRepository accountCaseRepository;

        public AccountBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            accountRepository = new AccountRespository(unitOfWork);
            accountTypeRepository = new AccountTypeRespository(unitOfWork);
            tagRepository = new TagRepository(unitOfWork);
            accountTagRepository = new AccountTagRepository(unitOfWork);
            taskItemRepository = new TaskItemRepository(unitOfWork);
            contactRepository = new ContactRepository(unitOfWork);
            fileAttachmentRepository = new FileAttachmentRepository(unitOfWork);
            salesOrderRepository = new SalesOrderRepository(unitOfWork);
            accountCaseRepository = new AccountCaseRepository(unitOfWork);

        }

        /// <summary>
        /// Function used to Add/Update Account information
        /// </summary>
        /// <param name="accountModel"></param>
        public void AddUpdateAccount(AccountModel accountModel)
        {
            Account account = new Account();

            if (accountModel.ParentAccountId == 0)
                accountModel.ParentAccountId = null;
            if (accountModel.AccountTypeId == 0)
                accountModel.AccountTypeId = null;
            if (accountModel.IndustryId == 0)
                accountModel.IndustryId = null;


            //if account id is 0 then account will be added
            if (accountModel.AccountId == 0)
            {
                AutoMapper.Mapper.Map(accountModel, account);
                account.CreatedDate = DateTime.UtcNow;
                account.ModifiedDate = DateTime.UtcNow;
                account.Address = ValidateAddress(account.Address, accountModel.AddressModel);
                account.Address1 = ValidateAddress(account.Address1, accountModel.AddressModel1);


                accountRepository.Insert(account);
            }
            else   //if account id is not 0 then account will be updated
            {
                account = accountRepository.SingleOrDefault(x => x.AccountId == accountModel.AccountId);

                // Created by and CreatedDate will not change during mapping
                accountModel.CreatedBy = account.CreatedBy;
                accountModel.CreatedDate = account.CreatedDate;

                AutoMapper.Mapper.Map(accountModel, account);
                account.ModifiedDate = DateTime.UtcNow;
                account.Address = ValidateAddress(account.Address, accountModel.AddressModel);
                account.Address1 = ValidateAddress(account.Address1, accountModel.AddressModel1);
                accountRepository.Update(account);

            }


            List<int> listAllExistingTagIds = new List<int>();
            List<int> listNewTagIds = new List<int>();
            List<int> listContactAssociatedTagIds = new List<int>();
            List<int> listDeleteTagIds = new List<int>();

            listContactAssociatedTagIds = account.AccountTags.Select(x => x.TagId).ToList();

            if (!string.IsNullOrEmpty(accountModel.AccountTagIds))
            {
                string[] arrTagIds = accountModel.AccountTagIds.Split(',');
                for (int i = 0; i < arrTagIds.Length; i++)
                {
                    int tagId = arrTagIds[i].Decrypt();

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


            if (!string.IsNullOrEmpty(accountModel.NewTagNames))
            {
                List<int> lisNewAddedTagIds = new List<int>();
                lisNewAddedTagIds = AddTagsToSystem(accountModel.NewTagNames, accountModel.CompanyId, accountModel.CreatedBy.Value);

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
                AccountTag accountTag;
                for (int i = 0; i < listNewTagIds.Count; i++)
                {
                    accountTag = new AccountTag();
                    accountTag.AccountId = account.AccountId;
                    accountTag.TagId = listNewTagIds[i];
                    accountTagRepository.Insert(accountTag);

                }


            }

            if (listDeleteTagIds.Count > 0)
                accountTagRepository.Delete(x => x.AccountId == account.AccountId && listDeleteTagIds.Contains(x.TagId));


        }

        public List<TagModel> GetMostUsedAccountTags(int companyId)
        {
            List<TagModel> lisTagModels = new List<TagModel>();

            List<Tag> listTags = tagRepository.GetAll(x => x.CompanyId == companyId && x.AccountTags.Count() > 0).OrderByDescending(y => y.AccountTags.Count()).Distinct().Skip(0).Take(5).ToList();

            AutoMapper.Mapper.Map(listTags, lisTagModels);

            return lisTagModels;


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
        public IList<AccountTypeModel> GetAccountTypes()
        {
            List<AccountTypeModel> listAccountType = new List<AccountTypeModel>();
            //Select the accounts those are not child of any other account
            listAccountType = accountTypeRepository.GetAll().Select(y => new AccountTypeModel() { AccountTypeId = y.AccountTypeId, AccountTypeName = y.AccountTypeName }).ToList();

            listAccountType.Insert(0, new AccountTypeModel { AccountTypeId = 0, AccountTypeName = Constants.CULTURE_SPECIFIC_DROPDOWNS_SELECT_OPTION });


            return listAccountType;
        }

        public IList<ParentAccountListModel> GetParentAccountListByCompanyID(int companyId)
        {
            List<AccountModel> accountsModel = new List<AccountModel>();
            //Select the accounts those are not child of any other account

            List<ParentAccountListModel> parentAccountListModel = accountRepository.GetAll(x => x.CompanyId == companyId && x.RecordDeleted == false && x.ParentAccountId == null).Select(y => new ParentAccountListModel() { AccountId = y.AccountId, AccountName = y.AccountName }).ToList();


            parentAccountListModel.Insert(0, new ParentAccountListModel() { AccountId = 0, AccountName = Constants.CULTURE_SPECIFIC_DROPDOWNS_SELECT_OPTION });


            return parentAccountListModel;
        }

        public IList<AccountListModel> GetAccountListByCompanyID(int companyId,int userid)
        {
            List<AccountListModel> AccountListModel = new List<AccountListModel>();

           // List<AccountListModel> AccountListModel = accountRepository.GetAll(x => x.CompanyId == companyId && x.RecordDeleted == false).Select(y => new AccountListModel() { AccountId = y.AccountId, AccountName = y.AccountName }).ToList();
            List<SSP_GetAccountsListbyUserId_Result> accounts = accountRepository.GetAccountsListByUserId(userid, companyId);
            AutoMapper.Mapper.Map(accounts, AccountListModel);

            //AccountListModel.Insert(0, new AccountListModel() { AccountId = 0, AccountName = "---Select---" });


            return AccountListModel;
        }

        public List<AccountModel> GetAccountsByOwnerIdAndCompanyID(int companyId, int ownerId)
        {
            List<AccountModel> accountsModel = new List<AccountModel>();
            //List<Account> accounts = accountRepository.GetAll(l => l.CompanyId == companyId && l.OwnerId == ownerId).ToList();
           // List<Account> accounts = accountRepository.GetAll(l => l.CompanyId == companyId && l.RecordDeleted == false).ToList();
            List<SSP_GetAccountsListbyUserId_Result> accounts = accountRepository.GetAccountsListByUserId(ownerId, companyId);
            AutoMapper.Mapper.Map(accounts, accountsModel);
            //    AccountsModel.Insert(0, new AccountModel { AccountId = 0, FirstName = "---Select---" });
            return accountsModel;
        }
        public AccountModel AccountDetail(int accountId)
        {
            AccountModel accountmodel = null;
            Account account = accountRepository.SingleOrDefault(u => u.AccountId == accountId && u.RecordDeleted == false);
           
            account.Leads = account.Leads.Where(x => x.RecordDeleted == false).ToList();
            account.AccountCases = account.AccountCases.Where(x => x.RecordDeleted == false).ToList();
            accountmodel = new AccountModel();
            accountmodel.LeadsModels = new List<LeadModel>();
            accountmodel.AccountCaseModels = new List<AccountCaseModel>();
            accountmodel.AccountTagModels = new List<AccountTagModel>();
            if (account != null)
            {
                AutoMapper.Mapper.Map(account, accountmodel);
                AutoMapper.Mapper.Map(account.AccountType, accountmodel.AccountTypeModel);
                AutoMapper.Mapper.Map(account.Leads, accountmodel.LeadsModels);
                AutoMapper.Mapper.Map(account.Address, accountmodel.AddressModel);
                AutoMapper.Mapper.Map(account.Address1, accountmodel.AddressModel1);
                AutoMapper.Mapper.Map(account.AccountCases, accountmodel.AccountCaseModels);
                AutoMapper.Mapper.Map(account.AccountTags, accountmodel.AccountTagModels);
                AutoMapper.Mapper.Map(account.User, accountmodel.UserModel);
                AutoMapper.Mapper.Map(account.FileAttachments, accountmodel.FileAttachmentModels);
            }
            return accountmodel;
        }

        public bool DeleteAccount(int accountId, int userId)
        {

            Account account = accountRepository.SingleOrDefault(u => u.AccountId == accountId && u.RecordDeleted == false);

            if (account != null)
            {


                //First delete all member accounts
                if (DeleteAllMemberAccount(accountId, userId))
                {
                    //Delete all account related items 
                    if (DeleteAllAccountItems(accountId, userId))
                    {
                        account.RecordDeleted = true;
                        account.ModifiedBy = userId;
                        account.ModifiedDate = DateTime.UtcNow;
                        accountRepository.Update(account);
                    }


                }



            }

            return true;

        }

        private bool DeleteAllMemberAccount(int parentAccountId, int userId)
        {
            List<Account> memberAccounts = accountRepository.GetAll(u => u.ParentAccountId == parentAccountId && u.RecordDeleted == false).ToList();

            for (int i = 0; i < memberAccounts.Count; i++)
            {

                if (memberAccounts[i] != null)
                {
                    if (DeleteAllAccountItems(memberAccounts[i].AccountId, userId))
                    {
                        memberAccounts[i].RecordDeleted = true;
                        memberAccounts[i].ModifiedBy = userId;
                        memberAccounts[i].ModifiedDate = DateTime.UtcNow;
                        accountRepository.Update(memberAccounts[i]);
                    }
                }

            }

            return true;
        }

        public bool DeleteMemberAccount(int memberAccountId, int parentAccountId, int userId)
        {
            Account memberAccount = accountRepository.SingleOrDefault(u => u.AccountId == memberAccountId && u.ParentAccountId == parentAccountId && u.RecordDeleted == false);



            if (memberAccount != null)
            {
                if (DeleteAllAccountItems(memberAccount.AccountId, userId))
                {
                    memberAccount.RecordDeleted = true;
                    memberAccount.ModifiedBy = userId;
                    memberAccount.ModifiedDate = DateTime.UtcNow;
                    accountRepository.Update(memberAccount);
                }
            }


            return true;
        }

        private bool DeleteAllAccountItems(int accountId, int userId)
        {

            //Attachements

            List<FileAttachment> accountFileAttachments = fileAttachmentRepository.GetAll(x => x.AccountId == accountId).ToList();

            for (int i = 0; i < accountFileAttachments.Count; i++)
            {
                accountFileAttachments[i].RecordDeleted = true;

            }


            //Activities
            List<TaskItem> accountTaskItems = taskItemRepository.GetAll(x => x.AssociatedModuleId == (int)Utility.Enums.Module.Account && x.AssociatedModuleValue == accountId).ToList();

            for (int i = 0; i < accountTaskItems.Count; i++)
            {
                accountTaskItems[i].RecordDeleted = true;
                accountTaskItems[i].ModifiedBy = userId;
                accountTaskItems[i].ModifiedDate = DateTime.UtcNow;
            }


            //SaleOrders
            List<SalesOrder> accountSalesOrders = salesOrderRepository.GetAll(x => x.AccountId == accountId).ToList();

            for (int i = 0; i < accountSalesOrders.Count; i++)
            {
                accountSalesOrders[i].RecordDeleted = true;
                accountSalesOrders[i].ModifiedBy = userId;
                accountSalesOrders[i].ModifiedDate = DateTime.UtcNow;
            }

            //Contacts

            List<Contact> accountContacts = contactRepository.GetAll(x => x.AccountId == accountId).ToList();

            for (int i = 0; i < accountContacts.Count; i++)
            {
                accountContacts[i].RecordDeleted = true;
                accountContacts[i].ModifiedBy = userId;
                accountContacts[i].ModifiedDate = DateTime.UtcNow;
            }


            //Cases
            List<AccountCase> accountCases = accountCaseRepository.GetAll(x => x.AccountId == accountId).ToList();

            for (int i = 0; i < accountCases.Count; i++)
            {
                accountCases[i].RecordDeleted = true;

                accountCases[i].ModifiedBy = userId;
                accountCases[i].ModifiedDate = DateTime.UtcNow;
            }


            fileAttachmentRepository.UpdateAll(accountFileAttachments);
            taskItemRepository.UpdateAll(accountTaskItems);
            salesOrderRepository.UpdateAll(accountSalesOrders);
            contactRepository.UpdateAll(accountContacts);
            accountCaseRepository.UpdateAll(accountCases);

            return true;

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

        public List<AccountModel> GetTaggedAccounts(int tagId, int companyId, int currentPage, int pageSize, ref int totalRecords)
        {
            List<AccountModel> listAccountModel = new List<AccountModel>();
            List<Account> listAccounts = new List<Account>();

            totalRecords = accountRepository.Count(x => x.AccountTags.Where(a => a.TagId == tagId).Count() > 0 && x.CompanyId == companyId && x.RecordDeleted == false);
            listAccounts = accountRepository.GetPagedRecords(x => x.AccountTags.Where(a => a.TagId == tagId).Count() > 0 && x.CompanyId == companyId && x.RecordDeleted == false, y => y.AccountName, currentPage > 0 ? currentPage : 1, pageSize).ToList();

            AutoMapper.Mapper.Map(listAccounts, listAccountModel);
            return listAccountModel;
        }


        public List<AccountModel> GetTaggedAccountSearchByName(string tagSearchName, int companyId, int currentPage, Int16 pageSize, ref int totalRecords)
        {
            List<AccountModel> listAccountModel = new List<AccountModel>();
            List<Account> listAccount = new List<Account>();

            //totalRecords = contactTagRepository.Count(x => x.TagId == tagId && x.Contact.CompanyId == companyId && x.Contact.RecordDeleted == false);

            //listContacts = contactTagRepository.GetPagedRecords(x => x.TagId == tagId && x.Contact.CompanyId == companyId && x.Contact.RecordDeleted == false, y => y.Contact.FirstName, currentPage, pageSize).Select(c => c.Contact).ToList();

            totalRecords = accountRepository.Count(x => x.AccountTags.Where(c => c.Tag.TagName.ToLower() == tagSearchName.ToLower()).Count() > 0 && x.CompanyId == companyId && x.RecordDeleted == false);

            listAccount = accountRepository.GetPagedRecords(x => x.AccountTags.Where(c => c.Tag.TagName.ToLower() == tagSearchName.ToLower()).Count() > 0 && x.CompanyId == companyId && x.RecordDeleted == false, y => y.AccountName, currentPage, pageSize).ToList();


            AutoMapper.Mapper.Map(listAccount, listAccountModel);

            return listAccountModel;
        }



        public List<AccountModel> GetAccounts(int companyId, int currentPage, int pageSize, ref int totalRecords)
        {
            List<AccountModel> listAccountModel = new List<AccountModel>();
            List<Account> listAccounts = new List<Account>();
            totalRecords = accountRepository.Count(x => x.CompanyId == companyId && x.RecordDeleted == false);
            listAccounts = accountRepository.GetPagedRecords(x => x.CompanyId == companyId && x.RecordDeleted == false, y => y.AccountName, currentPage > 0 ? currentPage : 1, pageSize).ToList();
            AutoMapper.Mapper.Map(listAccounts, listAccountModel);
            return listAccountModel;
        }
        public List<AccountModel> GetAccountsByUserId(int companyId, int userId, int? tagId, string tagName, int currentPage, int pageSize, ref int totalRecords)
        {
            List<AccountModel> listAccountModel = new List<AccountModel>();
            List<Account> listAccounts = accountRepository.GetAccountsByUserId(userId, companyId, tagId, tagName, currentPage, pageSize, out totalRecords);
            AutoMapper.Mapper.Map(listAccounts, listAccountModel);
            return listAccountModel;
        }

        public List<AccountModel> GetChildAccount(int companyId, int accountId)
        {
            List<AccountModel> listAccountModel = new List<AccountModel>();
            List<Account> listAccounts = new List<Account>();
            listAccounts = accountRepository.GetAll(r => r.CompanyId == companyId && r.ParentAccountId == accountId && r.RecordDeleted == false).ToList();
            AutoMapper.Mapper.Map(listAccounts, listAccountModel);
            return listAccountModel;
        }

    }
}
