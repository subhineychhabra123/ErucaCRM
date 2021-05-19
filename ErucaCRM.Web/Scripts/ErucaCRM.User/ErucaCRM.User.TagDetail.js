jQuery.namespace('ErucaCRM.User.TagDetail');

var viewModel;

ErucaCRM.User.TagDetail.pageLoad = function () {


    viewModel = new ErucaCRM.User.TagDetail.pageViewModel();
    ko.applyBindings(viewModel, document.getElementById("InnerContentContainer"));

}



// A view model that represent a Test report query model.
ErucaCRM.User.TagDetail.TagAccountListQueryModel = function (data) {

    var self = this;
    self.AccountId = data.AccountId;

    self.AccountType = data.AccountTypeName;
    self.AccountName = data.AccountName;
    self.AccountOwner = data.AccountOwner;
    self.DetailUrl = "/User/AccountDetail/" + data.AccountId;


}

// A view model that represent a Test report query model.
ErucaCRM.User.TagDetail.TagContactListQueryModel = function (data) {

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
    self.UrlViewContact = "/User/ContactView/" + data.ContactId;


}

//Page view model



ErucaCRM.User.TagDetail.TagLeadListQueryModel = function (data) {
 
    var self = this;
    self.LeadId = data.LeadId; 
    self.Title = data.Title
    self.LeadCompany = data.LeadCompany;
    self.Description = data.Description;
    self.UrlViewLead = "/User/Leads#" + data.LeadId;
}

ErucaCRM.User.TagDetail.pageViewModel = function () {
    //Class variables

    var self = this;
    var objAccountListInfo = new Object();

    objAccountListInfo.TagId = $("#TagId").val();
    objAccountListInfo.CurrentPage = 1;

    var objContactListInfo = new Object();
    objContactListInfo.TagId = $("#TagId").val();
    objContactListInfo.CurrentPage = 1;

    var objLeadListInfo = new Object();
    objLeadListInfo.TagId = $("#TagId").val();
    objLeadListInfo.CurrentPage = 1;


    var AccountListPagingMethodName = "AccountListGetPageRecords";
    var ContactListPagingMethodName = "ContactListGetPageRecords";
    var LeadListPagingMethodName = "LeadListGetPageRecords";
    var controllerUrl = "/User/";
    var Message = {
        Failure: 'Failure',
        Success: 'Success'
    }

    self.TagListQueryModel = ko.observableArray();
    self.queryModel = ko.observable();
    self.status = ko.observable();
    this.AccountList = ko.observableArray([]);
    this.ContactList = ko.observableArray([]);
    this.LeadList = ko.observableArray([]);

    self.getAccountList = function (objAccountListInfo) {

        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetTaggedAccountList", objAccountListInfo, function onSuccess(response) {
            self.RenderAccountList(response);
            //ErucaCRM.Framework.Common.ApplyPermission('._roleselector');
        }, function onError(err) {
            self.status(err.Message);
        });

    }

    self.getContactList = function (objContactListInfo) {

        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetTaggedContactList", objContactListInfo, function onSuccess(response) {
            self.RenderContactList(response);
            //ErucaCRM.Framework.Common.ApplyPermission('._roleselector');
        }, function onError(err) {
            self.status(err.Message);
        });

    }

    self.getLeadList = function (objLeadListInfo) {

        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetTaggedLeadList", objLeadListInfo, function onSuccess(response) {
            self.RenderLeadList(response);
            //ErucaCRM.Framework.Common.ApplyPermission('._roleselector');
        }, function onError(err) {
            self.status(err.Message);
        });


    }




    self.RenderAccountList = function (AccountListData) {
        $("#divNoRecordAccount").hide();
        $("#AccountList").children().remove();

        if (AccountListData.ListAccount.length == 0) {
            $("#divNoRecordAccount").html("<b>" + ErucaCRM.Messages.Tag.NoAccountRecordFound + "</b>");
            $("#divNoRecordAccount").show();
        }

        ko.utils.arrayForEach(AccountListData.ListAccount, function (Account) {
            self.AccountList.push(new ErucaCRM.User.TagDetail.TagAccountListQueryModel(Account));
        });
        ErucaCRM.Framework.Common.ApplyPermission('._roleselector');
        $("#PagerAccountList").html(self.GetPaging(AccountListData.TotalRecords, objAccountListInfo.CurrentPage, AccountListPagingMethodName));
    };




    self.RenderContactList = function (ContactListData) {
        $("#divNoRecordContact").hide();
        $("#ContactList").children().remove();

        if (ContactListData.ListContacts.length == 0) {
            $("#divNoRecordContact").html("<b>" + ErucaCRM.Messages.Tag.NoContactRecordFound + "</b>");
            $("#divNoRecordContact").show();
        }

        ko.utils.arrayForEach(ContactListData.ListContacts, function (Contact) {
            self.ContactList.push(new ErucaCRM.User.TagDetail.TagContactListQueryModel(Contact));
        });
        ErucaCRM.Framework.Common.ApplyPermission('._roleselector');
        $("#PagerContactList").html(self.GetPaging(ContactListData.TotalRecords, objContactListInfo.CurrentPage, ContactListPagingMethodName));
    };



    self.RenderLeadList = function (LeadListData) {
      
        $("#divNoRecordLead").hide();
        $("#LeadList").children().remove();
        if (LeadListData.ListLead.length == 0) {
            $("#divNoRecordLead").html("<b>" + ErucaCRM.Messages.Tag.NoLeadRecordFound + "</b>");
            $("#divNoRecordLead").show();
        }
        ko.utils.arrayForEach(LeadListData.ListLead, function (Lead) {
            self.LeadList.push(new ErucaCRM.User.TagDetail.TagLeadListQueryModel(Lead));
        });
        ErucaCRM.Framework.Common.ApplyPermission('._roleselector');
        $("#PagerContactList").html(self.GetPaging(LeadListData.TotalRecords, objLeadListInfo.CurrentPage, LeadListPagingMethodName));
    };





    self.AccountListGetPageRecords = function (currentPageNo) {

        objAccountListInfo.TagId = $("#TagId").val();
        objAccountListInfo.CurrentPage = currentPageNo;


        self.getAccountList(objAccountListInfo);

    }

    self.ContactListGetPageRecords = function (currentPageNo) {

        objContactListInfo.TagId = $("#TagId").val();
        objContactListInfo.CurrentPage = currentPageNo;
        self.getContactList(objContactListInfo);
    }

    self.LeadListGetPageRecords = function (currentPageNo) {
        objLeadListInfo.TagId = $("#TagId").val();
        objLeadListInfo.CurrentPage = currentPageNo;
        self.getLeadList(objLeadListInfo);
    }

    self.GetPaging = function (Rowcount, currentPage, methodName) {
        return ErucaCRM.Framework.Core.GetPagger(Rowcount, currentPage, methodName);
    }


    self.getAccountList(objAccountListInfo);
    self.getContactList(objContactListInfo);
    self.getLeadList(objLeadListInfo);
}
