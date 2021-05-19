using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ErucaCRM.Web.Infrastructure;

namespace ErucaCRM.Web.ViewModels
{
    [CultureModuleAttribute(ModuleName = "SalesOrder")]
    public class SaleOrdersVM : BaseModel
    {
        public string SalesOrderId { get; set; }
        public string Subject { get; set; }
        public string OwnerName { get; set; }
        public string AccountName { get; set; }
        public string AccountId { get; set; }
        public Nullable<int> ContactId { get; set; }
        public string OwnerId { get; set; }
        public string Carrier { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        //public string OwnerName { get; set; }
        //public string LeadName { get; set; }
        //public String AccountName { get; set; }
        public String SaleOrderDueDate
        {
            get
            {

                return this.DueDate != null ? this.DueDate.Value.ToShortDateString() : "";
            }
        }

        public GridHeader GridHeaders
        {
            get
            {
                return new GridHeader();
            }
        }

        public PageButton PageButtons
        {
            get
            {
                return new PageButton();
            }

        }


        public class GridHeader
        {
            public string GridHeaderSubject { get; set; }
            public string GridHeaderCarrier { get; set; }
            public string GridHeaderDueDate { get; set; }
            public string GridHeaderOwnerName { get; set; }
            public string GridHeaderLeadName { get; set; }
            public string GridHeaderAccountName { get; set; }

        }

        public class PageButton
        {
            public string EditButtonToolTip { get; set; }
            public string DeleteButtonToolTip { get; set; }
        }
    }


}