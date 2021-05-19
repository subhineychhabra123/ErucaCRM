using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ErucaCRM.Web.Infrastructure;

namespace ErucaCRM.Web.ViewModels
{
    [CultureModuleAttribute(ModuleName = "CultureInformation")]
    public class CultureInformationVM
    {
        [Required(ErrorMessage = "CultureNameRequired")]
        public string CultureInformationId { get; set; }

        public string CultureName { get; set; }
        public string Language { get; set; }
        public string LabelsXML { get; set; }
        public string ExcelFilePath { get; set; }
        private string _cultureDescription;
        public string CultureDescription
        {

            get
            {
                if (!String.IsNullOrEmpty(CultureName) && !String.IsNullOrEmpty(Language))
                {
                    this._cultureDescription = Language.Trim() + "(" + CultureName + ")";
                    return this._cultureDescription;
                }
                else return String.Empty;
            }
            set
            {
                this._cultureDescription = value;

            }

        }
        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }


    }
}