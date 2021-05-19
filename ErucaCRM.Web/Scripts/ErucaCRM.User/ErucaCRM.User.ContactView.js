jQuery.namespace('ErucaCRM.User.ContactView');

var viewModel;
var currentPage = 1;
var filterBy = "Active";
var IsLeadsLastStage = false;
ErucaCRM.User.ContactView.pageLoad = function () {
    currentPage = 1;
    filterBy = "Active";
    viewModel = new ErucaCRM.User.ContactView.pageViewModel();
    ko.applyBindings(viewModel);
    if ($("#docContactList").children("tr").length == 0 && $("#contactdocs").find("tr._dataRow").length == 0) {
        HtmlNoRecords = "<span><b> " + ErucaCRM.Messages.Account.NoAccountAttachmentRecordFound + "</b></span>";
        $("#trNoContactDocRecord").children("td").html(HtmlNoRecords);
        $("#trNoContactDocRecord").show();
    }
}

ErucaCRM.User.ContactView.GetGridData = function (currentPage, filterBy) {

    viewModel = new ErucaCRM.User.ContactView.pageViewModel(currentPage, filterBy);
    ko.applyBindings(viewModel, document.getElementById("InnerContentContainer"));
   

}


// A view model that represent a Test report query model.
ErucaCRM.User.ContactView.TaskItemListQueryModel = function (data) {

    var self = this;
    self.TaskId = data.TaskId;
    self.Subject = data.Subject;
    self.DueDate = data.TaskDueDate;
    self.TaskStatus = data.TaskStatus;
    self.Priority = data.PriorityName;

    var ContactID = $("._contactId").find("input[type=hidden]").val();

    self.EditUrl = "/User/TaskItem?taskID_encrypted=" + data.TaskId + "&mod=Contact&val_encrypted=" + ContactID + "&returnurl="+window.location.href+"";

    self.DetailUrl = "/User/ViewTaskItemDetail?mod=Contact&taskID_encrypted=" + data.TaskId;

   
}

ErucaCRM.User.ContactView.DocListQueryModel = function (data) {   
    var self = this;
    self.FileName = data.FileName;
    self.AttachedBy = data.AttachedBy;
    self.DocId = data.DocId;
    self.FilePath = data.FilePath;
}
//Page view model

ErucaCRM.User.ContactView.pageViewModel = function () {
    //Class variables
    var self = this;
    var objTaskItemInfo = new Object();
    objTaskItemInfo.ContactID = $("._contactId").find("input[type=hidden]").val();
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
    this.DocContactList = ko.observableArray([]);

    self.getTaskItemList = function (objTaskItemInfo) {
        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetContactTasks", objTaskItemInfo, function onSuccess(response) {
            self.RenderTaskItems(response);
            ErucaCRM.Framework.Common.ApplyPermission('._roleselector');
        }, function onError(err) {
            ErucaCRM.Framework.Core.ShowMessage(err.Message, true);
        });

    }

    self.RenderTaskItems = function (TaskItemListData) {
        $("#divNoRecord").hide();
        $("#TaskItemListData").children().remove();
        self.TaskItemList.removeAll();
        if (TaskItemListData.List.length == 0) {
            $("#divNoRecord").html("<br/><center><b>"+ ErucaCRM.Messages.TaskItem.NoTaskFound+"</b></center>");
            $("#divNoRecord").show();
        }

        ko.utils.arrayForEach(TaskItemListData.List, function (TaskItem) {
            self.TaskItemList.push(new ErucaCRM.User.ContactView.TaskItemListQueryModel(TaskItem));
        });

        $("#Pager").html(self.GetPaging(TaskItemListData.TotalRecords, currentPage, PagingMethodName));
    };

    self.GetPageData = function (currentPageNo) {

        currentPage = currentPageNo;
        var objTaskItemInfo = new Object();
        objTaskItemInfo.CurrentPage = currentPageNo;
        objTaskItemInfo.ContactID = $("#ContactId").val();
        objTaskItemInfo.FilterParameters = new Array();
       
        self.getTaskItemList(objTaskItemInfo);

    }



    self.GetTaskItemDetail = function (taskID) {

    }

    self.GetPaging = function (Rowcount, currentPage, methodName) {
        return ErucaCRM.Framework.Core.GetPagger(Rowcount, currentPage, methodName);
    }

    self.DeleteTaskItem = function (task) {
     
        if (confirm(ErucaCRM.Messages.TaskItem.ConfirmTaskDeleteAction)) {
            var data = new Object();
            data.id_encrypted = task.TaskId;
            ErucaCRM.Framework.Core.doDeleteOperation(controllerUrl + "DeleteTaskItem", data, function onSuccess(response) {
                self.TaskItemList.remove(task);
                
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


 

    self.getTaskItemList(objTaskItemInfo);

    var uploadObj;

    var Message = {
        Failure: 'Failure',
        Success: 'Success'
    }




    uploadObj = $("#fileuploader").uploadFile({
      
        url: '/User/UploadContactDocument',
        multiple: false,
        autoSubmit: false,
        fileName: "docs",
        maxFileSize: 1024 * 10000,
        dynamicFormData: function () {
            var data = self.setPostData();
            return data;
        },
      
        showStatusAfterSuccess: false,
        onSubmit: function (files) {

        },
        onSuccess: function (files, data, xhr) {
            ErucaCRM.Framework.Core.ShowMessage(data.response.Message, false);
          
            self.DocContactList.push(new ErucaCRM.User.ContactView.DocListQueryModel(data));
            ErucaCRM.Framework.Common.ApplyPermission('._roleselector');
            //$('#contactdocs > tbody:last').append('<tr><td><a target=_blank href="' + data.FilePath + '">' + data.FileName + '</a></td><td>' + data.AttachedBy + '</td><td> <a docid=' + data.DocId + ' docname="' + data.DocName + '" class="_deleteDoc" href="javascript:void(0)">delete</a></td></tr>');
            $('div.hidden').fadeOut();
            $("#trNoContactDocRecord").hide();
          
        },
        afterUploadAll: function () {
            
            $('#FileUploadSection').fadeOut("slow");
        },
        onError: function (files, status, errMsg) {

        }
    });


    $("._adddoc").bind("click", function () {
        ErucaCRM.Framework.Core.OpenRolePopup("#FileUploadSection");
    });

    $("._attachDoc").bind("click", function () {
        
        if (uploadObj.selectedFiles > 0) {
            uploadObj.startUpload();
            uploadObj.selectedFiles = 0;
          
        } else {
            alert(ErucaCRM.Messages.TaskItem.PleaseSelectFile);
            return;
        }
    });

    self.RemoveDoc = function (docs) {
        var response = confirm(ErucaCRM.Messages.Document.ConfirmDocumentDeleteAction);
        if (!response)
            return false;
        var actionName = "RemoveContactDocument";
    
        var docid = docs.DocId
        var obj = new Object();
        obj.DocumentId = docid;
        obj.DocumentPath = docs.FilePath;
        ErucaCRM.Framework.Core.doPostOperation
               (
                   "/User/" + actionName,
                   obj,
                   function onSuccess(response) {
                       if (response.Status == Message.Failure) { ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Document.DocumentNotRemoved, true); }
                       else if (response.Status == Message.Success) {
                           self.DocContactList.remove(docs);
                           ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Document.DocumentRemoved, false);
                      
                           if ($("#docContactList").children("tr").length == 0 && $("#contactdocs").find("tr._dataRow").length == 0) {
                           
                               HtmlNoRecords = "<span><b> " + ErucaCRM.Messages.Account.NoAccountAttachmentRecordFound + "</b></span>";
                               $("#trNoContactDocRecord").children("td").html(HtmlNoRecords);
                               $("#trNoContactDocRecord").show();
                           }
                       }
                   },
                   function onError(err) {
                       self.status(err.Message);
                   }
               );
    }
    $("#contactdocs").on("click", "._deleteDoc", function () {
        var response = confirm(ErucaCRM.Messages.Document.ConfirmDocumentDeleteAction);
        if (!response)
            return false;

        var actionName = "RemoveContactDocument";
        $td = this;
        var docid = $($td).attr("docid");
        var docname = $($td).attr("docname");
        var obj = new Object();
        obj.DocumentId = docid;
        obj.DocumentPath = docname;
        ErucaCRM.Framework.Core.doPostOperation
               (
                   controllerUrl + actionName,
                   obj,
                   function onSuccess(response) {
                       if (response.Status == Message.Failure) { ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Document.DocumentNotRemoved, true); }
                       else if (response.Status == Message.Success) {
                           var tr = $($td).closest('tr');
                           //tr.css("background-color", "#FF3700");
                           tr.fadeOut(400, function () {
                               tr.remove();
                           });
                          
                           if ($("#contactdocs").find("tr._dataRow").length == 1 && $("#docContactList").children("tr").length == 0) {
                             
                               HtmlNoRecords = "<span><b> " + ErucaCRM.Messages.Account.NoAccountAttachmentRecordFound + "</b></span>";
                               $("#trNoContactDocRecord").children("td").html(HtmlNoRecords);
                               $("#trNoContactDocRecord").show();
                           }
                           ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Document.DocumentRemoved, false);

                       }
                   },
                   function onError(err) {
                       self.status(err.Message);
                   }
               );

    });


    var availableTags = ["Bussiness Manager", "Potential", "IMPORTANT", "Jan", "Feb", "March", "April", "May", "June", "July", "August", "Sept", "Oct", "Nov", "Dec", "Seminar", "Webinar", "Interested", "Soon", "Contact"];
    $("#tags").autocomplete({
        source: availableTags
    });



    self.setPostData = function () {
        var obj = new Object;
        obj.ContactId = $("._contactId").find("input[type=hidden]").val();
        return obj;
    }
}
