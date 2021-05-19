jQuery.namespace('ErucaCRM.User.AccountCases');

var accountCasesViewModel;
var currentAccountCasePage = 1;
var IsLeadsLastStage= false;
var ReserveSort = null;
var StoreSortvalue = null;
var Visibility = {
    visible: 'visible',
    hidden: 'hidden'
}
ErucaCRM.User.AccountCases.pageLoad = function () {
    currentAccountCasePage = 1;
    TagId = "";
    TagSearchName = "";
    CurrentTagName = "";
    accountCasesViewModel = new ErucaCRM.User.AccountCases.pageViewModel();
    ko.applyBindings(accountCasesViewModel, document.getElementById("InnerContentContainer"));
}
ErucaCRM.User.AccountCases.AccountCasesListQueryModel = function (data) {
    var self = this;
    self.AccountCaseId = data.AccountCaseId;
    self.AccountId = data.AccountIdEncrypted;
    self.CaseNumber = data.CaseNumber;
    self.Subject = data.Subject;
    self.CaseOrigin = data.CaseOrigin;
    self.PriorityName = data.PriorityName;
    self.AccountName = data.AccountName;
    self.CaseOwnerName = data.CaseOwnerName;
    self.DetailUrl = '/User/AccountCaseDetail/' + data.AccountCaseId;
    self.EditUrl = '/User/AccountCase?accountId_encrypted=' + data.AccountIdEncrypted + '&caseId_encrypted=' + data.AccountCaseId+'&returnurl='+window.location.href;
    return self;
}
ErucaCRM.User.AccountCases.pageViewModel = function () {
    //Class variables
    var self = this;
    var objAccountCaseInfo = new Object();
    objAccountCaseInfo.CurrentPage = currentAccountCasePage;
    var AccountCasePagingMethodName = "accountCasesViewModel.GetPageData"; //"accountCasesViewModel.GetAccountCasesList";
    $("#divFilterInfo").hide();
    var controllerUrl = "/User/";
    var Message = {
        Failure: 'Failure',
        Success: 'Success'
    }

  
    self.AccountCasesList = ko.observableArray([]);
    self.ShowNoAccountCaseFoundMsg = ko.observable(Visibility.hidden);
    self.NoAccountCasesFoundMsg = ko.observable();
    self.GetAccountCasesList = function (objAccountCasesInfo) {
        self.HideMessage();
       
        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetAccountCases", objAccountCasesInfo, function onSuccess(response) {
            self.RenderAccountCasesList(response);

            ErucaCRM.Framework.Common.ApplyPermission('._roleselector');
        }, function onError(err) {
            ErucaCRM.Framework.Core.ShowMessage(err.Message, true);
        });
}


    self.GetPageData = function (currentPageNumber)
    {
        var objAccountCaseInfo = new Object();
        currentAccountCasePage = currentPageNumber;
        if (StoreSortvalue != null && ReserveSort == "")
        {
            objAccountCaseInfo.sortColumnName = StoreSortvalue;
            objAccountCaseInfo.sortdir = 'D';
        }
        else if (ReserveSort!=null) {
            objAccountCaseInfo.sortColumnName = StoreSortvalue;
            objAccountCaseInfo.sortdir = 'A';
        }
        objAccountCaseInfo.currentPage = currentPageNumber;
        objAccountCaseInfo.FilterParameters = new Array();
        self.GetAccountCasesList(objAccountCaseInfo);
    }
    $('._common').click(function () {
        $(".Dse").removeClass("Dse");
        $(".Ase").removeClass("Ase");
        var objAccountCaseInfo = new Object();
        var sort = $(this).attr('data-sortby');
        var dir = $(this).attr('data-sortdir');
        if (sorting = true && $(this).attr('data-sortby') == ReserveSort) {
            dir = 'D';
            $(this).addClass("Dse").removeClass("Ase");
            ReserveSort = "";
            sorting = false;
        }
        else {
            sorting = true;
            ReserveSort = $(this).attr('data-sortby');
            StoreSortvalue = $(this).attr('data-sortby');
            $(this).addClass("Ase").removeClass("Dse");
        }
        objAccountCaseInfo.sortColumnName = sort;
        objAccountCaseInfo.sortdir = dir;
        objAccountCaseInfo.CurrentPage = currentAccountCasePage;
        self.GetAccountCasesList(objAccountCaseInfo);
    });

    self.RenderAccountCasesList = function (AccountCasesListData) {
        self.AccountCasesList.removeAll();
        if (AccountCasesListData.ListAccountCases.length == 0) {
            self.ShowMessage();
        }
        ko.utils.arrayForEach(AccountCasesListData.ListAccountCases, function (AccountCase) {

            self.AccountCasesList.push(new ErucaCRM.User.AccountCases.AccountCasesListQueryModel(AccountCase));

        });
        $("#AccountCasesPager").html(self.GetPaging(AccountCasesListData.TotalRecords, currentAccountCasePage, AccountCasePagingMethodName));
    };
    self.GetPaging = function (totalRecords, currentPage, methodName) {
        return ErucaCRM.Framework.Core.GetPagger(totalRecords, currentPage, methodName);
    }
    self.ShowMessage = function () {
        self.ShowNoAccountCaseFoundMsg(Visibility.visible);
        self.NoAccountCasesFoundMsg(ErucaCRM.Messages.AccountCase.NoAccountCaseFound);
    }
    self.HideMessage = function () {
        self.ShowNoAccountCaseFoundMsg(Visibility.hidden);
        self.NoAccountCasesFoundMsg(ErucaCRM.Messages.AccountCase.NoAccountCaseFound);
    }


    self.GetAccountCasesList(objAccountCaseInfo);


    self.DeleteCase = function (accountCase) {
        var PostData = new Object();
        PostData.caseId_encrypted = accountCase.AccountCaseId;
        PostData.accountId_encrypted = accountCase.AccountId;
        if (confirm(ErucaCRM.Messages.AccountCase.ConfirmAccountDelete)) {
            ErucaCRM.Framework.Core.doDeleteOperation(controllerUrl + "DeleteAccountCase", PostData,
            function SuccessCallBack(response) {
                if (response.Status == "Success") {
 
         
                    self.AccountCasesList.remove(accountCase);
                    ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Account.RecordDeletedSuccess, false);
                    //self.GetAccountCasesList(1);
                    if (self.AccountCasesList().length != 0) {
                        self.GetPageData(currentAccountCasePage);
                    }
                   
                    else {
                        if (currentAccountCasePage == 1) {
                            self.GetPageData(1);
                        }
                        else {
                            currentAccountCasePage = currentAccountCasePage - 1;
                  
                            self.GetPageData(currentAccountCasePage);
                        }
                    }
                    
                }
                else {
                    ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Account.RecordDeletionFailure, true);
                }
            }, function FailureCallback() { })


        }
    }
}