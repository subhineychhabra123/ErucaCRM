
var jqXHRData;
$(document).ready(function () {


    initAutoFileUpload();

    $("#hl-start-upload-with-size").on('click', function () {
        if (jqXHRData) {
            var isStartUpload = true;
            var uploadFile = jqXHRData.files[0];

            if (!(/\.(gif|jpg|jpeg|tiff|png)$/i).test(uploadFile.name)) {
                alert('You must select an image file only');
                isStartUpload = false;
            } else if (uploadFile.size > 4000000) { // 4mb
                alert('Please upload a smaller image, max size is 4 MB');
                isStartUpload = false;
            }
            if (isStartUpload) {
               
                jqXHRData.submit();
            }
        }
        return false;
    });
});
function IsValidImageAndSize(jqXHRData) {
    var isStartUpload = true;
    var uploadFile = jqXHRData.files[0];
    if (!(/\.(gif|jpg|jpeg|tiff|png)$/i).test(uploadFile.name)) {
        alert('You must select an image file only');
        isStartUpload = false;
    } else if (uploadFile.size > 4000000) { // 4mb
        alert('Please upload a smaller image, max size is 4 MB');
        isStartUpload = false;
    }
    return isStartUpload;
}
function initAutoFileUpload() {
    'use strict';
  
    $('#fileupload').fileupload({
        autoUpload: true,
        url: '/User/UploadImage',
        dataType: 'json',
        add: function (e, data) {
            if (IsValidImageAndSize(data)) {
                ErucaCRM.Framework.Common.OpenPopup('#profileimgpop');
                
                var jqXHR = data.submit()
                    .success(function (data, textStatus, jqXHR) {
                       
                        if (data.statusCode == 200) {
                            var d = new Date();
                            if ($("#ProfileImage").length > 0) {
                                $("#ProfileImage").attr("src", data.file + "?" + d.getTime());
                                $("._profileimg").attr("src", data.file + "?" + d.getTime());
                            }
                            else {
                                $("#ProfileOtherImage").attr("src", data.file + "?" + d.getTime());
                            }
                        }
                        else {

                        }
                        $('#profileimgpop').hide();
                    })
                    .error(function (data, textStatus, errorThrown) {
                        if (typeof (data) != 'undefined' || typeof (textStatus) != 'undefined' || typeof (errorThrown) != 'undefined') {
                            alert(textStatus + errorThrown + data);
                        }
                    });
            }
        },
        fail: function (event, data) {
            if (data.files[0].error) {
                alert(data.files[0].error);
            }
        }
    });
}

