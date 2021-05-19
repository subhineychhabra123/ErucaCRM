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
    public class SalesOrderBusiness : ISalesOrderBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private SalesOrderRepository salesOrderRepository;
        public SalesOrderBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            salesOrderRepository = new SalesOrderRepository(unitOfWork);
        }
        public void AddSalesOrder(SalesOrderModel salesOrderModel)
        {
            SalesOrder salesOrder = new SalesOrder();



            if (salesOrderModel.SalesOrderId == 0)
            {
                AutoMapper.Mapper.Map(salesOrderModel, salesOrder);

                //salesOrder.Address = ValidateAddress(salesOrder.Address);
                //salesOrder.Address1 = ValidateAddress(salesOrder.Address1);
                salesOrder.Address = ValidateAddress(salesOrder.Address, salesOrderModel.AddressModel);
                //salesOrder.Address1 = ValidateAddress(salesOrder.Address1, salesOrderModel.AddressModel1);

                //Add Sale order products from model to entity
                ProductSalesOrderAssociation objProductSalesOrderAssociation;

                foreach (ProductSalesOrderAssociationModel objProductSalesOrderAssociationModel in salesOrderModel.ProductSalesOrderAssociationModels)
                {


                    objProductSalesOrderAssociation = new ProductSalesOrderAssociation();
                    objProductSalesOrderAssociation.AssociatedProduct = new AssociatedProduct();
                    objProductSalesOrderAssociation.AssociatedProduct.ProductId = objProductSalesOrderAssociationModel.AssociatedProduct.ProductId;
                    objProductSalesOrderAssociation.AssociatedProduct.Quantity = objProductSalesOrderAssociationModel.AssociatedProduct.Quantity;
                    objProductSalesOrderAssociation.AssociatedProduct.QtyInStock = objProductSalesOrderAssociationModel.AssociatedProduct.QtyInStock;
                    objProductSalesOrderAssociation.AssociatedProduct.UnitPrice = objProductSalesOrderAssociationModel.AssociatedProduct.UnitPrice;
                    objProductSalesOrderAssociation.AssociatedProduct.ListPrice = objProductSalesOrderAssociationModel.AssociatedProduct.ListPrice;
                    objProductSalesOrderAssociation.AssociatedProduct.DiscountType = objProductSalesOrderAssociationModel.AssociatedProduct.DiscountType;
                    objProductSalesOrderAssociation.AssociatedProduct.DiscountAmount = objProductSalesOrderAssociationModel.AssociatedProduct.DiscountAmount;
                    objProductSalesOrderAssociation.AssociatedProduct.TaxApplied = objProductSalesOrderAssociationModel.AssociatedProduct.TaxApplied;
                    objProductSalesOrderAssociation.AssociatedProduct.TaxAmount = objProductSalesOrderAssociationModel.AssociatedProduct.TaxAmount;
                    objProductSalesOrderAssociation.AssociatedProduct.VatApplied = objProductSalesOrderAssociationModel.AssociatedProduct.VatApplied;
                    objProductSalesOrderAssociation.AssociatedProduct.VatAmount = objProductSalesOrderAssociationModel.AssociatedProduct.VatAmount;

                    salesOrder.ProductSalesOrderAssociations.Add(objProductSalesOrderAssociation);

                }

                salesOrderRepository.Insert(salesOrder);
            }
            else
            {

                salesOrder = salesOrderRepository.SingleOrDefault(x => x.SalesOrderId == salesOrderModel.SalesOrderId && x.RecordDeleted == false);
                if (salesOrder != null)
                {


                    AutoMapper.Mapper.Map(salesOrderModel, salesOrder);

                    salesOrder.Address = ValidateAddress(salesOrder.Address, salesOrderModel.AddressModel);
                    //salesOrder.Address1 = ValidateAddress(salesOrder.Address1, salesOrderModel.AddressModel1);

                    foreach (ProductSalesOrderAssociationModel objProductSalesOrderAssociationModel in salesOrderModel.ProductSalesOrderAssociationModels)
                    {

                        ProductSalesOrderAssociation objProductSalesOrderAssociation = salesOrder.ProductSalesOrderAssociations.Where(x => x.AssociatedProductId == objProductSalesOrderAssociationModel.AssociatedProductId).FirstOrDefault();

                        if (objProductSalesOrderAssociation != null)
                        {
                            objProductSalesOrderAssociation.AssociatedProduct.ProductId = objProductSalesOrderAssociationModel.AssociatedProduct.ProductId;
                            objProductSalesOrderAssociation.AssociatedProduct.Quantity = objProductSalesOrderAssociationModel.AssociatedProduct.Quantity;
                            objProductSalesOrderAssociation.AssociatedProduct.QtyInStock = objProductSalesOrderAssociationModel.AssociatedProduct.QtyInStock;
                            objProductSalesOrderAssociation.AssociatedProduct.UnitPrice = objProductSalesOrderAssociationModel.AssociatedProduct.UnitPrice;
                            objProductSalesOrderAssociation.AssociatedProduct.ListPrice = objProductSalesOrderAssociationModel.AssociatedProduct.ListPrice;
                            objProductSalesOrderAssociation.AssociatedProduct.DiscountType = objProductSalesOrderAssociationModel.AssociatedProduct.DiscountType;
                            objProductSalesOrderAssociation.AssociatedProduct.DiscountAmount = objProductSalesOrderAssociationModel.AssociatedProduct.DiscountAmount;
                            objProductSalesOrderAssociation.AssociatedProduct.TaxApplied = objProductSalesOrderAssociationModel.AssociatedProduct.TaxApplied;
                            objProductSalesOrderAssociation.AssociatedProduct.TaxAmount = objProductSalesOrderAssociationModel.AssociatedProduct.TaxAmount;
                            objProductSalesOrderAssociation.AssociatedProduct.VatApplied = objProductSalesOrderAssociationModel.AssociatedProduct.VatApplied;
                            objProductSalesOrderAssociation.AssociatedProduct.VatAmount = objProductSalesOrderAssociationModel.AssociatedProduct.VatAmount;



                        }
                        else
                        {
                            objProductSalesOrderAssociation = new ProductSalesOrderAssociation();
                            objProductSalesOrderAssociation.AssociatedProduct = new AssociatedProduct();
                            objProductSalesOrderAssociation.AssociatedProduct.ProductId = objProductSalesOrderAssociationModel.AssociatedProduct.ProductId;
                            objProductSalesOrderAssociation.AssociatedProduct.Quantity = objProductSalesOrderAssociationModel.AssociatedProduct.Quantity;
                            objProductSalesOrderAssociation.AssociatedProduct.QtyInStock = objProductSalesOrderAssociationModel.AssociatedProduct.QtyInStock;
                            objProductSalesOrderAssociation.AssociatedProduct.UnitPrice = objProductSalesOrderAssociationModel.AssociatedProduct.UnitPrice;
                            objProductSalesOrderAssociation.AssociatedProduct.ListPrice = objProductSalesOrderAssociationModel.AssociatedProduct.ListPrice;
                            objProductSalesOrderAssociation.AssociatedProduct.DiscountType = objProductSalesOrderAssociationModel.AssociatedProduct.DiscountType;
                            objProductSalesOrderAssociation.AssociatedProduct.DiscountAmount = objProductSalesOrderAssociationModel.AssociatedProduct.DiscountAmount;
                            objProductSalesOrderAssociation.AssociatedProduct.TaxApplied = objProductSalesOrderAssociationModel.AssociatedProduct.TaxApplied;
                            objProductSalesOrderAssociation.AssociatedProduct.TaxAmount = objProductSalesOrderAssociationModel.AssociatedProduct.TaxAmount;
                            objProductSalesOrderAssociation.AssociatedProduct.VatApplied = objProductSalesOrderAssociationModel.AssociatedProduct.VatApplied;
                            objProductSalesOrderAssociation.AssociatedProduct.VatAmount = objProductSalesOrderAssociationModel.AssociatedProduct.VatAmount;

                            salesOrder.ProductSalesOrderAssociations.Add(objProductSalesOrderAssociation);

                        }


                    }
                    salesOrderRepository.Update(salesOrder);
                }
            }
        }

        public Boolean DeleteSaleOrder(int saleOrderId, int userId)
        {

            SalesOrder salesOrder = salesOrderRepository.SingleOrDefault(x => x.SalesOrderId == saleOrderId && x.RecordDeleted == false);

            if (salesOrder != null)
            {
                salesOrder.RecordDeleted = true;
                salesOrder.ModifiedBy = userId;
                salesOrder.ModifiedDate = DateTime.UtcNow;

                foreach (Invoice objInvoice in salesOrder.Invoices)
                {
                    objInvoice.SalesOrderId = null;

                }

                salesOrderRepository.Update(salesOrder);
                return true;
            }
            return false;

        }


        private Address ValidateAddress(Address oldAddress, AddressModel newAddress)
        {
            Address objAddress = null;
            //if Address is already there then update the address;
            if (oldAddress != null && newAddress != null)
            {

                oldAddress.Street = newAddress.Street;
                oldAddress.City = newAddress.City;
                oldAddress.State = newAddress.State;
                oldAddress.Zipcode = newAddress.Zipcode;

                if (newAddress.CountryId.GetValueOrDefault() > 0)
                    oldAddress.CountryId = newAddress.CountryId.GetValueOrDefault();
                else
                    oldAddress.CountryId = null;
                return oldAddress;


            }
            else if (newAddress != null)
            {
                if (newAddress.Street != null || newAddress.City != null || newAddress.Zipcode != null || newAddress.State != null || newAddress.CountryId > 0)
                {
                    objAddress = new Address();

                    objAddress.Street = newAddress.Street;
                    objAddress.City = newAddress.City;
                    objAddress.State = newAddress.State;
                    objAddress.Zipcode = newAddress.Zipcode;

                    if (newAddress.CountryId.GetValueOrDefault() > 0)
                        objAddress.CountryId = newAddress.CountryId.GetValueOrDefault();
                    else
                        objAddress.CountryId = null;
                    return objAddress;
                }

            }

            return objAddress;
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
        public List<SalesOrderModel> GetSalesOrderDropDownList(int companyId)
        {
            List<SalesOrder> quoteList = salesOrderRepository.GetAll(x => x.CompanyId == companyId && x.RecordDeleted == false).ToList();
            List<SalesOrderModel> salesOrderModelList = new List<SalesOrderModel>();
            AutoMapper.Mapper.Map(quoteList, salesOrderModelList);
            salesOrderModelList.Insert(0, new SalesOrderModel { QuoteId = 0, Subject = Constants.CULTURE_SPECIFIC_DROPDOWNS_SELECT_OPTION });
            return salesOrderModelList;
        }
        public SalesOrderModel GetSalesOrderDetail(int salesOrderId)
        {
            salesOrderRepository = new SalesOrderRepository(unitOfWork);

            SalesOrderModel salesOrderModel = new SalesOrderModel();
            SalesOrder salesOrder = salesOrderRepository.SingleOrDefault(x => x.SalesOrderId == salesOrderId && x.RecordDeleted == false);

            AutoMapper.Mapper.Map(salesOrder.Address, salesOrderModel.AddressModel);
            AutoMapper.Mapper.Map(salesOrder.Account, salesOrderModel.AccountModel);
            //AutoMapper.Mapper.Map(salesOrder.Address1, salesOrderModel.AddressModel1);
            AutoMapper.Mapper.Map(salesOrder.ProductSalesOrderAssociations, salesOrderModel.ProductSalesOrderAssociationModels);

            AutoMapper.Mapper.Map(salesOrder, salesOrderModel);
            return salesOrderModel;
        }
        public List<SalesOrderModel> GetSalesOrderList(int companyId, int accountId)
        {
            List<SalesOrderModel> salesOrderModelList = new List<SalesOrderModel>();
            List<SalesOrder> salesOrderList = salesOrderRepository.GetAll(x => x.CompanyId == companyId && x.RecordDeleted == false && x.AccountId == accountId).ToList();
            AutoMapper.Mapper.Map(salesOrderList, salesOrderModelList);
            return salesOrderModelList;
        }

//        public List<SalesOrderModel> GetSalesOrdersByCompanyId(int userid, int companyId, int currentPage, int pageSize, ref int totalRecords)
//        {
//            List<SalesOrderModel> salesOrderModelList = new List<SalesOrderModel>();

//          //  totalRecords = salesOrderRepository.Count(x => x.CompanyId == companyId && x.RecordDeleted == false);
//           // List<SalesOrder> salesOrderList = salesOrderRepository.GetPagedRecordsDecending(x => x.CompanyId == companyId && x.RecordDeleted == false, y => y.CreatedDate, currentPage, pageSize).ToList();
//            List<SSP_SalesOrderListbyUserId_Result> salesOrder = salesOrderRepository.GetSalesOrderListByUserId(userid, companyId, currentPage, pageSize, ref totalRecords);
//           // AutoMapper.Mapper.Map(salesOrderList, salesOrderModelList);
//AutoMapper.Mapper.Map(salesOrder, salesOrderModelList);
//            return salesOrderModelList;


//        }
        //pankaj pandey
        public List<SalesOrderModel> GetSalesOrdersByCompanyId(int userid, int companyId, int currentPage, int pageSize, ref int totalRecords, string sortColumnName, string sortdir)
        {
            List<SalesOrderModel> salesOrderModelList = new List<SalesOrderModel>();

            //  totalRecords = salesOrderRepository.Count(x => x.CompanyId == companyId && x.RecordDeleted == false);
            // List<SalesOrder> salesOrderList = salesOrderRepository.GetPagedRecordsDecending(x => x.CompanyId == companyId && x.RecordDeleted == false, y => y.CreatedDate, currentPage, pageSize).ToList();
            List<SSP_SalesOrderListbyUserId_Result> salesOrder = salesOrderRepository.GetSalesOrderListByUserId(userid, companyId, currentPage, pageSize, ref totalRecords, sortColumnName, sortdir);
            // AutoMapper.Mapper.Map(salesOrderList, salesOrderModelList);
            AutoMapper.Mapper.Map(salesOrder, salesOrderModelList);
            return salesOrderModelList;


        }



    }
}
