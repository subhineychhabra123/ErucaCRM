using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ErucaCRM.Web.ViewModels
{
    public class AccountContactVM 
    {

            public int AccountContactId { get; set; }
            public Nullable<int> AccountId { get; set; }
            public Nullable<int> ContactId { get; set; }
            public Nullable<int> CreatedBy { get; set; }
            public Nullable<int> ModifiedBy { get; set; }
            public virtual AccountVM Account { get; set; }
            public virtual ContactVM Contact { get; set; }
        }
    
    
    public class AccountLeadContactInfo
    {
       public  string AccountId { get; set; }
       public string contactId { get; set; }
       public string leadId { get; set; }

    }

    }

