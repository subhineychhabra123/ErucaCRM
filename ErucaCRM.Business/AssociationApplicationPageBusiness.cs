using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErucaCRM.Business.Interfaces;
using ErucaCRM.Repository;
using ErucaCRM.Repository.Infrastructure.Contract;

namespace ErucaCRM.Business
{
    class AssociationApplicationPageBusiness
    {
    private readonly IUnitOfWork unitOfWork;
    private readonly AssociationApplicationPageRepository associationApplicationPageRepository;
        public AssociationApplicationPageBusiness(ErucaCRM.Repository.Infrastructure.Contract.IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
            associationApplicationPageRepository = new AssociationApplicationPageRepository(unitOfWork);
        }

    
    }
}
