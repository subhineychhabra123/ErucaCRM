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
   public class AccountContactRepository : BaseRepository<AccountContact>
    {
        public AccountContactRepository(IUnitOfWork unit)
            : base(unit)
        {

        }
        public List<SSP_GetContactForLeadAccountContacts_Result> GetContactsByAccountId(int userId, int AccountId, int companyId, string filterby)
        {
            Entities entities = (Entities)this.UnitOfWork.Db;
            ObjectParameter objParam = new ObjectParameter("totalrecords", 0);
           // List<SSP_GetContactsByAccountId_Result> ContactList = entities.SSP_GetContactsByAccountId(AccountId, userId, companyId).ToList();
            List<SSP_GetContactForLeadAccountContacts_Result> ContactList = entities.SSP_GetContactForLeadAccountContacts(userId, companyId, AccountId, filterby, 1, 1, "", 0, objParam).ToList();
            return ContactList;
        }
    }
}
