using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Domain
{
    public class ContactModel
    {
        /// <summary>
        /// Gets or sets the ContactId value.
        /// </summary>
        public virtual Int32 ContactId { get; set; }


        public virtual Int32? AccountId { get; set; }
        public virtual Int32? LeadId { get; set; }

        /// <summary>
        /// Gets or sets the OwnerId value.
        /// </summary>
        public virtual Int32 OwnerId { get; set; }
        public virtual String JobPosition { get; set; }
      
        /// <summary>
        /// Gets or sets the OwnerName value.
        /// </summary>
        public virtual string OwnerName { get; set; }
        public string NewTagNames { get; set; }
        public string ContactTagIds { get; set; }
        /// <summary>
        /// Gets or sets the LeadSourceId value.
        /// </summary>
        public virtual Int32 LeadSourceId { get; set; }

        /// <summary>
        /// Gets or sets the FirstName value.
        /// </summary>
        public virtual String FirstName { get; set; }

        /// <summary>
        /// Gets or sets the LastName value.
        /// </summary>
        public virtual String LastName { get; set; }

        /// <summary>
        /// Gets or sets the DOB value.
        /// </summary>
        public virtual DateTime? DOB { get; set; }

        /// <summary>
        /// Gets or sets the EmailAddress value.
        /// </summary>
        public virtual String EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the Title value.
        /// </summary>
        public virtual String Title { get; set; }

        /// <summary>
        /// Gets or sets the Phone value.
        /// </summary>
        public virtual String Phone { get; set; }

        /// <summary>
        /// Gets or sets the HomePhone value.
        /// </summary>
        public virtual String HomePhone { get; set; }

        /// <summary>
        /// Gets or sets the OtherPhone value.
        /// </summary>
        public virtual String OtherPhone { get; set; }

        /// <summary>
        /// Gets or sets the Mobile value.
        /// </summary>
        public virtual String Mobile { get; set; }

        /// <summary>
        /// Gets or sets the Fax value.
        /// </summary>
        public virtual String Fax { get; set; }

        /// <summary>
        /// Gets or sets the Department value.
        /// </summary>
        public virtual String Department { get; set; }
        public string ContactCompanyName { get; set; }
        /// <summary>
        /// Gets or sets the Assistant value.
        /// </summary>
        public virtual String Assistant { get; set; }

        /// <summary>
        /// Gets or sets the ReportsTo value.
        /// </summary>
        public virtual String ReportsTo { get; set; }

        /// <summary>
        /// Gets or sets the AddressId value.
        /// </summary>
        public virtual Int32? AddressId { get; set; }

        /// <summary>
        /// Gets or sets the AsstPhone value.
        /// </summary>
        public virtual String AsstPhone { get; set; }

        /// <summary>
        /// Gets or sets the Description value.
        /// </summary>
        public virtual String Description { get; set; }

        /// <summary>
        /// Gets or sets the CompanyId value.
        /// </summary>
        public virtual Int32 CompanyId { get; set; }

        /// <summary>
        /// Gets or sets the CreatedBy value.
        /// </summary>
        public virtual Int32 CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets the ModifiedBy value.
        /// </summary>
        public virtual Int32 ModifiedBy { get; set; }

        /// <summary>
        /// Gets or sets the CreatedDate value.
        /// </summary>
        public virtual DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the ModifiedDate value.
        /// </summary>
        public virtual DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Gets or sets the RecordDeleted value.
        /// </summary>
        public virtual Boolean RecordDeleted { get; set; }

        public virtual string Tags { get; set; }
        private string _contactName;
        public string ContactName
        {

            get
            {
               return ((this.FirstName ?? "") + " " + (this.LastName ?? ""));
            
            }
            set
            {
                 this._contactName= value;
            }
        }

        private AddressModel _address;
        public virtual AddressModel AddressModel
        {
            get
            {
                if (this._address == null)
                    this._address = new AddressModel();
                return this._address;
            }
            set { this._address = value; }
        }
        private UserModel _user;
        public virtual UserModel UserModel
        {
            get
            {
                if (this._user == null)
                    this._user = new UserModel();
                return this._user;
            }
            set { this._user = value; }
        }

        private ICollection<FileAttachmentModel> _FileAttachmentModels;
        public virtual ICollection<FileAttachmentModel> FileAttachmentModels
        {
            get
            {
                if (this._FileAttachmentModels == null)
                    this._FileAttachmentModels = new List<FileAttachmentModel>();
                return this._FileAttachmentModels;
            }
            set { this._FileAttachmentModels = value; }
        }
        public virtual ICollection<ContactTagModel> ContactTagModels { get; set; }
        //public virtual ICollection<AccountContactModel> AccountContacts { get; set; }
    
    }
}