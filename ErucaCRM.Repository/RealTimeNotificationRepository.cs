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
    public class RealTimeNotificationRepository : BaseRepository<RealTimeNotificationConnection>
    {
        public RealTimeNotificationRepository(IUnitOfWork unit)
            : base(unit)
        {
        }
        public List<int?> GetRealTimeConnectionId(int companyId, int userId, int leadid, out int maxLeadAuditId)
        {
            Entities entities = (Entities)this.UnitOfWork.Db;
            ObjectParameter objParam = new ObjectParameter("MaxLeadAuditId", 0);
            List<int?> conectionid = entities.SSP_RealTimeNotifications(companyId, userId, leadid,objParam).ToList();
            maxLeadAuditId = Convert.ToInt32(objParam.Value);
            return conectionid;
        }
    }
}
