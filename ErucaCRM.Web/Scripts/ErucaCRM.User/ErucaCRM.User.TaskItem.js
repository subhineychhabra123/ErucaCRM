/*************************************************************************************************************/

$(document).ready(function () {
    // $('.datepicker').datepicker({ dateFormat: "yy-mm-dd", changeMonth: true, changeYear: true, yearRange: '1900:' + new Date().getFullYear() });
    ErucaCRM.Framework.Core.SetGlobalDatepicker('DueDate');
    $("#AssociatedModuleId").change(function () { $("#AssociatedModuleValue").val(0); loadAssociatedModules() });
    $("#drpAssociateModule").change(function () { selectAssociatedModuleValue() });
    $("#drpOwners").change(function () { selectOwnerId() });

    if ($('#AssociatedModuleId').children('option').length > 0)
        loadAssociatedModules();
});

function selectOwnerId() {
    $("#OwnerId").val($('#drpOwners').val());
}
function loadAssociatedModules() {

    var associateModuleId = $("#AssociatedModuleId").val();

    var postData = { ModuleId: associateModuleId }
    $.ajax({
        url: '/User/GetAssociatedModuleValues',
        data: JSON.stringify(postData),
        async: "true",
        type: "POST",
        dataType: "text",
        returnType: "json",
        contentType: "application/json, charset=UTF-8",
        success: function (response) {
            response = JSON.parse(response);
            $('#drpAssociateModule').html("");
            $('#drpAssociateModule').append("<option value=0>" + ErucaCRM.Messages.DropDowns.SelectOption + "</option>");
            $.each(response, function (index, element) {
                $('#drpAssociateModule').append("<option value='" + element.Id + "'>" + element.value + "</option>");
            });
            if ($("#AssociatedModuleValue").val() != 0)
                $('#drpAssociateModule').val($("#AssociatedModuleValue").val());

            $("#AssociatedModuleValue").val($('#drpAssociateModule').val());
        },
        error: function (a, b, c) {
            alert("Error occured while posting comments!");
        }
    });
}
function selectAssociatedModuleValue() {
    $("#AssociatedModuleValue").val($('#drpAssociateModule').val());
}
