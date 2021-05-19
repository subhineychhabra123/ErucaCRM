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
    public class ProductBusiness : IProductBusiness
    {
        private readonly ProductRepository productRepository;

        public ProductBusiness(IUnitOfWork _unitOfWork)
        {
            productRepository = new ProductRepository(_unitOfWork);
        }

        public List<ProductModel> GetProductDropDownList(int companyId)
        {
            List<ProductModel> productModelList = new List<ProductModel>();
            List<Product> productList = productRepository.GetAll(x => x.CompanyId == companyId && x.RecordDeleted == false).ToList();
            AutoMapper.Mapper.Map(productList, productModelList);
            return productModelList;
        }

        public ProductModel GetProductDetail(int productId)
        {

            ProductModel productModel = new ProductModel();
            Product product = productRepository.SingleOrDefault(x => x.ProductId == productId && x.RecordDeleted == false);
            AutoMapper.Mapper.Map(product, productModel);
            return productModel;

        }

        public IList<ProductModel> GetNonAssociatedProducts(int companyId, int? leadId)
        {
            List<ProductModel> productModelList = new List<ProductModel>();
            List<Product> productList = productRepository.GetAll(x => x.CompanyId == companyId && x.RecordDeleted == false && x.ProductLeadAssociations.Any(y => y.ProductId == x.ProductId && y.LeadId == (leadId.HasValue ? leadId : y.LeadId)) == false).ToList();
            AutoMapper.Mapper.Map(productList, productModelList);
            return productModelList;
        }
        public int AddNewProduct(ProductModel productModel)
        {
            Product product = new Product();
            AutoMapper.Mapper.Map(productModel, product);
            productRepository.Insert(product);
            return product.ProductId;

        }

        public List<ProductModel> GetLeadProducts(int companyId, int LeadId, int currentPage, int pageSize, ref int totalRecords)
        {

            List<ProductModel> productModelList = new List<ProductModel>();

            totalRecords = productRepository.GetAll(x => x.CompanyId == companyId && x.RecordDeleted == false && x.ProductLeadAssociations.Any(y => y.LeadId == LeadId)).Count();
            List<Product> productList = productRepository.GetPagedRecords(x => x.CompanyId == companyId && x.RecordDeleted == false && x.ProductLeadAssociations.Any(y => y.LeadId == LeadId),y=>y.CreatedDate,currentPage>0?currentPage:1,pageSize).ToList();

            
            AutoMapper.Mapper.Map(productList, productModelList);
            return productModelList;

        }


     public List<ProductModel> GetProductsByCompanyId(int companyId,int leadId, int currentPage, int pageSize, ref int totalRecords)
        {
            List<ProductModel> productModelList = new List<ProductModel>();

            totalRecords = productRepository.Count(x => x.CompanyId == companyId && x.RecordDeleted == false && x.ProductLeadAssociations.Any(y => y.LeadId == leadId) == false);
            List<Product> productList = productRepository.GetPagedRecords(x => x.CompanyId == companyId && x.RecordDeleted == false && x.ProductLeadAssociations.Any(y => y.LeadId == leadId) == false, y => y.CreatedDate, currentPage > 0 ? currentPage : 1, pageSize).ToList();
            AutoMapper.Mapper.Map(productList, productModelList);
            return productModelList;
        }

    }
}
