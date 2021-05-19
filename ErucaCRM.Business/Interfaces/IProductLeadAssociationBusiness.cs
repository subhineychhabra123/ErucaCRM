using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErucaCRM.Domain;
using ErucaCRM.Repository;

namespace ErucaCRM.Business.Interfaces
{
    public interface IProductLeadAssociationBusiness
    {
        void AddProductToLead(ProductLeadAssociationModel productLeadAssociationModel);
        void RemoveProductToLead(int leadId, int productId);
       
    }
}
