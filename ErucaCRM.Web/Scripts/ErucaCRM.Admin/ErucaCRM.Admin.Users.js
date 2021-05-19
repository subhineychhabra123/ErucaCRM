jQuery.namespace('ErucaCRM.User.List');

var viewModel;
var currentPage = 1;
var userStatus = "Active";
var Activate = "";
ErucaCRM.User.List.pageLoad = function () {

    currentPage = 1;

    userStatus = "Active";
    viewModel = new ErucaCRM.User.List.pageViewModel(currentPage, userStatus);
    ko.applyBindings(viewModel);

    $("#ActiveInActive").html("Active")
    $("#ActiveInActive").hide();
    $("#ActiveInActive").html($("#ActiveInActive").attr("Activate"));
    $("#DropDownListUserStatus option[value=Active]").text("Active");
    $("#DropDownListUserStatus option[value=Inactive]").text("Inactive");
  
}

ErucaCRM.User.List.GetGridData = function (currentPage, userStatus) {

    viewModel = new ErucaCRM.User.List.pageViewModel(currentPage, userStatus);
    ko.applyBindings(viewModel);

}


// A view model that represent a Test report query model.
ErucaCRM.User.List.UserListQueryModel = function (data) {
     var self = this;
    var d = new Date();
    self.UserId = data.UserId;
    self.IsCurrentUser = data.IsCurrentUser;
    self.Name = data.FirstName + " " + (data.LastName == null ? "" : data.LastName);
    self.EmailId = data.EmailId;
  
    self.Designation = "";
    if (data.Role != undefined) {
        self.Designation = data.Role.RoleName;
    }
    self.ProfileName = "";
    if (data.Profile != undefined) {
        self.ProfileName = data.Profile.ProfileName;
    }
    if (data.ImageURL == null) {
        self.Url = "/Uploads/users/no_image.gif";
    }
    else {
        self.Url = "/Uploads/users/" + data.ImageURL;
    }

    self.Url = self.Url + "?=" + d.getUTCMilliseconds();
    self.UserDetail = "/Admin/UserProfile/" + data.UserId;//{new @userId=" + data.UserId + "}";
  
    self.UrlDelete = "DeleteUser/{new @userId=" + data.UserId + "}";
    self.UrlError = "this.onerror = null; this.src = '/Uploads/users/no_image.gif'";
    //  self.ActivateText = ErucaCRM.Messages.User.ActivateText;
    //  self.DeactivateText = ErucaCRM.Messages.User.DeactivateText;
    self.MoreDetailsText = "more detail";
}
//Page view model

ErucaCRM.User.List.pageViewModel = function (currentPage, userStatus) {
    //Class variables
    var self = this;
    var objUserInfo = new Object();
    objUserInfo.CurrentPage = currentPage;
    objUserInfo.UserStatus = userStatus;
    var PagingMethodName = "GetPageData";

    var controllerUrl = "/Admin/";
    var Message = {
        Failure: 'Failure',
        Success: 'Success'
    }

    self.UserListQueryModel = ko.observableArray();
    self.queryModel = ko.observable();
    self.status = ko.observable();
    this.UserList = ko.observableArray([]);
    self.getUserList = function (objUserInfo) {
    	
        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetAdminUserList", objUserInfo, function onSuccess(response) {
            self.SuccessfullyRetrievedModelsFromAjax(response);

        }, function onError(err) {
            self.status(err.Message);
        });

    }



    self.getUserList(objUserInfo);

    self.SuccessfullyRetrievedModelsFromAjax = function (UserListData) {
        $("#divNoRecord").hide();
        $("#UserListData").children().remove();
        if (UserListData.listUser.length == 0) {
            $("#divNoRecord").html("<b>" + "NoUserFound" + "</b>");
            $("#divNoRecord").show();
        }

        ko.utils.arrayForEach(UserListData.listUser, function (User) {
            self.UserList.push(new ErucaCRM.User.List.UserListQueryModel(User));
        });
        $(".detail").html("more detail");
        $("#Pager").html(self.GetPaging(UserListData.TotalRecords, currentPage, PagingMethodName));
    };

    self.GetPageData = function (currentPageNo) {

        currentPage = currentPageNo;
        var objUserInfo = new Object();
        objUserInfo.CurrentPage = currentPageNo;
        objUserInfo.UserStatus = $("#DropDownListUserStatus").val();
        self.getUserList(objUserInfo);

    }

    $("#ActiveInActive").click(function () {

        var listObjUsers = new Array();
        var objUser = new Object();

        $("#UserListData input:checked").each(function () {
            currentPage = 1
            objUser = new Object();
            objUser.UserID = $(this).attr("id");
            objUser.CurrentPage = currentPage;
            objUser.UserStatus = $("#DropDownListUserStatus").val();
            listObjUsers.push(objUser);
        });

        self.UpdateUserStatus(listObjUsers);

    });

    self.UpdateUserStatus = function (listObjUsers) {

        ErucaCRM.Framework.Core.doPostOperation(controllerUrl + "UpdateUserStatus", listObjUsers, function onSuccess(response) {
            self.SuccessfullyUpdateUserStatus(response);

        }, function onError(err) {
            self.status(err.Message);
        });
    };

    self.SuccessfullyUpdateUserStatus = function (result) {

        if (result == "UpdateSucess") {
            $("#ActiveInActive").hide();
            var objUserInfo = new Object();
            objUserInfo.CurrentPage = currentPage;
            objUserInfo.UserStatus = userStatus;

            self.getUserList(objUserInfo);
        }
        else {
            alert("User Status Not Updated");
            return false;
        }


    }

    $("#DropDownListUserStatus").change(function () {
        currentPage = 1;
        $("#ActiveInActive").hide();

        userStatus = $("#DropDownListUserStatus").val();
        var objUserInfo = new Object();
        objUserInfo.CurrentPage = currentPage;
        objUserInfo.UserStatus = userStatus;
        self.getUserList(objUserInfo);


    });


    self.ActivateDeactivate = function (obj) {

        if ($("#UserListData input:checked").length > 0)
            $("#ActiveInActive").show();
        else
            $("#ActiveInActive").hide();

        if ($("#DropDownListUserStatus").val() == "Active") {
            if ($("#ActiveInActive").html() == "Active") {
                $("#ActiveInActive").html("Inactive");
            }
        }
        else {
            if ($("#ActiveInActive").html() == "Inactive") {
                $("#ActiveInActive").html("Active");
            }

        }
    }


    self.GetPaging = function (Rowcount, currentPage, methodName) {
        return ErucaCRM.Framework.Core.GetPagger(Rowcount, currentPage, methodName);
    }

}

