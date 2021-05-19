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
using AutoMapper;

namespace ErucaCRM.Business
{
    public class ProductLeadAssociationBusiness : IProductLeadAssociationBusiness
    {
        private readonly ProductLeadAssociationRepository productLeadAssociationRepository;
        public ProductLeadAssociationBusiness(IUnitOfWork _unitOfWork)
        {
            productLeadAssociationRepository = new ProductLeadAssociationRepository(_unitOfWork);
        }

        public void AddProductToLead(ProductLeadAssociationModel productLeadAssociationModel)
        {
            ProductLeadAssociation productLeadAssociation = new ProductLeadAssociation();
            Mapper.Map(productLeadAssociationModel, productLeadAssociation);
            productLeadAssociation.CreatedDate = DateTime.UtcNow;
            this.productLeadAssociationRepository.Insert(productLeadAssociation);
        }


        public void RemoveProductToLead(int leadId, int productId)
        {
            this.productLeadAssociationRepository.Delete(where => where.LeadId == leadId && where.ProductId == productId);
        }
    }
}
