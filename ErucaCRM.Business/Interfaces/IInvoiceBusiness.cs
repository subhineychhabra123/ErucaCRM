using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErucaCRM.Domain;
using ErucaCRM.Repository;
namespace ErucaCRM.Business.Interfaces
{
    public interface IInvoiceBusiness
    {
        void AddInvoice(InvoiceModel invoiceModel);
        InvoiceModel GetInvoiceDetail(int invoiceId);
        List<InvoiceModel> GetInvoiceList(int companyId, int leadId);
        List<InvoiceModel> GetInvoicesByCompanyId(int companyId, int currentPage, int pageSize, ref int totalRecords);
        bool DeleteInvoice(int invoiceId, int companyId, int userId);
    
    }
}
