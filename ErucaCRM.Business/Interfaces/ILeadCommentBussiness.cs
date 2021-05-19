using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErucaCRM.Domain;

namespace ErucaCRM.Business.Interfaces
{
    public interface ILeadCommentBussiness
    {
        List<LeadCommentModel> GetCommentsByLeadId(int leadId, int CurrentPage, int PageSize, ref int totalrecord);
        LeadCommentModel AddCommentInLead(LeadCommentModel leadCommentModel);
    }
}
