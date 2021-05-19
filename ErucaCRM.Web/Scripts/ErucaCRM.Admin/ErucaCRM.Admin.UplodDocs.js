jQuery.namespace('ErucaCRM.Admin.UploadDocs')
ErucaCRM.Admin.UploadDocs.pageLoad = function () {

    ErucaCRM.Admin.UploadDocs.pageViewModel();
}
ErucaCRM.Admin.UploadDocs.pageViewModel = function () {
    var self = this;
    var uploadObj;
    var methodName;
    var Message = {
        FileNotUploaded: "You have not uploaded any language file.",
        FileSaved: "Language file has been saved",
        SaveResponse: undefined
    }

    uploadObj = $("#fileuploader").uploadFile({
        url: '/Admin/UpdateCultureDocument' ,
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
            $(".ajax-file-upload").next("span").html('<a href="#" onclick="downloadCultureFile()">' + files + '</a>');
            if (data.success == true) {
                ErucaCRM.Framework.Core.ShowMessage(data.response, false);
                $('#FileUploaded').val("True");
            }
            else {
                ErucaCRM.Framework.Core.ShowMessage(data.response, true);
                $('#FileUploaded').val("False");
            }
        },
        afterUploadAll: function () {

        },
        onError: function (files, status, errMsg) {

        }
    });

    $("._attachDoc").bind("click", function () {
        uploadObj.startUpload();
    });
    $('._download').click(function () {
        {
            var id_encrypted = $('#CultureInformationId').val();

            $('#iframeDownload').attr("src", "/Admin/DownloadFile?id_encrypted=" + id_encrypted);
        }
    });
    $('#chkActive').click(function () {
        var PostData = new Object();
        if ($('#chkActive').is(':checked')) {
            Message.SaveResponse = "Language is Activated.";
        }
        else {
            Message.SaveResponse = "Language is De-Activated.";
        }
        PostData.IsActive = $('#chkActive').is(':checked')
      
        if ($('#ExcelFilePath').val() != "" || $('#FileUploaded').val() == "True") {
            PostData.CultureInformationId = $('#CultureInformationId').val();
            ErucaCRM.Framework.Core.doPostOperation("/Admin/" + "SaveLanguage", PostData, function onSuccess(response) {
                ErucaCRM.Framework.Core.ShowMessage(Message.SaveResponse, false);
            });
        }
        else {
            ErucaCRM.Framework.Core.ShowMessage(Message.FileNotUploaded, true);
        }

    })
    self.setPostData = function () {
        var obj = new Object();
        obj.CultureInformationId = $("#CultureInformationId").val();
        obj.CultureName = $("#CultureName").val();
        obj.Language = $("#Language").val();
        obj.IsActive = $('#chkActive').is(':checked');
        obj.ExcelFilePath = $("#ExcelFilePath").val();
        
        return obj;
    }

}

function downloadCultureFile() {
    var id_encrypted = $('#CultureInformationId').val();
    $('#iframeDownload').attr("src", "/Admin/DownloadFile?id_encrypted=" + id_encrypted);
}