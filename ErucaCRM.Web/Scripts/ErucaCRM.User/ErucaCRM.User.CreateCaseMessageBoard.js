jQuery.namespace('ErucaCRM.User.CreateCaseMessageBoard');

var viewModel;
var currentPage = 1;
var IsLeadsLastStage = false;
var taskItemsCurrentPage = 1;
var AccountCaseAttachmentsCurrentPage = 1;
var controllerUrl = "/User/";
var Message = {
    Failure: 'Failure',
    Success: 'Success'
}
var config = ErucaCRM.Framework.Core.Config;
ErucaCRM.User.CreateCaseMessageBoard.caseMessageBoardViewModel = function (queryModel) {

    var self = this;
    self.Description = queryModel.Description;
    self.CreatedBy = queryModel.CreatedByName;
    self.CreatedDate = queryModel.CreatedDateTimeText// == null ? null : dateFormat(eval(queryModel.CreatedDate.replace(/\/Date\((\d+)\)\//gi, "new Date($1)")), "HCisoUtcDateTime");
    self.UserDetailUrl = "/User/UserProfile/" + queryModel.UserId;
    self.CaseMessageBoardId = queryModel.CaseMessageBoardId;
    self.Attachments = ko.observableArray([]);
    ko.utils.arrayForEach(queryModel.FileAttachments, function (val) {
        self.Attachments.push(new ErucaCRM.User.CreateCaseMessageBoard.caseMessageBoardAttachmentViewModel(val));
    })
    return self;
}
ErucaCRM.User.CreateCaseMessageBoard.caseMessageBoardAttachmentViewModel = function (queryModel) {
    var self = this;
    self.Title = queryModel.DocumentName;
    self.FileSrc = queryModel.DocumentPath;
    self.FileName = queryModel.DocumentName +"."+ queryModel.CaseMessageBoardFilePath.substr((Math.max(0, queryModel.CaseMessageBoardFilePath.lastIndexOf(".")) || Infinity) + 1);
    return self;
}
ErucaCRM.User.CreateCaseMessageBoard.accountCaseAttachmentViewModel = function (queryModel) {
    var self = this;
    self.DocumentName = queryModel.DocumentName;
    self.Title = queryModel.DocumentName;
    self.DocumentPath = queryModel.AccountCaseFilePath;
    self.Name = queryModel.UserName;
    self.UserDetailUrl = "/User/UserProfile/" + queryModel.UserIdEncrypt;
    self.DocumentId = queryModel.DocumentId;
    self.DeleteDoc = function (data) {
        var response = confirm(ErucaCRM.Messages.Document.ConfirmDocumentDeleteAction);
        if (!response)
            return false;

        var actionName = "RemoveAccountCaseDocument";
        var obj = new Object();
        obj.DocumentId = data.DocumentId;
        obj.DocumentPath = data.DocumentPath
        ErucaCRM.Framework.Core.doPostOperation
               (
                   controllerUrl + actionName,
                   obj,
                   function onSuccess(response) {
                       if (response.Status == Message.Failure) { ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Document.DocumentNotRemoved, true); }
                       else if (response.Status == Message.Success) {
                           viewModel.AccountCaseAttachments.remove(data);                           
                           if (viewModel.AccountCaseAttachments().length == 0) {
                               $("._noAccountCaseAtttachmentsFound").show();
                           }
                           ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Document.DocumentRemoved, false);
                       }
                   },
                   function onError(err) {
                       self.status(err.Message);
                   }
               );

    }
    return self;
}
ErucaCRM.User.CreateCaseMessageBoard.TaskItemListQueryModel = function (data) {
    var self = this;
    self.TaskId = data.TaskId;
    self.Subject = data.Subject;
    self.DueDate = data.TaskDueDate;
    self.TaskStatus = data.TaskStatus;
    self.Priority = data.PriorityName;
    var AccountCaseId = $("._accountCaseId").find("input[type=hidden]").val();
    self.EditUrl = "/User/TaskItem?taskID_encrypted=" + data.TaskId + "&mod=AccountCase&val_encrypted=" + AccountCaseId + '&returnurl=' + window.location.href;;
    self.DetailUrl = "/User/ViewTaskItemDetail?mod=AccountCase&taskID_encrypted=" + data.TaskId;
}
ErucaCRM.User.CreateCaseMessageBoard.pageLoad = function () {
    currentPage = 1;
    viewModel = new ErucaCRM.User.CreateCaseMessageBoard.pageViewModel();
    ko.applyBindings(viewModel, document.getElementById("InnerContentContainer"));
}
ErucaCRM.User.CreateCaseMessageBoard.pageViewModel = function () {
    //Class variables
    var self = this;
    self.CaseMessageBoardList = ko.observableArray([]);
    self.FileAttachments = [];
    self.TaskItemList = ko.observableArray([]);
    self.AccountCaseAttachments = ko.observableArray([]);
    var PagingMethodName = "GetMessageBoardPageData";
    var TaskItemsPagingMethodName = "GetTaskItemsData";
    var AccountCaseAttachmentsPagingMethodName = "GetAccountCaseFileAttachmentsData";
    var responseAfterAllFilesUpload;
    var HaveFilesToUpload = false;
    var AreFilesUploaded = false;
    var objMessageBoardInfo = new Object();
    objMessageBoardInfo.CurrentPage = currentPage;
    self.Core = ErucaCRM.Framework.Core;
    var uploadObj;
    var uploadAccountCaseDocObj;
    uploadObj = $("#fileuploader").uploadFile({
        url: '/User/UploadMessageBoardDocument',
        multiple: true,
        maxFileCount: 5,
        autoSubmit: false,
        fileName: "docs",
        maxFileSize: 1024 * 10000,
        dynamicFormData: function () {
            var data = self.setPostData();
            return data;
        },
        showStatusAfterSuccess: false,
        multiDragErrorStr: "Multiple File Drag &amp; Drop is not allowed.",
        onSubmit: function (files) {


        },
        onSuccess: function (files, data, xhr) {
            var obj = new Object();
            obj.DocumentPath = data.DocumentPath;
            obj.DocumentName = data.DocumentName;
            obj.CaseMessageBoardFilePath = data.DocumentPath;
            self.FileAttachments.push(obj);
            $(".ajax-file-upload").next("span").find('b').remove();
            $(".ajax-file-upload").next("span").append('<a href="#" >' + data.DocumentName + '  </a><span style="color: #000000;">,</span>&nbsp;');
        },
        afterUploadAll: function (files) {
            self.InsertNewMessage();
            $('._AddNewMessageBoard').fadeOut("slow");
    
        },
        onSelect: function () {

        },
        onError: function (files, status, errMsg) {

        }
    });

   
    uploadAccountCaseDocObj = $("#AccountCaseFileuploader").uploadFile({
        url: '/User/UploadAccountCaseDocument',
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
      
            var accountCaseAttachment = new Object();
            accountCaseAttachment.UserName = data.AttachedBy;
            accountCaseAttachment.DocumentName = data.DocumentName;
            accountCaseAttachment.AccountCaseFilePath = data.FilePath;
            accountCaseAttachment.UserIdEncrypt = data.UserId;;
            accountCaseAttachment.DocumentId = data.DocId;
            var pages = self.AccountCaseAttachments().length;
            if (pages >= config.PageSize) {
                self.AccountCaseAttachments.pop();
            }
         
            self.AccountCaseAttachments.push(new ErucaCRM.User.CreateCaseMessageBoard.accountCaseAttachmentViewModel(accountCaseAttachment));
            ErucaCRM.Framework.Common.ApplyPermission('._roleselector');
                $('._noAccountCaseAtttachmentsFound').hide();
           
         
        },
        afterUploadAll: function (files) {

            $('#AccountCaseFileUploadSection').fadeOut("fast");
        },
        onSelect: function () {

        },
        onError: function (files, status, errMsg) {

        }
    });

    $('._close').click(function () {
        uploadObj.cancelAll();
        uploadObj.errorLog.empty();
        uploadAccountCaseDocObj.cancelAll();
        uploadAccountCaseDocObj.errorLog.empty();   
    });
    $("._AddAccountCaseDoc").bind("click", function () {
        //$(".ajax-file-upload").next("span").html('');
        ErucaCRM.Framework.Core.OpenRolePopup("#AccountCaseFileUploadSection");
    });
    
    $("._AccountCaseAttachDoc").bind("click", function () {
        uploadAccountCaseDocObj.startUpload();
    });

    $("._submitNewMessageBoard").bind("click", function () {
        var messageDescription = $.trim($("._MessageBoardDescription").val());
        if (messageDescription != '') {
            if (uploadObj.selectedFiles > 0) {
                uploadObj.startUpload();
                uploadObj.selectedFiles = 0;

            }
            else {
                self.InsertNewMessage();
                $('._AddNewMessageBoard').fadeOut("slow");
            }
            
           

        }
        else {
            ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.AccountCase.MessageBoardMessageDescriptionRequired, true);
            //alert("Enter your message description");
        }
    });
    self.DeleteTaskItem = function (task) {

        if (confirm(ErucaCRM.Messages.TaskItem.ConfirmTaskDeleteAction)) {
            var data = new Object();
            data.id_encrypted = task.TaskId;
            ErucaCRM.Framework.Core.doDeleteOperation(controllerUrl + "DeleteTaskItem", data, function onSuccess(response) {

                self.TaskItemList.remove(task);
                if (self.TaskItemList().length == 0) {
                    self.GetTaskItemsData(taskItemsCurrentPage - 1);
                }
                else {
                    if (taskItemsCurrentPage == 1) {
                        self.GetTaskItemsData(1);
                    }
                    else {
                        self.GetTaskItemsData(taskItemsCurrentPage);
                    }
                }
            });
        }
    }
    self.setPostData = function () {
        var obj = new Object;
        obj.AccountCaseId = $("._accountCaseId").find("input[type=hidden]").val();
        obj.Description = $('._MessageBoardDescription').val();
        obj.CaseMessageBoardId = null;
        obj.FileAttachments = self.FileAttachments;
        return obj;
    }

    $('._newCaseMessageBoard').click(function () {
        self.FileAttachments = [];
        $(".ajax-file-upload").next("span").html('');
        $("._MessageBoardDescription")[0].value='';
        ErucaCRM.Framework.Common.OpenPopup('._AddNewMessageBoard');
    });

    self.BindMessageBoard = function (response) {
        if (response.response.Status == Message.Success) {           
            uploadObj.fileCounter = 1;
            response.UserId = response.UserId;
            var savedSuccessfully = ErucaCRM.Messages.AccountCase.MessageBoardMessageSavedSuccessfully
            ErucaCRM.Framework.Core.ShowMessage(savedSuccessfully, false);
            response.FileAttachments = self.FileAttachments
            var pages = self.CaseMessageBoardList().length;
            if (pages >= config.PageSize) {
                self.CaseMessageBoardList.pop();
            }

            self.CaseMessageBoardList.unshift(new ErucaCRM.User.CreateCaseMessageBoard.caseMessageBoardViewModel(response));
            $("._MessageBoardNoRecord").hide();
            $('._AddNewMessageBoard').fadeOut("fast");
        }
    }
    self.InsertNewMessage = function () {
        self.Core.doPostOperation(controllerUrl + "SaveMessageBoardMessage", self.setPostData(), function (response) {
            self.BindMessageBoard(response);
        })
    }
    self.GetMessageBoardPageData = function (currentPageNo) {
        currentPage = currentPageNo;
        var objMessageBoardInfo = new Object();
        objMessageBoardInfo.AccountCaseId = $("._accountCaseId").find("input[type=hidden]").val();
        objMessageBoardInfo.CurrentPage = currentPageNo;
        self.GetMessageBoardList(objMessageBoardInfo);

    }
    self.GetTaskItemsData = function (currentPageNo) {
        taskItemsCurrentPage = currentPageNo;
        var objMessageBoardInfo = new Object();
        objMessageBoardInfo.AccountCaseId = $("._accountCaseId").find("input[type=hidden]").val();
        objMessageBoardInfo.CurrentPage = currentPageNo;
        self.GetTaskItemsList(objMessageBoardInfo);

    }
    self.GetAccountCaseFileAttachmentsData = function (currentPageNo) {
        AccountCaseAttachmentsCurrentPage = currentPageNo;
        var objMessageBoardInfo = new Object();
        objMessageBoardInfo.AccountCaseId = $("._accountCaseId").find("input[type=hidden]").val();
        objMessageBoardInfo.CurrentPage = currentPageNo;
        self.GetAccountCaseAttachmentList(objMessageBoardInfo);

    }
    self.GetMessageBoardList = function (objMessageBoardInfo) {

        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetCaseMessageBoardMessages", objMessageBoardInfo, function onSuccess(response) {
            self.RenderMessageBoard(response);
            ErucaCRM.Framework.Common.ApplyPermission('._roleselector');
        }, function onError(err) {
            ErucaCRM.Framework.Core.ShowMessage(err.Message, true);
        });

    }
    self.GetTaskItemsList = function (objMessageBoardInfo) {

        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetAccountCaseTasks", objMessageBoardInfo, function onSuccess(response) {
            self.RenderTaskItems(response);
            ErucaCRM.Framework.Common.ApplyPermission('._roleselector');
        }, function onError(err) {
            ErucaCRM.Framework.Core.ShowMessage(err.Message, true);
        });

    }
    self.GetAccountCaseAttachmentList = function (objMessageBoardInfo) {

        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetAccountCaseAttachments", objMessageBoardInfo, function onSuccess(response) {
            
            self.RenderAccountCaseAttachments(response);
            ErucaCRM.Framework.Common.ApplyPermission('._roleselector');
        }, function onError(err) {
            ErucaCRM.Framework.Core.ShowMessage(err.Message, true);
        });

    }
    self.GetPaging = function (Rowcount, currentPage, methodName, uniqueMethodName) {
        return ErucaCRM.Framework.Core.GetPagger(Rowcount, currentPage, methodName, uniqueMethodName);
    }
    self.RenderMessageBoard = function (MessageBoardListData) {
        $("._MessageBoardNoRecord").hide();
        $("#AssociatedMessageBoardMessages").children().remove();

        if (MessageBoardListData.List.length == 0) {
            $("._MessageBoardNoRecord").html("<b>" + ErucaCRM.Messages.AccountCase.NoMessageBoardRecordFound + "</b>");
            $("._MessageBoardNoRecord").show();
        }

        ko.utils.arrayForEach(MessageBoardListData.List, function (MessageBoard) {

            self.CaseMessageBoardList.push(new ErucaCRM.User.CreateCaseMessageBoard.caseMessageBoardViewModel(MessageBoard));

        });

        $("._MessageBoardPager").html(self.GetPaging(MessageBoardListData.TotalRecords, currentPage, PagingMethodName, "MessageBoardPager"));
    };
    self.RenderTaskItems = function (TaskItemListData) {
        $("._noTaskItemsFound").hide();

        $("#TaskItemListData").children().remove();

        if (TaskItemListData.List.length == 0) {
            $("._noTaskItemsFound").html("<b>" + ErucaCRM.Messages.AccountCase.NoTaskItemRecordFound + "</b>");
            $("._noTaskItemsFound").show();
        }

        ko.utils.arrayForEach(TaskItemListData.List, function (taskItem) {

            self.TaskItemList.push(new ErucaCRM.User.CreateCaseMessageBoard.TaskItemListQueryModel(taskItem));

        });
    
        $("._taskItemsPager").html(self.GetPaging(TaskItemListData.TotalRecords, taskItemsCurrentPage, TaskItemsPagingMethodName, "TaskItemPager"));
    };
    self.RenderAccountCaseAttachments = function (accountCaseAttachments) {
        $("._noAccountCaseAtttachmentsFound").hide();

        $("#AccountCaseAttachments").children().remove();

        if (accountCaseAttachments.List.length == 0) {
            $("._noAccountCaseAtttachmentsFound td").html("<b>" + ErucaCRM.Messages.AccountCase.NoAttachmentRecordFound + "</b>");
            $("._noAccountCaseAtttachmentsFound").show();
        }

        ko.utils.arrayForEach(accountCaseAttachments.List, function (accountCaseAttachment) {
            self.AccountCaseAttachments.push(new ErucaCRM.User.CreateCaseMessageBoard.accountCaseAttachmentViewModel(accountCaseAttachment));
        });
        
        $("._AccountCaseAttachmentPager").html(self.GetPaging(accountCaseAttachments.TotalRecords, AccountCaseAttachmentsCurrentPage, AccountCaseAttachmentsPagingMethodName, "AccountCaseAttachmentPager"));
    };
    self.GetMessageBoardPageData(currentPage);
    self.GetTaskItemsData(currentPage);
    self.GetAccountCaseFileAttachmentsData(currentPage);

}