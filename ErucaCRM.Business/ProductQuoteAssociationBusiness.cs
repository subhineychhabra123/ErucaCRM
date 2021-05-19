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
  public  class ProductQuoteAssociationBusiness : IProductQuoteAssociationBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ProductQuoteAssociationRepository productQuoteAssociationRepository;
        public ProductQuoteAssociationBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            productQuoteAssociationRepository = new ProductQuoteAssociationRepository(unitOfWork);
        }

        public void RemoveQuoteFromProduct(int quoteId)
        {
            this.productQuoteAssociationRepository.Delete(where => where.QuoteId == quoteId);
        }
    }
}
