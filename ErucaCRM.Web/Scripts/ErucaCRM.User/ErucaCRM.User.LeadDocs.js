jQuery.namespace('ErucaCRM.User.LeadDocs');


var viewModel;

ErucaCRM.User.LeadDocs.pageLoad = function () {
    
    viewModel = new ErucaCRM.User.LeadDocs.pageViewModel();
    ko.applyBindings(viewModel, document.getElementById("InnerContentContainer"));


}




//Page view model
ErucaCRM.User.LeadDocs.pageViewModel = function () {
    //Class variables
    var self = this;
    var controllerUrl = "/User/";

    var uploadObj;

    var Message = {
        Failure: 'Failure',
        Success: 'Success'
    }

    uploadObj = $("#fileuploader").uploadFile({
        url: '/User/UploadLeadDocument',
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
            $('#leaddocs > tbody:last').append('<tr><td><a target=_blank href="' + data.FilePath + '">' + data.FileName + '</a></td><td>' + data.AttachedBy + '</td><td> <a docid=' + data.DocId + ' class="_deleteDoc" href="javascript:void(0)">delete</a></td></tr>');
            $('div.hidden').fadeOut();
        },
        afterUploadAll: function () {

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
        uploadObj.startUpload();
    });
    $("#leaddocs").on("click", "._deleteDoc", function () {

        var response = confirm("Are you sure you want to delete this document from attachment.");
        if (!response)
            return false;

        var actionName = "RemoveLeadDocument";
        $td = this;
        var docid = $($td).attr("docid");
        var obj = new Object();
        obj.DocumentId = docid;
        ErucaCRM.Framework.Core.doPostOperation
               (
                   controllerUrl + actionName,
                   obj,
                   function onSuccess(response) {
                       if (response.Status == Message.Failure) { ErucaCRM.Framework.Core.ShowMessage(response.Message, true); }
                       else if (response.Status == Message.Success) {
                           var tr = $($td).closest('tr');
                          // tr.css("background-color", "#FF3700");
                           tr.fadeOut(400, function () {
                               tr.remove();
                           });
                           ErucaCRM.Framework.Core.ShowMessage(response.Message, false);

                       }
                   },
                   function onError(err) {
                       self.status(err.Message);
                   }
               );

    });

    self.setPostData = function () {
        var obj = new Object;
        obj.LeadId = $("._leadId").find("input[type=hidden]").val();
        return obj;
    }
}
