using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Domain
{
    public class ProductQuoteAssociationModel
    {

        public int AssociationQuoteId { get; set; }
        public Nullable<int> AssociatedProductId { get; set; }
        public Nullable<int> QuoteId { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool RecordDeleted { get; set; }
        private AssociatedProductModel _associatedProduct;
        public virtual AssociatedProductModel AssociatedProduct
        {
            get
            {
                if (this._associatedProduct == null)
                {
                    this._associatedProduct = new AssociatedProductModel();
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
