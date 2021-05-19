using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErucaCRM.Utility;
namespace ErucaCRM.Domain
{
    public class AccountTagModel
    {
        public int AccountTagId { get; set; }
        public int AccountId { get; set; }
        public int TagId { get; set; }
      
        public virtual TagModel TagModel { get; set; }
    }
}
