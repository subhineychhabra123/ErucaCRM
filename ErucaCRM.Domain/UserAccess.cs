using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Domain
{
    public class UserAccess
    {
        public UserAccess()
        {
            this.IsValidToken = false;
            this.hasMethodPermission = false;
        }
        public bool IsValidToken { get; set; }
        public bool hasMethodPermission { get; set; }
    }
}
