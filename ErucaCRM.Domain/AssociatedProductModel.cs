using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Domain
{
   public class AssociatedProductModel
    {
        public int AssociatedProductId { get; set; }
        public int ProductId { get; set; }
        public Nullable<decimal> QtyInStock { get; set; }
        public Nullable<decimal> Quantity { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<decimal> ListPrice { get; set; }
        public Nullable<int> DiscountType { get; set; }
        public Nullable<decimal> DiscountAmount { get; set; }
        public Nullable<bool> TaxApplied { get; set; }
        public Nullable<decimal> TaxAmount { get; set; }
        public Nullable<bool> VatApplied { get; set; }
        public Nullable<decimal> VatAmount { get; set; }
        public string ProductDescription { get; set; }
        
       private ProductModel _ProductModel;
        public ProductModel Product
        {
            get
            {
                if (this._ProductModel == null)
                {
                    this._ProductModel = new ProductModel();
                }
                return this._ProductModel;
            }
            set
            {
                this._ProductModel = value;
            }
        }
    }
}
