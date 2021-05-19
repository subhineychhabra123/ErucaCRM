using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErucaCRM.Domain;
using ErucaCRM.Repository;

namespace ErucaCRM.Business.Interfaces
{
    public interface IProductBusiness
    {
        List<ProductModel> GetProductDropDownList(int companyId);
        IList<ProductModel> GetNonAssociatedProducts(int companyId, int? leadId);
        ProductModel GetProductDetail(int productId);
        int AddNewProduct(ProductModel productModel);
        List<ProductModel> GetLeadProducts(int companyId, int LeadId, int currentPage, int pageSize, ref int totalRecords);
        List<ProductModel> GetProductsByCompanyId(int companyId,int leadId, int currentPage, int pageSize, ref int totalRecords);
    
    }
}
