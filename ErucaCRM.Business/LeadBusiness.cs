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
using System.Transactions;

namespace ErucaCRM.Business
{
    public class LeadBusiness : ILeadBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly LeadRepository leadRepository;
        private readonly TaskItemRepository taskItemRepository;
        private readonly StageRepository stageRepository;
        private readonly ILeadAuditBusiness leadAuditBusiness;
        private readonly AccountRespository accountRepository;
        private readonly ContactRepository contactRepository;
        private readonly ModuleRepository moduleRepository;
        private readonly FileAttachmentRepository fileAttachmentrepository;
        private readonly LeadStatusRepository leadStatusRepository;
        private readonly SalesOrderRepository salesOrderRepository;
        private readonly StageBusiness stageBusiness;
        private readonly RatingBusiness ratingBusiness;
        private readonly TagRepository tagRepository;
        private readonly LeadTagRepository leadTagRepository;
        private readonly LeadContactRepository leadContactRepository;
        private readonly AccountContactRepository accountcontactRepository;
        public LeadBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            leadRepository = new LeadRepository(unitOfWork);
            taskItemRepository = new TaskItemRepository(unitOfWork);
            stageRepository = new StageRepository(unitOfWork);
            leadAuditBusiness = new LeadAuditBusiness(unitOfWork);
            accountRepository = new AccountRespository(_unitOfWork);
            contactRepository = new ContactRepository(_unitOfWork);
            moduleRepository = new ModuleRepository(_unitOfWork);
            leadStatusRepository = new LeadStatusRepository(_unitOfWork);
            fileAttachmentrepository = new FileAttachmentRepository(_unitOfWork);
            stageBusiness = new StageBusiness(_unitOfWork);
            ratingBusiness = new RatingBusiness(_unitOfWork);
            leadContactRepository = new LeadContactRepository(_unitOfWork);
            accountcontactRepository = new AccountContactRepository(_unitOfWork);
            salesOrderRepository = new SalesOrderRepository(_unitOfWork);
            tagRepository = new TagRepository(_unitOfWork);
            leadTagRepository = new LeadTagRepository(_unitOfWork);
        }
        public LeadModel AddLead(LeadModel leadModel)
        {
            Lead lead = new Lead();
            LeadModel _leadmodel = new LeadModel();
            RatingModel ratingModel = new RatingModel();
            if (!leadModel.LeadSourceId.HasValue || leadModel.LeadSourceId.Value == 0)
            {
                leadModel.LeadSourceId = null;
                leadModel.LeadSourceModel = null;
            }
            if (!leadModel.LeadStatusId.HasValue || leadModel.LeadStatusId.Value == 0)
            {
                leadModel.LeadStatusId = null;
                leadModel.LeadStatusModel = null;
            }
            if (!leadModel.IndustryId.HasValue || leadModel.IndustryId.Value == 0)
            {
                leadModel.IndustryId = null;
                leadModel.IndustryModel = null;
            }

            AutoMapper.Mapper.Map(leadModel, lead);
            lead.RatingId = leadModel.RatingId > 0 ? leadModel.RatingId : null;
            lead.CreatedDate = DateTime.UtcNow;
            lead.CreatedBy = leadModel.CreatedBy;
            lead.ModifiedDate = DateTime.UtcNow;
            // StageModel stageModel = stageBusiness.GetIntialStage((int)leadModel.CompanyId);
            List<Stage> stageList = new List<Stage>();
            stageList = stageRepository.GetAll(r => r.CompanyId == lead.CompanyId && r.RecordDeleted == false).OrderBy(r => r.StageOrder).ToList();


            if (lead.AccountId > 0)
            {
                if (stageList.Count > 2)
                {
                    Stage stage = stageList.FirstOrDefault(r => r.StageOrder >= Constants.ACCOUNT_LEAD_STAGE);
                    lead.StageId = stage.StageId;
                    lead.RatingId = stage.Rating.RatingId;
                }
                else
                {
                    lead.StageId = stageList.SingleOrDefault(r => r.IsInitialStage == true).StageId;
                }

            }
            else
            {
                lead.StageId = stageList.SingleOrDefault(r => r.IsInitialStage == true).StageId;
            }
            lead.Address = ValidateAddress(lead.Address, leadModel.AddressModel);
            leadRepository.Insert(lead);
            AddLeadToDateAudit(lead, false);
            AutoMapper.Mapper.Map(lead, _leadmodel);
            _leadmodel.Rating = ratingModel;
            return _leadmodel;
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

        public IList<LeadModel> GetAllLeadByOwnerId(int companyId, int ownerId)
        {
            List<LeadModel> leadsModel = new List<LeadModel>();
            //List<Lead> leads = leadRepository.GetAll(l => l.CompanyId == companyId && l.LeadOwnerId == ownerId).ToList();
           // List<Lead> leads = leadRepository.GetAll(l => l.CompanyId == companyId && l.RecordDeleted == false && l.ClosingDate == null).ToList();
            List<SSP_LeadsListbyUserId_Result> leads = leadRepository.GetLeadsListByUserId(ownerId, companyId);
            AutoMapper.Mapper.Map(leads, leadsModel);
            return leadsModel;
        }

        public LeadModel GetLeadDetail(int leadId)
        {
            LeadModel leadModel = new LeadModel();
            Lead lead = leadRepository.SingleOrDefault(x => x.LeadId == leadId && x.RecordDeleted == false);

            AutoMapper.Mapper.Map(lead, leadModel);
            if (leadModel.Stage.IsLastStage == true)
            {
                if (leadModel.IsClosedWin != null)
                {
                    leadModel.Rating.Icons = CommonFunctions.GetRatingImage("", true, (bool)leadModel.IsClosedWin);
                }
            }

            if (lead != null)
            {
                if (lead.Address != null)
                {
                    AutoMapper.Mapper.Map(lead.Address, leadModel.AddressModel);
                    AutoMapper.Mapper.Map(lead.Address.Country, leadModel.AddressModel.CountryModel);
                }
                //AutoMapper.Mapper.Map(lead.FileAttachments, leadModel.FileAttachmentModels);
                //AutoMapper.Mapper.Map(lead.Industry, leadModel.IndustryModel);
                //AutoMapper.Mapper.Map(lead.LeadSource, leadModel.LeadSourceModel);
                //AutoMapper.Mapper.Map(lead.LeadStatu, leadModel.LeadStatusModel);
                //AutoMapper.Mapper.Map(lead.ProductLeadAssociations, leadModel.ProductLeadAssociationModels);
                //AutoMapper.Mapper.Map(lead.User, leadModel.UserModel);
                leadModel.FinalStageId = stageRepository.SingleOrDefault(r => r.IsLastStage == true && r.CompanyId == lead.CompanyId).StageId;
            }
            if (lead == null)
            {
                leadModel.RecordDeleted = true;
            }
            return leadModel;
        }

        public LeadModel GetLeadDetailForEdit(int leadId)
        {
            LeadModel leadModel = new LeadModel();
            Lead lead = leadRepository.SingleOrDefault(x => x.LeadId == leadId && x.RecordDeleted == false);

            AutoMapper.Mapper.Map(lead, leadModel);
            AutoMapper.Mapper.Map(lead.Address, leadModel.AddressModel);
            // AutoMapper.Mapper.Map(lead.Address.Country, leadModel.AddressModel.CountryModel);
            return leadModel;
        }

        //public IList<LeadModel> GetLeads(int companyId, int currentPage, int pageSize, ref int totalRecords)
        //{
        //    List<LeadModel> listLeadModel = new List<LeadModel>();
        //    List<Lead> listLeads = new List<Lead>();
        //    totalRecords = leadRepository.Count(x => x.CompanyId == companyId && x.RecordDeleted == false);
        //    listLeads = leadRepository.GetPagedRecords(x => x.CompanyId == companyId && x.RecordDeleted == false, y => y.Title, currentPage > 0 ? currentPage : 1, pageSize).ToList();
        //    AutoMapper.Mapper.Map(listLeads, listLeadModel);
        //    return listLeadModel;
        //}


        public void DeleteLead(LeadModel leadModel)
        {
            Lead lead = leadRepository.SingleOrDefault(where => where.LeadId == leadModel.LeadId && where.RecordDeleted == false);
            if (lead != null)
            {
               
              
               
                
                //De Associated lead Associated Quotes, products, Sales orders, Invoices & Tasks
                // De Associated lead Assiciated Quotes
                foreach (Quote objLeadQuote in lead.Quotes)
                {
                    objLeadQuote.LeadId = null;

                }

                // De Associated lead Assiciated Products
                foreach (ProductLeadAssociation objProductLeadAssociation in lead.ProductLeadAssociations)
                {
                    objProductLeadAssociation.LeadId = null;

                }

                // De Associated lead Associated SaleOrders
                //foreach (SalesOrder objLeadSalesOrder in lead.SalesOrders)
                //{
                //    objLeadSalesOrder.LeadId = null;

                //}

                // De Associated lead Associated invoice
                foreach (Invoice objLeadInvoice in lead.Invoices)
                {
                    objLeadInvoice.LeadId = null;

                }

                // De Associated lead Associated tasks
                List<TaskItem> listLeadTaskItems = taskItemRepository.GetAll(x => x.AssociatedModuleId == (int)ErucaCRM.Utility.Enums.Module.Lead && x.AssociatedModuleValue == leadModel.LeadId).ToList();

                foreach (TaskItem objTaskItems in listLeadTaskItems)
                {
                    objTaskItems.AssociatedModuleId = null;
                    objTaskItems.AssociatedModuleValue = null;
                }
                taskItemRepository.UpdateAll(listLeadTaskItems);
                lead.RecordDeleted = true;
                lead.ModifiedBy = leadModel.ModifiedBy;
                lead.ModifiedDate = DateTime.UtcNow;
                leadRepository.Update(lead);
                AddLeadToDateAudit(lead, false);

                foreach (var leadtag in lead.LeadTags.ToList())
                {
                    //var checklead = leadTagRepository.SingleOrDefault(x => x.TagId == leadtag.TagId && x.LeadId != leadtag.LeadId);
                    //if (checklead == null)
                    //{

                    //    Tag tag = new Tag();
                    //    tag = tagRepository.SingleOrDefault(x => x.TagId == leadtag.TagId);
                    //    //tag.RecordDeleted = true;
                    //    tagRepository.Update(tag);
                    //}

                    leadTagRepository.Delete(x => x.LeadId == leadtag.LeadId && x.LeadTagId == leadtag.LeadTagId);


                }
            }
        }

        public LeadModel UpdateLead(LeadModel leadModel)
        {
            LeadAuditModel leadAuditModel = new LeadAuditModel();

            if (!leadModel.LeadSourceId.HasValue || leadModel.LeadSourceId.Value == 0)
            {
                leadModel.LeadSourceId = null;
                leadModel.LeadSourceModel = null;
            }
            if (!leadModel.LeadStatusId.HasValue || leadModel.LeadStatusId.Value == 0)
            {
                leadModel.LeadStatusId = null;
                leadModel.LeadStatusModel = null;
            }
            if (!leadModel.IndustryId.HasValue || leadModel.IndustryId.Value == 0)
            {
                leadModel.IndustryId = null;
                leadModel.IndustryModel = null;

            }

            Lead lead = leadRepository.SingleOrDefault(x => x.LeadId == leadModel.LeadId && x.RecordDeleted == false);
            Lead leadForAudit = new Lead();
            if ((lead.ClosingDate.HasValue && lead.ClosingDate != leadModel.ClosingDate)
                ||
                (lead.Amount.HasValue && lead.Amount != leadModel.Amount) || (lead.Amount == null && leadModel.Amount.HasValue)
                )
            {
                AutoMapper.Mapper.Map(leadModel, leadForAudit);
                leadForAudit.StageId = lead.StageId;
                leadForAudit.CreatedBy = leadModel.ModifiedBy;

                AddLeadToDateAudit(leadForAudit, false);
            }
            if (!leadModel.LeadOwnerId.HasValue || leadModel.LeadOwnerId == 0)
            {
                leadModel.LeadOwnerId = lead.LeadOwnerId;
            }
            leadModel.CompanyId = lead.CompanyId;
            leadModel.StageId = leadModel.StageId == null || leadModel.StageId == 0 ? lead.StageId : leadModel.StageId;
            leadModel.CreatedBy = leadModel.CreatedBy == null ? lead.CreatedBy : leadModel.CreatedBy;
            leadModel.CreatedDate = lead.CreatedDate;
            AutoMapper.Mapper.Map(leadModel, lead);
            lead.RatingId = leadModel.RatingId > 0 ? leadModel.RatingId : null;
            lead.Address = ValidateAddress(lead.Address, leadModel.AddressModel);
            //if (lead.ContactId == 0)
            lead.ModifiedDate = DateTime.UtcNow;
            leadRepository.Update(lead);
            lead = leadRepository.SingleOrDefault(x => x.LeadId == lead.LeadId && x.RecordDeleted == false);
            AutoMapper.Mapper.Map(lead, leadModel);
            //Tag Association

            List<int> listAllExistingTagIds = new List<int>();
            List<int> listNewTagIds = new List<int>();
            List<int> listContactAssociatedTagIds = new List<int>();
            List<int> listDeleteTagIds = new List<int>();

            listContactAssociatedTagIds = lead.LeadTags.Select(x => x.TagId).ToList();

            if (!string.IsNullOrEmpty(leadModel.LeadTagIds))
            {
                string[] arrTagIds = leadModel.LeadTagIds.Split(',');
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


            if (!string.IsNullOrEmpty(leadModel.NewTagNames))
            {
                List<int> lisNewAddedTagIds = new List<int>();
                lisNewAddedTagIds = AddTagsToSystem(leadModel.NewTagNames, leadModel.CompanyId.Value, leadModel.CreatedBy.Value);

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
                LeadTag leadTag;
                for (int i = 0; i < listNewTagIds.Count; i++)
                {
                    leadTag = new LeadTag();
                    leadTag.LeadId = lead.LeadId;
                    leadTag.TagId = listNewTagIds[i];
                    leadTagRepository.Insert(leadTag);

                }


            }

            if (listDeleteTagIds.Count > 0)
                leadTagRepository.Delete(x => x.LeadId == lead.LeadId && listDeleteTagIds.Contains(x.TagId));

            return leadModel;
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

               Tag gettag = tagRepository.SingleOrDefault(t => t.TagName.ToLower() == newTagNames.ToLower() && (t.CompanyId == tag.CompanyId) && (t.RecordDeleted == false));
               if (gettag==null)
               {
                   tagRepository.Insert(tag);
                   lisNewTagIds.Add(tag.TagId);
               }
               else
               {
                   lisNewTagIds.Add(gettag.TagId);
               }
               

            }
            return lisNewTagIds;

        }



        public List<LeadModel> GetLeadDropDownList(int companyId)
        {
            List<Lead> leadList = leadRepository.GetAll(x => x.CompanyId == companyId && x.RecordDeleted == false).ToList();
            List<LeadModel> leadModelList = new List<LeadModel>();
            AutoMapper.Mapper.Map(leadList, leadModelList);
            leadModelList.Insert(0, new LeadModel { LeadId = 0, FirstName = Constants.CULTURE_SPECIFIC_DROPDOWNS_SELECT_OPTION });

            return leadModelList;

        }
        public LeadModel UpdateLead(int leadId, int stageId, int ratingId)
        {
            LeadModel leadModel = new LeadModel();
            leadModel.LeadId = leadId;
            leadModel.StageId = stageId;
            leadModel.RatingId = ratingId;
            leadModel = Update(leadModel);

            return leadModel;
        }
        public LeadModel UpdateLeadStage(LeadModel leadModel)
        {

            //LeadModel leadModel = new LeadModel();
            int leadId, stageId, modifyBy;
            bool isClosedWin;
            leadId = leadModel.LeadId;
            stageId = (int)leadModel.StageId;
            isClosedWin = (bool)leadModel.IsClosedWin;
            modifyBy = (int)leadModel.ModifiedBy;
            RatingModel ratingModel = new RatingModel();
            Stage stage = new Stage();
            stage = stageRepository.SingleOrDefault(r => r.StageId == stageId && r.RecordDeleted == false);
            leadModel.LeadId = leadId;
            leadModel.StageId = stageId;
            int? ratingId = stage.DefaultRatingId;
            leadModel.RatingId = ratingId;
            ratingModel = ratingBusiness.GetRatingByRatingId((int)(leadModel.RatingId == null ? 0 : leadModel.RatingId));
            bool? isLastStage = stage.IsLastStage == null ? false : stage.IsLastStage;

            leadModel = Update(leadModel, (bool)isLastStage);
            leadModel.Rating = ratingModel;
            if (isLastStage == true)
            {
                Lead lead = leadRepository.SingleOrDefault(r => r.LeadId == leadModel.LeadId);

                if (isClosedWin == true && lead.AccountId == null)
                {//Code for Converting lead to Account as the lead is closed with win status

                    using (TransactionScope tscope = new TransactionScope(TransactionScopeOption.RequiresNew))
                    {
                        int leadModuleId = Convert.ToInt32(Utility.Enums.Module.Lead);
                        int accountModuleId = Convert.ToInt32(Utility.Enums.Module.Account);
                        Account account = new Account();
                        account.AccountName = lead.LeadCompanyName ?? lead.Title;
                        account.AccountOwnerId = lead.LeadOwnerId.Value;
                        account.CompanyId = lead.CompanyId;
                        accountRepository.Insert(account);
                        account.CreatedBy = modifyBy;
                        account.CreatedDate = DateTime.UtcNow;
                        int accountId = account.AccountId;


                        //Code to Create Sale order automatically just after creating new order

                        SalesOrder saleaOrder = new SalesOrder();
                        saleaOrder.Subject = lead.Title;
                        saleaOrder.AccountId = account.AccountId;
                        saleaOrder.CompanyId = account.CompanyId;
                        saleaOrder.OwnerId = account.CreatedBy;
                        saleaOrder.OrderAmount = lead.Amount;
                        saleaOrder.GrandTotal = lead.Amount;
                        saleaOrder.CreatedBy = account.CreatedBy;
                        saleaOrder.CreatedDate = DateTime.UtcNow;
                        saleaOrder.ModifiedBy = account.ModifiedBy;
                        saleaOrder.ModifiedDate = DateTime.UtcNow;
                        salesOrderRepository.Insert(saleaOrder);

                        //salesOrderRepository
                        //code for saving leadcontact to account contact
                        List<AccountContact> accountContactList = new List<AccountContact>();
                        AccountContact accountcontact = new AccountContact();
                        ICollection<LeadContact> leadContacts = new List<LeadContact>();
                        leadContacts = lead.LeadContacts;
                        foreach (var leadcontact in leadContacts)
                        {
                            accountcontact.AccountId = accountId;
                            accountcontact.ContactId = leadcontact.ContactId;
                            accountcontact.CreatedDate = DateTime.UtcNow;
                            accountcontact.CreatedBy = modifyBy;
                            accountContactList.Add(accountcontact);
                        }

                        accountcontactRepository.InsertAll(accountContactList);
                        //code for saving leadDocument TO accountDocument
                        List<FileAttachment> fileAttacmentList = new List<FileAttachment>();
                        fileAttacmentList = fileAttachmentrepository.GetAll(r => r.LeadId == leadId).ToList();
                        foreach (FileAttachment file in fileAttacmentList)
                        {
                            file.AccountId = accountId;
                        }
                        fileAttachmentrepository.UpdateAll(fileAttacmentList);
                        //Code for Converting LeadActivity to account Activity
                        List<TaskItem> activityList = new List<TaskItem>();
                        activityList = taskItemRepository.GetAll(r => r.AssociatedModuleId == leadModuleId && r.AssociatedModuleValue == leadId).ToList();
                        List<TaskItem> listActivityToAdd = new List<TaskItem>();
                        foreach (TaskItem task in activityList)
                        {
                            TaskItem taskObj = new TaskItem();
                            taskObj.CompanyId = task.CompanyId;
                            taskObj.AssociatedModuleId = accountModuleId;
                            taskObj.AssociatedModuleValue = accountId;
                            taskObj.Description = task.Description;
                            taskObj.DueDate = task.DueDate;
                            taskObj.EndDate = task.EndDate;
                            taskObj.CreatedDate = task.CreatedDate;
                            taskObj.CreatedBy = task.CreatedBy;
                            taskObj.Subject = task.Subject;
                            taskObj.Status = task.Status;
                            taskObj.OwnerId = task.OwnerId;
                            listActivityToAdd.Add(taskObj);
                        }
                        taskItemRepository.InsertAll(listActivityToAdd);
                        //Associating Lead TO NewLy created account
                        lead.AccountId = accountId;
                        lead.ClosingDate = DateTime.UtcNow;
                        lead.IsLeadConvertedToAccount = true;
                        lead.RatingId = ratingId;
                        lead.StageId = stageId;
                        lead.ModifiedBy = modifyBy;
                        lead.ModifiedDate = DateTime.UtcNow;
                        lead.IsClosed = true;
                        leadRepository.Update(lead);
                        tscope.Complete();
                    }
                    leadModel.Rating.Icons = CommonFunctions.GetRatingImage("", true, true);
                }
                else
                {
                    leadModel.Rating.Icons = CommonFunctions.GetRatingImage("", true, false);
                    lead.ClosingDate = DateTime.UtcNow;
                    lead.IsLeadConvertedToAccount = false;
                    lead.RatingId = ratingId;
                    lead.StageId = stageId;
                    lead.ModifiedBy = modifyBy;
                    lead.ModifiedDate = DateTime.UtcNow;
                    lead.LeadStatusId = (int?)Utility.Enums.LeadStatus.Lost;    //5 is set for lost  lead                         
                    lead.IsClosed = true;
                    leadRepository.Update(lead);
                }


            }
            return leadModel;
        }



        public LeadModel UpdateLeadRating(int leadId, int ratingId, int ModifiedBy)
        {
            LeadModel leadModel = new LeadModel();
            leadModel.LeadId = leadId;
            leadModel.RatingId = ratingId;
            leadModel.ModifiedBy = ModifiedBy;
            leadModel = Update(leadModel);

            return leadModel;
        }

        private LeadModel Update(LeadModel leadModel, bool isLastStage = false)
        {
            bool isUpdate = false;
            Lead lead = leadRepository.SingleOrDefault(x => x.LeadId == leadModel.LeadId && x.RecordDeleted == false);
            StageModel stage = stageBusiness.GetIntialStage((int)lead.CompanyId);
            if (stage.DefaultRatingId == null)
            {
                lead.RatingId = leadModel.RatingId;
            }
            if (lead != null)
            {
                if (leadModel.StageId != null && leadModel.StageId > 0)
                {
                    lead.StageId = leadModel.StageId;
                    isUpdate = true;
                }
                if (leadModel.RatingId != null && leadModel.RatingId > 0)
                {
                    lead.RatingId = leadModel.RatingId;
                    isUpdate = true;
                }
                if (isUpdate)
                {
                    lead.IsClosedWin = leadModel.IsClosedWin;
                    lead.ModifiedBy = leadModel.ModifiedBy;
                    AddLeadToDateAudit(lead, isLastStage);
                    lead.ModifiedBy = leadModel.ModifiedBy;
                    lead.ModifiedDate = DateTime.UtcNow;
                    leadRepository.Update(lead);
                }
            }
            leadModel = new LeadModel();
            AutoMapper.Mapper.Map(lead, leadModel);
            leadModel.Rating.Icons = (leadModel.RatingId == null) ?
              CommonFunctions.GetRatingImage("", leadModel.Stage.IsLastStage == null ? false : (bool)leadModel.Stage.IsLastStage, leadModel.IsClosedWin == null ? false : (bool)leadModel.IsClosedWin) : CommonFunctions.GetRatingImage(leadModel.Rating.Icons, leadModel.Stage.IsLastStage == null ? false : (bool)leadModel.Stage.IsLastStage, leadModel.IsClosedWin == null ? false : (bool)leadModel.IsClosedWin);
            return leadModel;
        }

        private void AddLeadToDateAudit(Lead lead, bool islastStage = false)
        {
            LeadAuditModel leadAuditModel = new LeadAuditModel();
            leadAuditModel = leadAuditBusiness.GetLeadAuditByLeadId(lead.LeadId);
            if (leadAuditModel.LeadId != null)
            {
                if (lead.RecordDeleted == true)
                {
                    leadAuditModel.CreatedBy = lead.ModifiedBy;
                    leadAuditModel.ActivityType = (int)Enums.ActivityType.LeadDeleted;
                    leadAuditModel.ClosingDate = DateTime.UtcNow;
                    leadAuditModel.ToDate = DateTime.UtcNow;
                }
                else if (lead.StageId != leadAuditModel.StageId && lead.StageId != null)
                {
                    leadAuditModel.ToDate = DateTime.UtcNow;
                    leadAuditBusiness.Update(leadAuditModel);
                    AutoMapper.Mapper.Map(lead, leadAuditModel);
                    leadAuditModel.ToDate = null;
                    leadAuditModel.CreatedBy = lead.ModifiedBy;
                    leadAuditModel.ActivityType = (int)Enums.ActivityType.LeadStageChanged;
                    if (islastStage)
                        leadAuditModel.ClosingDate = DateTime.UtcNow;
                    leadAuditModel.FromDate = DateTime.UtcNow;
                }
                else if (lead.RatingId != leadAuditModel.RatingId || lead.Amount != leadAuditModel.Amount)
                {
                    if (lead.RatingId != leadAuditModel.RatingId && lead.RatingId != null&&lead.RatingId!=0)
                        leadAuditModel.ActivityType = (int)Enums.ActivityType.LeadRatingChanged;
                    else if (lead.Amount != leadAuditModel.Amount && lead.Amount != null)
                        leadAuditModel.ActivityType = (int)Enums.ActivityType.LeadAmountChanged;
                    AutoMapper.Mapper.Map(lead, leadAuditModel);
                    leadAuditModel.CreatedBy = lead.ModifiedBy;
                }
            }
            else
            {
                AutoMapper.Mapper.Map(lead, leadAuditModel);
                leadAuditModel.ActivityType = (int)Enums.ActivityType.LeadAdded;
                leadAuditModel.FromDate = DateTime.UtcNow;
            }
            leadAuditModel.CreatedDate = DateTime.UtcNow;
            leadAuditBusiness.Add(leadAuditModel);
        }

        public List<LeadModel> GetLeads(int userId, int companyId, int stageId, int tagId, string leadName, int currentPage, int pageSize, ref int totalRecords)
        {
            List<LeadModel> listLeadModel = new List<LeadModel>();
            List<Lead> listLeads = new List<Lead>();
            listLeads = leadRepository.GetLeadsByUserId(userId, companyId, stageId, currentPage, tagId, leadName, pageSize, out totalRecords);
            //totalRecords = leadRepository.Count(x => x.CompanyId == companyId && x.StageId == stageId && x.RecordDeleted == false);
            //listLeads = leadRepository.GetPagedRecords(x => x.CompanyId == companyId && x.StageId == stageId && x.RecordDeleted == false, y => y.FirstName, currentPage > 0 ? currentPage : 1, pageSize).Select(c => new Lead { LeadId = c.LeadId, FirstName = c.FirstName, LastName = c.LastName, StageId = c.StageId, RatingId = c.RatingId, ClosingDate = c.ClosingDate, LeadCompanyName = c.LeadCompanyName, Title = c.Title }).ToList();
            AutoMapper.Mapper.Map(listLeads, listLeadModel);
            listLeadModel.ForEach(x => x.Rating.Icons = (x.RatingId == null) ?
                CommonFunctions.GetRatingImage("", x.Stage.IsLastStage == null ? false : (bool)x.Stage.IsLastStage, x.IsClosedWin == null ? false : (bool)x.IsClosedWin) : CommonFunctions.GetRatingImage(x.Rating.Icons, x.Stage.IsLastStage == null ? false : (bool)x.Stage.IsLastStage, x.IsClosedWin == null ? false : (bool)x.IsClosedWin));
            return listLeadModel;
        }

        public List<LeadModel> GetLeadsWeb(int userId, int companyId, int stageId, string tagName, string leadName, int currentPage, int pageSize, ref int totalRecords)
        {
            List<LeadModel> listLeadModel = new List<LeadModel>();
            List<Lead> listLeads = new List<Lead>();
            listLeads = leadRepository.GetLeadsByUserIdWeb(userId, companyId, stageId, currentPage, tagName, leadName, pageSize, false, 0, out totalRecords);
            //totalRecords = leadRepository.Count(x => x.CompanyId == companyId && x.StageId == stageId && x.RecordDeleted == false);
            //listLeads = leadRepository.GetPagedRecords(x => x.CompanyId == companyId && x.StageId == stageId && x.RecordDeleted == false, y => y.FirstName, currentPage > 0 ? currentPage : 1, pageSize).Select(c => new Lead { LeadId = c.LeadId, FirstName = c.FirstName, LastName = c.LastName, StageId = c.StageId, RatingId = c.RatingId, ClosingDate = c.ClosingDate, LeadCompanyName = c.LeadCompanyName, Title = c.Title }).ToList();
            AutoMapper.Mapper.Map(listLeads, listLeadModel);
            listLeadModel.ForEach(x => x.Rating.Icons = (x.RatingId == null) ?
                CommonFunctions.GetRatingImage("", x.Stage.IsLastStage == null ? false : (bool)x.Stage.IsLastStage, x.IsClosedWin == null ? false : (bool)x.IsClosedWin) : CommonFunctions.GetRatingImage(x.Rating.Icons, x.Stage.IsLastStage == null ? false : (bool)x.Stage.IsLastStage, x.IsClosedWin == null ? false : (bool)x.IsClosedWin));
            return listLeadModel;
        }

        public List<TagModel> GetLeadTags(int companyId)
        {
            List<TagModel> lisTagModels = new List<TagModel>();

            List<Tag> listTags = tagRepository.GetAll(companyId);
            AutoMapper.Mapper.Map(listTags, lisTagModels);

            return lisTagModels;


        }

        public List<LeadModel> GetLeadsbyStageIdWeb(int userId, int companyId, int stageId, int currentPage, string tagName, string LeadName, int pageSize, bool IsLoadMore, int leadId, ref int totalRecords)
        {
            List<LeadModel> listLeadModel = new List<LeadModel>();
            List<Lead> listLeads = new List<Lead>();
            listLeads = leadRepository.GetLeadsByUserIdWeb(userId, companyId, stageId, currentPage, tagName, LeadName, pageSize, IsLoadMore, leadId, out totalRecords);
            AutoMapper.Mapper.Map(listLeads, listLeadModel);
            listLeadModel.ForEach(x => x.Rating.Icons = (x.RatingId == null) ?
            CommonFunctions.GetRatingImage("", x.Stage.IsLastStage == null ? false : (bool)x.Stage.IsLastStage, x.IsClosedWin == null ? false : (bool)x.IsClosedWin) : CommonFunctions.GetRatingImage(x.Rating.Icons, x.Stage.IsLastStage == null ? false : (bool)x.Stage.IsLastStage, x.IsClosedWin == null ? false : (bool)x.IsClosedWin));
            return listLeadModel;
        }


        public List<LeadModel> GetLeadsbyStageId(int userId, int companyId, int stageId, int currentPage, int tagId, string LeadName, int pageSize, ref int totalRecords)
        {
            List<LeadModel> listLeadModel = new List<LeadModel>();
            List<Lead> listLeads = new List<Lead>();
            listLeads = leadRepository.GetLeadsByUserId(userId, companyId, stageId, currentPage, tagId, LeadName, pageSize, out totalRecords);
            //totalRecords = leadRepository.Count(x => x.CompanyId == companyId && x.StageId == stageId && x.RecordDeleted == false);
            //listLeads = leadRepository.GetPagedRecords(x => x.CompanyId == companyId && x.StageId == stageId && x.RecordDeleted == false, y => y.Title, currentPage > 0 ? currentPage : 1, pageSize).Select(c => new Lead { LeadId = c.LeadId,StageId = c.StageId, RatingId = c.RatingId, ClosingDate = c.ClosingDate, LeadCompanyName = c.LeadCompanyName, Title = c.Title }).ToList();
            AutoMapper.Mapper.Map(listLeads, listLeadModel);
            return listLeadModel;
        }
        public List<LeadStagesJSONModel> GetLeadsByStageGroup(int userId, int companyId, int currentPage, int pageSize, ref int totalRecords)
        {
            int totalRecord = 0;
            List<LeadStagesJSONModel> leadJsonModelList = new List<LeadStagesJSONModel>();
            LeadStagesJSONModel leadJsonModel;
            List<Stage> stageList = stageRepository.GetAll(c => c.CompanyId == companyId && c.RecordDeleted == false).OrderBy(c => c.StageOrder).ToList();
            
            foreach (Stage stage in stageList)
            {
                leadJsonModel = new LeadStagesJSONModel();
               // totalRecords = 0;
                //leadJsonModel.Leads = null;
                leadJsonModel.StageId = stage.StageId;
                leadJsonModel.IsInitialStage = stage.IsInitialStage ?? false;
                leadJsonModel.IsLastStage = stage.IsLastStage ?? false;
                leadJsonModel.StageName = stage.StageName;
                leadJsonModel.TotalRecords = totalRecord;
                leadJsonModelList.Add(leadJsonModel);
            }

            return leadJsonModelList;
        }
        /// <summary>
        /// for mobile fetching stages with their leads and related information
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="companyId"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public List<LeadStagesJSONModel> GetLeadsByStageGroupformobile(int userId, int companyId, int currentPage, int pageSize, ref int totalRecords)
        {
            int LeadId = 0;
            int tagId=0;
            string LeadName=null;
            int totalRecord = 0;
            List<LeadStagesJSONModel> leadJsonModelList = new List<LeadStagesJSONModel>();
            LeadStagesJSONModel leadJsonModel;
            List<Stage> stageList = stageRepository.GetAll(c => c.CompanyId == companyId && c.RecordDeleted == false).OrderBy(c => c.StageOrder).ToList();

            foreach (Stage stage in stageList)
            {
                leadJsonModel = new LeadStagesJSONModel();
                
                // totalRecords = 0;
                leadJsonModel.Leads = GetLeadsbyStageIdWeb(userId, companyId, stage.StageId, currentPage, null, LeadName, pageSize, false, LeadId, ref totalRecords).ToList();
                foreach (LeadModel lead in leadJsonModel.Leads)
                {
                    TaskItemBusiness taskItemBusiness = new TaskItemBusiness(unitOfWork);
                    List<TaskItemModel> taskItemModel = new List<TaskItemModel>();
                    taskItemModel = taskItemBusiness.GetLeadTasks(companyId, lead.LeadId, 1, 10, ref totalRecords).ToList();
                    //string sortColumnName=" ";
                    //string sortDir=" ";
                    //taskItemModel = taskItemBusiness.GetTasks(userId,companyId, currentPage, 10, ref totalRecords, "", "");
                    leadJsonModel.Leads.Find(x=>x.LeadId==lead.LeadId).TaskItems=taskItemModel;

                }
                leadJsonModel.StageId = stage.StageId;
                leadJsonModel.IsInitialStage = stage.IsInitialStage ?? false;
                leadJsonModel.IsLastStage = stage.IsLastStage ?? false;
                leadJsonModel.StageName = stage.StageName;
                leadJsonModel.TotalRecords = totalRecord;
                leadJsonModelList.Add(leadJsonModel);
            }
            
            return leadJsonModelList;
        }
        public List<GetLeadAnalyticData_Result> GetLeadAnalyticData(string Interval, int CompanyId)
        {
            List<GetLeadAnalyticData_Result> data = leadRepository.GetLeadAnalyticData(Interval, CompanyId);
            return data;
        }

        public List<YearWiseLeadModel> GetYearWiseLeadCount(int CompanyId)
        {
            List<SSP_GetYearWiseLeadCount_Result> data = leadRepository.GetYearWiseLeadCount(CompanyId);
            List<YearWiseLeadModel> yearLeads = new List<YearWiseLeadModel>();
            AutoMapper.Mapper.Map(data, yearLeads);

            return yearLeads;
        }
        public List<YearWiseLeadModel> GetMonthWiseLeadCount(int CompanyId)
        {
            List<ssp_GetMonthWiseLeadCount_Result> data = leadRepository.GetMonthWiseLeadCount(CompanyId);
            List<YearWiseLeadModel> yearLeads = new List<YearWiseLeadModel>();
            AutoMapper.Mapper.Map(data, yearLeads);

            return yearLeads;
        }

        public List<YearWiseLeadModel> GetWeekWiseLeadCount(int CompanyId)
        {
            List<ssp_GetWeekWiseLeadCount_Result> data = leadRepository.GetWeekWiseLeadCount(CompanyId);
            List<YearWiseLeadModel> weeklyLeads = new List<YearWiseLeadModel>();
            AutoMapper.Mapper.Map(data, weeklyLeads);

            return weeklyLeads;
        }



        public List<LeadModel> GetTaggedLead(int tagId, int companyId, int currentPage, Int16 pageSize, ref int totalRecords)
        {
            List<LeadModel> listLeadModel = new List<LeadModel>();
            List<Lead> listLead = new List<Lead>();


            totalRecords = leadRepository.Count(x => x.LeadTags.Where(c => c.TagId == tagId).Count() > 0 && x.CompanyId == companyId && x.RecordDeleted == false);

            listLead = leadRepository.GetPagedRecords(x => x.LeadTags.Where(c => c.TagId == tagId).Count() > 0 && x.CompanyId == companyId && x.RecordDeleted == false, y => y.Title, currentPage, pageSize).ToList();


            AutoMapper.Mapper.Map(listLead, listLeadModel);

            return listLeadModel;
        }



        public LeadByStarRatingPercentageModel GetLeadsByStarRatingPercentage(int CompanyId)
        {
            List<SSP_LeadsByRatingAndStages_Result> data = leadRepository.GetLeadsByStarRatingPercentage(CompanyId);
            //  List<string> StageName = new List<string>();
            string[] StageName = data.Select(x => x.stagename).Distinct().ToArray();
            int[] RatingConstant = data.Select(x => x.ratingconstant).Distinct().ToArray();
            List<int[]> ratingArray = new List<int[]>();
            List<String> ratingNames = new List<string>();

            for (int i = 1; i <= RatingConstant.Length; i++)
            {
                int[] leads = data.Where(x => x.ratingconstant == i).Select(x => x.totalleads).ToArray();
                ratingArray.Add(leads);

                ratingNames.Add(GetRatingText(i));
            }

            LeadByStarRatingPercentageModel starRatingPercentageModel = new LeadByStarRatingPercentageModel();
            starRatingPercentageModel.StageName = StageName;
            starRatingPercentageModel.RatingName = ratingNames.ToArray();
            starRatingPercentageModel.RatingArray = ratingArray;
            return starRatingPercentageModel;
        }

        private string GetRatingText(int ratingConstant)
        {
            switch (ratingConstant)
            {
                case 1:
                    return CommonFunctions.GetGlobalizedLabel("DashBoard", "OneStar");
                case 2:
                    return CommonFunctions.GetGlobalizedLabel("DashBoard", "TwoStar");
                case 3:
                    return CommonFunctions.GetGlobalizedLabel("DashBoard", "ThreeStar");
                case 4:
                    return CommonFunctions.GetGlobalizedLabel("DashBoard", "FourStar");
                case 5:
                    return CommonFunctions.GetGlobalizedLabel("DashBoard", "FiveStar");
                default:
                    return "";


            }

        }


    }


}
