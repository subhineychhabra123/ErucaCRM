using ErucaCRM.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Business.Interfaces
{
    public interface IAccountCaseBusiness
    {
        void AddUpdateCase(AccountCaseModel caseModel, int companyId);

        AccountCaseModel GetCaseByCaseId(int caseId);
        string GetCaseOwnerName(int ownerId);
        AccountCaseModel GetCase(int caseId);
        IList<AccountCaseModel> GetCases(int accountId, int currentPage, int pageSize, ref int totalRecords);
        //IList<ParentCaseListModel> GetParentCaseListByCompanyID(int companyId);
        AccountCaseModel GetAccountCaseInfo(int accountCaseId);
        AccountCaseModel CaseDetail(int caseId);
        bool DeleteCase(int caseId, int userId);
        bool DeleteAccountCase(int caseId, int accountId, int userId);
        List<AccountCaseModel> GetAccountCasesByUserId(int userId, int currentPage, int pageSize, ref int totalRecords, string sortColumnName,string sortDir);
        List<AccountCaseModel> GetDashBoardAccountCases(int companyId, int userId);
    }
}
