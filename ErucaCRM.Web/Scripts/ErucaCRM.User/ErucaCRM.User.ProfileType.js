//Register name space
jQuery.namespace('ErucaCRM.User.ProfileType');
var viewModel;
ErucaCRM.User.ProfileType.pageLoad = function () {
    viewModel = new ErucaCRM.User.ProfileType.pageViewModel();
    ko.applyBindings(viewModel, document.getElementById("InnerContentContainer"));
   
}

// A view model that represent a Test report query model.
ErucaCRM.User.ProfileType.profileTypeViewModel = function (data) {
    var self = this;
    self.ProfileId = data.ProfileId;
    self.ProfileName = data.ProfileName;
    self.CompanyId = data.CompanyId;
    var descriptionkey=data.Description
    self.Description = ErucaCRM.Messages.Profile.descriptionkey == undefined ? descriptionkey : ErucaCRM.Messages.Profile.descriptionkey;
    self.Url = "EditProfileType/" + data.ProfileId;
    self.DetailUrl = "ProfileDetail/" + data.ProfileId;
    self.Visible = data.IsDefaultForRegisterdUser == true || data.IsDefaultForStaffUser == false ? false : true;
  
}

//Page view model
ErucaCRM.User.ProfileType.pageViewModel = function (emailId) {
    //Class variables
    var self = this;
    var controllerUrl = "/User/";
    var Message = {
        Failure: 'Failure',
        Success: 'Success'
    }

    self.profileTypeViewModels = ko.observableArray();
    self.queryModel = ko.observable();
    self.status = ko.observable();
    this.profileType = ko.observableArray([]);
    self.getProfileTypeList = function () {
        ErucaCRM.Framework.Core.getJSONData(controllerUrl + "ProfileTypeList", function onSuccess(response) {
            self.profileType.removeAll();
            self.SuccessfullyRetrievedModelsFromAjax(response);
            //ko.applyBindings(self.profileType);
           

        }, function onError(err) {
            self.status(err.Message);
        });
    }
    self.getProfileTypeList();
    self.CurrentProfileData = ko.observable();
    self.SuccessfullyRetrievedModelsFromAjax = function (profileTypeList) {
        ko.utils.arrayForEach(profileTypeList, function (profile) {
            self.profileType.push(new ErucaCRM.User.ProfileType.profileTypeViewModel(profile));
        });
    };
    self.deletedprofileId = ko.observable();
    self.OpenReassignProfilePopup=function(data)
    { 
        if (confirm(ErucaCRM.Messages.Profile.ConfirmProfileDeletionAction)) {
            self.CurrentProfileData = data;
            var deletedprofileId = ko.utils.unwrapObservable(data.ProfileId)
            $('._proflelstdropdown').empty();
            $('._proflelstdropdown').append("<option value='0'>" + ErucaCRM.Messages.DropDowns.SelectOption + "</option>");
            self.BindProfileList(self.profileType(), deletedprofileId);
            ErucaCRM.Framework.Common.OpenPopup('div.assignProfile');
        }
    }
    self.BindProfileList=function(data, deletedprofileId)
    {
        $.each(data, function (index, element) {
            if (element.ProfileId != deletedprofileId) {
              
                $('._proflelstdropdown').append("<option value='" + element.ProfileId + "'>" + element.ProfileName + "</option>");
               
                
            }
        });

    }
    $('._reassignrolebtn').click(function () {
        var reassignedId=$('._proflelstdropdown').val();
        if (reassignedId != '' && reassignedId != "0") {
            self.DeleteProfile(self.CurrentProfileData, reassignedId);
        } else {
           alert(ErucaCRM.Messages.Profile.SelectProfileRequired);
        }
    });
    self.DeleteProfile = function (data,reassignProfileId) {
     
        var profileId = ko.utils.unwrapObservable(data.ProfileId)
        var PostData = new Object();
        PostData.profileId_encrypted = profileId;
        PostData.reassignProfileId_encrypted = reassignProfileId;
        ErucaCRM.Framework.Core.doDeleteOperation("DeleteProfile", PostData,
                     function SuccessCallBack(response) {
                         if (response.Status == true) {
                          
                             ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Profile.ProfileDeletedSuccess, false);
                             self.profileType.remove(function (item) { return item.ProfileId == data.ProfileId });
                             $('._close').click();
                         }
                         else {
                             ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Profile.ProfileDeletedFailure, true);
                         }
                     }, function FailureCallback() { })

    }
}