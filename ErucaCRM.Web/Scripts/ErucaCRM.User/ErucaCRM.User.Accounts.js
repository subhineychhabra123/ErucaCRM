/// <reference path="ErucaCRM.User.TaskItem.js" />
jQuery.namespace('ErucaCRM.User.Accounts');

var viewModel;
var currentPage = 1;
var TagId = "";
var TagSearchName = "";
var CurrentTagName = "";
ErucaCRM.User.Accounts.pageLoad = function () {

    currentPage = 1;
    TagId = "";
    TagSearchName = "";
    CurrentTagName = "";
    viewModel = new ErucaCRM.User.Accounts.pageViewModel();
    ko.applyBindings(viewModel, document.getElementById("InnerContentContainer"));
  
}





// A view model that represent a Test report query model.


ErucaCRM.User.Accounts.TagListQueryModel = function (data) {

    var self = this;
    self.TagId = data.TagId;
    self.TagName = data.TagName;

}
// A view model that represent a Test report query model.
ErucaCRM.User.Accounts.AccountListQueryModel = function (data) {
  
    var self = this;
    self.AccountId = data.AccountId;

    self.OwnerId = data.OwnerId;
    self.SaleOrderDueDate = data.SaleOrderDueDate;
    self.AccountType = data.AccountTypeName;
    self.AccountName = data.AccountName;
    self.AccountOwner = data.AccountOwner;
    self.DetailUrl = "AccountDetail/" + data.AccountId;
    self.EditUrl = "Account/" + data.AccountId + "?returnurl=" + window.location.href;
    //self.OwnerUrl = "UserProfile/" + data.OwnerId;
    //self.AccountUrl = "ViewAccountDetail/" + data.AccountId;
    self.DeleteSaleOrderUrl = "DeleteAccount/" + data.AccountId;
    self.AccountOwnerDetailUrl = "UserProfile/" + data.AccountOwnerId;

}
//Page view model

ErucaCRM.User.Accounts.pageViewModel = function () {
    //Class variables

    var self = this;
    var objAccountInfo = new Object();
    objAccountInfo.CurrentPage = currentPage;

    var PagingMethodName = "GetPageData";
    $("#divFilterInfo").hide();
    var controllerUrl = "/User/";
    var Message = {
        Failure: 'Failure',
        Success: 'Success'
    }

    self.AccountListQueryModel = ko.observableArray();
    self.queryModel = ko.observable();
    self.status = ko.observable();
    this.AccountList = ko.observableArray([]);
    this.TagList = ko.observableArray([]);

    self.getAccountList = function (objAccountInfo) {

        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetAccounts", objAccountInfo, function onSuccess(response) {
            self.RenderAccountList(response);
            ErucaCRM.Framework.Common.ApplyPermission('._roleselector');
        }, function onError(err) {
            ErucaCRM.Framework.Core.ShowMessage(err.Message, true);
        });

    }
    //change Query string value when click on account tab
    self.TabAccount = function () {
        if (history.pushState) {
            var newurl = window.location.protocol + "//" + window.location.host + window.location.pathname + '?tabid=1';
            window.history.pushState({ path: newurl }, '', newurl);
        }
    }
    //change Query string value when click on contact tab
    self.TabContacts = function () {
        if (history.pushState) {
            var newurl = window.location.protocol + "//" + window.location.host + window.location.pathname + '?tabid=2';
            window.history.pushState({ path: newurl }, '', newurl);
        }
    }
    self.RenderAccountList = function (AccountListData) {
        $("#divNoRecord").hide();

        $("#AccountList").children().remove();

        if (AccountListData.ListAccount.length == 0) {
            $("#divNoRecord").html("<b>" + ErucaCRM.Messages.Account.NoAccountFound + "</b>");
            $("#divNoRecord").show();
        }

        ko.utils.arrayForEach(AccountListData.ListAccount, function (Account) {
           

            self.AccountList.push(new ErucaCRM.User.Accounts.AccountListQueryModel(Account));

        });

        $("#Pager").html(self.GetPaging(AccountListData.TotalRecords, currentPage, PagingMethodName));
    };

    self.DeleteAccount = function (accountId) {
     
        if (confirm(ErucaCRM.Messages.Account.ConfirmAccountDeleteAction)) {

            var data = new Object();
            data.id_encrypted = accountId;

            ErucaCRM.Framework.Core.doDeleteOperation(controllerUrl + "DeleteAccount", data, function onSuccess(response) {

                if (Message.Success == response.Status) {

                    ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Account.RecordDeletedSuccess, false);
                    self.GetPageData(1);
                    //currentPage = 1;
                    //var objAccountInfo = new Object();
                    //objAccountInfo.CurrentPage = currentPage;
                    //self.getAccountList(objAccountInfo);
                }
                else {
                    ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Account.AccountDeletedFailure, true);
                }
            });
        }

    }

    self.GetPageData = function (currentPageNo) {

        currentPage = currentPageNo;
        var objAccountInfo = new Object();
       

        if (TagId == "" && TagSearchName == "") {
            var objAccountInfo = new Object();
            objAccountInfo.CurrentPage = currentPageNo;
            self.getAccountList(objAccountInfo);
        }
        else if (TagSearchName != "") {
            objAccountInfo.CurrentPage = currentPageNo;
            objAccountInfo.TagSearchName = TagSearchName;
            self.getTagNameSearchAccountList(objAccountInfo);
        }
        else {

            objAccountInfo.TagId = TagId;
            objAccountInfo.CurrentPage = currentPageNo;
            self.getTaggedAccountList(objAccountInfo);

        }

      //  self.getAccountList(objAccountInfo);

    }





    self.GetPaging = function (Rowcount, currentPage, methodName) {
        return ErucaCRM.Framework.Core.GetPagger(Rowcount, currentPage, methodName);
    }

   

    self.getAccountList(objAccountInfo);


    self.getTagList = function () {

        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetMostUsedAccountTags", null, function onSuccess(response) {
            self.RenderTagList(response);
            ErucaCRM.Framework.Common.ApplyPermission('._roleselector');
        }, function onError(err) {
            self.status(err.Message);
        });

    }

    self.RenderTagList = function (TagListData) {

        ko.utils.arrayForEach(TagListData.listTags, function (Tag) {
            self.TagList.push(new ErucaCRM.User.Accounts.TagListQueryModel(Tag));
        });

    };


    $("#ButtonSearchByTag").click(function (e) {
        $("#TextBoxTagSearchName").val(" ");//vacant the search textbox everytime the ButtonSearchByTag is clicked--MAnoj Singh
        e.stopPropagation();
        e.preventDefault();
        
     
        if ($('#divTagSearch:visible').size() > 0) {
            $("#divTagSearch").hide();
        }
        else {
            $("._popup").hide();
            $("#divTagSearch").show();
        }
       
   
      //  $("#divTagSearch").show();
       
    });


    $("#ButtonTagSearchName").click(function () {

        if ($("#TextBoxTagSearchName").val() == "") {
            alert("Please enter tag name for search");
            return false;

        }
        currentPage = 1;
        TagId = "";
        TagSearchName = $("#TextBoxTagSearchName").val();
        CurrentTagName = TagSearchName;
        var objAccountInfo = new Object();
        objAccountInfo.CurrentPage = 1;
        objAccountInfo.TagSearchName = TagSearchName;
        self.getTagNameSearchAccountList(objAccountInfo);
        $("#TextBoxTagSearchName").val("");
        $("#spanCurrentTagName").html("");
        $("#divTagSearch").hide();
        $("#divFilterInfo").hide();

    });


    self.ClearTagFilter = function () {

        currentPage = 1;
        TagId = "";
        TagSearchName = "";
        filter = "";
        CurrentTagName = "";
        var objAccountInfo = new Object();
        objAccountInfo.CurrentPage = 1;
              self.getAccountList(objAccountInfo);
        $("#divFilterInfo").hide();
        return false;
    }

    self.HideTagFilterMenu = function () {
        $("#TextBoxTagSearchName").val("");
        $("#divTagSearch").hide();
    }

    self.GetTaggedAccounts = function (tagObj) {
        currentPage = 1;
        TagSearchName = "";
        var objAccountInfo = new Object();
        objAccountInfo.TagId = tagObj.TagId;
        CurrentTagName = tagObj.TagName;
        TagId = tagObj.TagId;
        objAccountInfo.CurrentPage = 1;
        self.getTaggedAccountList(objAccountInfo);
        $("#TextBoxTagSearchName").val("");
        $("#spanCurrentTagName").html("");
        $("#divTagSearch").hide();
        $("#divFilterInfo").hide();

    }

    self.getTaggedAccountList = function (objAccountInfo) {

        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetAccounts", objAccountInfo, function onSuccess(response) {

            $("#spanCurrentTagName").html(CurrentTagName);
            $("#divFilterInfo").show();
            self.RenderAccountList(response);
            //ErucaCRM.Framework.Common.ApplyPermission('._roleselector');
        }, function onError(err) {
            self.status(err.Message);
        });

    }

    self.getTagNameSearchAccountList = function (objAccountInfo) {

        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetAccounts", objAccountInfo, function onSuccess(response) {

            $("#spanCurrentTagName").html(CurrentTagName);
            $("#divFilterInfo").show();
            self.RenderAccountList(response);
            //ErucaCRM.Framework.Common.ApplyPermission('._roleselector');
        }, function onError(err) {
            self.status(err.Message);
        });

    }

    self.getTagList();
}
