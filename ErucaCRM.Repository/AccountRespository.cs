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
    public class AccountRespository : BaseRepository<Account>
    {
        public AccountRespository(IUnitOfWork unit)
            : base(unit)
        {

        }
        public List<Account> GetAccountsByUserId(int userId, int companyId, int? tagId, string tagName, int currentPage, int pageSize, out int totalRecords)
        {
            Entities entities = (Entities)this.UnitOfWork.Db;
            ObjectParameter objParam = new ObjectParameter("totalRecords", 0);
            List<Account> accounts = entities.GetAccountsByUserId(userId, companyId, tagName, tagId, currentPage, pageSize, objParam).ToList();
            totalRecords = Convert.ToInt32(objParam.Value);
            return accounts;
        }

        public List<SSP_GetAccountsListbyUserId_Result> GetAccountsListByUserId(int userId, int companyId)
        {
            Entities entities = (Entities)this.UnitOfWork.Db;
            List<SSP_GetAccountsListbyUserId_Result> accounts = entities.SSP_GetAccountsListbyUserId_Result(userId, companyId, 1, 1).ToList();
          
            return accounts;
        }

    }
}
