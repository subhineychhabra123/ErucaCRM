/// <reference path="ErucaCRM.User.TaskItem.js" />
jQuery.namespace('ErucaCRM.User.SaleOrders');

var viewModel;
var currentPage = 1;
flag = 0;
var ReserveSort = "";
var StoreSortvalue = null;
ErucaCRM.User.SaleOrders.pageLoad = function () {
    currentPage = 1;
    viewModel = new ErucaCRM.User.SaleOrders.pageViewModel();
    ko.applyBindings(viewModel, document.getElementById("InnerContentContainer"));
   
}



// A view model that represent a Test report query model.
ErucaCRM.User.SaleOrders.SaleOrderListQueryModel = function (data) {

    var self = this;
    self.SalesOrderId = data.SalesOrderId;
    self.Subject = data.Subject;
    self.OwnerId = data.OwnerId;
    self.SaleOrderDueDate = data.SaleOrderDueDate;
    self.Carrier = data.Carrier;
    self.OwnerName = data.OwnerName;
    self.AccountName = data.AccountName;


    self.DetailUrl = "ViewSalesOrderDetail/" + data.SalesOrderId;    
    self.EditUrl = "EditSalesOrder/" + data.SalesOrderId+"?returnurl=" + window.location.href;
    self.OwnerUrl = "UserProfile/" + data.OwnerId;
    self.AccountUrl = "AccountDetail/" + data.AccountId;
    self.DeleteSaleOrderUrl = "DeleteSaleOrder/" + data.SalesOrderId;

}
//Page view model

ErucaCRM.User.SaleOrders.pageViewModel = function () {
    //Class variables

    var self = this;
    var objSaleOrderInfo = new Object();
    objSaleOrderInfo.CurrentPage = currentPage;

    var PagingMethodName = "GetPageData";

    var controllerUrl = "/User/";
    var Message = {
        Failure: 'Failure',
        Success: 'Success'
    }

    self.SaleOrderListQueryModel = ko.observableArray();
    self.queryModel = ko.observable();
    self.status = ko.observable();
    this.SaleOrderList = ko.observableArray([]);


    self.getSaleOrderList = function (objSaleOrderInfo) {

        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetSaleOrders", objSaleOrderInfo, function onSuccess(response) {
            self.RenderSaleOrders(response);
            ErucaCRM.Framework.Common.ApplyPermission('._roleselector');
        }, function onError(err) {
            ErucaCRM.Framework.Core.ShowMessage(err.Message, true);
        });

    }


    $('._common').click(function () {

        $(".Dse").removeClass("Dse");
        $(".Ase").removeClass("Ase");
        var objTaskItemInfo = new Object();
        var sort = $(this).attr('data-sortby');
        var dir = $(this).attr('data-sortdir');
        if (flag == 0) {


            $(this).addClass("Ase").removeClass("Dse");
            flag = 1;
            ReserveSort = $(this).attr('data-sortby');
            StoreSortvalue = $(this).attr('data-sortby');
        }
        else if (flag == 1 && $(this).attr('data-sortby') == ReserveSort) {
            dir = 'D';
            $(this).addClass("Dse").removeClass("Ase");
            flag = 0;
        }
        else {
            ReserveSort = "";
            flag = 1;
            $(this).addClass("Ase").removeClass("Dse");
            ReserveSort = $(this).attr('data-sortby');
            StoreSortvalue = $(this).attr('data-sortby');
        }
        objSaleOrderInfo.sortColumnName = sort;
        objSaleOrderInfo.sortdir = dir;
        objSaleOrderInfo.CurrentPage = currentPage;
        self.getSaleOrderList(objSaleOrderInfo);
    });

    self.RenderSaleOrders = function (SaleOrderListData) {
      
        $("#divNoRecord").hide();
        self.SaleOrderList.removeAll();
        $("#SaleOrderListData").children().remove();

        if (SaleOrderListData.List.length == 0) {
            $("#divNoRecord").html("<b>" + ErucaCRM.Messages.SalesOrder.NoSalesOrderFound + "</b>");
            $("#divNoRecord").show();
        }

        ko.utils.arrayForEach(SaleOrderListData.List, function (SaleOrder) {
            self.SaleOrderList.push(new ErucaCRM.User.SaleOrders.SaleOrderListQueryModel(SaleOrder));
        });
        $("#Pager").html(self.GetPaging(SaleOrderListData.TotalRecords, currentPage, PagingMethodName));
    };

    self.DeleteSaleOrder = function (saleOrder) {

        if (confirm(ErucaCRM.Messages.SalesOrder.ConfirmSalesOrderDeleteAction)) {

            var data = new Object();
            data.id_encrypted = saleOrder.SalesOrderId;
                    

            ErucaCRM.Framework.Core.doDeleteOperation(controllerUrl + "DeleteSaleOrder", data, function onSuccess(response) {
    
                if (Message.Success == response.Status) {

                    ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.SalesOrder.SalesOrderDeletedSuccess, false);                       
                    self.SaleOrderList.remove(saleOrder);
                    if (self.SaleOrderList().length== 0) {
                        self.GetPageData(currentPage-1);
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
                    ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.SalesOrder.SalesOrderDeletedFailure, true);
                }
            });
        }

    }

    self.GetPageData = function (currentPageNo) {
        var objSaleOrderInfo = new Object();
        if (StoreSortvalue != null && flag == 0)
        {
            objSaleOrderInfo.sortColumnName = StoreSortvalue;
            objSaleOrderInfo.sortdir = 'D';
            currentPage = currentPageNo;
            objSaleOrderInfo.CurrentPage = currentPageNo;
            objSaleOrderInfo.FilterParameters = new Array();
            self.getSaleOrderList(objSaleOrderInfo);

        }
        else if (StoreSortvalue !=null && flag == 1) {
            objSaleOrderInfo.sortColumnName = StoreSortvalue;
            objSaleOrderInfo.sortdir = 'A';
            currentPage = currentPageNo;
            objSaleOrderInfo.CurrentPage = currentPageNo;
            objSaleOrderInfo.FilterParameters = new Array();
            self.getSaleOrderList(objSaleOrderInfo);
        }
        else {
            currentPage = currentPageNo;
            objSaleOrderInfo.CurrentPage = currentPageNo;
            objSaleOrderInfo.FilterParameters = new Array();
            self.getSaleOrderList(objSaleOrderInfo);
        }
    }
   
    self.GetPaging = function (Rowcount, currentPage, methodName) {
        return ErucaCRM.Framework.Core.GetPagger(Rowcount, currentPage, methodName);
    }


    self.getSaleOrderList(objSaleOrderInfo);
}
