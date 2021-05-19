using ErucaCRM.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Business.Interfaces
{
    public interface ITaskItemBusiness
    {
        TaskItemModel AddTask(TaskItemModel taskModel);
        IList<TaskStatusModel> GetTaskStatus();
        TaskItemModel GetTask(int taskId);
        String GetTaskOwnerName(int ownerId);
        String GetTaskAssociatedPersonName(int moduleId, int moduleValue);
    //    IList<TaskItemModel> GetTasks(int userId, int companyId, int currentPage, int pageSize, ref int totalRecords);
        List<TaskItemModel> GetTasks(int userId, int companyId, int currentPage, int pageSize, ref int totalRecords, string sortColumnName, string sortDir);
        IList<TaskItemModel> GetContactTasks(int companyId, int contactID, int currentPage, int pageSize, ref int totalRecords);
        IList<TaskItemModel> GetAccountTasks(int companyId, int leadID, int currentPage, int pageSize, ref int totalRecords);
        bool DeleteTaskItem(int taskId, int userId);
        bool DeleteAccountTaskItem(int taskId,int accountId, int userId);
        bool DeleteTaskItem(int taskId,int leadId,int userId);
        IList<TaskItemModel> GetAccountCaseTasks(int companyId, int accountCaseId, int currentPage, int pageSize, ref int totalRecords);
        IList<TaskItemModel> GetLeadTasks(int companyId, int leadID, int currentPage, int pageSize, ref int totalRecords);
        IList<TaskItemModel> GetDashBoardTasks(int companyId, int userId);
        List<DashboarActivitiesModel> GetTasks(int userId, int companyId, int currentPage, int pageSize, string Filter, ref int totalRecords);
           }
}
