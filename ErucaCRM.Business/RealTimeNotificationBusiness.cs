using ErucaCRM.Business.Interfaces;
using ErucaCRM.Repository;
using ErucaCRM.Repository.Infrastructure.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErucaCRM.Domain;
namespace ErucaCRM.Business
{
    public class RealTimeNotificationBusiness : IRealTimeNotificationBusiness
    {
        private readonly RealTimeNotificationRepository realTimeNotificationRepository;
        private readonly IUnitOfWork unitOfWork;

        public RealTimeNotificationBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            realTimeNotificationRepository = new RealTimeNotificationRepository(unitOfWork);
        }
        public void JoinGroup(int companyId,int userId, string connectionId)
        {
            throw new NotImplementedException();
        }

        public void RemoveGroup(int companyId, int userId, string connectionId)
        {
            throw new NotImplementedException();
        }

        public void OnConnected(int companyId, int userId, string connectionId)
        {
            RealTimeNotificationModel realTimeNotificationModel = new RealTimeNotificationModel();
            realTimeNotificationModel.ConnectionId = connectionId;
            realTimeNotificationModel.IsConnected = true;
            realTimeNotificationModel.UserId = userId;
            realTimeNotificationModel.CompanyId = companyId;
            RealTimeNotificationConnection realTimeNotificationConnection = new RealTimeNotificationConnection();
            AutoMapper.Mapper.Map(realTimeNotificationModel, realTimeNotificationConnection);
            realTimeNotificationRepository.Insert(realTimeNotificationConnection);

        }

        public void OnDisconnected(int companyId, int userId, string connectionId)
        {
            RealTimeNotificationConnection connection = realTimeNotificationRepository.SingleOrDefault(x => x.CompanyId == companyId && x.UserId == userId && x.ConnectionId == connectionId);
            if (connection != null)
            {
                connection.IsConnected = false;

                realTimeNotificationRepository.Update(connection);
            }
        }
        public List<RealTimeNotificationModel> GetNotifyByClientId(int userid)
        {  List<RealTimeNotificationModel> realTimeNotificationModelList = new List<RealTimeNotificationModel>();
            List<RealTimeNotificationConnection> realTimeNotificationConnectionList = realTimeNotificationRepository.GetAll(x => x.UserId == userid&&x.IsConnected==true).ToList();
            AutoMapper.Mapper.Map(realTimeNotificationConnectionList, realTimeNotificationModelList);
            return realTimeNotificationModelList;
        }
        public List<int?> GetNotifyClientByCompanyId(int companyid,int userId,int leadId,ref  int maxLeadAuditId)
        {

            List<int?> realTimeNotificationConnectionList = realTimeNotificationRepository.GetRealTimeConnectionId(companyid, userId, leadId,out maxLeadAuditId).ToList();
            return realTimeNotificationConnectionList;
        }
    }
}
