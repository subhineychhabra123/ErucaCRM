using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErucaCRM.Domain;
using ErucaCRM.Repository;
using System.Data;

namespace ErucaCRM.Business.Interfaces
{
    public interface ICultureInformationBusiness
    {
        List<CultureInformationModel> GetUserCultures();
        void EditDocument(CultureInformationModel cultureinfomodel);
        List<ErucaCRM.Domain.CultureInformationModel> LoadAllUserCultures();
        List<CultureInformationModel> GetAllCultures();
        CultureInformationModel GetCultureDetails(int CultureInformationId);
        void UpdateDefaultLanguage(int CultureInformationId);
        List<CultureInformationModel> GetAllCultureNames();
        void SaveLanguage(CultureInformationModel cultureInformationModel);
        CultureInformationModel GetDefaultCultureDetail();
        string ProcessCultureSpecificData(DataSet ds, string cultureName);
    }
}
