using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Domain
{
    public class TaskItemModel
    {
        public string AudioFileName { get; set; }
        public string AudioFileDuration { get; set; }
        public int LeadId { get; set; }

        public int TaskId { get; set; }
        public String Subject { get; set; }
        public int Status { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime EndDate { get; set; }
        public int OwnerId { get; set; }
        public string OwnerName { get; set; }
        public int PriorityId { get; set; }
        public string PriorityName {
            get
            {
                return Enum.GetName(typeof(Utility.Enums.TaskPriority), this.PriorityId);
            }
        }
        public string TaskStatus {

            get
            {
                return Enum.GetName(typeof(Utility.Enums.TaskStaus), this.Status);

            }
        
        }
        public String Description { get; set; }
        public int AssociatedModuleId { get; set; }
        public int AssociatedModuleValue { get; set; }
        public int CompanyId { get; set; }
        public int CreatedBy { get; set; }
        public string OwnerImage { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Boolean RecordDeleted { get; set; }


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
    }
}


