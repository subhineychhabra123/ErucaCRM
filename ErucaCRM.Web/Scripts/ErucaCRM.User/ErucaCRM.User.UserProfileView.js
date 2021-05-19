jQuery.namespace('ErucaCRM.User.UserProfileView');
var viewModel;

ErucaCRM.User.UserProfileView.pageLoad = function () {

    viewModel = new ErucaCRM.User.UserProfileView.pageViewModel();
    ko.applyBindings(viewModel, document.getElementById("InnerContentContainer"));


}

ErucaCRM.User.UserProfileView.pageViewModel = function () {

    var self = this;

    $("#ChangePassword").click(function () {
        $("#TextBoxCurrentPassword").val("");
        $("#TextBoxNewPassword").val("");
        $("#TextBoxConfirmPassword").val("");

        ErucaCRM.Framework.Common.OpenPopup("#ChangePasswordSection");

    });


    $("#ButtonChangePassword").click(function () {
     
        if ($("#TextBoxCurrentPassword").val() == "") {
            alert(ErucaCRM.Messages.User.CurrentPasswordRequired);
            $("#TextBoxCurrentPassword").focus();
            return false;
        }
        if ($("#TextBoxNewPassword").val() == "") {
            alert(ErucaCRM.Messages.User.NewPasswordRequired);
            $("#TextBoxNewPassword").focus();
            return false;
        } else if ($("#TextBoxNewPassword").val().length< 6) {
            alert(ErucaCRM.Messages.User.Password6charRequired);
            return false;
            $("#TextBoxNewPassword").focus();
        }
        if ($("#TextBoxConfirmPassword").val() == "") {
            alert(ErucaCRM.Messages.User.ConfirmPasswordRequired);
            $("#TextBoxConfirmPassword").focus();
            return false;
        } 

        var varCurrentPassword = $("#TextBoxCurrentPassword").val();
        var varNewPassword = $("#TextBoxNewPassword").val();
        var varConfirmPassword = $("#TextBoxConfirmPassword").val();

        if (varNewPassword != varConfirmPassword) {
            alert(ErucaCRM.Messages.User.PasswordMatchRequired);
            return false;
        }





        var objUserChangePasswordInfo = new Object();
        objUserChangePasswordInfo.CurrentPassword = varCurrentPassword;
        objUserChangePasswordInfo.NewPassword = varNewPassword;

        self.ChangePassword(objUserChangePasswordInfo);



    });

    self.ChangePassword = function (objUserChangePasswordInfo) {
        $td = this;
        var Message = {
            Failure: 'Failure',
            Success: 'Success'
        }
        var controllerUrl = "/User/";

        var actionName = "ChangePassword";
        var hiddenSection = $('#ChangePasswordSection');
        ErucaCRM.Framework.Core.doPostOperation
               (

                   controllerUrl + actionName,
                   objUserChangePasswordInfo,
                   function onSuccess(response) {

                       if (response.Status == Message.Failure) { ErucaCRM.Framework.Core.ShowMessage(response.Message, true); }
                       else if (response.Status == Message.Success) {
                           if (response.Message == "true") {
                               ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.User.PasswordChangeSuccess, false);
                               $(hiddenSection).fadeOut();
                           }
                           else {
                               ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.User.CurrentPasswordIncorrect, true);

                           }





                       }
                   },
                   function onError(err) {
                       self.status(err.Message);
                   }
               );
    }


    self.CheckMinLen = function (obj) {
        if (obj.value.length < 6) {
            alert(ErucaCRM.Messages.User.Password6charRequired);
            $("#TextBoxNewPassword").focus();
            return false;
        }

    }

}

