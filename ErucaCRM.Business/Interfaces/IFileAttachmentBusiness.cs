using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErucaCRM.Domain;
using ErucaCRM.Repository;

namespace ErucaCRM.Business.Interfaces
{
    public interface IFileAttachmentBusiness
    {
        FileAttachmentModel AddDocument(FileAttachmentModel fileAttachmentModel);
        void RemoveDocument(FileAttachmentModel fileAttachmentModel);
        FileAttachmentModel UpdateDocument(FileAttachmentModel fileAttachmentModel);
        IList<FileAttachmentModel> GetAttachmentsByAccountCaseId(int accountCaseId, int currentPage, int pageSize, ref int totalRecords);
        List<FileAttachmentModel> GetLeadDocuments(int companyId, int LeadId, int currentPage, int pageSize, ref int totalRecords);
       
    }
}
