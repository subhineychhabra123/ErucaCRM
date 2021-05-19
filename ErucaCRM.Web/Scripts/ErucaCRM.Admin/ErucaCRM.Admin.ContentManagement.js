jQuery.namespace('ErucaCRM.Admin.ContentManagement');

var viewModel;
var currentPage = 1;
var filterBy = "Active";
ErucaCRM.Admin.ContentManagement.pageLoad = function () {
    viewModel = new ErucaCRM.Admin.ContentManagement.pageViewModel();
    ko.applyBindings(viewModel);

}
ErucaCRM.Admin.ContentManagement.CultureListQueryModel = function (data) {
   
    var self = this;
    self.CultureInformationId = data.CultureInformationId;
    self.CultureName = data.CultureName;
    self.Language = data.Language;
    self.Status = data.IsActive==true?"Active":"In-Active"
    self.DefaultLanguage = data.IsDefault==true?data.Language:"";
    self.IsDefault = data.IsDefault;
    self.IsActive = data.IsActive;
  
};
ErucaCRM.Admin.ContentManagement.pageViewModel = function (currentPage, filterBy) {
    //Class variables
    var self = this;
    var objCultureInfo = new Object();
    objCultureInfo.CurrentPage = currentPage;
    objCultureInfo.FilterParameters = new Array();
    var PagingMethodName = "GetPageData";

    var controllerUrl = "/Admin/";
    var Message = {
        Failure: 'Failure',
        Success: 'Success'
    }
    $("._attachDoc").bind("click", function () {
        uploadObj.startUpload();
    });

    self.CulturListQueryModal = ko.observableArray();
    self.queryModel = ko.observable();
    self.status = ko.observable();
    self.CultureList = ko.observableArray([]);

    self.getCultureList = function (objLeadInfo) {
        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetCultures", objLeadInfo,
            function onSuccess(response) {
                self.RenderCultures(response);
             
            }, function onError(err) {
                ErucaCRM.Framework.Core.ShowMessage(err.Message, true);
            });
    }
    

    self.RenderCultures = function (CultureListData) {
        $("#divNoRecord").hide();
        $("#LeadListData").children().remove();
        if (CultureListData.List.length == 0) {
            $("#divNoRecord").html("<b>No lead found for the selected status</b>");
            $("#divNoRecord").show();
        }
        self.CultureList.removeAll();
        ko.utils.arrayForEach(CultureListData.List, function (Lead) {
            self.CultureList.push(new ErucaCRM.Admin.ContentManagement.CultureListQueryModel(Lead));
        });

        $("#Pager").html(self.GetPaging(CultureListData.TotalRecords, currentPage, PagingMethodName));
    };

    self.GetPageData = function (currentPageNo) {

        currentPage = currentPageNo;
        var objCultureInfo = new Object();
        objCultureInfo.CurrentPage = currentPageNo;
        objCultureInfo.FilterParameters = new Array();
        self.getCultureList(objCultureInfo);

    }
    self.Navigate=function(cultureInformationId)
    {
        location.href = "ManageLanguage?id_encrypted=" + cultureInformationId;
    }
    self.setDefaultLanguage = function (cultureInformationId, IsActive) {
        if (IsActive == true) {
            if (confirm("Are you sure to change the default language to selected language?")) {
                var data = new Object();
                data.id_encrypted = cultureInformationId;
                ErucaCRM.Framework.Core.doPostOperation(controllerUrl + "UpdateDefaultLanguage", data, function onSuccess(response) {
                    var objCultureInfo = new Object();
                    objCultureInfo.CurrentPage = 1;
                    objCultureInfo.FilterParameters = new Array();
                    self.getCultureList(objCultureInfo);
                });

            }
        }
        else {
            ErucaCRM.Framework.Core.ShowMessage("You can't select a In-Active language to default language.", true);
        }
    }

    self.GetPaging = function (Rowcount, currentPage, methodName) {
        return ErucaCRM.Framework.Core.GetPagger(Rowcount, currentPage, methodName);
    }

   

    self.getCultureList(objCultureInfo);


}