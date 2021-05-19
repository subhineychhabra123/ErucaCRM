using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ErucaCRM.Web.Infrastructure;

namespace ErucaCRM.Web.ViewModels
{
    [CultureModuleAttribute(ModuleName = "ProductSalesOrderAssociation")]
    public class ProductSalesOrderAssociationVM
    {
        public int AssociationSalesOrderId { get; set; }
        public Nullable<int> AssociatedProductId { get; set; }
        public string SalesOrderId { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<bool> RecordDeleted { get; set; } 
        public virtual AssociatedProductVM AssociatedProduct { get; set; }
    }
}