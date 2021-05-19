jQuery.namespace('ErucaCRM.Admin.EditApplicationPage');

var viewModel;
var viewAllCustomPages = false;
//var filterBy = "Active";
ErucaCRM.Admin.EditApplicationPage.pageLoad = function () {
    viewAllCustomPages = false;
    viewModel = new ErucaCRM.Admin.EditApplicationPage.pageViewModel();
    ko.applyBindings(viewModel);

}
ErucaCRM.Admin.EditApplicationPage.EditApplicationQueryModel = function (data) {

    var self = this;
    self.PageTitle = data.PageTitle;

    self.RemoveAction = data.RemoveAction;
    self.Action = data.Action + $("#PageTitle").val();
    //  self.CultureInformationId = data.CultureInformationId;
    self.CustomPageId = data.CustomPageId;
    self.ApplicationPageId = data.ApplicationPageId;
    self.UrlEdit = "EditApplicationPage?id_encrypted=" + $("#CultureInformationId").val() + "&pageId_encrypted=" + data.CustomPageId + "&returnId=" + $('#ApplicationPageId').val() + "&redirectedFrom=" + $('#PageTitle').val();
    self.UrlDelete = "#";
    self.ApplicationPageUrl = $('#PageTitle').val()

    self.PageUrl = $('#PageTitle').val();
    self.NavigateText = data.RemoveAction == true ? "Click Here" : "",
    self.NavigateUrl = "/" + $('#PageTitle').val() + "/" + data.PageUrl;
};
ErucaCRM.Admin.EditApplicationPage.pageViewModel = function () {
    //Class variables
    var self = this;
    var controllerUrl = "/Admin/";
    var Message = {
        Failure: 'Failure',
        Success: 'Success'
    }

    var objApplicationPageInfo = new Object();
    objApplicationPageInfo.ApplicationPageId = $("#ApplicationPageId").val();
    objApplicationPageInfo.CultureInformationId = $("#CultureInformationId").val();
    objApplicationPageInfo.IsViewAll = viewAllCustomPages;

    self.EditApplicationQueryModel = ko.observableArray();
    //self.queryModel = ko.observable();
    self.status = ko.observable();
    self.CustomPageList = ko.observableArray([]);

    self.getCustomPageList = function (objApplicationPageInfo) {
        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetCustomPageList", objApplicationPageInfo,
            function onSuccess(response) {

                self.RenderCustomPagesList(response);

            }, function onError(err) {
                ErucaCRM.Framework.Core.ShowMessage(err.Message, true);
            });
    }

    self.getCustomPageList(objApplicationPageInfo);

    self.RenderCustomPagesList = function (CustomPagesList) {
        $("#CustomPageListData").html("");

        ko.utils.arrayForEach(CustomPagesList.CustomPageList, function (CustomPages) {
            self.CustomPageList.push(new ErucaCRM.Admin.EditApplicationPage.EditApplicationQueryModel(CustomPages));
        });

    };

    self.AddRemoveCustomPage = function (obj) {


        var objApplicationPageInfo = new Object();
        objApplicationPageInfo.ApplicationPageId = $(obj).attr("ApplicationPageId");
        objApplicationPageInfo.CultureInformationId = $("#CultureInformationId").val();
        objApplicationPageInfo.CustomPageId = $(obj).attr("CustomPageId");//$("#CultureInformationId").val();
        objApplicationPageInfo.IsViewAll = viewAllCustomPages;

        if ($(obj).attr("RemoveAction") == "true") {
            if (confirm("Are you sure you want to De-Associate this page from the " + $('#PageTitle').val() + " page.")) {
                ErucaCRM.Framework.Core.doPostOperation(controllerUrl + "RemoveCustomPage", objApplicationPageInfo, function onSuccess(response) {
                    self.SuccessfullyRemovedCustomPage(response);

                }, function onError(err) {
                    self.status(err.Message);
                });
            }
            else return false;
        }
        else {

            ErucaCRM.Framework.Core.doPostOperation(controllerUrl + "AddCustomPageToApplicationPage", objApplicationPageInfo, function onSuccess(response) {
                self.SuccessfullyAddedCustomPage(response);

            }, function onError(err) {
                self.status(err.Message);
            });

        }
    }

    self.SuccessfullyRemovedCustomPage = function (response) {

        if (response.Message == "true") {
            var objApplicationPageInfo = new Object();
            objApplicationPageInfo.ApplicationPageId = $("#ApplicationPageId").val();
            objApplicationPageInfo.CultureInformationId = $("#CultureInformationId").val();
            objApplicationPageInfo.IsViewAll = viewAllCustomPages;
            self.getCustomPageList(objApplicationPageInfo);
        }
    }

    self.SuccessfullyAddedCustomPage = function (response) {
        if (response.Message == "true") {
            var objApplicationPageInfo = new Object();
            objApplicationPageInfo.ApplicationPageId = $("#ApplicationPageId").val();
            objApplicationPageInfo.CultureInformationId = $("#CultureInformationId").val();
            objApplicationPageInfo.IsViewAll = viewAllCustomPages;
            self.getCustomPageList(objApplicationPageInfo);
        }

    }
    self.DeleteCustomPage = function (obj) {
        var objApplicationPageInfo = new Object();
        objApplicationPageInfo.pageId_encrypted = $(obj).attr('CustomPageId');
        if (confirm("Are you sure you want to delete this page?")) {
            ErucaCRM.Framework.Core.doDeleteOperation(controllerUrl + "DeleteCustomPage", objApplicationPageInfo, function onSuccess(response) {
                ErucaCRM.Framework.Core.ShowMessage(response.message, false);
                var objApplicationPageInfo = new Object();
                objApplicationPageInfo.ApplicationPageId = $("#ApplicationPageId").val();
                objApplicationPageInfo.CultureInformationId = $("#CultureInformationId").val();
                objApplicationPageInfo.IsViewAll = viewAllCustomPages;
                self.getCustomPageList(objApplicationPageInfo);
            }, function onError(err) {
                self.status(err.Message);
            });
        }
    }
    $('._viewAll').click(function () {
        if ($(this).attr('viewAll') == "true") {
            viewAllCustomPages = true;
            $(this).text("View Associated Pages");
            $(this).attr('viewAll', "false");
        }
        else {
            viewAllCustomPages = false;
            $(this).text("View All Pages");
            $(this).attr('viewAll', "true");
        }
        var objApplicationPageInfo = new Object();
        objApplicationPageInfo.ApplicationPageId = $("#ApplicationPageId").val();
        objApplicationPageInfo.CultureInformationId = $("#CultureInformationId").val();
        objApplicationPageInfo.IsViewAll = viewAllCustomPages;
        self.getCustomPageList(objApplicationPageInfo);

    });
    self.Navigate = function (PageName, TitleName) {
        var HostName = document.location.host;
        NavigateUrl = HostName + "/" + PageName + "/" + TitleName;
        window.open(NavigateUrl);

    }
    $('._useDefault').click(function () {

        if ($(this).val() == "UseDefault") {
            $('._default').val("");
            $('#UseDefault').val("True");
            $('._default').attr('disabled', 'disabled');
            $('#optionContent').css({ opacity: .5 });
           
        }
        else {
            $('#UseDefault').val("False");
            $('._default').removeAttr('disabled');
            $('#optionContent').css({ opacity: 1 });
        }
    });
    if ($('#UseDefault').val() == "True") {
        $('#radioDefault').attr('checked', true)
        $('._default').attr('disabled', 'disabled');
        $('#optionContent').css({ opacity: .5 });
    }
    else {
        $('#radioCustom').attr('checked', true)
        $('._default').removeAttr('disabled');
        $('#UseDefault').css({ opacity: 1 });

    }
}