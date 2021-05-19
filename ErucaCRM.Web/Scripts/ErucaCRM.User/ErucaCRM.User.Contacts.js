jQuery.namespace('ErucaCRM.User.Contacts');

var viewModel;
var currentPage = 1;
var TagId = "";
var TagSearchName = "";
var CurrentTagName = "";
var filter = $("#DropDownListContactFilter").val();
ErucaCRM.User.Contacts.pageLoad = function () {

    currentPage = 1;
    TagId = "";
    TagSearchName = "";
    CurrentTagName = "";
    filter = $("#DropDownListContactFilter").val();
    viewModel = new ErucaCRM.User.Contacts.pageViewModel(currentPage, filter);
    ko.applyBindings(viewModel, document.getElementById("InnerContentContainer"));

}




// A view model that represent a Test report query model.
ErucaCRM.User.Contacts.ContactListQueryModel = function (data) {

    var self = this;
    self.ContactId = data.ContactId;
    self.ContactName = data.FirstName
    if (data.LastName != null)
        self.ContactName = self.ContactName + " " + data.LastName;
    self.ContactPhone = "";
    if (data.Phone != null)
        self.ContactPhone = data.Phone;//{new @userId=" + data.UserId + "}";
    self.Company = data.ContactCompanyName;
    self.ContactEmail = data.EmailAddress;
    self.OwnerName = data.OwnerName;
    self.UrlEdit = "EditContact/" + data.ContactId+"?returnurl="+window.location.href;
    self.UrlViewContact = "ContactView/" + data.ContactId;
}


ErucaCRM.User.Contacts.ContactListQueryModelForValidation = function (data) {

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

// A view model that represent a Test report query model.
ErucaCRM.User.Contacts.TagListQueryModel = function (data) {

    var self = this;
    self.TagId = data.TagId;
    self.TagName = data.TagName;

}
//Page view model


ErucaCRM.User.Contacts.pageViewModel = function (currentPage, filter) {
    //Class variables
    var self = this;
    var objContactListInfo = new Object();
    objContactListInfo.TagId = "";
    objContactListInfo.TagSearchName = "";
    objContactListInfo.CurrentPageNo = currentPage;
    objContactListInfo.FilterBy = filter;
    objContactListInfo.IsSearchByTag = false;
    objContactListInfo.SearchTags = "";
    var PagingMethodName = "GetPageRecords";
    $("#divFilterInfo").hide();
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
                if (data.ContactsWithErrror.length > 0) {
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
                if (data.success == false && data.response == "NoRecordFound")
                {
                    ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Contact.NoRecordInFile, true, false);
                }
                else
                {
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
        if ($('.ajax-file-upload-statusbar').length == 1) {
            uploadObj.startUpload();
           
        }
        else {


        }
    });
   
    self.ContactListQueryModel = ko.observableArray();
    self.queryModel = ko.observable();
    self.status = ko.observable();
    this.ContactList = ko.observableArray([]);
    this.ContactListNotValidated = ko.observableArray([]);
    this.TagList = ko.observableArray([]);
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
            self.RenderTagList(response);
            ErucaCRM.Framework.Common.ApplyPermission('._roleselector');
        }, function onError(err) {
            self.status(err.Message);
        });

    }


    self.RenderTagList = function (TagListData) {

        ko.utils.arrayForEach(TagListData.listTags, function (Tag) {
            self.TagList.push(new ErucaCRM.User.Contacts.TagListQueryModel(Tag));
        });

    };

    self.getTaggedContactList = function (objContactListInfo) {

        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetTaggedContactList", objContactListInfo, function onSuccess(response) {

            $("#spanCurrentTagName").html(CurrentTagName);
            $("#divFilterInfo").show();
            self.SuccessfullyRetrievedModelsFromAjax(response);
            //ErucaCRM.Framework.Common.ApplyPermission('._roleselector');
        }, function onError(err) {
            self.status(err.Message);
        });

    }

    self.getTagNameSearchContactList = function (objContactListInfo) {

        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetTaggedContactSearchByName", objContactListInfo, function onSuccess(response) {

            $("#spanCurrentTagName").html(CurrentTagName);
            $("#divFilterInfo").show();
            self.SuccessfullyRetrievedModelsFromAjax(response);
            //ErucaCRM.Framework.Common.ApplyPermission('._roleselector');
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
        $("#TextBoxTagSearchName").val("");
        $("#spanCurrentTagName").html("");
        $("#divTagSearch").hide();
        $("#divFilterInfo").hide();

    }

    self.SuccessfullyRetrievedModelsFromAjax = function (ContactListData) {
        $("#divNoRecord").hide();
        $("#ContactListData").children().remove();
        if (ContactListData.ListContacts.length == 0) {
            $("#divNoRecord").html("<b>" + ErucaCRM.Messages.Contact.NoRecordFound + "</b>");
            $("#divNoRecord").show();
        }

        ko.utils.arrayForEach(ContactListData.ListContacts, function (Contact) {
            self.ContactList.push(new ErucaCRM.User.Contacts.ContactListQueryModel(Contact));
        });

        $("#Pager").html(self.GetPaging(ContactListData.TotalRecords, currentPage, PagingMethodName));
    };
    self.SuccessfullyRetrievedModelsFromAjaxForNotValidatedContact = function (ContactListData) {
        $("#ContactListDataError").children().remove();
        ko.utils.arrayForEach(ContactListData, function (Contact) {
            self.ContactListNotValidated.push(new ErucaCRM.User.Contacts.ContactListQueryModelForValidation(Contact));
        });

        $("#Pager").html(self.GetPaging(ContactListData.TotalRecords, currentPage, PagingMethodName));
    };
    $("#TextBoxTagSearch").blur(function () {
        if ($("#TextBoxTagSearch").val() == "") {
            var objContactListInfo = new Object();
            objContactListInfo.CurrentPageNo = 1;
            objContactListInfo.FilterBy = $("#DropDownListContactFilter").val();
            objContactListInfo.IsSearchByTag = false;
            objContactListInfo.SearchTags = ""
            self.getContactList(objContactListInfo);
        }
    });

    $("#ButtonSearchByTag").click(function (e) {
        $("#TextBoxTagSearchName").val(" ");//vacants the textbox everytime the ButtonSearchByTag is clicked--by Manoj Singh 
        e.stopPropagation();
        $("._popup").hide();
        $("#divTagSearch").show();
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
        var objContactListInfo = new Object();
        objContactListInfo.CurrentPage = 1;
        objContactListInfo.TagSearchName = TagSearchName;
        self.getTagNameSearchContactList(objContactListInfo);
        $("#TextBoxTagSearchName").val("");
        $("#spanCurrentTagName").html("");
        $("#divTagSearch").hide();
        $("#divFilterInfo").hide();

    });

    $("#ButtonTagSerach").click(function () {

        if ($("#TextBoxTagSearch").val() == "") {
            alert(ErucaCRM.Messages.Contact.TagRequired);
            return false;
        }

        var serachtag = $("#TextBoxTagSearch").val();
        serachtag = serachtag.split(' ').join('');
        $("#DropDownListContactFilter").val("Allcontacts");
        var objContactListInfo = new Object();
        objContactListInfo.CurrentPageNo = 1;
        objContactListInfo.FilterBy = "";
        objContactListInfo.IsSearchByTag = true;
        objContactListInfo.SearchTags = serachtag

        currentPage = 1;
        self.getContactList(objContactInfo);

    });

    $("#DropDownListContactFilter").change(function () {
        var objContactListInfo = new Object();
        objContactListInfo.CurrentPageNo = currentPage;
        objContactListInfo.FilterBy = $("#DropDownListContactFilter").val();
        objContactListInfo.IsSearchByTag = false;
        objContactListInfo.SearchTags = "";

        $("#ContactListData").children().remove();

        self.getContactList(objContactListInfo);
    });

    self.ClearTagFilter = function () {

        currentPage = 1;
        TagId = "";
        TagSearchName = "";
        filter = "";
        CurrentTagName = "";
        var objContactListInfo = new Object();
        objContactListInfo.CurrentPageNo = 1;
        objContactListInfo.FilterBy = $("#DropDownListContactFilter").val();
        objContactListInfo.IsSearchByTag = false;
        objContactListInfo.SearchTags = "";

        self.getContactList(objContactListInfo);
        $("#divFilterInfo").hide();
        return false;
    }
    self.HideTagFilterMenu = function () {
        $("#TextBoxTagSearchName").val("");
        $("#divTagSearch").hide();
    }

    self.GetPageRecords = function (currentPageNo) {
    
        var serachtag = ""; 
        var IsSearchByTag = false;
        if ($("#TextBoxTagSearch").val() != "") {
            serachtag = $("#TextBoxTagSearch").val();
            serachtag = serachtag.split(' ').join('');
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

    self.DeleteContact = function (ContactId) {
        if (confirm(ErucaCRM.Messages.Contact.ConfirmContactDeleteAction)) {

            var data = new Object();
            data.id_encrypted = ContactId;

            ErucaCRM.Framework.Core.doDeleteOperation(controllerUrl + "DeleteContact", data, function onSuccess(response) {
                if (Message.Success == response.Status) {

                    ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Contact.RecordDeletedSuccess, false);
                    currentPageNo = 1;
                    self.GetPageRecords(currentPageNo);
                }
                else {
                    ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Contact.ContactDeletedFailure, true);
                }
            });
        }

    }

    self.GetPaging = function (Rowcount, currentPage, methodName) {
        return ErucaCRM.Framework.Core.GetPagger(Rowcount, currentPage, methodName);

      
    }
    self.OpenBulkFileUploader = function () {

        ErucaCRM.Framework.Core.OpenRolePopup("#BulkFileUploadSection");
        $('._contactlistwitherror').hide();

    }




}

