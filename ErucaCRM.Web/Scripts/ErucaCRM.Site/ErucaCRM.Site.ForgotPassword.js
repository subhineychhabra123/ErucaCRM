//Register name space
jQuery.namespace('ErucaCRM.Site.ForgotPassword');
var viewModel;

ErucaCRM.Site.ForgotPassword.pageLoad = function () {
    viewModel = new ErucaCRM.Site.ForgotPassword.pageViewModel();
    ko.applyBindings(viewModel);
}

// A view model that represent a Test report query model.
ErucaCRM.Site.ForgotPassword.forgotPasswordViewModel = function (queryModel) {
    var self = this;
    self.EmailId = queryModel.EmailId;
}

//Page view model
ErucaCRM.Site.ForgotPassword.pageViewModel = function (emailId) {  
    //Class variables
    var self = this;
    var controllerUrl = "/Site/";
    var Message = {
        Failure: 'Failure',
        Success: 'Success'
    }
    $("#RecoverPassword").click(function () {
       
        self.EmailId = $.trim($("#EmailId").val());
        if (self.EmailId == '') {
            $(".forgotpassword-error").text(ErucaCRM.Messages.Login.EmailRequired).show()
            self.HideMessage();
        }
        else {
            if (self.IsEmailValid(self.EmailId) == true) { self.getPassword(); }
            else { $(".forgotpassword-error").text(ErucaCRM.Messages.Registration.InvalidEmailAddress).show(); }
            self.HideMessage()
        }
    });

    self.forgotPasswordViewModels = ko.observableArray();
    self.queryModel = ko.observable();
    self.status = ko.observable();
    //User Search by param
    self.getPassword = function () {
     
        ErucaCRM.Framework.Core.doPostOperation
                (
                    controllerUrl + "ForgotPassword",
                    self,
                    function onSuccess(response)
                    {
                        $('#EmailId').val("");
                        if (response.Status == Message.Failure) {
                           
                            $(".forgotpassword-error").text(response.Message).removeClass("msgssuccess").show();
                            self.HideMessage();
                            return false;
                           
                        }
                        else if (response.Status == Message.Success) {
                            $(".forgotpassword-error").text(response.Message).addClass("msgssuccess").show();;
                            self.HideMessage();
                            return false;
                        }
                        return false;
                    },
                    function onError(err) {
                       
                        self.status(err.Message);
                    }
                );
      
    }
    self.HideMessage = function () {
       
        setTimeout(function () { $(".forgotpassword-error").hide() }, 2000);

    }

    self.IsEmailValid = function (sEmail) {
      
        var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        if (filter.test(sEmail)) {
            return true;
        }
        else {
            return false;
        }
    }

}