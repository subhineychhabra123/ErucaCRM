using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErucaCRM.Repository.Infrastructure;
using ErucaCRM.Repository.Infrastructure.Contract;
using System.Data.Objects;

namespace ErucaCRM.Repository
{
    public class SalesOrderRepository : BaseRepository<SalesOrder>
    {
        public SalesOrderRepository(IUnitOfWork unit)
            : base(unit)
        {

        }

        public List<SSP_SalesOrderListbyUserId_Result> GetSalesOrderListByUserId(int userId, int companyId, int CurrentPage, int PageSize, ref int totalrecords, string sortColumnName, string sortdir)
        {
            Entities entities = (Entities)this.UnitOfWork.Db;
            ObjectParameter objParam = new ObjectParameter("totalRecords", 0);
            List<SSP_SalesOrderListbyUserId_Result> accounts = entities.SSP_SalesOrderListbyUserId(userId, companyId, CurrentPage, PageSize, objParam, sortColumnName, sortdir).ToList();
            totalrecords = Convert.ToInt32(objParam.Value);
            return accounts;
        }
    
    }
}