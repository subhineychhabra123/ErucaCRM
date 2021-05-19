using ErucaCRM.Repository.Infrastructure;
using ErucaCRM.Repository.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Repository
{
    public class AccountCaseRepository : BaseRepository<AccountCase>
    {
        public AccountCaseRepository(IUnitOfWork unit)
            : base(unit)
        {

        }
        public List<SSP_GetAccountCasesByUserId_Result> GetAccountCasesByUserId(int userId, int currentPage, int pageSize, out int totalRecords, string sortColumnName, string sortDir)
        {
            Entities entities = (Entities)this.UnitOfWork.Db;
            ObjectParameter objParam = new ObjectParameter("totalRecords", 0);
            List<SSP_GetAccountCasesByUserId_Result> accountCases = entities.GetAccountCasesByUserId(userId, currentPage, pageSize, objParam, sortColumnName, sortDir).ToList();
            totalRecords = Convert.ToInt32(objParam.Value);
            return accountCases;
        }
    }
}
