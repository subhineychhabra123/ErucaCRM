//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ErucaCRM.Repository
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProductInvoiceAssociation
    {
        public int AssociationInvoiceId { get; set; }
        public Nullable<int> AssociatedProductId { get; set; }
        public Nullable<int> InvoiceId { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool RecordDeleted { get; set; }
    
        public virtual AssociatedProduct AssociatedProduct { get; set; }
        public virtual Invoice Invoice { get; set; }
    }
}
