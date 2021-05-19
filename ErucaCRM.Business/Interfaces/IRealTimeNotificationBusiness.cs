using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErucaCRM.Domain;
namespace ErucaCRM.Business.Interfaces
{
  public interface IRealTimeNotificationBusiness
    {
      List<int?> GetNotifyClientByCompanyId(int companyid, int userId, int leadId, ref  int maxLeadAuditId);
      List<RealTimeNotificationModel> GetNotifyByClientId(int userid);
      void JoinGroup(int companyId,int userId, string connectionId);
      void RemoveGroup(int companyId,int userid, string connectionId);
      void OnConnected(int companyId, int userId, string connectionId);
      void OnDisconnected(int companyId, int userId, string connectionId);
    }
}
