jQuery.namespace('ErucaCRM.User.Quotes');

var viewModel;
var currentPage = 1;

ErucaCRM.User.Quotes.pageLoad = function () {
    currentPage = 1;
    viewModel = new ErucaCRM.User.Quotes.pageViewModel();
    ko.applyBindings(viewModel, document.getElementById("InnerContentContainer"));
   // ErucaCRM.Framework.Common.ApplyPermission();
}



// A view model that represent a Test report query model.
ErucaCRM.User.Quotes.QuoteListQueryModel = function (data) {

    var self = this;
    self.QuoteId = data.QuoteId;
    self.Subject = data.Subject;
    self.OwnerId = data.OwnerId;
    self.ValidityDate = data.ValidityDate;
    self.Carrier = data.Carrier;
    self.OwnerName = data.OwnerName;
    self.LeadName = data.LeadName;


    self.DetailUrl = "ViewQuoteDetail/" + data.QuoteId;
    self.EditUrl = "EditQuote/" + data.QuoteId;
    self.OwnerUrl = "UserProfile/" + data.OwnerId;
    self.LeadUrl = "ViewLeadDetail/" + data.LeadId;

}
//Page view model

ErucaCRM.User.Quotes.pageViewModel = function () {
    //Class variables

    var self = this;
    var objQuoteInfo = new Object();
    objQuoteInfo.CurrentPage = currentPage;

    var PagingMethodName = "GetPageData";

    var controllerUrl = "/User/";
    var Message = {
        Failure: 'Failure',
        Success: 'Success'
    }

    self.QuoteListQueryModel = ko.observableArray();
    self.queryModel = ko.observable();
    self.status = ko.observable();
    this.QuoteList = ko.observableArray([]);

    self.getQuoteList = function (objQuoteInfo) {

        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetQuotes", objQuoteInfo, function onSuccess(response) {
            self.RenderQuotes(response);
            ErucaCRM.Framework.Common.ApplyPermission('._roleselector');
        }, function onError(err) {
            ErucaCRM.Framework.Core.ShowMessage(err.Message, true);
        });

    }

    self.RenderQuotes = function (QuoteListData) {

        $("#divNoRecord").hide();

        $("#QuoteListData").children().remove();

        if (QuoteListData.List.length == 0) {
            $("#divNoRecord").html("<b>" + ErucaCRM.Messages.Quote.NoQuoteFound + "</b>");
            $("#divNoRecord").show();
        }

        ko.utils.arrayForEach(QuoteListData.List, function (Quote) {
            self.QuoteList.push(new ErucaCRM.User.Quotes.QuoteListQueryModel(Quote));
        });

        $("#Pager").html(self.GetPaging(QuoteListData.TotalRecords, currentPage, PagingMethodName));
    };

    self.GetPageData = function (currentPageNo) {

        currentPage = currentPageNo;
        var objQuoteInfo = new Object();
        objQuoteInfo.CurrentPage = currentPageNo;

        self.getQuoteList(objQuoteInfo);

    }

    self.GetPaging = function (Rowcount, currentPage, methodName) {
        return ErucaCRM.Framework.Core.GetPagger(Rowcount, currentPage, methodName);
    }

    self.getQuoteList(objQuoteInfo);
    self.DeleteQuotes = function (data) {
        var quoteId = ko.utils.unwrapObservable(data.QuoteId);
        if (confirm(ErucaCRM.Messages.Quote.ConfirmQuoteDeleteAction)) {
            var PostData = new Object();   
            PostData.Id_encrypted = quoteId;
            ErucaCRM.Framework.Core.doDeleteOperation("DeleteQuote", PostData,
            function SuccessCallBack(response) {
                if (response.Status == "True") {
                    self.QuoteList.remove(function (item) { return item.QuoteId == data.QuoteId });
                    ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Quote.QuoteDeletedSuccess, false);
                }
                else {
                    ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Quote.QuoteDeletedFailure, true);
                }
            }, function FailureCallback() { })
        }

    }

}

