using ErucaCRM.Domain;
using ErucaCRM.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ErucaCRM.Web.Infrastructure;

namespace ErucaCRM.Web.ViewModels
{
    [CultureModuleAttribute(ModuleName = "Invoice")]
    public class InvoicesVM:BaseModel
    {
        public string InvoiceId { get; set; }
        public string Subject { get; set; }
        public string OwnerId { get; set; }      
        public Nullable<System.DateTime> InvoiceDate { get; set; }       
        public Nullable<System.DateTime> DueDate { get; set; }
        public Nullable<int> StatusId { get; set; }
        public string LeadId { get; set; }
        public string StatusName
        {
            get
            {
                if (this.StatusId != null)
                    return ((Enums.Status)this.StatusId).ToString();
                return "";
            }

        }
        public string OwnerName { get; set; }
        public string LeadName { get; set; } 
        public String InvoiceCreationDate
        {
            get
            {              
                return this.InvoiceDate != null ? this.InvoiceDate.Value.ToShortDateString() : "";
            }
        }

        public String InvoiceDueDate
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

        public class GridHeader
        {
            public string GridHeaderSubject { get; set; }
            public string GridHeaderInvoiceDate { get; set; }
            public string GridHeaderDueDate { get; set; }
            public string GridHeaderOwnerName { get; set; }
            public string GridHeaderLeadName { get; set; }


        }

    }
}