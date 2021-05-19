//Register name space
jQuery.namespace('ErucaCRM.User.RoleAssignment');
var viewModel;

ErucaCRM.User.RoleAssignment.pageLoad = function () {
    viewModel = new ErucaCRM.User.RoleAssignment.pageViewModel();
    ko.applyBindings(viewModel, document.getElementById("InnerContentContainer"));
}

// A view model that represent a Test report query model.
ErucaCRM.User.RoleAssignment.roleAssignmentViewModel = function (queryModel) {
    var self = this;
    self.UserId = queryModel.UserId;
    self.UserTypeId = queryModel.UserTypeId;
}

//Page view model
ErucaCRM.User.RoleAssignment.pageViewModel = function (hasAccess) {
    //Class variables
    var self = this;
    var controllerUrl = "/User/";
    var Message = {
        Failure: 'Failure',
        Success: 'Success'
    }
    $('._roleAssignment table').on("click", "#chkAllrole", function () {
        var checked = $(this).is(':checked');
        if (checked == true) {
            $('._roleAssignment table').find("._chkrole").prop('checked', $(this).prop("checked"));
            $('._roleAssignment table').find("select").removeAttr('disabled');
        }
        else {
            $('._roleAssignment table').find("._chkrole").prop('checked', $(this).prop("checked"));
            $('._roleAssignment table').find("select").attr('disabled', 'disabled');
        }
    });
    $('._roleAssignment table').on("click", "#chkrole", function () {
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
        var permissionCount = $("._roleAssignment").find("._chkrole:checked").length;
        if (permissionCount > 0) {
            var obj = self.GetAssignedRoles();
            self.assignRoles(obj);
        }
        else { alert("Oops! nothing to save here.") }
    });
    self.queryModel = ko.observable();
    self.status = ko.observable();
    self.GetAssignedRoles = function () {
        var rolePermissions = new Array();
        $("._roleAssignment table").find("._chkrole:checked").each(function (i, ob) {
            var chk = this;
            var $Tr = $(chk).parent().parent();
            var Ischecked = $(chk).is(':checked');
            if (Ischecked == true) {
                var roleId = $Tr.find("select").val();
                var userId = $Tr.find("input[type=hidden]").val();
                var obj = new Object();
                obj.UserTypeId = roleId;
                obj.UserId = userId;
                rolePermissions.push(obj);
            }
        });
        return rolePermissions;
    }
    self.assignRoles = function (roleList) {
        ErucaCRM.Framework.Core.doPostOperation
                (
                    controllerUrl + "RoleAssignment",
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