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
    public class LeadCommentBussiness : ILeadCommentBussiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly LeadCommentRepository leadCommentRepository;
        public LeadCommentBussiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            leadCommentRepository = new LeadCommentRepository(unitOfWork);
        }
        public List<LeadCommentModel> GetCommentsByLeadId(int leadId, int CurrentPage, int PageSize, ref int totalrecord)
        {
            List<LeadCommentModel> leadCommentModellist = new List<LeadCommentModel>();
            totalrecord = leadCommentRepository.Count(c => c.LeadId == leadId);
            List<LeadComment> leadCommentlist = leadCommentRepository.GetPagedRecords(c => c.LeadId == leadId, x => x.LeadCommentId, CurrentPage > 0 ? CurrentPage : 1, PageSize).ToList();
            AutoMapper.Mapper.Map(leadCommentlist, leadCommentModellist);
            return leadCommentModellist;
        }

        public LeadCommentModel AddCommentInLead(LeadCommentModel leadCommentModel)
        {
            int NewCommentId=0;
            LeadComment leadComment=new LeadComment();
            leadCommentModel.CreatedDate = DateTime.UtcNow;
            AutoMapper.Mapper.Map(leadCommentModel, leadComment);
           leadCommentRepository.Insert(leadComment);
           AutoMapper.Mapper.Map(leadComment, leadCommentModel);
           return leadCommentModel;
        }
    }
}
