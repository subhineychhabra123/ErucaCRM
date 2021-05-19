using ErucaCRM.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Business.Interfaces
{
    public interface ICaseMessageBoardBusiness
    {
        CaseMessageBoardModel Add(CaseMessageBoardModel caseMessageBoardModel);
       bool Update(CaseMessageBoardModel caseMessageBoardModel);
       List<CaseMessageBoardModel> GetCaseMessageBoardMessages(int accountCaseId, int currentPage, int pageSize, ref int totalRecords);
    }
}
