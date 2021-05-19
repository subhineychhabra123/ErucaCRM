using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using ErucaCRM.Utility;
namespace ErucaCRM.Domain
{
   public class CaseMessageBoardModel
    {
        public int CaseMessageBoardId { get; set; }
        public int AccountCaseId { get; set; }
        public string Description { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedDateTimeText
        {
            get
            {
                return CreatedDate.ToDateTimeNow().ToString("MMMM dd, yyyy H:mm:ss");
            }
        }
       [ScriptIgnore]
        public virtual AccountCaseModel AccountCase { get; set; }
        public virtual ICollection<FileAttachmentModel> FileAttachments { get; set; }
        public virtual UserModel User { get; set; }
    }
}
