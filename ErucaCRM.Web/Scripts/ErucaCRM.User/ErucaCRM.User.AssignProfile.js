//Register name space
jQuery.namespace('ErucaCRM.User.AssignProfile');
var viewModel;

ErucaCRM.User.AssignProfile.pageLoad = function () {
    viewModel = new ErucaCRM.User.AssignProfile.pageViewModel();
    ko.applyBindings(viewModel, document.getElementById("InnerContentContainer"));
}

// A view model that represent a Test report query model.
ErucaCRM.User.AssignProfile.assignProfileViewModel = function (queryModel) {
    var self = this;
    self.UserId = queryModel.UserId;
    self.ProfileId = queryModel.UserTypeId;
}

//Page view model
ErucaCRM.User.AssignProfile.pageViewModel = function (hasAccess) {
    //Class variables
    var self = this;
    var controllerUrl = "/User/";
    var Message = {
        Failure: 'Failure',
        Success: 'Success'
    }
    $('._assignProfile table').on("click", "#chkAllProfiles", function () {
        var checked = $(this).is(':checked');
        if (checked == true) {
            $('._assignProfile table').find("._chkProfile").prop('checked', $(this).prop("checked"));
            $('._assignProfile table').find("select").removeAttr('disabled');
        }
        else {
            $('._assignProfile table').find("._chkProfile").prop('checked', $(this).prop("checked"));
            $('._assignProfile table').find("select").attr('disabled', 'disabled');
        }
    });
    $('._assignProfile table').on("click", "#chkProfile", function () {
        var checked = $(this).is(':checked');
        var $Td = $(this).parent();
        if (checked == true) {
            $Td.parent().find("select").removeAttr('disabled');
        }
        else {
            $Td.parent().find("select").attr('disabled', 'disabled');
        }
    });
    $("._savePermissions").on("click", function () {
        var permissionCount = $("._assignProfile").find("._chkProfile:checked").length;
        if (permissionCount > 0) {
            var obj = self.getAssignedProfiles();
            self.assignRoles(obj);
        }
        else { alert(ErucaCRM.Messages.Profile.NothingToSave) }
    });
    self.queryModel = ko.observable();
    self.status = ko.observable();
    self.getAssignedProfiles = function () {
        var profilePermissions = new Array();
        $("._assignProfile table").find("._chkProfile:checked").each(function (i, ob) {
            var chk = this;
            var $Tr = $(chk).parent().parent();
            var Ischecked = $(chk).is(':checked');
            if (Ischecked == true) {
                var profileId = $Tr.find("select").val();
                var userId = $Tr.find("input[type=hidden]").val();
                var obj = new Object();
                obj.ProfileId = profileId;
                obj.UserId = userId;
                profilePermissions.push(obj);
            }
        });
        return profilePermissions;
    }
    self.assignRoles = function (roleList) {
        ErucaCRM.Framework.Core.doPostOperation
                (
                    controllerUrl + "AssignProfile",
                    roleList,
                    function onSuccess(response) {
                        if (response.Status == Message.Failure) { $(".error").text(response.Message).removeClass("msgssuccess") }
                        else if (response.Status == Message.Success) {
                            $(".error").text(response.Message).addClass("msgssuccess");
                            window.location.href = window.location.href;
                        }
                    },
                    function onError(err) {
                        self.status(err.Message);
                    }
                );
    }
}