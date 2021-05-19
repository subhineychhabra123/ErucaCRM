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
    public class QuoteBusiness : IQuoteBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly QuoteRepository quoteRepository;
        private readonly SalesOrderRepository salesOrderRepository;
        private readonly ProductQuoteAssociationRepository productQuoteAssociationRepository;
        public QuoteBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            quoteRepository = new QuoteRepository(unitOfWork);
            salesOrderRepository = new SalesOrderRepository(unitOfWork);
            productQuoteAssociationRepository = new ProductQuoteAssociationRepository(unitOfWork);
        }

        public void AddQuote(QuoteModel quoteModel)
        {
            Quote quote = new Quote();

            // quote.Address = ValidateAddress(quote.Address);
            // quote.Address1 = ValidateAddress(quote.Address1);
            if (quoteModel.QuoteId > 0)
            {

                quote = quoteRepository.SingleOrDefault(x => x.QuoteId == quoteModel.QuoteId && x.RecordDeleted == false);

                AutoMapper.Mapper.Map(quoteModel, quote);
                foreach (ProductQuoteAssociationModel objProductQuoteAssociationModel in quoteModel.ProductQuoteAssociationModels)
                {

                    ProductQuoteAssociation objProductQuoteAssociation = quote.ProductQuoteAssociations.Where(x => x.AssociatedProductId == objProductQuoteAssociationModel.AssociatedProductId).FirstOrDefault();

                    if (objProductQuoteAssociation != null)
                    {
                        objProductQuoteAssociation.AssociatedProduct.ProductId = objProductQuoteAssociationModel.AssociatedProduct.ProductId;
                        objProductQuoteAssociation.AssociatedProduct.Quantity = objProductQuoteAssociationModel.AssociatedProduct.Quantity;
                        objProductQuoteAssociation.AssociatedProduct.QtyInStock = objProductQuoteAssociationModel.AssociatedProduct.QtyInStock;
                        objProductQuoteAssociation.AssociatedProduct.UnitPrice = objProductQuoteAssociationModel.AssociatedProduct.UnitPrice;
                        objProductQuoteAssociation.AssociatedProduct.ListPrice = objProductQuoteAssociationModel.AssociatedProduct.ListPrice;
                        objProductQuoteAssociation.AssociatedProduct.DiscountType = objProductQuoteAssociationModel.AssociatedProduct.DiscountType;
                        objProductQuoteAssociation.AssociatedProduct.DiscountAmount = objProductQuoteAssociationModel.AssociatedProduct.DiscountAmount;
                        objProductQuoteAssociation.AssociatedProduct.TaxApplied = objProductQuoteAssociationModel.AssociatedProduct.TaxApplied;
                        objProductQuoteAssociation.AssociatedProduct.TaxAmount = objProductQuoteAssociationModel.AssociatedProduct.TaxAmount;
                        objProductQuoteAssociation.AssociatedProduct.VatApplied = objProductQuoteAssociationModel.AssociatedProduct.VatApplied;
                        objProductQuoteAssociation.AssociatedProduct.VatAmount = objProductQuoteAssociationModel.AssociatedProduct.VatAmount;



                    }
                    else
                    {
                        objProductQuoteAssociation = new ProductQuoteAssociation();
                        objProductQuoteAssociation.AssociatedProduct = new AssociatedProduct();
                        objProductQuoteAssociation.AssociatedProduct.ProductId = objProductQuoteAssociationModel.AssociatedProduct.ProductId;
                        objProductQuoteAssociation.AssociatedProduct.Quantity = objProductQuoteAssociationModel.AssociatedProduct.Quantity;
                        objProductQuoteAssociation.AssociatedProduct.QtyInStock = objProductQuoteAssociationModel.AssociatedProduct.QtyInStock;
                        objProductQuoteAssociation.AssociatedProduct.UnitPrice = objProductQuoteAssociationModel.AssociatedProduct.UnitPrice;
                        objProductQuoteAssociation.AssociatedProduct.ListPrice = objProductQuoteAssociationModel.AssociatedProduct.ListPrice;
                        objProductQuoteAssociation.AssociatedProduct.DiscountType = objProductQuoteAssociationModel.AssociatedProduct.DiscountType;
                        objProductQuoteAssociation.AssociatedProduct.DiscountAmount = objProductQuoteAssociationModel.AssociatedProduct.DiscountAmount;
                        objProductQuoteAssociation.AssociatedProduct.TaxApplied = objProductQuoteAssociationModel.AssociatedProduct.TaxApplied;
                        objProductQuoteAssociation.AssociatedProduct.TaxAmount = objProductQuoteAssociationModel.AssociatedProduct.TaxAmount;
                        objProductQuoteAssociation.AssociatedProduct.VatApplied = objProductQuoteAssociationModel.AssociatedProduct.VatApplied;
                        objProductQuoteAssociation.AssociatedProduct.VatAmount = objProductQuoteAssociationModel.AssociatedProduct.VatAmount;

                        quote.ProductQuoteAssociations.Add(objProductQuoteAssociation);

                    }

                }

                quoteRepository.Update(quote);
            }
            else
            {

                AutoMapper.Mapper.Map(quoteModel, quote);


                //Add quote products from model to entity
                ProductQuoteAssociation objProductQuoteAssociation;

                foreach (ProductQuoteAssociationModel objProductQuoteAssociationModel in quoteModel.ProductQuoteAssociationModels)
                {


                    objProductQuoteAssociation = new ProductQuoteAssociation();
                    objProductQuoteAssociation.AssociatedProduct = new AssociatedProduct();
                    objProductQuoteAssociation.AssociatedProduct.ProductId = objProductQuoteAssociationModel.AssociatedProduct.ProductId;
                    objProductQuoteAssociation.AssociatedProduct.Quantity = objProductQuoteAssociationModel.AssociatedProduct.Quantity;
                    objProductQuoteAssociation.AssociatedProduct.QtyInStock = objProductQuoteAssociationModel.AssociatedProduct.QtyInStock;
                    objProductQuoteAssociation.AssociatedProduct.UnitPrice = objProductQuoteAssociationModel.AssociatedProduct.UnitPrice;
                    objProductQuoteAssociation.AssociatedProduct.ListPrice = objProductQuoteAssociationModel.AssociatedProduct.ListPrice;
                    objProductQuoteAssociation.AssociatedProduct.DiscountType = objProductQuoteAssociationModel.AssociatedProduct.DiscountType;
                    objProductQuoteAssociation.AssociatedProduct.DiscountAmount = objProductQuoteAssociationModel.AssociatedProduct.DiscountAmount;
                    objProductQuoteAssociation.AssociatedProduct.TaxApplied = objProductQuoteAssociationModel.AssociatedProduct.TaxApplied;
                    objProductQuoteAssociation.AssociatedProduct.TaxAmount = objProductQuoteAssociationModel.AssociatedProduct.TaxAmount;
                    objProductQuoteAssociation.AssociatedProduct.VatApplied = objProductQuoteAssociationModel.AssociatedProduct.VatApplied;
                    objProductQuoteAssociation.AssociatedProduct.VatAmount = objProductQuoteAssociationModel.AssociatedProduct.VatAmount;

                    quote.ProductQuoteAssociations.Add(objProductQuoteAssociation);

                }



                quoteRepository.Insert(quote);
            }
        }


        private Address ValidateAddress(Address address)
        {
            if (address.Street == null && address.City == null && address.CountryId == 0 && address.State == null)
                address = null;
            else if (address.CountryId.HasValue && address.CountryId == 0)
                address.CountryId = null;

            if (address != null)
            {
                address.Country = null;
            }
            return address;
        }
        public List<QuoteModel> GetQuoteDropDownList(int companyId)
        {
            List<Quote> quoteList = quoteRepository.GetAll(x => x.CompanyId == companyId && x.RecordDeleted == false).ToList();
            List<QuoteModel> quoteModelList = new List<QuoteModel>();
            AutoMapper.Mapper.Map(quoteList, quoteModelList);
            quoteModelList.Insert(0, new QuoteModel { QuoteId = 0, Subject = Constants.CULTURE_SPECIFIC_DROPDOWNS_SELECT_OPTION });
            return quoteModelList;
        }
        public QuoteModel GetQuoteDetail(int quoteId)
        {
            QuoteModel quoteModel = new QuoteModel();
            Quote quote = quoteRepository.SingleOrDefault(x => x.QuoteId == quoteId && x.RecordDeleted == false);
            AutoMapper.Mapper.Map(quote, quoteModel);
            AutoMapper.Mapper.Map(quote.User, quoteModel.UserModel);
            AutoMapper.Mapper.Map(quote.Lead, quoteModel.LeadModel);
            AutoMapper.Mapper.Map(quote.ProductQuoteAssociations, quoteModel.ProductQuoteAssociationModels);
            return quoteModel;
        }
        public List<QuoteModel> GetQuoteList(int companyId, int leadId)
        {
            List<QuoteModel> quoteModelList = new List<QuoteModel>();
            List<Quote> quoteList = quoteRepository.GetAll(x => x.CompanyId == companyId && x.RecordDeleted == false && x.LeadId == leadId).ToList();
            AutoMapper.Mapper.Map(quoteList, quoteModelList);
            return quoteModelList;
        }

        public List<QuoteModel> GetQuotesByCompanyId(int companyId, int currentPage, int pageSize, ref int totalRecords)
        {
            List<QuoteModel> quoteModelList = new List<QuoteModel>();
            totalRecords = quoteRepository.Count(x => x.CompanyId == companyId && x.RecordDeleted == false);
            List<Quote> quoteList = quoteRepository.GetPagedRecords(x => x.CompanyId == companyId && x.RecordDeleted == false, y => y.Subject, currentPage, pageSize).ToList();

            AutoMapper.Mapper.Map(quoteList, quoteModelList);
            return quoteModelList;
        }
        public bool DeleteQuote(int quoteId, int? companyId, int userId)
        {
            Quote quote = new Quote();
            List<ProductQuoteAssociation> productQuotes = new List<ProductQuoteAssociation>();
            quote = quoteRepository.SingleOrDefault(r => r.QuoteId == quoteId && r.CompanyId == companyId && r.RecordDeleted == false);
            if (quote != null)
            {

                foreach (SalesOrder saleorder in quote.SalesOrders)
                {
                    saleorder.QuoteId = null;
                    saleorder.ModifiedBy = userId;
                    saleorder.ModifiedDate = DateTime.UtcNow;
                }

                //code for deletingrecord from productquoteassociation
                foreach (ProductQuoteAssociation association in quote.ProductQuoteAssociations)
                {
                    association.QuoteId = null;
                    association.RecordDeleted = true;
                }

                quote.RecordDeleted = true;
                quote.ModifiedBy = userId;
                quote.ModifiedDate = DateTime.UtcNow;
                quoteRepository.Update(quote);
                return true;
            }
            else
                return false;
        }
    }
}
