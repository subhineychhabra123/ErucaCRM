jQuery.namespace('ErucaCRM.User.Tasks');

var viewModel;
var currentPage = 1;
var IsLeadsLastStage = false;
var filterBy = "Active";
flag = 0;
var ReserveSort = "";
var StoreSortvalue = null;
ErucaCRM.User.Tasks.pageLoad = function () {
    currentPage = 1;
    viewModel = new ErucaCRM.User.Tasks.pageViewModel();
    ko.applyBindings(viewModel, document.getElementById("InnerContentContainer"));

}

ErucaCRM.User.Tasks.GetGridData = function (currentPage, filterBy) {

    viewModel = new ErucaCRM.User.Tasks.pageViewModel(currentPage, filterBy);
    ko.applyBindings(viewModel, document.getElementById("InnerContentContainer"));
}


// A view model that represent a Test report query model.
ErucaCRM.User.Tasks.TaskItemListQueryModel = function (data) {
    var self = this;
    self.TaskId = data.TaskId;
    self.Subject = data.Subject;
    self.DueDate = data.TaskDueDate;
    self.TaskStatus = data.TaskStatus;
    self.Priority = data.PriorityName;
    self.Owner = new Object();
    self.Owner.ContactName = "";
    if (data.User != undefined) {
        self.Owner.ContactName = data.User.FirstName + " " + data.User.LastName;
    }
    self.DetailUrl = "ViewTaskItemDetail?taskID_encrypted=" + data.TaskId;
    self.EditUrl = "TaskItem?taskID_encrypted=" + data.TaskId + "&returnurl=" + window.location.href;
    
}
//Page view model

ErucaCRM.User.Tasks.pageViewModel = function () {
    //Class variables
    var self = this;
    var objTaskItemInfo = new Object();
    objTaskItemInfo.CurrentPage = currentPage;
    objTaskItemInfo.FilterParameters = new Array();
    var PagingMethodName = "GetPageData";

    var controllerUrl = "/User/";
    var Message = {
        Failure: 'Failure',
        Success: 'Success'
    }

    self.TaskItemListQueryModel = ko.observableArray();
    self.queryModel = ko.observable();
    self.status = ko.observable();
    this.TaskItemList = ko.observableArray([]);

    self.getTaskItemList = function (objTaskItemInfo) {
        self.TaskListList = ko.observableArray([]);
        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetTasks", objTaskItemInfo, function onSuccess(response) {
            self.RenderTaskItems(response);
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
        objTaskItemInfo.sortColumnName = sort;
        objTaskItemInfo.sortdir = dir;
        objTaskItemInfo.CurrentPage = currentPage;
        self.getTaskItemList(objTaskItemInfo);
    });
    self.RenderTaskItems = function (TaskItemListData) {
        self.TaskItemList.removeAll();
        $("#divNoRecord").hide();
        $("#TaskItemListData").children().remove();
        if (TaskItemListData.List.length == 0) {
            $("#divNoRecord").html("<b>" + ErucaCRM.Messages.TaskItem.NoTaskFound + "</b>");
            $("#divNoRecord").show();
        }

        ko.utils.arrayForEach(TaskItemListData.List, function (TaskItem) {
            self.TaskItemList.push(new ErucaCRM.User.Tasks.TaskItemListQueryModel(TaskItem));
        });

        $("#Pager").html(self.GetPaging(TaskItemListData.TotalRecords, currentPage, PagingMethodName));
    };

    self.GetPageData = function (currentPageNo) {
        var objTaskItemInfo = new Object();
        if (StoreSortvalue != null && flag == 0)
        {
            objTaskItemInfo.sortColumnName = StoreSortvalue;
            objTaskItemInfo.sortdir = 'D';
            currentPage = currentPageNo;
            objTaskItemInfo.CurrentPage = currentPageNo;
            objTaskItemInfo.FilterParameters = new Array();
            self.getTaskItemList(objTaskItemInfo);

        }
        else if (StoreSortvalue !=null && flag == 1) {
            objTaskItemInfo.sortColumnName = StoreSortvalue;
            objTaskItemInfo.sortdir = 'A';
            currentPage = currentPageNo;
            objTaskItemInfo.CurrentPage = currentPageNo;
            objTaskItemInfo.FilterParameters = new Array();
            self.getTaskItemList(objTaskItemInfo);
        }
        else {
            currentPage = currentPageNo;
            objTaskItemInfo.CurrentPage = currentPageNo;
            objTaskItemInfo.FilterParameters = new Array();
            self.getTaskItemList(objTaskItemInfo);
        }
    }

    self.DeleteTaskItem = function (Task) {

        if (confirm(ErucaCRM.Messages.TaskItem.ConfirmTaskDeleteAction)) {
            var data = new Object();
            data.id_encrypted = Task.TaskId;
            ErucaCRM.Framework.Core.doDeleteOperation(controllerUrl + "DeleteTaskItem", data, function onSuccess(response) {
                // self.GetPageData(1);
                self.TaskItemList.remove(Task);
                if (self.TaskItemList().length == 0) {
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
            });
        }
    }

    self.GetTaskItemDetail = function (taskID) {

    }

    self.GetPaging = function (Rowcount, currentPage, methodName) {
        return ErucaCRM.Framework.Core.GetPagger(Rowcount, currentPage, methodName);
    }

    //self.DeleteTaskItem = function (taskID) {
    //    if (confirm("Are you sure you want to delete this task?")) {
    //        var data = new Object();
    //        data.TaskId = taskID;
    //        ErucaCRM.Framework.Core.doDeleteOperation(controllerUrl + "DeleteTaskItem", data, function onSuccess(response) {
    //            var objTaskItemInfo = new Object();
    //            objTaskItemInfo.CurrentPage = 1;
    //            objTaskItemInfo.FilterParameters = new Array();
    //            self.getTaskItemList(objTaskItemInfo);
    //        });
    //    }
    //}

    self.getTaskItemList(objTaskItemInfo);
}
