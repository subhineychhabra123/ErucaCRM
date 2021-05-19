using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Utility
{
    public class Enums
    {
        public enum UserType
        {
            Admin = 1,
            User = 2,
            SubAdmin = 3
        }

        public enum ProfileF
        {
            Administrator = 1,

        }
        public enum ResponseResult
        {
            Success,
            Failure,
            NameExist,
            StageOrderExist
        }
        public enum RenderType
        {
            Table = 1,
            List = 2
        }
        public enum Carrier
        {
            FedEX,
            UPS,
            USPS,
            DHL,
            BlueDart
        }
        public enum TaskPriority
        {
            Urgent = 1,
            High = 2,
            Medium = 3,
            Low = 4,
            Normal = 5
        }
        public enum CasePriority
        {
            Urgent = 1,
            High = 2,
            Medium = 3,
            Low = 4,
            Normal = 5
        }
        public enum CaseType
        {
            None = 1,
            Problem = 2,
            FeatueRequest = 3,
            Question = 4,

        }
        public enum CaseOrigin
        {
            None = 1,
            Email = 2,
            Phone = 3,
            Web = 4,

        }
        public enum CaseStaus
        {
            ToDo = 1,
            InProgress = 2,
            Halted = 3,
            Completed = 4,
            Closed = 5
        }
        public enum TaskStaus
        {
            ToDo = 1,
            InProgress = 2,
            Halted = 3,
            Completed = 4,
            Closed = 5
        }

        public enum Module
        {
            Lead = 3,
            Contact = 4,
            Account = 18,
            AccountCase = 19,
        }
        public enum Status
        {
            [Description("Created")]
            Created = 1,
            [Description("Approved")]
            Approved = 2,
            [Description("Delivered")]
            Delivered = 3,
            [Description("Cancelled")]
            Cancelled = 4
        }
        public enum DiscountType
        {
            [Description("No Discount")]
            NoDiscount = 1,
            [Description("% of Price")]
            PercentageOfPrice = 2,
            [Description("Fixed Price")]
            FixedPrice = 3
        }

        public enum LeadStatus
        {
            CloseWin = 1,
            CloseLost = 0,
            Lost = 5
        }
        public enum Rating
        {
            LastRating = 5

        }

        public enum ActivityType
        {
            LeadAdded = 1,
            LeadAmountChanged = 2,
            LeadRatingChanged = 3,
            LeadStageChanged = 4,
            LeadDeleted = 5
        }

        public enum ContactList
        {
            LeadContacts,
            AccountContacts,
            GetAllMyContacts
          
        }
        public enum LogErrorType
        {
            RecentActivity=1,
            DelayLead=2
        }
        public enum LeadAudioStatus
        { 
            Added=1,
            Updated=2,
            None=3,
            Delete=4
        }
      
    }

}
