using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using ErucaCRM.Domain;
using ErucaCRM.Business;
using ErucaCRM.Repository.Infrastructure.Contract;
using ErucaCRM.Repository.Infrastructure;
using System.Threading.Tasks;
using ErucaCRM.Utility;
using ErucaCRM.Web.ViewModels;

namespace ErucaCRM.Web
{
    public class RealTimeNotificationHub : Hub<IRealTimeNotificationHub>
    {
        IUnitOfWork unitOfWork;
        public RealTimeNotificationHub()
        {
            unitOfWork = new UnitOfWork();
        }
        public void SendNotification(string lead_id_encrypted)
        {
            List<HomeVM> homeVMList = new List<HomeVM>();
            List<HomeModel> homeModelList = new List<HomeModel>();
            RealTimeNotificationBusiness realTimeNotificationBusiness = new RealTimeNotificationBusiness(unitOfWork);
            HomeBusiness homeBusiness = new HomeBusiness(unitOfWork);
            LeadAuditBusiness leadAuditBusiness = new LeadAuditBusiness(unitOfWork);          
            int maxLeadAuditId = 0;
            int companyid=Convert.ToInt32(Context.User.Identity.Name.Split(new[] { '|' })[1]);
            int curentuserid=Convert.ToInt32(Context.User.Identity.Name.Split(new[] { '|' })[0]);
            List<int?> realTimeNotificationId = realTimeNotificationBusiness.GetNotifyClientByCompanyId(companyid, curentuserid, lead_id_encrypted.Decrypt(), ref maxLeadAuditId);
            homeModelList = homeBusiness.GetRecentActivitiesForHome(1, 1, maxLeadAuditId, false, 0, 0);
            AutoMapper.Mapper.Map(homeModelList, homeVMList);        
            
            foreach (int userid in realTimeNotificationId)
            {
                Clients.User(userid.Encrypt()).NewNotification(new { RecentActivities = homeVMList.Select(x => new { CreatedBy = x.CreatedBy, ImageURL = x.ImageURL, LeadAuditId = x.LeadAuditId, ActivityText = x.ActivityText, ActivityCreatedTime = x.ActivityCreatedTime }), MaxLeadAuditID = maxLeadAuditId.Encrypt() });
               
            }
            
            
        }
        public void CallerNotification(string maxLeadAuditId_encrypted, bool updateNotification)
        {
          
           // RealTimeNotificationBusiness realTimeNotificationBusiness = new RealTimeNotificationBusiness(unitOfWork);
            LeadAuditBusiness leadAuditBusiness = new LeadAuditBusiness(unitOfWork);
          //  List<RealTimeNotificationModel> realTimeNotificationModel = realTimeNotificationBusiness.GetNotifyByClientId(Convert.ToInt32(Context.RequestCookies["userid"].Value));
            int maxLeadAuditId = 0;
            int totalNotification = 0;
            if (maxLeadAuditId_encrypted != "0")
            {
                int.TryParse(maxLeadAuditId_encrypted, out maxLeadAuditId);
            }
            leadAuditBusiness.GetNotification(ReadConfiguration.PageSize, Convert.ToInt32(Context.User.Identity.Name.Split(new[] { '|' })[1]), Convert.ToInt32(Context.User.Identity.Name.Split(new[] { '|' })[0]), ref maxLeadAuditId, updateNotification, ref totalNotification);


            Clients.Caller.NewNotification(totalNotification);
        }
        public Task JoinGroup(string groupName)
        {           
            return Groups.Add(Context.ConnectionId, groupName);
        }
        public Task LeaveGroup(string groupName)
        {
            return Groups.Remove(Context.ConnectionId, groupName);
        }
        public override Task OnConnected()
        {
            RealTimeNotificationBusiness realTimeNotificationBusiness = new RealTimeNotificationBusiness(unitOfWork);
            realTimeNotificationBusiness.OnConnected(Convert.ToInt32(Context.User.Identity.Name.Split(new[] { '|' })[1]), Convert.ToInt32(Context.User.Identity.Name.Split(new[] { '|' })[0]), Context.ConnectionId);
            return base.OnConnected();
        }

        public override Task OnDisconnected()
        {
            RealTimeNotificationBusiness realTimeNotificationBusiness = new RealTimeNotificationBusiness(unitOfWork);
            realTimeNotificationBusiness.OnDisconnected(Convert.ToInt32(Context.User.Identity.Name.Split(new[] { '|' })[1]), Convert.ToInt32(Context.User.Identity.Name.Split(new[] { '|' })[0]), Context.ConnectionId);           
            return base.OnDisconnected();
        }
    }
}