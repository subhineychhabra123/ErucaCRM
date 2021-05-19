jQuery.namespace('ErucaCRM.User.Invoices');

var viewModel;
var currentPage = 1;

ErucaCRM.User.Invoices.pageLoad = function () {

    currentPage = 1;
   
    viewModel = new ErucaCRM.User.Invoices.pageViewModel();
    ko.applyBindings(viewModel, document.getElementById("InnerContentContainer"));
  
}



// A view model that represent a Test report query model.
ErucaCRM.User.Invoices.InvoiceListQueryModel = function (data) {

    var self = this;
    self.InvoiceId = data.InvoiceId;
    self.Subject = data.Subject;
    self.OwnerId = data.OwnerId;
    self.InvoiceCreationDate = data.InvoiceCreationDate;
    self.InvoiceDueDate = data.InvoiceDueDate;
    self.OwnerName = data.OwnerName;
    self.LeadName = data.LeadName;


    self.DetailUrl = "ViewInvoiceDetail/" + data.InvoiceId;
    self.EditUrl = "EditInvoice/" + data.InvoiceId;    
    self.OwnerUrl = "UserProfile/" + data.OwnerId;
    self.LeadUrl = "ViewLeadDetail/" + data.LeadId;

}
//Page view model

ErucaCRM.User.Invoices.pageViewModel = function () {
    //Class variables

    var self = this;
    var objInvoiceInfo = new Object();
    objInvoiceInfo.CurrentPage = currentPage;

    var PagingMethodName = "GetPageData";

    var controllerUrl = "/User/";
    var Message = {
        Failure: 'Failure',
        Success: 'Success'
    }

    self.InvoiceListQueryModel = ko.observableArray();
    self.queryModel = ko.observable();
    self.status = ko.observable();
    this.InvoiceList = ko.observableArray([]);

    self.getInvoiceList = function (objInvoiceInfo) {

        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetInvoices", objInvoiceInfo, function onSuccess(response) {
            self.RenderInvoices(response);
            ErucaCRM.Framework.Common.ApplyPermission('._roleselector');
        }, function onError(err) {
            ErucaCRM.Framework.Core.ShowMessage(err.Message, true);
        });

    }

    self.RenderInvoices = function (InvoiceListData) {

        $("#divNoRecord").hide();

        $("#InvoiceListData").children().remove();

        if (InvoiceListData.List.length == 0) {
            $("#divNoRecord").html("<b>" + ErucaCRM.Messages.Invoice.NoInvoiceFound + "</b>");
            $("#divNoRecord").show();
        }

        ko.utils.arrayForEach(InvoiceListData.List, function (Invoice) {
            self.InvoiceList.push(new ErucaCRM.User.Invoices.InvoiceListQueryModel(Invoice));
        });

        $("#Pager").html(self.GetPaging(InvoiceListData.TotalRecords, currentPage, PagingMethodName));
    };

    self.GetPageData = function (currentPageNo) {

        currentPage = currentPageNo;
        var objInvoiceInfo = new Object();
        objInvoiceInfo.CurrentPage = currentPageNo;

        self.getInvoiceList(objInvoiceInfo);

    }





    self.GetPaging = function (Rowcount, currentPage, methodName) {
        return ErucaCRM.Framework.Core.GetPagger(Rowcount, currentPage, methodName);
    }



    self.getInvoiceList(objInvoiceInfo);
    self.DeleteInvoice = function (data) {
        var invoiceId = ko.utils.unwrapObservable(data.InvoiceId);
        if (confirm(ErucaCRM.Messages.Invoice.ConfirmInvoiceDeleteAction)) {
            var PostData = new Object();
            PostData.Id_encrypted = invoiceId;
            ErucaCRM.Framework.Core.doDeleteOperation("DeleteInvoice", PostData,
            function SuccessCallBack(response) {
                if (response.Status == "True") {
                    self.InvoiceList.remove(function (item) { return item.InvoiceId == data.InvoiceId });
                    ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Invoice.InvoiceDeletedSuccess, false);
                }
                else {
                    ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Invoice.InvoiceDeletedFailure, true);
                }
            }, function FailureCallback() { })
        }

    }
}

