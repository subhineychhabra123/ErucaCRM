using ErucaCRM.Web.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ErucaCRM.Web.ViewModels
{
    [CultureModuleAttribute(ModuleName = "AccountSetting")]
    public class AccountSettingVM : BaseModel
    {
        public int UserSettingId { get; set; }
        public bool IsSendNotificationsRecentActivities { get; set; }
        public int UserId { get; set; }
        public class PageLabel
        {
            public string RecentActivitiesNotifications{ get; set; }           

        }
        public PageLabel PageLabels
        {
            get { return new PageLabel(); }
        }
    }
}