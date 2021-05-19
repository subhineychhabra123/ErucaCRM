using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErucaCRM.Domain;
using ErucaCRM.Repository;

namespace ErucaCRM.Business.Interfaces
{
    public interface ISalesOrderBusiness
    {
        void AddSalesOrder(SalesOrderModel salesOrderModel);
        List<SalesOrderModel> GetSalesOrderDropDownList(int companyId);
        SalesOrderModel GetSalesOrderDetail(int salesOrderId);
        List<SalesOrderModel> GetSalesOrderList(int companyId, int leadId);
     
        List<SalesOrderModel> GetSalesOrdersByCompanyId(int userid, int companyId, int currentPage, int pageSize, ref int totalRecords, string sortColumnName, string sortdir);
        Boolean DeleteSaleOrder(int saleOrderId, int userId);
    }
}
