using ErucaCRM.Domain;
using ErucaCRM.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ErucaCRM.Web.Infrastructure;

namespace ErucaCRM.Web.ViewModels
{
    [CultureModuleAttribute(ModuleName = "AssociatedProduct")]
    public class AssociatedProductVM
    {
        public int AssociatedProductId { get; set; }
        public int ProductId { get; set; }
        public Nullable<decimal> QtyInStock { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal ListPrice { get; set; }
        public Nullable<int> DiscountType { get; set; }
        private Nullable<decimal> _discountAmount;
        public Nullable<decimal> DiscountAmount
        {
            get
            { return _discountAmount.GetValueOrDefault(); }
            set { _discountAmount = value.GetValueOrDefault(); }
        }

        public string CalcDiscountAmount
        {
            get
            {
                if (this.DiscountType == 2)
                {
                    return ((this.DiscountAmount * this.TotalAmount) / 100).ToString();
                }
                else
                    return "";

            }

        }
        private Nullable<bool> _taxApplied;
        public Nullable<bool> TaxApplied
        {
            get
            { return _taxApplied.GetValueOrDefault(); }
            set { _taxApplied = value.GetValueOrDefault(); }
        }

        private Nullable<decimal> _taxAmount;
        public Nullable<decimal> TaxAmount
        {
            get
            { return _taxAmount.GetValueOrDefault(); }
            set { _taxAmount = value.GetValueOrDefault(); }
        }
        private Nullable<bool> _vatApplied;
        public Nullable<bool> VatApplied
        {
            get
            {
                return _vatApplied.GetValueOrDefault();
            }

            set { _vatApplied = value.GetValueOrDefault(); }
        }
        private Nullable<decimal> _vatAmount;
        public Nullable<decimal> VatAmount
        {
            get
            {
                return _vatAmount.GetValueOrDefault();
            }

            set { _vatAmount = value.GetValueOrDefault(); }
        }

        public string ProductDescription { get; set; }
        public decimal TotalAmount
        {
            get
            {
                return ListPrice * Quantity;
            }

        }
        public decimal TotalDiscount
        {
            get
            {
                decimal totalDiscount = 0;
                if (this.DiscountType == (int)Enums.DiscountType.PercentageOfPrice)
                {
                    totalDiscount = TotalAmount * ((DiscountAmount.GetValueOrDefault()) / 100);
                }
                else if (this.DiscountType == (int)Enums.DiscountType.FixedPrice)
                {
                    totalDiscount = DiscountAmount.GetValueOrDefault();
                }
                return totalDiscount;
            }

        }
        public decimal TotalTax
        {
            get
            {
                decimal saleTax = 0;
                decimal vatTax = 0;
                if (TaxApplied == true)
                {
                    saleTax = calculateTax(TaxAmount, TotalAfterDiscount);
                }
                if (VatApplied == true)
                {
                    vatTax = calculateTax(VatAmount, TotalAfterDiscount);
                }
                return saleTax + vatTax;

            }

        }
        public decimal TotalAfterDiscount
        {
            get
            {
                return TotalAmount - TotalDiscount;
            }
        }

        public decimal NetTotal { get { return TotalAfterDiscount + TotalTax; } }
        public ProductModel Product { get; set; }
        private decimal calculateTax(decimal? taxInPercentage, decimal? amount)
        {
            decimal tax = 0;
            tax = amount.GetValueOrDefault() * (taxInPercentage.GetValueOrDefault() / 100);
            return tax;
        }
    }

}