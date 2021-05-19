using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ErucaCRM.Web.ViewModels
{
    public class ContentApplicationPageVM
    {
        public int ContentApplicationPageId { get; set; }
        public string CultureInformationId { get; set; }
        public string ApplicationPageId { get; set; }
       [AllowHtml]
        public string MetaTitle { get; set; }
        [AllowHtml]
        public string MetaDescription { get; set; }
        [AllowHtml]
        public string PageContent { get; set; }
        public bool UseDefaultContent { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

        public ApplicationPageVM ApplicationPage { get; set; }
        public CultureInformationVM CultureInformation { get; set; }
    }
}