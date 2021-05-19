//Register name space
jQuery.namespace('ErucaCRM.User.ProfileDetail');
var viewModel;

ErucaCRM.User.ProfileDetail.pageLoad = function () {
    viewModel = new ErucaCRM.User.ProfileDetail.pageViewModel();
    ko.applyBindings(viewModel, document.getElementById("InnerContentContainer"));
}

// A view model that represent a Test report query model.
ErucaCRM.User.ProfileDetail.profileDetailViewModel = function (queryModel) {
    var self = this;
    self.HasAccess = queryModel.HasAccess;
    self.ProfilePermissionId = queryModel.ProfilePermissionId;
}

//Page view model
ErucaCRM.User.ProfileDetail.pageViewModel = function (hasAccess) {
    //Class variables
    var self = this;
    var controllerUrl = "/User/";
    var Message = {
        Failure: 'Failure',
        Success: 'Success'
    }

    $('#PermissionContainer').on("click", "#chkPermission", function () {
   
        var checked = $(this).is(':checked');
        var $Td = $(this).parent();
        var colName = $.trim($Td.parent().children().index($($Td)));
        var title = $.trim($Td.closest("table").find("th").eq(colName).text());        
        if (title == "View" && checked == false) {
            $Td.parent().find("input[type=checkbox]").removeAttr('checked');
        }
    });
    $("._savePermissions").on("click", function () {
        var permissionCount = $("#PermissionContainer").find("input[type=checkbox]").length;
        if (permissionCount > 0) {
            var obj = self.GetPermissions();
            self.changeAccess(obj);
        }
        else { alert(ErucaCRM.Messages.Profile.NothingToSave) }
    });
  
   
    self.queryModel = ko.observable();
    self.status = ko.observable();
    self.GetPermissions = function () {
        var profilePermissions = new Array();
        $("#PermissionContainer").find("input[type=checkbox]").each(function (i, ob) {
            var chk = this;
            var permissionId = $(chk).attr("ppid");
            var hasAccess = $(chk).is(':checked');
            var profileId = $(chk).attr("pid");
            var obj = new Object();
            obj.HasAccess = hasAccess;
            obj.ProfilePermissionId = permissionId;
            obj.ProfileId = profileId;
            profilePermissions.push(obj);
        });
        return profilePermissions;
    }
    //User Search by param
    self.changeAccess = function (permissionList) {
        ErucaCRM.Framework.Core.doPostOperation
                (
                    controllerUrl + "UpdateProfilePermissionAccess",
                    permissionList,
                    function onSuccess(response) {                      
                        if (response.Status == Message.Failure) {
                            ErucaCRM.Framework.Core.ShowMessage(response.Message, true);
                        }
                        else if (response.Status == Message.Success) {
                            ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Profile.ProfileSavedSuccess, false);
                            setTimeout("window.history.back()", 3000);
                        }

                    },
                    function onError(err) {
                        self.status(err.Message);
                    }
                );
    }
}