using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ErucaCRM.Utility;
using ErucaCRM.Business.Interfaces;
using ErucaCRM.Business;
using ErucaCRM.Repository.Infrastructure.Contract;
using ErucaCRM.Repository.Infrastructure;
using ErucaCRM.Domain;

namespace ErucaCRM.Web.ViewModels
{
    public static class LayoutHelper
    {
        public static string Module { get { return "Layout"; } }

        public static string ButtonLogin { get { return CommonFunctions.GetGlobalizedLabel(Module, "ButtonLogin"); } }
        public static string ButtonSignUp { get { return CommonFunctions.GetGlobalizedLabel(Module, "ButtonSignUp"); } }

        public static string SearchWaterMark { get { return CommonFunctions.GetGlobalizedLabel(Module, "SearchWaterMark"); } }
        public static class Menus
        {
            public static string Home
            {
                get { return CommonFunctions.GetGlobalizedLabel(Module, "Home"); }

            }


        }

      
       
       


        

        public static string GetActiveCultureIcons()
        {
            string _cultureIconHtml = "";
            IUnitOfWork unitOfWork = new UnitOfWork();
            ICultureInformationBusiness CultureInfoBusiness = new CultureInformationBusiness(unitOfWork);
            List<CultureInformationModel> listCultureInformationModel = CultureInfoBusiness.GetAllCultureNames();
            string culturename = "";
            _cultureIconHtml = "<select id='ddlGlobalCulture'>";
            for (int i = 0; i < listCultureInformationModel.Count; i++)
            {
                culturename = listCultureInformationModel[i].CultureName;
                if (CultureInformationManagement.CurrentUserCulture == culturename)
                {
                    //_cultureIconHtml = _cultureIconHtml + "&nbsp; <a href='#' class='socialicon'><img height='16px' width='24px' title='" + listCultureInformationModel[i].Language + "' src='/Uploads/CultureIcons/" + culturename + ".png' Culture='" + culturename + "' onclick='javascript:ErucaCRM.Framework.Core.SetCulture(this)' /></a>";
                    _cultureIconHtml = _cultureIconHtml + "<Option selected='selected'  value=" + culturename + ">"  + listCultureInformationModel[i].Language + "</option>";
                }
                else
                {
                    _cultureIconHtml = _cultureIconHtml + "<Option value=" + culturename + ">" + listCultureInformationModel[i].Language + "</option>";
                }

            }

            _cultureIconHtml = _cultureIconHtml + "</select>";
            return _cultureIconHtml;
        }



    }





}