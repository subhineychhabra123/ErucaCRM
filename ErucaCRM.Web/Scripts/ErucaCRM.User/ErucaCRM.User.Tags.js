jQuery.namespace('ErucaCRM.User.Tags');

var viewModel;
var currentPage = 1;
ErucaCRM.User.Tags.pageLoad = function () {

    currentPage = 1;
    viewModel = new ErucaCRM.User.Tags.pageViewModel();
    ko.applyBindings(viewModel, document.getElementById("InnerContentContainer"));

}



// A view model that represent a Test report query model.
ErucaCRM.User.Tags.TagListQueryModel = function (data) {

    var self = this;
    self.TagId = data.TagId;
    self.TagName = data.TagName;
    self.Description = data.Description;
    self.TagDetailUrl = "TagDetail/" + data.TagId;
}

//Page view model


ErucaCRM.User.Tags.pageViewModel = function () {
    //Class variables

    var self = this;
    var objTagInfo = new Object();
    objTagInfo.CurrentPage = currentPage;
    objTagInfo.FilterBy = "";

    var PagingMethodName = "GetPageRecords";

    var controllerUrl = "/User/";
    var Message = {
        Failure: 'Failure',
        Success: 'Success'
    }

    self.TagListQueryModel = ko.observableArray();
    self.queryModel = ko.observable();
    self.status = ko.observable();
    this.TagList = ko.observableArray([]);

    self.getTagList = function (objTagInfo) {

        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetTagList", objTagInfo, function onSuccess(response) {
            self.SuccessfullyRetrievedModelsFromAjax(response);
            //ErucaCRM.Framework.Common.ApplyPermission('._roleselector');
        }, function onError(err) {
            self.status(err.Message);
        });
    }

  

    self.getTagList(objTagInfo);

    self.SuccessfullyRetrievedModelsFromAjax = function (TagListData) {
        $("#divNoRecord").hide();
        $("#TagListData").children().remove();

        if (TagListData.ListTags.length == 0) {
            $("#divNoRecord").html("<b>" + ErucaCRM.Messages.Tag.NoRecordFound + "</b>");
            $("#divNoRecord").show();
        }

        ko.utils.arrayForEach(TagListData.ListTags, function (Tag) {
            self.TagList.push(new ErucaCRM.User.Tags.TagListQueryModel(Tag));
            ErucaCRM.Framework.Common.ApplyPermission('._roleselector');
        });

        $("#Pager").html(self.GetPaging(TagListData.TotalRecords, currentPage, PagingMethodName));
        $("#Pager").hide();
    };

    $('._newTag').click(function () {
        //$('._tagHeader').text("Add Edit Tag");
        $('#hdnTagId').val(0);
        $('._tagName').val('');
        $('._tagDescription').val('');
        $('._tagHeader').text(ErucaCRM.Messages.Tag.PopUpHeaderAddTag);
        ErucaCRM.Framework.Common.OpenPopup('#tagpop');


    });

    self.EditTag = function (data) {
    
        $('._tagHeader').text(ErucaCRM.Messages.Tag.PopUpHeaderEditTag);  
        var tagId = ko.utils.unwrapObservable(data.TagId);
        var tagName = ko.utils.unwrapObservable(data.TagName);
        var tagDescription = ko.utils.unwrapObservable(data.Description);

        $('#hdnTagId').val(tagId);
        $('._tagName').val(tagName);
        $('._tagDescription').val(tagDescription);
        ErucaCRM.Framework.Common.OpenPopup('#tagpop');
    }

    self.DeleteTag = function (data) {
        var response = confirm(ErucaCRM.Messages.Tag.ConfirmTagDeleteAction);
        if (response) {
            var PostData = new Object();
            PostData.Id_encrypted = data.TagId;
            ErucaCRM.Framework.Core.doDeleteOperation(controllerUrl + "DeleteTag", PostData,
            function SuccessCallBack(response) {
                if (response.Status == "True") {

                    ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Tag.TagDeletedSuccess, false);
                    var objTagInfo = new Object();
                    objTagInfo.CurrentPage = 1;
                    self.getTagList(objTagInfo);
                }
                else {
                    ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Tag.TagDeletedDeletedFailure, true);
                }
            }, function FailureCallback() { })

        }
        else
            return false;

    }

    $('._submitbtn').click(function () {
        var actionName = "UpdateTag";
        var successMessage = ErucaCRM.Messages.Role.RoleSaved;
        self.TagId = $('#hdnTagId').val();
        self.TagName = $.trim($("._tagName").val());
        self.Description = $.trim($("._tagDescription").val());
        if (self.TagName == '') {
            $(".error").text(ErucaCRM.Messages.Tag.TagNameRequired).removeClass("msgssuccess"); return;
        }





        if (self.TagId == 0) {
            actionName = "AddTag";
            successMessage = ErucaCRM.Messages.Tag.TagCreatedSuccesfully;//ErucaCRM.Messages.Role.RoleCreated;
        }
        self.SaveTag(actionName, successMessage);
      
    });


    self.SaveTag = function (actionName, successMessage) {
        var PostData = new Object();
        PostData.TagId = self.TagId;
        PostData.TagName = self.TagName;
        PostData.Description = self.Description;

        ErucaCRM.Framework.Core.doPostOperation
                (
                    controllerUrl + actionName,
                    PostData,
                    function onSuccess(response) {
                        if (response.Status == Message.Failure) { ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Tag.TagAlreadyExists, true); }
                        else if (response.Status == Message.Success) {
                            ErucaCRM.Framework.Core.ShowMessage(successMessage, false);

                            var objTagInfo = new Object();
                            objTagInfo.CurrentPage = currentPage;


                            self.getTagList(objTagInfo);

                            $('._close').click();
                        }
                    },
                    function onError(err) {

                        self.status(err.Message);
                    }
                );
    }


    self.GetPageRecords = function (currentPageNo) {

        currentPage = currentPageNo;

        var objTagInfo = new Object();
        objTagInfo.CurrentPage = currentPage;


        self.getTagList(objTagInfo);

    }


    self.GetPaging = function (Rowcount, currentPage, methodName) {
        return ErucaCRM.Framework.Core.GetPagger(Rowcount, currentPage, methodName);
    }




}

