using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ErucaCRM.Web.Infrastructure;

namespace ErucaCRM.Web.ViewModels
{
    [CultureModuleAttribute(ModuleName = "ProductLeadAssociation")]
    public class ProductLeadAssociationVM
    {
        /// <summary>
        /// Gets or sets the AssociationLeadId value.
        /// </summary>
        public virtual Int32 AssociationLeadId { get; set; }

        /// <summary>
        /// Gets or sets the ProductId value.
        /// </summary>
        public virtual Int32 ProductId { get; set; }

        /// <summary>
        /// Gets or sets the LeadId value.
        /// </summary>
        public virtual string LeadId { get; set; }

        public virtual ProductVM Product { get; set; }
    }
}