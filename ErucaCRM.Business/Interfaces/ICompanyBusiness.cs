using ErucaCRM.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErucaCRM.Business.Interfaces
{
  public  interface ICompanyBusiness
    {
        List<CompanyModel> GetOrgnaisations(int currentPage, Boolean companyStatus, Int16 pageSize,int AdminCompanyId, ref long totalRecords);
       bool UpdateCompanyStatus(int[] CompanyIds, Boolean userStatus, int ownerUserID);

       CompanyModel GetCompanyDetail(int CompanyId);
    }
}
