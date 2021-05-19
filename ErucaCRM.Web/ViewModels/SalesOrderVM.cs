using ErucaCRM.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ErucaCRM.Utility;
using ErucaCRM.Web.Infrastructure;

namespace ErucaCRM.Web.ViewModels
{
    [CultureModuleAttribute(ModuleName = "SalesOrder")]
    public class SalesOrderVM : BaseModel
    {
        public string SalesOrderId { get; set; }
          [Required(ErrorMessage = "SalesOrder.SubjectNameRequired")]
        public string Subject { get; set; }
        [Display(Name = "Quote Name")]
        public string QuoteId { get; set; }
        [Required(ErrorMessage = "SalesOrder.LeadNameRequired")]
        [Display(Name = "Lead Name")]
        public string AccountId { get; set; }
        public Nullable<int> ContactId { get; set; }
        [Required(ErrorMessage = "SalesOrder.OwnerNameRequired")]
        public Nullable<int> OwnerId { get; set; }
        public string Carrier { get; set; }
        [Display(Name = "Sales Commission")]
        public Nullable<decimal> SalesCommission { get; set; }
        public Nullable<int> StatusId { get; set; }
        [Display(Name = "Due Date")]
      //  [Required(ErrorMessage = "SalesOrder.OrderDateRequired")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> DueDate { get; set; }
        public Nullable<decimal> SubTotal { get; set; }
        public Nullable<int> DiscountType { get; set; }
        public Nullable<decimal> DiscountAmount { get; set; }
        public decimal TotalMainAmount
        {
            get
            {
                return this.SubTotal.GetValueOrDefault();
            }

        }
        public decimal TotalMainDiscount
        {
            get
            {
                decimal totalDiscount = 0;
                if (this.DiscountType == (int)Enums.DiscountType.PercentageOfPrice)
                {
                    totalDiscount = TotalMainAmount * ((DiscountAmount.GetValueOrDefault()) / 100);
                }
                else if (this.DiscountType == (int)Enums.DiscountType.FixedPrice)
                {
                    totalDiscount = DiscountAmount.GetValueOrDefault();
                }
                return totalDiscount;
            }

        }

        public decimal TotalMainAfterDiscount
        {
            get
            {
                return TotalMainAmount - TotalMainDiscount;

            }

        }
        public decimal TotalMainTax
        {
            get
            {
                decimal saleTax = 0;
                decimal vatTax = 0;
                if (TaxApplied == true)
                {
                    saleTax = calculateTax(TaxAmount, TotalMainAfterDiscount);
                }
                if (VatApplied == true)
                {
                    vatTax = calculateTax(VatAmount, TotalMainAfterDiscount);
                }
                return saleTax + vatTax;

            }

        }
        public Nullable<bool> TaxApplied { get; set; }
        public Nullable<decimal> TaxAmount { get; set; }
        public Nullable<bool> VatApplied { get; set; }
        public Nullable<decimal> VatAmount { get; set; }
        public Nullable<int> AdjustmentType { get; set; }
        public Nullable<decimal> AdjustmentAmount { get; set; }
        public Nullable<decimal> OrderAmount { get; set; }
        public string ViewOrderAmount
        {
            get
            {
                return OrderAmount.Value.ToString("N2");
            }
        }
        public Nullable<decimal> GrandTotal { get; set; }
        public string ViewGrandTotal
        {
            get
            {
                return GrandTotal.Value.ToString("N2");
            }
        }
           
        public string Terms { get; set; }
        public string Description { get; set; }
        public Nullable<int> BillingAddressId { get; set; }
        public Nullable<int> ShippingAddressId { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<bool> RecordDeleted { get; set; }
        public virtual BillingAddressVM Address { get; set; }
        public virtual ShippingAddressVM Address1 { get; set; }
        public virtual ICollection<ProductSalesOrderAssociationVM> ProductSalesOrderAssociations { get; set; }
        public virtual UserVM User { get; set; }
        public virtual AccountVM Account { get; set; }
        public virtual QuoteVM Quote { get; set; }


        private decimal calculateTax(decimal? taxInPercentage, decimal? amount)
        {
            decimal tax = 0;
            tax = amount.GetValueOrDefault() * (taxInPercentage.GetValueOrDefault() / 100);
            return tax;
        }

        public PageSubHeader PageSubHeaders
        {
            get { return new PageSubHeader(); }

        }
        public GridHeader GridHeaders
        {
            get
            {
                return new GridHeader();
            }
        }

        public PageLabel PageLabels
        {
            get { return new PageLabel(); }

        }
        public PageButton PageButtons
        {
            get { return new PageButton(); }

        }
        public PageDropDownItemText PageDropDownItemsText
        {
            get { return new PageDropDownItemText(); }

        }


        public class GridHeader
        {
            public string GridHeaderProductName { get; set; }
            public string GridHeaderQtyInStock { get; set; }
            public string GridHeaderQty { get; set; }
            public string GridHeaderListPrice { get; set; }
            public string GridHeaderUnitPrice { get; set; }
            public string GridHeaderTotal { get; set; }

        }

        public class PageSubHeader
        {
            public string PageSubHeaderSaleOrderInfo { get; set; }
            public string PageSubHeaderAddressInfo { get; set; }
            public string PageSubHeaderProductDetails { get; set; }
            public string PageSubHeaderDescription { get; set; }
            public string PageSubHeaderShippingAddressInfo { get; set; }
            public string PageSubHeaderBillingAddressInfo { get; set; }

        }

        public class PageLabel
        {
            public string Discount { get; set; }
            public string TotalAfterDiscount { get; set; }
            public string Tax { get; set; }
            public string SaleTax { get; set; }
            public string Vat { get; set; }
            public string Adjustment { get; set; }
            public string NetTotal { get; set; }
            public string SubTotal { get; set; }
            public string GrandTotal { get; set; }

        }

        public class PageDropDownItemText
        {
            public string NoDiscount { get; set; }
            public string PercentOfPrice { get; set; }
            public string FixedPrice { get; set; }
            public string Plus { get; set; }
            public string Minus { get; set; }

        }

        public class PageButton
        {
            public string ButtonAddProduct { get; set; }
            public string ButtonRemoveProduct { get; set; }

        }


    }
}