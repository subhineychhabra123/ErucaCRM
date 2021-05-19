//Register name space
jQuery.namespace('ErucaCRM.Admin.EditProfile');
var viewModel;

ErucaCRM.Admin.EditProfile.pageLoad = function (userId) {
    viewModel = new ErucaCRM.Admin.EditProfile.pageViewModel(userId);
    ko.applyBindings(viewModel, document.getElementById("InnerContentContainer"));
}

// A view model that represent a Test report query model.
ErucaCRM.Admin.EditProfile.editProfileQueryModel = function (queryModel) {
    var self = this;
    self.UseId = queryModel.UserId;
    //Product detail
    self.showDetail = function () {
        viewModel.getEditProfileById(self.UserId);
    }
}

//Page view model
ErucaCRM.Admin.EditProfile.pageViewModel = function (userId) {

    //Class variables
    var self = this;
    var controllerUrl = "/User";
    self.userId = userId;
    self.totalRecCount = 0;

    // Ko
    self.editProfileQueryModels = ko.observableArray();
    self.queryModel = ko.observable();
    self.status = ko.observable();
    //User Search by param
    self.getAllEditProfiles = function (bindForFirstTime) {

        var data = {
            corporation:self.userId
        };
        
        // Json operation
        ErucaCRM.Framework.Core.getJSONDataBySearchParam
        (
            controllerUrl + "GetProfileById",
            data,
            function onSuccess(editProfileQueryModels) {
                self.editProfileQueryModels = ko.observableArray([]);
                
            },
            function onError(err) {
                self.status(err.Message);
            }
        );
    }

    self.getEditProfileById = function (ProductId) {
        window.location.href = 'ProductDetail/' + ProductId;
    }

    self.DataSourceForGrid = function (ResultSet) {
        
        return datasource;
    }

    self.ParseEditProfiles = function (ResultSet) {
        $.each(ResultSet, function (index, queryModel) {
            self.editProfileQueryModels.push(new ErucaCRM.Admin.EditProfile.editProfileQueryModel(queryModel));
        })
        return self.editProfileQueryModels._latestValue;
    }

    self.getAllEditProfiles(false);
}