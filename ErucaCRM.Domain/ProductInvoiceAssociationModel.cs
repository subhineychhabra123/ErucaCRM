using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Domain
{
   public class ProductInvoiceAssociationModel
    {
        public int AssociationInvoiceId { get; set; }
        public Nullable<int> AssociatedProductId { get; set; }
        public Nullable<int> InvoiceId { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool RecordDeleted { get; set; }

        private AssociatedProductModel _AssociatedProductModel;
        public virtual AssociatedProductModel AssociatedProduct
        {
            get
            {
                if (this._AssociatedProductModel == null)
                    this._AssociatedProductModel = new AssociatedProductModel();
                return this._AssociatedProductModel;
            }
            set { this._AssociatedProductModel = value; }
        }
    }
}
