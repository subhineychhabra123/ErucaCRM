using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErucaCRM.Domain;

namespace ErucaCRM.Business.Interfaces
{
    public interface IAccountBusiness
    {

        void AddUpdateAccount(AccountModel accountModel);
        List<AccountModel> GetAccountsByOwnerIdAndCompanyID(int companyId, int ownerId);
        IList<ParentAccountListModel> GetParentAccountListByCompanyID(int companyId);
        IList<AccountListModel> GetAccountListByCompanyID(int companyId,int userid);
        IList<AccountTypeModel> GetAccountTypes();
        AccountModel AccountDetail(int accountId);
        List<AccountModel> GetAccounts(int companyId, int currentPage, int pageSize, ref int totalRecords);
        bool DeleteAccount(int accountId, int userId);
        List<AccountModel> GetChildAccount(int companyId, int accountId);
        List<AccountModel> GetTaggedAccounts(int tagId, int companyId, int currentPage, int pageSize, ref int totalRecords);
        List<AccountModel> GetTaggedAccountSearchByName(string tagSearchName, int companyId, int currentPage, Int16 pageSize, ref int totalRecords);
        List<TagModel> GetMostUsedAccountTags(int companyId);
        bool DeleteMemberAccount(int memberAccountId, int parentAccountId, int userId);
        List<AccountModel> GetAccountsByUserId(int companyId, int userId, int? tagId, string tagName, int currentPage, int pageSize, ref int totalRecords);
    }
}
