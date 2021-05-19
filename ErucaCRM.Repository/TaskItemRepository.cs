using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Transactions;
using ErucaCRM.Repository.Infrastructure;
using ErucaCRM.Repository.Infrastructure.Contract;
using System.Linq;


namespace ErucaCRM.Repository
{
    public class TaskItemRepository : BaseRepository<TaskItem>
    {
        public TaskItemRepository(IUnitOfWork unit)
            : base(unit)
        {

        }

    
        public List<SSP_GetActivitiesByUserId_Result> GetActivitiesByUserId(int userId, int companyId, int currentPage, int pageSize, out int totalRecords, string sortColumnName, string sortDir)
        {
            Entities entities = (Entities)this.UnitOfWork.Db;
            ObjectParameter objParam = new ObjectParameter("totalRecords", 0);
            List<SSP_GetActivitiesByUserId_Result> taskitems = entities.SSP_GetActivitiesByUserId(userId, companyId, currentPage, pageSize, objParam, sortColumnName, sortDir).ToList();
            totalRecords = Convert.ToInt32(objParam.Value);

            return taskitems;
        }

        public List<SSP_GetActivities_Result> GetActivities(int userId, int companyId, int currentPage, int pageSize, string Filter, out int totalRecords)
        {
            Entities entities = (Entities)this.UnitOfWork.Db;
            ObjectParameter objParam = new ObjectParameter("totalRecords", 0);
           // List<TaskItem> taskitems = entities.SSP_GetActivities(userId, companyId, currentPage, pageSize, Filter, objParam).ToList();
            List<SSP_GetActivities_Result> taskitems = entities.SSP_GetActivities(userId, companyId, currentPage, pageSize, Filter, objParam).ToList();
            totalRecords = Convert.ToInt32(objParam.Value);

            return taskitems;
        }

        //public string GetTaskOwnerName(int ownerId)
        //{
        //    Entities entities = (Entities)this.UnitOfWork.Db;
        //    String ownerName = entities.SSP_GetOwnerNameTask(ownerId).ToString();
        //    return ownerName;
        //}
    }
}

