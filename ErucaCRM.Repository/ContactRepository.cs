using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErucaCRM.Repository.Infrastructure;
using ErucaCRM.Repository.Infrastructure.Contract;
using System.Data.Objects;

namespace ErucaCRM.Repository
{
    public class ContactRepository : BaseRepository<Contact>
    {
        public ContactRepository(IUnitOfWork unit)
            : base(unit)
        {

        }
        public bool DeleteContact(int contactId)
        {
            Entities entities = (Entities)this.UnitOfWork.Db;
            entities.SSP_DeleteContact(contactId);
            return true;
        }

        public List<SSP_GetContactForLeadAccountContacts_Result> GetContactsByUserId(int userId, int companyId, string tagName, int tagId, string filter, int currentPage, int pageSize, out int totalrecords)
        {
            Entities entities = (Entities)this.UnitOfWork.Db;
            ObjectParameter objParam = new ObjectParameter("totalrecords", 0);
            List<SSP_GetContactForLeadAccountContacts_Result> ContactList = entities.SSP_GetContactForLeadAccountContacts(userId, companyId,0, filter, currentPage, pageSize, tagName, tagId, objParam).ToList();
            totalrecords = Convert.ToInt32(objParam.Value);
            return ContactList;
        }

        public List<SSP_GetContactForLeadAccountContacts_Result> GetContactsByLeadId(int userId, int leadId, string filterby, int companyId, int currentPage, int pageSize,string tagName,int tagid, out int totalrecords)
        {
            Entities entities = (Entities)this.UnitOfWork.Db;
            ObjectParameter objParam = new ObjectParameter("totalrecords", 0);
            List<SSP_GetContactForLeadAccountContacts_Result> ContactList = entities.SSP_GetContactForLeadAccountContacts(userId, companyId, leadId, filterby, currentPage, pageSize, tagName, tagid, objParam).ToList();
            totalrecords = Convert.ToInt32(objParam.Value);
            return ContactList;
        }
        public List<SSP_GetContactsByLeadId_Result> GetAssociatedContactByLeadId(int leadId,int userId, int companyId, int currentPage, int pageSize, out int totalrecords)
        {
            Entities entities = (Entities)this.UnitOfWork.Db;
            ObjectParameter objParam = new ObjectParameter("totalrecords", 0);
            List<SSP_GetContactsByLeadId_Result> ContactList = entities.SSP_GetContactsByLeadId(leadId,userId,companyId,currentPage,pageSize, objParam).ToList();
            totalrecords = Convert.ToInt32(objParam.Value);
            return ContactList;
        }


        public List<SSP_NonAssociatedContactList_Result> NonAssociatedContactList(int companyId, int currentPage, Int16 pageSize, int currentOwnerID, string filterBy, int filterId, out int totalRecords)
        {
            Entities entities = (Entities)this.UnitOfWork.Db;
            ObjectParameter objParam = new ObjectParameter("totalrecords", 0);
            List<SSP_NonAssociatedContactList_Result> ContactList = entities.SSP_NonAssociatedContactList(currentOwnerID, companyId, filterBy, filterId, currentPage, pageSize, objParam).ToList();
            totalRecords = Convert.ToInt32(objParam.Value);
            return ContactList;
        }

        public List<SSP_GetContactForLeadAccountContacts_Result> GetContactsList(int userId, int companyId,int filterId ,string filterby,int curruntPage,int PageSize,int TagId,string TagName)
        {
            Entities entities = (Entities)this.UnitOfWork.Db;
               ObjectParameter objParam = new ObjectParameter("totalrecords", 0);
           List<SSP_GetContactForLeadAccountContacts_Result> ContactList = entities.SSP_GetContactForLeadAccountContacts(userId, companyId,filterId,filterby,curruntPage,PageSize,TagName,TagId,objParam).ToList();
            return ContactList;
        }

     

    }
}
