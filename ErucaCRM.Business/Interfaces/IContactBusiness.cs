using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErucaCRM.Domain;
using ErucaCRM.Repository;
using System.Data;

namespace ErucaCRM.Business.Interfaces
{
    public interface IContactBusiness
    {
        int AddContact(ContactModel contact);

        List<ContactBulkUploadModel> BulkInsertContact(DataTable dt, int UserId, int CompanyId);
        ContactModel GetContactByContactId(int contactId);
        void UpdateContact(ContactModel contact);
        List<ContactModel> GetAllContacts(int companyId, int currentPage, Int16 pageSize, int currentOwnerID, string tagName, int tagId, string filterBy, ref int totalRecords);
        List<ContactModel> GetAllContactsByTagSearch(int companyId, int currentPage, Int16 pageSize, int currentOwnerID, string searchTags, ref int totalRecords);
        List<ContactModel> GetTaggedContacts(int tagId, int companyId, int currentPage, Int16 pageSize, ref int totalRecords);
        IList<ContactModel> GetContactsByOwnerIdAndCompanyID(int companyId, int ownerId, string tagName = "", int tagId = 0, int currentPage = 1, int pagesize = 0);
        List<AccountContactModel> GetContactByAccountId(int userId,int companyId, int accountId);
        Boolean DeleteAccountContact(int contactId, int accountId,int userId);
        Boolean DeleteLeadContact(int contactId, int accountId, int userId);
        List<TagModel> GetMostUsedContactTags(int companyId);
        List<ContactModel> GetTaggedContactSearchByName(string tagSearchName, int companyId, int currentPage, Int16 pageSize, ref int totalRecords);
        List<ContactModel> GetContactList(int UserId, int CompanyId, int filterId = 0, int curruntPage = 1, int pageSize = 0, int tagId = 0, string TagName = "");
        void AssociateLeadToContact(int LeadId, int ContactId,int UserId);

        List<ContactModel> GetContactByLeadId(int userId, int CompanyId, int LeadId, int CurrentPage, int pagesize, ref int totalrecord, string tagName = "", int tagId = 0);
        void AssociateAccountToContact(List<AccountContactModel> accountcontactlist,int createdBy);
        List<ContactModel> NonAssociatedContactList(int companyId, int currentPage, Int16 pageSize, int currentOwnerID, string filterBy, int AccountId, ref int totalRecords);
        bool IsLeadContactExists(int companyId, int LeadId);
        bool DeleteContact(int contactId, int userId);
        void AssociateLeadToContact(List<LeadContactModel> leadContactModelList, int createdBy);
        void DeleteFileFromServerAfterUpload(string FilePath);

        List<ContactModel> GetAssociatedContactByLeadId(int userId, int leadId, int companyId, int currentPage, int pageSize, ref int totalrecords);
    }
    
}
