using System;

namespace ErucaCRM.Domain
{
    public class ProductLeadAssociationModel
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
		public virtual Int32 LeadId { get; set; }

		/// <summary>
		/// Gets or sets the CreatedBy value.
		/// </summary>
		public virtual Int32 CreatedBy { get; set; }

		/// <summary>
		/// Gets or sets the ModifiedBy value.
		/// </summary>
		public virtual Int32? ModifiedBy { get; set; }

		/// <summary>
		/// Gets or sets the CreatedDate value.
		/// </summary>
		public virtual DateTime CreatedDate { get; set; }

		/// <summary>
		/// Gets or sets the ModifiedDate value.
		/// </summary>
		public virtual DateTime? ModifiedDate { get; set; }

		/// <summary>
		/// Gets or sets the RecordDeleted value.
		/// </summary>
		public virtual Boolean RecordDeleted { get; set; }

        private ProductModel _ProductModel;
        public virtual ProductModel Product
        {
            get
            {
                if (this._ProductModel == null)
                    this._ProductModel = new ProductModel();
                return this._ProductModel;
            }
            set { this._ProductModel = value; }
        }
    }
}