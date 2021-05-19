using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
namespace ErucaCRM.Web
{
    public interface IRealTimeNotificationHub
    {
        void NewNotification(object data);
    }
}