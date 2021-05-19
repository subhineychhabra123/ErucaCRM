using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ErucaCRM.Utility;
using ErucaCRM.Web.Infrastructure;

namespace ErucaCRM.Web.ViewModels
{
    [CultureModuleAttribute(ModuleName = "ProductQuoteAssociation")]
    public class ProductQuoteAssociationVM
    {
        public int AssociationQuoteId { get; set; }
        public Nullable<int> AssociatedProductId { get; set; }
        public string QuoteId { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<bool> RecordDeleted { get; set; }
        private AssociatedProductVM _associatedProduct;
        public virtual AssociatedProductVM AssociatedProduct
        {
            get
            {
                if (this._associatedProduct == null)
                {
                    this._associatedProduct = new AssociatedProductVM();
                }
                return this._associatedProduct;
            }
            set
            {
                this._associatedProduct = value;
            }
        }
    }
}