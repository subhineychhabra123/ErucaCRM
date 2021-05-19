using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ErucaCRM.Web.Infrastructure;

namespace ErucaCRM.Web.ViewModels
{

    [CultureModuleAttribute(ModuleName = "Tag")]
    public class TagVM:BaseModel
    {
        public string TagId { get; set; }
        public string TagName { get; set; }
        public string Description { get; set; }
        public int CompanyId { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool RecordDeleted { get; set; }
        public virtual CompanyVM Company { get; set; }

        public PageSubHeader PageSubHeaders
        {
            get
            {
                return new PageSubHeader();
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

        public PageLabel PageLabels
        {
            get
            {
                return new PageLabel();
            }

        }

        public class PageLabel
        {
            public string TagName { get; set; }
            public string TagDescription { get; set; }
        
        }

        public class PageSubHeader
        {
           
            public string AddTag { get; set; }
            public string EditTag { get; set; }
            public string PageSubHeaderTagInfo { get; set; }
            public string PageSubHeaderTagAccounts { get; set; }
            public string PageSubHeaderTagContacts { get; set; }
            public string PageSubHeaderTagLeads { get; set; }


        }

        public class GridHeader
        {
            //Contact List
            public string ContactName { get; set; }
            public string Phone { get; set; }
           
            public string Company { get; set; }

            //AcountList

            public string AccountName { get; set; }
           
            public string Contact { get; set; }
            public string AccountType { get; set; }
            public string AccountOwner { get; set; }


            public string Email { get; set; }
            public string LeadTitle { get; set; }
            public string LeadCompanyName { get; set; }
            public string Description { get; set; }   
        }

        public class PageButton
        {
            public string LinkButtonEdit { get; set; }
            public string LinkButtonDelete { get; set; }
            public string EditButtonToolTip { get; set; }
            public string DeleteButtonToolTip { get; set; }
        }
    }
}