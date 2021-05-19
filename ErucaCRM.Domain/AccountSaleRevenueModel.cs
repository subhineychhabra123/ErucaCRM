using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Domain
{
    public class AccountSaleRevenueModel
    {
        public string Month { get; set; }
        public string AccountName { get; set; }
        public decimal TotalAccountSaleRevenue { get; set; }
    }
}
