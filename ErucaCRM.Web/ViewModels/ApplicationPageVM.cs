using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ErucaCRM.Web.ViewModels
{
    public class ApplicationPageVM
    {

        public string ApplicationPageId { get; set; }
        [AllowHtml]
        public string PageTitle { get; set; }

        public string PageDescription { get; set; }
        [Required]
        [RegularExpression("([a-zA-Z0-9.&'-]+)", ErrorMessage = "Url should not contain special character or space.")]
        public string PageUrl { get; set; }
        public bool IsApplicationPage { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool RecordDeleted { get; set; }

        public ICollection<AssociationApplicationPageVM> AssociationApplicationPages { get; set; }
        public ICollection<AssociationApplicationPageVM> AssociationCustomPages { get; set; }
        public ContentApplicationPageVM ContentApplicationPage { get; set; }
    }
}