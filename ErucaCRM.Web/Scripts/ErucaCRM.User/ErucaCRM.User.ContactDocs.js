jQuery.namespace('ErucaCRM.User.ContactDocs');




ErucaCRM.User.ContactDocs.pageLoad = function () {

    viewModel = new ErucaCRM.User.ContactDocs.pageViewModel();
    ko.applyBindings(viewModel, document.getElementById("InnerContentContainer"));


}

//Page view model
ErucaCRM.User.ContactDocs.pageViewModel = function () {

    //Class variables
    var self = this;
    var uploadObj;

    var Message = {
        Failure: 'Failure',
        Success: 'Success'
    }

    var controllerUrl = "/User/";


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
            $('#contactdocs > tbody:last').append('<tr><td><a target=_blank href="' + data.FilePath + '">' + data.FileName + '</a></td><td>' + data.AttachedBy + '</td><td> <a docid=' + data.DocId + ' docname="' + data.DocName + '" class="_deleteDoc" href="javascript:void(0)">delete</a></td></tr>');
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

    $("._attachDoc").bind("click", function () {
        uploadObj.startUpload();
    });


    $("#contactdocs").on("click", "._deleteDoc", function () {
        var response = confirm("Are you sure you want to delete this document from attachment.");
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
                       if (response.Status == Message.Failure) { ErucaCRM.Framework.Core.ShowMessage(response.Message, true); }
                       else if (response.Status == Message.Success) {
                           var tr = $($td).closest('tr');
                           //tr.css("background-color", "#FF3700");
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