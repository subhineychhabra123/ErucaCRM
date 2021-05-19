using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ErucaCRM.Web.Infrastructure;

namespace ErucaCRM.Web.ViewModels
{
    [CultureModuleAttribute(ModuleName = "Quote")]
    public class QuotesVM : BaseModel
    {

        public string QuoteId { get; set; }

        public string Subject { get; set; }
        public string OwnerId { get; set; }
        public Nullable<int> ContactId { get; set; }
        public string LeadId { get; set; }
        public string Carrier { get; set; }
        public Nullable<System.DateTime> ValidTill { get; set; }
        public String ValidityDate
        {
            get
            {
                return this.ValidTill == null ? "" : this.ValidTill.Value.ToShortDateString();
            }
        }
        public string OwnerName { get; set; }
        public string LeadName { get; set; }

        public GridHeader GridHeaders
        {
            get
            {
                return new GridHeader();
            }
        }

        public class GridHeader
        {
            public string GridHeaderSubject { get; set; }
            public string GridHeaderCarrier { get; set; }
            public string GridHeaderValidityDate { get; set; }
            public string GridHeaderOwnerName { get; set; }
            public string GridHeaderLeadName { get; set; }


        }
    }
}