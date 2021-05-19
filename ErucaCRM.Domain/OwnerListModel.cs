using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErucaCRM.Utility;
namespace ErucaCRM.Domain
{
    public class OwnerListModel
    {
        public int LeadOwnerId { get; set; }
        private string _name;
        public string Name
        {
            get
            {
                _name = FirstName + " " + LastName;
                return _name;
            }
            set { _name = value; }
        }

        public string AccountOwnerId
        {
            get { return this.OwnerId.Encrypt(); }
        }
        public int OwnerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
