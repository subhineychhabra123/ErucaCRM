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
    public class TaskItemBusiness : ITaskItemBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly TaskItemRepository taskRepository;
        private readonly TaskStatusRepository taskStatusRepository;
        private readonly ContactRepository contactRepository;
        private readonly LeadRepository leadRepository;
        private readonly AccountRespository accountRepository;
        private readonly UserRepository userRepository;
        public TaskItemBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            taskRepository = new TaskItemRepository(unitOfWork);
            taskStatusRepository = new TaskStatusRepository(unitOfWork);
            contactRepository = new ContactRepository(unitOfWork);
            leadRepository = new LeadRepository(unitOfWork);
            accountRepository = new AccountRespository(unitOfWork);
        }

        public TaskItemModel AddTask(TaskItemModel taskModel)
        {
            TaskItem task = new ErucaCRM.Repository.TaskItem();
            AutoMapper.Mapper.Map(taskModel, task);
            if (taskModel.TaskId > 0)
            {
                TaskItem taskItem = taskRepository.SingleOrDefault(r => r.TaskId == taskModel.TaskId);
                taskItem.OwnerId = taskModel.OwnerId;
                taskItem.Description = taskModel.Description;
                taskItem.PriorityId = taskModel.PriorityId;
                taskItem.Status = taskModel.Status;
                taskItem.AssociatedModuleId = taskModel.AssociatedModuleId;
                taskItem.AssociatedModuleValue = taskModel.AssociatedModuleValue;
                taskItem.Subject = taskModel.Subject;
                taskItem.DueDate = taskModel.DueDate;
                taskItem.ModifiedBy = taskModel.ModifiedBy;
                taskItem.ModifiedDate = taskModel.ModifiedDate;
                taskItem.AudioFileDuration = taskModel.AudioFileDuration;
                taskItem.AudioFileName = taskModel.AudioFileName;
                taskRepository.Update(taskItem);
            }
            else
                taskRepository.Insert(task);
            taskModel.TaskId = task.TaskId;
            return taskModel;
        }

        public IList<TaskStatusModel> GetTaskStatus()
        {
            IList<TaskStatusModel> taskStatusModel = new List<TaskStatusModel>();
            IList<TaskStatu> taskStatus = taskStatusRepository.GetAll().ToList();
            AutoMapper.Mapper.Map(taskStatus, taskStatusModel);
            return taskStatusModel;
        }

        public TaskItemModel GetTask(int taskId)
        {
            TaskItemModel taskModel = new TaskItemModel();


            TaskItem task = taskRepository.GetAll(t => t.TaskId == taskId && t.RecordDeleted == false).FirstOrDefault();
            AutoMapper.Mapper.Map(task, taskModel);
            return taskModel;
        }

        public String GetTaskOwnerName(int ownerId)
        {
            UserRepository userRepo = new UserRepository(unitOfWork);
            User owner = userRepo.GetAll(u => u.UserId == ownerId && u.RecordDeleted == false).FirstOrDefault();
            return owner.FirstName + " " + owner.LastName;

        }

        //public IList<TaskItemModel> GetTasks(int userId, int companyId, int currentPage, int pageSize, ref int totalRecords)
        //{
        //    List<TaskItemModel> listTaskModel = new List<TaskItemModel>();
        //    List<TaskItem> listTasks = new List<TaskItem>();

        //    listTasks = taskRepository.GetActivitiesByUserId(userId, companyId, currentPage, pageSize, out totalRecords);
        //    AutoMapper.Mapper.Map(listTasks, listTaskModel);
        //    return listTaskModel;
        //}
    //    pankaj pandey
        public List<TaskItemModel> GetTasks(int userId, int companyId, int currentPage, int pageSize, ref int totalRecords, string sortColumnName, string sortDir )
        {
            List<TaskItemModel> listTaskModel = new List<TaskItemModel>();
            List<SSP_GetActivitiesByUserId_Result> listTasks = new List<SSP_GetActivitiesByUserId_Result>();

            listTasks = taskRepository.GetActivitiesByUserId(userId, companyId, currentPage, pageSize, out totalRecords,sortColumnName,sortDir);
            AutoMapper.Mapper.Map(listTasks, listTaskModel);
            return listTaskModel;
        }

        public List<DashboarActivitiesModel> GetTasks(int userId, int companyId, int currentPage, int pageSize, string Filter, ref int totalRecords)
        {
            List<DashboarActivitiesModel> listTaskModel = new List<DashboarActivitiesModel>();
            List<SSP_GetActivities_Result> listTasks = new List<SSP_GetActivities_Result>();
            listTasks = taskRepository.GetActivities(userId, companyId, currentPage, pageSize, Filter, out totalRecords);
            AutoMapper.Mapper.Map(listTasks, listTaskModel);
            return listTaskModel;
        }
        public IList<TaskItemModel> GetDashBoardTasks(int companyId, int userId)
        {
            List<TaskItemModel> listTaskModel = new List<TaskItemModel>();
            List<TaskItem> listTasks = new List<TaskItem>();

            DateTime dt = DateTime.UtcNow.Date;

            listTasks = taskRepository.GetAll(x => x.CompanyId == companyId && x.OwnerId == userId && x.RecordDeleted == false && x.DueDate >= dt).OrderByDescending(y => y.TaskId).ThenBy(p => p.DueDate).Skip(0).Take(5).ToList();
            AutoMapper.Mapper.Map(listTasks, listTaskModel);
            return listTaskModel;
        }
        public IList<TaskItemModel> GetContactTasks(int companyId, int contactID, int currentPage, int pageSize, ref int totalRecords)
        {
            List<TaskItemModel> listTaskModel = new List<TaskItemModel>();
            List<TaskItem> listTasks = new List<TaskItem>();
            int contactModuleID = Convert.ToInt32(Utility.Enums.Module.Contact);
            totalRecords = taskRepository.Count(x => x.CompanyId == companyId && x.RecordDeleted == false && x.AssociatedModuleId == contactModuleID && x.AssociatedModuleValue == contactID && x.RecordDeleted == false);
            listTasks = taskRepository.GetPagedRecords(x => x.CompanyId == companyId && x.RecordDeleted == false && x.AssociatedModuleId == contactModuleID && x.AssociatedModuleValue == contactID && x.RecordDeleted == false, y => y.Subject, currentPage > 0 ? currentPage : 1, pageSize).ToList();
            AutoMapper.Mapper.Map(listTasks, listTaskModel);
            return listTaskModel;
        }

        public IList<TaskItemModel> GetAccountTasks(int companyId, int accountId, int currentPage, int pageSize, ref int totalRecords)
        {
            List<TaskItemModel> listTaskModel = new List<TaskItemModel>();
            List<TaskItem> listTasks = new List<TaskItem>();
            int accountModuleID = Convert.ToInt32(Utility.Enums.Module.Account);
            totalRecords = taskRepository.Count(x => x.CompanyId == companyId && x.RecordDeleted == false && x.AssociatedModuleId == accountModuleID && x.AssociatedModuleValue == accountId && x.RecordDeleted == false);
            listTasks = taskRepository.GetPagedRecords(x => x.CompanyId == companyId && x.RecordDeleted == false && x.AssociatedModuleId == accountModuleID && x.AssociatedModuleValue == accountId && x.RecordDeleted == false, y => y.Subject, currentPage > 0 ? currentPage : 1, pageSize).ToList();
            AutoMapper.Mapper.Map(listTasks, listTaskModel);
            return listTaskModel;
        }

        public String GetTaskAssociatedPersonName(int moduleId, int moduleValue)
        {
            string taskAssociatedPersonName = "";

            if (moduleId == Convert.ToInt32(Utility.Enums.Module.Lead))
            {
                Lead objLead = leadRepository.SingleOrDefault(x => x.LeadId == moduleValue && x.RecordDeleted == false);

                if (objLead != null)
                {
                    taskAssociatedPersonName = objLead.Title;
                }
            }
            else if (moduleId == Convert.ToInt32(Utility.Enums.Module.Contact))
            {
                Contact objContact = contactRepository.SingleOrDefault(x => x.ContactId == moduleValue && x.RecordDeleted == false);

                if (objContact != null)
                {
                    taskAssociatedPersonName = objContact.FirstName + " " + objContact.LastName;
                }

            }
            else if (moduleId == Convert.ToInt32(Utility.Enums.Module.Account))
            {
                Account objAccount = accountRepository.SingleOrDefault(x => x.AccountId == moduleValue && x.RecordDeleted == false);
                if (objAccount != null)
                {
                    taskAssociatedPersonName = objAccount.AccountName;
                }
            }

            return taskAssociatedPersonName;

        }

        public bool DeleteTaskItem(int taskId, int userId)
        {

            TaskItem taskitem = taskRepository.SingleOrDefault(x => x.TaskId == taskId && x.RecordDeleted == false);
            if (taskitem != null)
            {
                taskitem.RecordDeleted = true;
                taskitem.AssociatedModuleId = null;
                taskitem.AssociatedModuleValue = null;
                taskitem.ModifiedBy = userId;
                taskitem.ModifiedDate = DateTime.UtcNow;
                taskRepository.Update(taskitem);
                return true;
            }
            else
                return false;
        }



        public bool DeleteTaskItem(int taskId, int leadId, int userId)
        {

            TaskItem taskitem = taskRepository.SingleOrDefault(x => x.TaskId == taskId && x.AssociatedModuleValue == leadId && x.RecordDeleted == false);
            if (taskitem != null)
            {
                taskitem.RecordDeleted = true;
                taskitem.AssociatedModuleId = null;
                taskitem.AssociatedModuleValue = null;
                taskitem.ModifiedBy = userId;
                taskitem.ModifiedDate = DateTime.UtcNow;
                taskRepository.Update(taskitem);
                return true;
            }
            else
                return false;
        }

        public bool DeleteAccountTaskItem(int taskId, int accountId, int userId)
        {

            TaskItem taskitem = taskRepository.SingleOrDefault(x => x.TaskId == taskId && x.AssociatedModuleValue == accountId && x.RecordDeleted == false);
            if (taskitem != null)
            {
                taskitem.RecordDeleted = true;
                taskitem.AssociatedModuleId = null;
                taskitem.AssociatedModuleValue = null;
                taskitem.ModifiedBy = userId;
                taskitem.ModifiedDate = DateTime.UtcNow;
                taskRepository.Update(taskitem);
                return true;
            }
            else
                return false;
        }
        public IList<TaskItemModel> GetAccountCaseTasks(int companyId, int accountCaseId, int currentPage, int pageSize, ref int totalRecords)
        {
            List<TaskItemModel> listTaskModel = new List<TaskItemModel>();
            List<TaskItem> listTasks = new List<TaskItem>();
            int accountCaseModuleID = Convert.ToInt32(Utility.Enums.Module.AccountCase);
            totalRecords = taskRepository.Count(x => x.CompanyId == companyId && x.RecordDeleted == false && x.AssociatedModuleId == accountCaseModuleID && x.AssociatedModuleValue == accountCaseId && x.RecordDeleted == false);
            listTasks = taskRepository.GetPagedRecords(x => x.CompanyId == companyId && x.RecordDeleted == false && x.AssociatedModuleId == accountCaseModuleID && x.AssociatedModuleValue == accountCaseId && x.RecordDeleted == false, y => y.Subject, currentPage > 0 ? currentPage : 1, pageSize).ToList();
            AutoMapper.Mapper.Map(listTasks, listTaskModel);
            return listTaskModel;
        }

        public IList<TaskItemModel> GetLeadTasks(int companyId, int LeadId, int currentPage, int pageSize, ref int totalRecords)
        {
            List<TaskItemModel> listTaskModel = new List<TaskItemModel>();
            List<TaskItem> listTasks = new List<TaskItem>();
            TaskItem taskItem=new TaskItem();
            int leadModuleID = Convert.ToInt32(Utility.Enums.Module.Lead);
            totalRecords = taskRepository.Count(x => x.CompanyId == companyId && x.RecordDeleted == false && x.AssociatedModuleId == leadModuleID && x.AssociatedModuleValue == LeadId && x.RecordDeleted == false);
            listTasks = taskRepository.GetPagedRecords(x => x.CompanyId == companyId && x.RecordDeleted == false && x.AssociatedModuleId == leadModuleID && x.AssociatedModuleValue == LeadId && x.RecordDeleted == false, y => y.CreatedDate, currentPage > 0 ? currentPage : 1, pageSize).ToList();
           // var ownerIdList=listTasks.Select(x => x.OwnerId).ToList();
            AutoMapper.Mapper.Map(listTasks, listTaskModel);

            return listTaskModel;
        }
    }



}
