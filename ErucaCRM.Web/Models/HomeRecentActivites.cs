using ErucaCRM.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ErucaCRM.WCFService.Models
{
    [DataContract]
    public class HomeRecentActivites
    {
      [DataMember]
        public int ActivityType { get; set; }
        public string ImageURL { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string ToDate { get; set; }
        [DataMember]
        public string  FromDate { get; set; }
        [DataMember]
        public string LeadAuditId { get; set; }
        [DataMember]
        public string ImageFullPath { get; set; }
       [DataMember]
        public System.DateTime CreatedDate { get; set; }
         [DataMember]
        public string LeadId { get; set; }
         [DataMember]
        public string CreatedBy { get; set; }
         [DataMember]
        public Nullable<decimal> Amount { get; set; }
         [DataMember]
        public string StageName { get; set; }
        // [DataMember]
        //public System.DateTime ActivityCreatedTime
        //{
        //    get
        //    {
        //        return CreatedDate.AddHours(Convert.ToInt32(SessionManagement.LoggedInUser.Offset));
        //    }
        //}
        //[DataMember]
        //public string ActivityText
        //{

        //    get
        //    {
        //        if (ActivityType == (int)Enums.ActivityType.LeadAdded)
        //        {
        //            return string.Format(CommonFunctions.GetGlobalizedLabel("DashBoard", "LeadAdded"), " <a href=\"/User/Leads/#" + this.LeadId + "\">" + this.Title + "</a>", " <a href=\"/User/UserProfile/" + this.CreatedBy + "\">" + this.FirstName + " " + this.LastName + " </a>", this);
        //        }
        //        else if (ActivityType == (int)Enums.ActivityType.LeadAmountChanged)
        //        {
        //            return string.Format(CommonFunctions.GetGlobalizedLabel("DashBoard", "LeadAmountChanged"), " <a href=\"/User/Leads/#" + this.LeadId + "\">" + this.Title + " </a>", " <a href=\"/User/UserProfile/" + this.CreatedBy + "\">" + this.FirstName + " " + this.LastName + " </a>", this.Amount, this.ActivityCreatedTime);
        //        }
        //        else if (ActivityType == (int)Enums.ActivityType.LeadRatingChanged)
        //        {
        //            return string.Format(CommonFunctions.GetGlobalizedLabel("DashBoard", "LeadRatingChanged"), " <a href=\"/User/Leads/#" + this.LeadId + "\">" + this.Title + " </a>", " <a href=\"/User/UserProfile/" + this.CreatedBy + "\">" + this.FirstName + " " + this.LastName + " </a>", this.StageName, this.ActivityCreatedTime);
        //        }
        //        else if (ActivityType == (int)Enums.ActivityType.LeadStageChanged)
        //        {
        //            return string.Format(CommonFunctions.GetGlobalizedLabel("DashBoard", "LeadStageChanged"), " <a href=\"/User/Leads/#" + this.LeadId + "\">" + this.Title + " </a>", this.StageName, " <a href=\"/User/UserProfile/" + this.CreatedBy + "\">" + this.FirstName + " " + this.LastName + " </a>", this.ActivityCreatedTime);
        //        }
        //        else if (ActivityType == (int)Enums.ActivityType.LeadDeleted)
        //        {
        //            return string.Format(CommonFunctions.GetGlobalizedLabel("DashBoard", "LeadDeleted"), this.Title, this.StageName, " <a href=\"/User/UserProfile/" + this.CreatedBy + "\">" + this.FirstName + " " + this.LastName + " </a>", this.ActivityCreatedTime);
        //        }
        //        else
        //        {
        //            return "";
        //        }
        //    }
        //}
    }
}