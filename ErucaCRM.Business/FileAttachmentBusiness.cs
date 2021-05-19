using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErucaCRM.Repository;
using ErucaCRM.Business.Interfaces;
using ErucaCRM.Repository.Infrastructure.Contract;
using ErucaCRM.Domain;
using ErucaCRM.Utility;

namespace ErucaCRM.Business
{
    public class FileAttachmentBusiness : IFileAttachmentBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly FileAttachmentRepository fileAttachmentRepository;
        public FileAttachmentBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            fileAttachmentRepository = new FileAttachmentRepository(unitOfWork);
        }
        public FileAttachmentModel AddDocument(FileAttachmentModel fileAttachmentModel)
        {
            FileAttachment fileAttachment = new FileAttachment();
            AutoMapper.Mapper.Map(fileAttachmentModel, fileAttachment);
            fileAttachmentRepository.Insert(fileAttachment);
            AutoMapper.Mapper.Map(fileAttachment, fileAttachmentModel);
            return fileAttachmentModel;
        }

        public FileAttachmentModel UpdateDocument(FileAttachmentModel fileAttachmentModel)
        {
            FileAttachment fileAttachment = new FileAttachment();
            AutoMapper.Mapper.Map(fileAttachmentModel, fileAttachment);
            fileAttachmentRepository.Update(fileAttachment);
            AutoMapper.Mapper.Map(fileAttachment, fileAttachmentModel);
            return fileAttachmentModel;
        }
        public void RemoveDocument(FileAttachmentModel fileAttachmentModel)
        {
            fileAttachmentRepository.Delete(where => where.DocumentId == fileAttachmentModel.DocumentId);
        }
        public FileAttachmentModel GetDocument(FileAttachmentModel fileAttachmentModel)
        {
            FileAttachmentModel fileAttModel = new FileAttachmentModel();

            FileAttachment fileAttachment = fileAttachmentRepository.SingleOrDefault(where => where.DocumentId == fileAttachmentModel.DocumentId);
            AutoMapper.Mapper.Map(fileAttachment, fileAttModel);
            return fileAttModel;
        }

        //public long GetLeadAudioDocId(int leadId)
        //{
        //    FileAttachmentModel fileAttModel = new FileAttachmentModel();
        //    long DocId = fileAttachmentRepository.(where => where.LeadId == leadId);
        //    return DocId;
        //}
      
        public IList<FileAttachmentModel> GetAttachmentsByAccountCaseId(int accountCaseId, int currentPage, int pageSize, ref int totalRecords)
        {
            List<FileAttachmentModel> fileAttachmentModelList = new List<FileAttachmentModel>();
            List<FileAttachment> fileAttachmentList = new List<FileAttachment>();
            totalRecords = fileAttachmentRepository.Count(x => x.AccountCaseId == accountCaseId && x.RecordDeleted == false);
            fileAttachmentList = fileAttachmentRepository.GetPagedRecords(x => x.AccountCaseId == accountCaseId && x.RecordDeleted == false, y => y.DocumentId, currentPage > 0 ? currentPage : 1, pageSize).ToList();
            AutoMapper.Mapper.Map(fileAttachmentList, fileAttachmentModelList);
            return fileAttachmentModelList;
        }
   

        public List<FileAttachmentModel> GetLeadDocuments(int companyId, int LeadId, int currentPage, int pageSize, ref int totalRecords)
        {
            List<FileAttachmentModel> fileattachmentModelList = new List<FileAttachmentModel>();
            totalRecords = fileAttachmentRepository.Count(x => x.CompanyId == companyId && x.RecordDeleted == false && x.LeadId == LeadId);
            List<FileAttachment> fileattachmentList = fileAttachmentRepository.GetPagedRecords(x => x.CompanyId == companyId && x.RecordDeleted == false && x.LeadId == LeadId, y => y.CreatedDate, currentPage > 0 ? currentPage : 1, pageSize).ToList(); ;
            AutoMapper.Mapper.Map(fileattachmentList, fileattachmentModelList);
            return fileattachmentModelList;

        }

    }

}

