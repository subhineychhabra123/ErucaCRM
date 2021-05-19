using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Domain
{
 
      public  class AccountContactModel
      {
          public int AccountContactId { get; set; }
          public Nullable<int> AccountId { get; set; }
          public Nullable<int> ContactId { get; set; }
          public Nullable<int> CreatedBy { get; set; }
          public Nullable<int> ModifiedBy { get; set; }
          public virtual AccountModel Account { get; set; }
          public virtual ContactModel Contact { get; set; }
      }
  
}
