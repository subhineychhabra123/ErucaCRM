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
    public class InvoiceBusiness : IInvoiceBusiness
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly InvoiceRepository invoiceRepository;
        private readonly ProductInvoiceAssociationRepository productInvoiceRepository;
        public InvoiceBusiness(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            invoiceRepository = new InvoiceRepository(unitOfWork);
            productInvoiceRepository = new ProductInvoiceAssociationRepository(unitOfWork);
        }
        public void AddInvoice(InvoiceModel invoiceModel)
        {
            Invoice invoice = new Invoice();
            if (invoiceModel.InvoiceId == 0)
            {
                AutoMapper.Mapper.Map(invoiceModel, invoice);



                invoice.Address = ValidateAddress(invoice.Address, invoiceModel.AddressModel);
                invoice.Address1 = ValidateAddress(invoice.Address1, invoiceModel.AddressModel1);


                //Add Sale order products from model to entity
                ProductInvoiceAssociation objProductInvoiceAssociation;

                foreach (ProductInvoiceAssociationModel objProductInvoiceAssociationModel in invoiceModel.ProductInvoiceAssociationModels)
                {


                    objProductInvoiceAssociation = new ProductInvoiceAssociation();
                    objProductInvoiceAssociation.AssociatedProduct = new AssociatedProduct();
                    objProductInvoiceAssociation.AssociatedProduct.ProductId = objProductInvoiceAssociationModel.AssociatedProduct.ProductId;
                    objProductInvoiceAssociation.AssociatedProduct.Quantity = objProductInvoiceAssociationModel.AssociatedProduct.Quantity;
                    objProductInvoiceAssociation.AssociatedProduct.QtyInStock = objProductInvoiceAssociationModel.AssociatedProduct.QtyInStock;
                    objProductInvoiceAssociation.AssociatedProduct.UnitPrice = objProductInvoiceAssociationModel.AssociatedProduct.UnitPrice;
                    objProductInvoiceAssociation.AssociatedProduct.ListPrice = objProductInvoiceAssociationModel.AssociatedProduct.ListPrice;
                    objProductInvoiceAssociation.AssociatedProduct.DiscountType = objProductInvoiceAssociationModel.AssociatedProduct.DiscountType;
                    objProductInvoiceAssociation.AssociatedProduct.DiscountAmount = objProductInvoiceAssociationModel.AssociatedProduct.DiscountAmount;
                    objProductInvoiceAssociation.AssociatedProduct.TaxApplied = objProductInvoiceAssociationModel.AssociatedProduct.TaxApplied;
                    objProductInvoiceAssociation.AssociatedProduct.TaxAmount = objProductInvoiceAssociationModel.AssociatedProduct.TaxAmount;
                    objProductInvoiceAssociation.AssociatedProduct.VatApplied = objProductInvoiceAssociationModel.AssociatedProduct.VatApplied;
                    objProductInvoiceAssociation.AssociatedProduct.VatAmount = objProductInvoiceAssociationModel.AssociatedProduct.VatAmount;

                    invoice.ProductInvoiceAssociations.Add(objProductInvoiceAssociation);

                }

                invoiceRepository.Insert(invoice);
            }
            else
            {
                invoice = invoiceRepository.SingleOrDefault(x => x.InvoiceId == invoiceModel.InvoiceId && x.RecordDeleted == false);

                if (invoice != null)
                {
                    AutoMapper.Mapper.Map(invoiceModel, invoice);

                    invoice.Address = ValidateAddress(invoice.Address, invoiceModel.AddressModel);
                    invoice.Address1 = ValidateAddress(invoice.Address1, invoiceModel.AddressModel1);


                    foreach (ProductInvoiceAssociationModel objProductInvoiceAssociationModel in invoiceModel.ProductInvoiceAssociationModels)
                    {

                        ProductInvoiceAssociation objProductInvoiceAssociation = invoice.ProductInvoiceAssociations.Where(x => x.AssociatedProductId == objProductInvoiceAssociationModel.AssociatedProductId).FirstOrDefault();

                        if (objProductInvoiceAssociation != null)
                        {
                            objProductInvoiceAssociation.AssociatedProduct.ProductId = objProductInvoiceAssociationModel.AssociatedProduct.ProductId;
                            objProductInvoiceAssociation.AssociatedProduct.Quantity = objProductInvoiceAssociationModel.AssociatedProduct.Quantity;
                            objProductInvoiceAssociation.AssociatedProduct.QtyInStock = objProductInvoiceAssociationModel.AssociatedProduct.QtyInStock;
                            objProductInvoiceAssociation.AssociatedProduct.UnitPrice = objProductInvoiceAssociationModel.AssociatedProduct.UnitPrice;
                            objProductInvoiceAssociation.AssociatedProduct.ListPrice = objProductInvoiceAssociationModel.AssociatedProduct.ListPrice;
                            objProductInvoiceAssociation.AssociatedProduct.DiscountType = objProductInvoiceAssociationModel.AssociatedProduct.DiscountType;
                            objProductInvoiceAssociation.AssociatedProduct.DiscountAmount = objProductInvoiceAssociationModel.AssociatedProduct.DiscountAmount;
                            objProductInvoiceAssociation.AssociatedProduct.TaxApplied = objProductInvoiceAssociationModel.AssociatedProduct.TaxApplied;
                            objProductInvoiceAssociation.AssociatedProduct.TaxAmount = objProductInvoiceAssociationModel.AssociatedProduct.TaxAmount;
                            objProductInvoiceAssociation.AssociatedProduct.VatApplied = objProductInvoiceAssociationModel.AssociatedProduct.VatApplied;
                            objProductInvoiceAssociation.AssociatedProduct.VatAmount = objProductInvoiceAssociationModel.AssociatedProduct.VatAmount;



                        }
                        else
                        {
                            objProductInvoiceAssociation = new ProductInvoiceAssociation();
                            objProductInvoiceAssociation.AssociatedProduct = new AssociatedProduct();
                            objProductInvoiceAssociation.AssociatedProduct.ProductId = objProductInvoiceAssociationModel.AssociatedProduct.ProductId;
                            objProductInvoiceAssociation.AssociatedProduct.Quantity = objProductInvoiceAssociationModel.AssociatedProduct.Quantity;
                            objProductInvoiceAssociation.AssociatedProduct.QtyInStock = objProductInvoiceAssociationModel.AssociatedProduct.QtyInStock;
                            objProductInvoiceAssociation.AssociatedProduct.UnitPrice = objProductInvoiceAssociationModel.AssociatedProduct.UnitPrice;
                            objProductInvoiceAssociation.AssociatedProduct.ListPrice = objProductInvoiceAssociationModel.AssociatedProduct.ListPrice;
                            objProductInvoiceAssociation.AssociatedProduct.DiscountType = objProductInvoiceAssociationModel.AssociatedProduct.DiscountType;
                            objProductInvoiceAssociation.AssociatedProduct.DiscountAmount = objProductInvoiceAssociationModel.AssociatedProduct.DiscountAmount;
                            objProductInvoiceAssociation.AssociatedProduct.TaxApplied = objProductInvoiceAssociationModel.AssociatedProduct.TaxApplied;
                            objProductInvoiceAssociation.AssociatedProduct.TaxAmount = objProductInvoiceAssociationModel.AssociatedProduct.TaxAmount;
                            objProductInvoiceAssociation.AssociatedProduct.VatApplied = objProductInvoiceAssociationModel.AssociatedProduct.VatApplied;
                            objProductInvoiceAssociation.AssociatedProduct.VatAmount = objProductInvoiceAssociationModel.AssociatedProduct.VatAmount;

                            invoice.ProductInvoiceAssociations.Add(objProductInvoiceAssociation);

                        }


                    }

                    invoiceRepository.Update(invoice);
                }
            }


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
        public InvoiceModel GetInvoiceDetail(int invoiceId)
        {
            InvoiceModel invoiceModel = new InvoiceModel();
            Invoice invoice = invoiceRepository.SingleOrDefault(x => x.InvoiceId == invoiceId && x.RecordDeleted == false);

            AutoMapper.Mapper.Map(invoice.Address, invoiceModel.AddressModel);
            AutoMapper.Mapper.Map(invoice.Address1, invoiceModel.AddressModel1);
            AutoMapper.Mapper.Map(invoice.ProductInvoiceAssociations, invoiceModel.ProductInvoiceAssociationModels);

            AutoMapper.Mapper.Map(invoice, invoiceModel);
            return invoiceModel;

        }
        public List<InvoiceModel> GetInvoiceList(int companyId, int leadId)
        {
            List<InvoiceModel> invoiceModelList = new List<InvoiceModel>();
            List<Invoice> invoiceList = invoiceRepository.GetAll(x => x.CompanyId == companyId && x.RecordDeleted == false && x.LeadId == leadId).ToList();
            AutoMapper.Mapper.Map(invoiceList, invoiceModelList);
            return invoiceModelList;
        }

        public List<InvoiceModel> GetInvoicesByCompanyId(int companyId, int currentPage, int pageSize, ref int totalRecords)
        {
            List<InvoiceModel> invoiceModelList = new List<InvoiceModel>();
            totalRecords = invoiceRepository.Count(x => x.CompanyId == companyId && x.RecordDeleted == false);
            List<Invoice> invoiceList = invoiceRepository.GetPagedRecords(x => x.CompanyId == companyId && x.RecordDeleted == false, y => y.Subject, currentPage, pageSize).ToList();
            AutoMapper.Mapper.Map(invoiceList, invoiceModelList);
            return invoiceModelList;
        }

        public bool DeleteInvoice(int invoiceId, int companyId, int userId)
        {
            Invoice invoice = new Invoice();
            invoice = invoiceRepository.SingleOrDefault(r => r.InvoiceId == invoiceId && r.CompanyId == companyId && r.RecordDeleted == false);
            //code for deleting record from productinvoiceassociation

            if (invoice != null)
            {
                foreach (ProductInvoiceAssociation association in invoice.ProductInvoiceAssociations)
                {
                    association.InvoiceId = null;
                    association.RecordDeleted = true;
                }

                invoice.RecordDeleted = true;
                invoice.ModifiedBy = userId;
                invoice.ModifiedDate = DateTime.UtcNow;
                invoiceRepository.Update(invoice);
                return true;
            }
            else
                return false;
        }
    }
}
