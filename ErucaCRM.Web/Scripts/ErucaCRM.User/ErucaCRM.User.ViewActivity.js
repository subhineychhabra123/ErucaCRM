jQuery.namespace('ErucaCRM.User.ViewActivity');
var viewModel;
var currentPage = 1;
var filterBy = "Active";
ErucaCRM.User.ViewActivity.pageLoad = function () {
    currentPage = 1;
    filterBy = "Active";
    viewModel = new ErucaCRM.User.ViewActivity.pageViewModel();
    ko.applyBindings(viewModel, document.getElementById("InnerContentContainer"));
}
ErucaCRM.User.ViewActivity.ActivityDocListQueryModel = function (data) {
    var self = this;
    self.FileName = data.FileName;
    self.AttachedBy = data.AttachedBy;
    self.DocId = data.DocId;
    self.FilePath = data.FilePath;

}
ErucaCRM.User.ViewActivity.pageViewModel = function () {
    //Class variables
    var self = this;
    self.ActivityDocList = ko.observableArray();
    var Message = {
        "Success": "Success",
        "Failure": "Failure"
    };
    var uploadObj;

    uploadObj = $("#fileuploader").uploadFile({
        url: '/User/UploadActivityDocument',
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
         
            self.ActivityDocList.push(new ErucaCRM.User.ViewActivity.ActivityDocListQueryModel(data))
            ErucaCRM.Framework.Common.ApplyPermission('._roleselector');
            //$('#activitydocs > tbody:last').append('<tr><td><a target=_blank href="' + data.FilePath + '">' + data.FileName + '</a></td><td>' + data.AttachedBy + '</td><td> <a docid=' + data.DocId + ' class="_deleteDoc" href="javascript:void(0)">Delete</a></td></tr>');
            $('div.hidden').fadeOut();
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
    $("#btnAddProduct").bind("click", function () {
        ErucaCRM.Framework.Core.OpenRolePopup("#AddProductSection");
    });

    $("#btnAddNewProduct").bind("click", function () {
        $(':text').each(function () { $(this).val(""); })
        ErucaCRM.Framework.Core.OpenRolePopup("#addNewProductSection");
    });



    $("._attachDoc").bind("click", function () {
        if (uploadObj.selectedFiles > 0) {
            uploadObj.startUpload();
            uploadObj.selectedFiles = 0;
          
        }
        else {
            alert(ErucaCRM.Messages.TaskItem.PleaseSelectFile);
            return;
        }
    });

    self.RemoveDoc = function (docs) {

        var response = confirm(ErucaCRM.Messages.Document.ConfirmDocumentDeleteAction);
        if (!response)
            return false;       
        var actionName = "RemoveActivityDocument";      
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
                           self.ActivityDocList.remove(docs);
                           ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Document.DocumentRemoved, false);

                       }
                   },
                   function onError(err) {
                       self.status(err.Message);
                   }
               );
    }
    $("._deleteDoc").click( function () {

        var response = confirm(ErucaCRM.Messages.Document.ConfirmDocumentDeleteAction);
        if (!response)
            return false;

        var actionName = "RemoveActivityDocument";
        $td = this;
        var docid = $($td).attr("docid");
        var obj = new Object();
        obj.DocumentId = docid;
        ErucaCRM.Framework.Core.doPostOperation
               (
                   "/User/" + actionName,
                   obj,
                   function onSuccess(response) {
                       if (response.Status == Message.Failure) { ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Document.DocumentNotRemoved, true); }
                       else if (response.Status == Message.Success) {
                           var tr = $($td).closest('tr');
                           //tr.css("background-color", "#FF3700");
                           tr.fadeOut(400, function () {
                               tr.remove();
                           });
                           ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Document.DocumentRemoved, false);

                       }
                   },
                   function onError(err) {
                       self.status(err.Message);
                   }
               );

    });

    self.setPostData = function () {
        var obj = new Object;
        obj.TaskId = $('#TaskId').val();
        return obj;
    }







}

