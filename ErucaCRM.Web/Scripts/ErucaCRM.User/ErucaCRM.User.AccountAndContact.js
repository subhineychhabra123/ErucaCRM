///Merging  Account and Contact js 
//--by Manoj Singh 

jQuery.namespace('ErucaCRM.User.AccountAndContact');

var viewModel;
var currentPage = 1;
var TagId = "";
var TagSearchName = "";
var CurrentTagName = "";

/////////////contacts Variables
var filter = $("#DropDownListContactFilter").val();

ErucaCRM.User.AccountAndContact.pageLoad = function () {//Contacts.js
    // debugger;
    currentPage = 1;
    TagId = "";
    TagSearchName = "";
    CurrentTagName = "";
    filter = $("#DropDownListContactFilter").val();

    viewModel = new ErucaCRM.User.AccountAndContact.pageViewModel(currentPage, filter);
    ko.applyBindings(viewModel, document.getElementById("InnerContentContainer"));
 
    // $("#tabAccount").click();


}
ErucaCRM.User.AccountAndContact.ContactListQueryModel = function (data) {
    var selectedTabId = ErucaCRM.Framework.Core.GetUrlVars()["tabid"];    
    var self = this;
    self.ContactId = data.ContactId;
    self.ContactName = data.FirstName
    if (data.LastName != null)
        self.ContactName = self.ContactName + " " + data.LastName;
    self.ContactPhone = "";
    if (data.Phone != null)
        self.ContactPhone = data.Phone;//{new @userId=" + data.UserId + "}";
    self.Company = data.ContactCompanyName;
    self.OwnerName = data.OwnerName;
    self.ContactEmail = data.EmailAddress;
  
    if (selectedTabId == undefined) {
        self.UrlEdit = "EditContact/" + data.ContactId + "?returnurl=" + window.location.href + "?tabid=2";
    }
   
    else if (selectedTabId == 1 || selectedTabId == 2) {
        var url = window.location.href.split("?");
        self.UrlEdit = "EditContact/" + data.ContactId + "?returnurl=" + url[0].toString() + "?tabid=2";;
    }
    
 
    self.UrlViewContact = "ContactView/" + data.ContactId;

}
ErucaCRM.User.AccountAndContact.ContactListQueryModelForValidation = function (data) {

    var self = this;
    self.ContactId = data.ContactId;
    self.FirstName = data.FirstName;
    self.LastName = data.LastName;
    self.ContactPhone = "";
    if (data.Phone != null)
        self.ContactPhone = data.Phone;//{new @userId=" + data.UserId + "}";
    self.Company = data.ContactCompanyName;
    self.ErrorDescription = data.ErrorDescription;
    self.ContactEmail = data.EmailAddress;
    self.UrlEdit = "EditContact/" + data.ContactId + "?returnurl=" + window.location.href;
    self.UrlViewContact = "ContactView/" + data.ContactId;
}
ErucaCRM.User.AccountAndContact.TagListQueryModel = function (data) {

    var self = this;
    self.TagId = data.TagId;
    self.TagName = data.TagName;

}
//ErucaCRM.User.AccountAndContact.TagListQueryModel = function (data) {//same in both the js

//    var self = this;
//    self.TagId = data.TagId;
//    self.TagName = data.TagName;

//}
ErucaCRM.User.AccountAndContact.AccountListQueryModel = function (data) {
    var self = this;
    self.AccountId = data.AccountId;
    var selectedTabId = ErucaCRM.Framework.Core.GetUrlVars()["tabid"];
    self.Phone = data.Phone;
    self.OwnerId = data.OwnerId;
    self.SaleOrderDueDate = data.SaleOrderDueDate;
    self.AccountType = data.AccountTypeName;
    self.AccountName = data.AccountName;
    self.AccountOwner = data.AccountOwner;
    self.DetailUrl = "AccountDetail/" + data.AccountId;   
    if (selectedTabId == 2) {       
        var url=window.location.href.split("?");
        self.EditUrl = "Account/" + data.AccountId + "?returnurl=" + url[0].toString();
    }
else {
        self.EditUrl = "Account/" + data.AccountId + "?returnurl=" + window.location.href;
}
  //  self.EditUrl = "Account/" + data.AccountId + "?returnurl=" + window.location.href;
    //self.OwnerUrl = "UserProfile/" + data.OwnerId;
    //self.AccountUrl = "ViewAccountDetail/" + data.AccountId;
    self.DeleteSaleOrderUrl = "DeleteAccount/" + data.AccountId;
    self.AccountOwnerDetailUrl = "UserProfile/" + data.AccountOwnerId;

}

ErucaCRM.User.AccountAndContact.pageViewModel = function (currentPage, filter) {
    //  debugger;
    //Class variables
    var self = this;
    var objContactListInfo = new Object();
    //Account.js start
    var objAccountInfo = new Object();
    objAccountInfo.CurrentPage = currentPage;
    var accountPagingMethodName = "GetPageData";
    //Account.js end
    objContactListInfo.TagId = "";
    objContactListInfo.TagSearchName = "";
    objContactListInfo.CurrentPageNo = currentPage;
    objContactListInfo.FilterBy = filter;
    objContactListInfo.IsSearchByTag = false;
    objContactListInfo.SearchTags = "";
    var contactPagingMethodName = "GetPageRecords";
    $("#ContactdivFilterInfo").hide();
    $("#AccountdivFilterInfo").hide();
    var controllerUrl = "/User/";
    var Message = {
        Failure: 'Failure',
        Success: 'Success'
    }

    var uploadObj;
    uploadObj = $("#fileuploader").uploadFile({
        url: '/User/BulkUploadContacts',
        multiple: false,
        autoSubmit: false,

        maxFileCount: 1,

        maxFileCountErrorStr: ErucaCRM.Messages.Contact.MaxFileCountError,
        fileName: "docs",
        maxFileSize: 1024 * 10000,
        onSelect: function (files) {

            return true; //to allow file submission.
        },

        dynamicFormData: function (e) {

            //var data = self.setPostData();
            //return data;
        },
        showStatusAfterSuccess: false,
        onSubmit: function (files) {

            if (files.length > 1) {
                return false;
            }
        },
        onSuccess: function (files, data, xhr) {
            if (data.success == true) {              
               
                ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Contact.ContactUploadedSuccessFully, false, false);
                if (data.ContactsWithErrror.length > 0) 
                {                   
                    self.SuccessfullyRetrievedModelsFromAjaxForNotValidatedContact(data.ContactsWithErrror);
                    $('._contactlistwitherror').fadeIn('slow');
                    $('._recordInserted').html(ErucaCRM.Messages.Contact.BulkUploadRecordInsertedMessage + ":" + data.recordInserted);
                    $('._validationMessageSection').html(ErucaCRM.Messages.Contact.BulkUploadErrorMessage);
                }
                else {
                    $('._close').click();
                }

                objContactListInfo.CurrentPageNo = 1;
                currentPage = 1;
                self.getContactList(objContactListInfo);

            }
            else {
                if (data.success == false && data.response == "NoRecordFound") {
                    ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Contact.NoRecordInFile, true, false);
                }
                else {
                    ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Contact.ContactNotUploadedMessage, true, false);


                }
                $('._contactlistwitherror').hide();
            }
        },
        afterUploadAll: function () {

        },
        onError: function (files, status, errMsg) {

        }
    });
    $('._close').click(function () {
        //$('.ajax-file-upload-cancel').click();
        uploadObj.cancelAll();
        uploadObj.errorLog.empty();

    });
    $("._attachDoc").bind("click", function () {
      
        if (uploadObj.selectedFiles > 0) {
            uploadObj.startUpload();
            uploadObj.selectedFiles = 0;
           
        }
        else {


        }
    });
    self.ContactListQueryModel = ko.observableArray();
    self.AccountListQueryModel = ko.observableArray();//account.js
    self.queryModel = ko.observable();
    self.status = ko.observable();
    this.ContactList = ko.observableArray([]);
    this.AccountList = ko.observableArray([]);//account.js
    this.ContactListNotValidated = ko.observableArray([]);
    this.TagList = ko.observableArray([]);
    this.TagListContact = ko.observableArray([]);
    //account.js start
    self.getAccountList = function (objAccountInfo) {
        // debugger;
        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetAccounts", objAccountInfo, function onSuccess(response) {
            self.RenderAccountList(response);
            ErucaCRM.Framework.Common.ApplyPermission('._roleselector');
        }, function onError(err) {
            ErucaCRM.Framework.Core.ShowMessage(err.Message, true);
        });

    }

    self.RenderAccountList = function (AccountListData) {
        // debugger;
        $("#divNoRecord").hide();

        $("#AccountList").children().remove();
        self.AccountList.removeAll();
        if (AccountListData.ListAccount.length == 0) {
            $("#divNoRecord").html("<b>" + ErucaCRM.Messages.Account.NoAccountFound + "</b>");
            $("#divNoRecord").show();
        }

        ko.utils.arrayForEach(AccountListData.ListAccount, function (Account) {

            self.AccountList.push(new ErucaCRM.User.AccountAndContact.AccountListQueryModel(Account));

        });

        $("#PagerAccounts").html(self.GetPagingAccount(AccountListData.TotalRecords, currentPage, accountPagingMethodName));
    };
   
    self.DeleteAccount = function (account) {

        if (confirm(ErucaCRM.Messages.Account.ConfirmAccountDeleteAction)) {

            var data = new Object();
            data.id_encrypted = account.AccountId;

            ErucaCRM.Framework.Core.doDeleteOperation(controllerUrl + "DeleteAccount", data, function onSuccess(response) {

                if (Message.Success == response.Status) {
                 
                    ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Account.RecordDeletedSuccess, false);
                    self.AccountList.remove(account);
                    //currentPage = 1;
                    //var objAccountInfo = new Object();
                    //objAccountInfo.CurrentPage = currentPage;
                    //self.getAccountList(objAccountInfo);
                    if (self.AccountList().length == 0) {
                        self.GetPageData(currentPage - 1);
                    }

                    else {
                        if (currentPage == 1) {
                            self.GetPageData(1);
                        }
                        else {
                            self.GetPageData(currentPage);

                        }
                    }
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



    }


    self.checkPermissionCookie = function () {
        var ViewPermission = ErucaCRM.Framework.Common.CookieExists("AccountVe");
        if (ViewPermission == false) {
            $("#TabContacts").parent('li').addClass("active");
            $("#contactsView").show();
        }

    }


    self.getAccountTagList = function () {

        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetMostUsedAccountTags", null, function onSuccess(response) {
            self.RenderTagList(response);
            ErucaCRM.Framework.Common.ApplyPermission('._roleselector');
        }, function onError(err) {
            self.status(err.Message);
        });

    }

    $("#AccountButtonSearchByTag").click(function (e) {
       
        $("#TextBoxAccountTagSearchName").val(" ");//vacant the search textbox everytime the AccountButtonSearchByTag is clicked--Manoj Singh
        e.stopPropagation();
        e.preventDefault();


        if ($('#AccountdivTagSearch:visible').size() > 0) {//Account Search
            $("#AccountdivTagSearch").hide();
        }
        else {
            $("._popup").hide();
            $("#AccountdivTagSearch").show();
        }




    });

    self.GetTaggedAccounts = function (tagObj) {//Account Search
        currentPage = 1;
        TagSearchName = "";
        var objAccountInfo = new Object();
        objAccountInfo.TagId = tagObj.TagId;
        CurrentTagName = tagObj.TagName;
        TagId = tagObj.TagId;
        objAccountInfo.CurrentPage = 1;
        self.getTaggedAccountList(objAccountInfo);
        $("#TextBoxAccountTagSearchName").val("");
        $("#AccountspanCurrentTagName").html("");
        $("#AccountdivTagSearch").hide();
        $("#AccountdivFilterInfo").show();
        $("#AccountdivFilterInfo").css({ "display": "inline-block" });

  
      

    }


    self.getTaggedAccountList = function (objAccountInfo) {

        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetAccounts", objAccountInfo, function onSuccess(response) {
            $("#AccountspanCurrentTagName").html(CurrentTagName);
            $("#divFilterInfo").show();
            self.RenderAccountList(response);
            ErucaCRM.Framework.Common.ApplyPermission('._roleselector');
        }, function onError(err) {
            self.status(err.Message);
        });

    }

    self.getTagNameSearchAccountList = function (objAccountInfo) {

        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetAccounts", objAccountInfo, function onSuccess(response) {
            $("#AccountspanCurrentTagName").html(CurrentTagName);
            $("#AccountdivFilterInfo").css({ "display": "inline-block" });
            $("#AccountdivFilterInfo").show();

           
          
            self.RenderAccountList(response);
            ErucaCRM.Framework.Common.ApplyPermission('._roleselector');
        }, function onError(err) {
            self.status(err.Message);
        });

    }

    self.getAccountTagList();


    //account.js end
    //contact.js starts
    self.getContactList = function (objContactListInfo) {

        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "ContactList", objContactListInfo, function onSuccess(response) {
            self.SuccessfullyRetrievedModelsFromAjax(response);
            ErucaCRM.Framework.Common.ApplyPermission('._roleselector');
        }, function onError(err) {
            self.status(err.Message);
        });

    }


    self.getTagList = function () {

        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetMostUsedContactTags", null, function onSuccess(response) {
            self.RenderContactTagList(response);
            ErucaCRM.Framework.Common.ApplyPermission('._roleselector');
        }, function onError(err) {
            self.status(err.Message);
        });

    }


    self.RenderTagList = function (TagListData) {//same in both
        ko.utils.arrayForEach(TagListData.listTags, function (Tag) {
            self.TagList.push(new ErucaCRM.User.AccountAndContact.TagListQueryModel(Tag));
        });

    };


    self.RenderContactTagList = function (TagListData) {//same in both
        ko.utils.arrayForEach(TagListData.listTags, function (Tag) {
            self.TagListContact.push(new ErucaCRM.User.AccountAndContact.TagListQueryModel(Tag));
        });

    };


    self.getTaggedContactList = function (objContactListInfo) {

        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetTaggedContactList", objContactListInfo, function onSuccess(response) {

            $("#ContactspanCurrentTagName").html(CurrentTagName);
            $("#ContactdivFilterInfo").show();
            self.SuccessfullyRetrievedModelsFromAjax(response);
            ErucaCRM.Framework.Common.ApplyPermission('._roleselector');
        }, function onError(err) {
            self.status(err.Message);
        });

    }

    self.getTagNameSearchContactList = function (objContactListInfo) {

        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetTaggedContactSearchByName", objContactListInfo, function onSuccess(response) {

            $("#ContactspanCurrentTagName").html(CurrentTagName);
            $("#ContactdivFilterInfo").show();
            self.SuccessfullyRetrievedModelsFromAjax(response);
            ErucaCRM.Framework.Common.ApplyPermission('._roleselector');
        }, function onError(err) {
            self.status(err.Message);
        });

    }

    self.getTagList();
    self.getContactList(objContactListInfo);


    self.GetTaggedContacts = function (tagObj) {
        TagSearchName = "";
        var objContactListInfo = new Object();
        objContactListInfo.TagId = tagObj.TagId;
        CurrentTagName = tagObj.TagName;
        TagId = tagObj.TagId;
        objContactListInfo.CurrentPage = 1;
        self.getTaggedContactList(objContactListInfo);
        $("#TextBoxContactTagSearchName").val("");
        $("#ContactspanCurrentTagName").html("");
        $("#ContactdivTagSearch").hide();
        $("#ContactdivFilterInfo").hide();

    }

    self.SuccessfullyRetrievedModelsFromAjax = function (ContactListData) {
        $("#ContactdivNoRecord").hide();
        $("#ContactListData").children().remove();
        self.ContactList.removeAll();
        if (ContactListData.ListContacts.length == 0) {
            $("#ContactdivNoRecord").html("<b>" + ErucaCRM.Messages.Contact.NoRecordFound + "</b>");
            $("#ContactdivNoRecord").show();
        }
      
        ko.utils.arrayForEach(ContactListData.ListContacts, function (Contact) {
            self.ContactList.push(new ErucaCRM.User.AccountAndContact.ContactListQueryModel(Contact));
        });

        $("#Pager").html(self.GetPaging(ContactListData.TotalRecords, currentPage, contactPagingMethodName));
    };
    self.SuccessfullyRetrievedModelsFromAjaxForNotValidatedContact = function (ContactListData) {
        $("#ContactListDataError").children().remove();
        ko.utils.arrayForEach(ContactListData, function (Contact) {
            self.ContactListNotValidated.push(new ErucaCRM.User.AccountAndContact.ContactListQueryModelForValidation(Contact));
        });

        $("#Pager").html(self.GetPaging(ContactListData.TotalRecords, currentPage, contactPagingMethodName));
    };
    $("#TextBoxTagSearch").blur(function () {
        if ($("#TextBoxTagSearch").val() == "") {
            var objContactListInfo = new Object();
            currentPage = 1;
            objContactListInfo.CurrentPageNo = 1;
            objContactListInfo.FilterBy = $("#DropDownListContactFilter").val();
            objContactListInfo.IsSearchByTag = false;
            objContactListInfo.SearchTags = ""
            self.getContactList(objContactListInfo);
        }
    });

    $("#ContactButtonSearchByTag").click(function (e) {
        $("#TextBoxContactTagSearchName").val(" ");
        e.stopPropagation();
        e.preventDefault();
        if ($('#ContactdivTagSearch:visible').size() > 0) {//Account Search
            $("#ContactdivTagSearch").hide();
        }
        else {
            $("._popup").hide();
            $("#ContactdivTagSearch").show();
        }
       
    });

    $("#ButtonContactTagSearchName").click(function () {

        if ($("#TextBoxContactTagSearchName").val().trim().length == 0) {
            alert(ErucaCRM.Messages.Contact.TagRequired);
            return false;

        }
        else {
            currentPage = 1;
            TagId = "";
            TagSearchName = $("#TextBoxContactTagSearchName").val();
            CurrentTagName = TagSearchName;
            var objContactListInfo = new Object();
            //var objAccountInfo = new Object();//account.js
            //objAccountInfo.CurrentPage = 1;//account.js
            //objAccountInfo.TagSearchName = TagSearchName;//account.js
            //self.getTagNameSearchAccountList(objAccountInfo);//account.js
            objContactListInfo.CurrentPage = 1;
            objContactListInfo.TagSearchName = TagSearchName;
            self.getTagNameSearchContactList(objContactListInfo);
            $("#TextBoxContactTagSearchName").val("");
            $("#ContactspanCurrentTagName").html("");
            $("#ContactdivTagSearch").hide();
            $("#ContactdivFilterInfo").hide();
        }

    });

    //Account Search Button
    $("#ButtonAccountTagSearchName").click(function () {
       
        if ($("#TextBoxAccountTagSearchName").val().trim().length==0) {
            alert(ErucaCRM.Messages.Contact.TagRequired);
            return false;

        }
        else {
           
            currentPage = 1;
            TagId = "";
            TagSearchName = $("#TextBoxAccountTagSearchName").val();
            CurrentTagName = TagSearchName;
            var objAccountInfo = new Object();
            objAccountInfo.CurrentPage = 1;
            objAccountInfo.TagSearchName = TagSearchName;
            self.getTagNameSearchAccountList(objAccountInfo);
            $("#TextBoxAccountTagSearchName").val("");
            $("#AccountspanCurrentTagName").html("");
            $("#AccountdivTagSearch").hide();
            $("#AccountdivFilterInfo").hide();
        }

    });


    
    $("#ButtonTagSearchName").click(function () {

        if ($("#TextBoxTagSearch").val() == "") {
            alert(ErucaCRM.Messages.Contact.TagRequired);
            return false;
        }

        var serachtag = $("#TextBoxTagSearch").val();
        serachtag = serachtag.split(' ').join('');
        $("#DropDownListContactFilter").val("Allcontacts");
        var objContactListInfo = new Object();
        currentPage = 1;
        objContactListInfo.CurrentPageNo = 1;
        objContactListInfo.FilterBy = "";
        objContactListInfo.IsSearchByTag = true;
        objContactListInfo.SearchTags = serachtag

        currentPage = 1;
        self.getContactList(objContactInfo);

    });

    $("#DropDownListContactFilter").change(function () {
        var objContactListInfo = new Object();
        currentPage = 1;
        objContactListInfo.CurrentPageNo = currentPage;
        objContactListInfo.FilterBy = $("#DropDownListContactFilter").val();
        objContactListInfo.IsSearchByTag = false;
        objContactListInfo.SearchTags = "";

        $("#ContactListData").children().remove();
        $(".divPager").empty();;
        self.getContactList(objContactListInfo);
    });

    self.ClearTagFilter = function () {
        currentPage = 1;
        TagId = "";
        TagSearchName = "";
        filter = "";
        CurrentTagName = "";
        var objContactListInfo = new Object();
        currentPage = 1;
        objContactListInfo.CurrentPageNo = 1;
        objContactListInfo.FilterBy = $("#DropDownListContactFilter").val();
        objContactListInfo.IsSearchByTag = false;
        objContactListInfo.SearchTags = "";

        self.getContactList(objContactListInfo);
        $("#ContactdivFilterInfo").hide();

        return false;
    }


    self.ClearAccountTagFilter = function () {
        TagId = "";
        TagSearchName = "";
        filter = "";
        CurrentTagName = "";
        var objAccountInfo = new Object();//account.js
        objAccountInfo.CurrentPage = 1;//account.js
        self.getAccountList(objAccountInfo);//account.js
        $("#AccountdivFilterInfo").hide();
        return false;
    }

    self.AccountHideTagFilterMenu = function () {
        $("#TextBoxAccountTagSearchName").val("");
        $("#AccountdivTagSearch").hide();
    }
    self.ContactHideTagFilterMenu = function () {
        $("#TextBoxContactTagSearchName").val("");
        $("#ContactdivTagSearch").hide();
    }

    self.GetPageRecords = function (currentPageNo) {
      
        var serachtag = "";
        var IsSearchByTag = false;
        if ($("#ContactTextBoxTagSearch").val() != "") {
            serachtag = $("#ContactTextBoxTagSearch").val();
            serachtag = serachtag.split(' ').join('');
            //serachtag = serachtag.replace(/ /g, "");
            IsSearchByTag = true;
        }


        currentPage = currentPageNo;

        var objContactListInfo = new Object();

        if (TagId == "" && TagSearchName == "") {
            objContactListInfo.CurrentPageNo = currentPageNo;
            objContactListInfo.FilterBy = $("#DropDownListContactFilter").val();
            objContactListInfo.IsSearchByTag = IsSearchByTag;
            objContactListInfo.SearchTags = serachtag;
            self.getContactList(objContactListInfo);
        }
        else if (TagSearchName != "") {
            objContactListInfo.CurrentPage = currentPageNo;
            objContactListInfo.TagSearchName = TagSearchName;
            self.getTagNameSearchContactList(objContactListInfo);
        }
        else {

            objContactListInfo.TagId = TagId;
            objContactListInfo.CurrentPage = currentPageNo;
            self.getTaggedContactList(objContactListInfo);

        }
       
    }

    self.DeleteContact = function (Contact) {
       
        if (confirm(ErucaCRM.Messages.Contact.ConfirmContactDeleteAction)) {

            var data = new Object();
            data.id_encrypted = Contact.ContactId;

            ErucaCRM.Framework.Core.doDeleteOperation(controllerUrl + "DeleteContact", data, function onSuccess(response) {
                if (Message.Success == response.Status) {
                    ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Contact.RecordDeletedSuccess, false);
                    //currentPageNo = 1;
                    //self.GetPageRecords(currentPageNo);
                    self.ContactList.remove(Contact)
                    if (self.ContactList().length == 0) {
                        self.GetPageRecords(currentPage - 1);
                    }

                    else {
                        if (currentPage == 1) {
                            self.GetPageRecords(1);
                        }
                        else {
                            self.GetPageRecords(currentPage);

                        }
                    }
                }
                else {
                    ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Contact.ContactDeletedFailure, true);
                }
            });
        }

    }

    self.GetPaging = function (Rowcount, currentPage, methodName) {//Same in both js
        return ErucaCRM.Framework.Core.GetPagger(Rowcount, currentPage, methodName);


    }
    self.GetPagingAccount = function (Rowcount, currentPage, methodName) {//Same in both js
        return ErucaCRM.Framework.Core.GetPagger(Rowcount, currentPage, methodName);
    }
    self.OpenBulkFileUploader = function () {

        ErucaCRM.Framework.Core.OpenRolePopup("#BulkFileUploadSection");
        $('._contactlistwitherror').hide();

    }

    self.getAccountList(objAccountInfo);
    self.checkPermissionCookie();
}
$("#tabAccount").click(function () {
    $("#TabContacts").parent('li').removeClass("active");
    $("#tabAccount").parent('li').addClass("active");
    $("#accountView").show();
    $("#contactsView").hide();
    if (history.pushState) {
        var newurl = window.location.protocol + "//" + window.location.host + window.location.pathname + '?tabid=1';
        window.history.pushState({ path: newurl }, '', newurl);
    }
});




$("#TabContacts").click(function () {
    $("#tabAccount").parent('li').removeClass("active");
    $("#TabContacts").parent('li').addClass("active");
    $("#contactsView").show();
    $("#accountView").hide();
    if (history.pushState) {
        var newurl = window.location.protocol + "//" + window.location.host + window.location.pathname + '?tabid=2';
        window.history.pushState({ path: newurl }, '', newurl);
    }
});
$(function () {

    var selectedTabId = ErucaCRM.Framework.Core.GetUrlVars()["tabid"];
    
    if (selectedTabId != undefined && selectedTabId != null && selectedTabId == 2) {
        $("#TabContacts").click();
    }

    //else if (selectedTabId != undefined && selectedTabId != null && selectedTabId == " ") {
    //    $("#TabContacts").click();
    //}
})


//contact.js end
