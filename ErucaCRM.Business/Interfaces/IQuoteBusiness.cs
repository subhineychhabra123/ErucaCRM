using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErucaCRM.Domain;
using ErucaCRM.Repository;

namespace ErucaCRM.Business.Interfaces
{
    public interface IQuoteBusiness
    {
        void AddQuote(QuoteModel quoteModel);
        List<QuoteModel> GetQuoteDropDownList(int companyId);
        QuoteModel GetQuoteDetail(int quoteId);
        List<QuoteModel> GetQuoteList(int companyId, int leadId);
        List<QuoteModel> GetQuotesByCompanyId(int companyId, int currentPage, int pageSize, ref int totalRecords);
        bool DeleteQuote(int quoteId, int? companyId,int userId);
   
    }
}
