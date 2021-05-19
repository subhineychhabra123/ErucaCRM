jQuery.namespace('ErucaCRM.User.Stages');
var viewModel;

ErucaCRM.User.Stages.pageLoad = function () {

    viewModel = new ErucaCRM.User.Stages.pageViewModel();
    ko.applyBindings(viewModel, document.getElementById("InnerContentContainer"));
}

ErucaCRM.User.Stages.CompanyStageListQueryModel = function (data) {

    var self = this;
    self.StageName = data.StageName;
    self.StageId = data.StageId;
    self.StageOrder = data.StageOrder;
    self.Rating = data.Rating.RatingConstant == 0 ? '' : data.Rating.RatingConstant;
    self.ShowIcon = data.Rating.Icons == null ? false : true;
    self.Icons = data.Rating.Icons != null ? "/Content/images/" + data.Rating.Icons + ".png" : "";
    self.IsInitialStage = data.IsInitialStage == null ? false : data.IsInitialStage;
    self.IsLastStage = data.IsLastStage == null ? false : data.IsLastStage;
};

ErucaCRM.User.Stages.pageViewModel = function () {
    var self = this;
    var controllerUrl = "/User/";
    var Message = {
        Failure: 'Failure',
        Success: 'Success'
    }

    ErucaCRM.Framework.Core.ApplyCultureValidations();
    self.CompanyStageListQueryModel = ko.observableArray();
    self.CompanyStageList = ko.observableArray([]);
    self.StageHierarchy = ko.observableArray([]);
    $('._ratings_stars').hover(
function () {
    $(this).prevAll().andSelf().addClass('ratings_over');
    $(this).nextAll().removeClass('ratings_vote');
},

function () {
    $(this).prevAll().andSelf().removeClass('ratings_over');
    Set_Rating($(this).parent(), $(this).parent().attr('avg'));
}
);
    function Set_Rating(widget, RatingCostant) {
        var avg = RatingCostant;
        $(widget).find('._star-' + avg).prevAll().andSelf().addClass('ratings_vote');
        $(widget).find('.star-' + avg).nextAll().removeClass('ratings_vote');
        if (RatingCostant == 0) {
            $(widget).find('._ratings_stars').removeClass('ratings_vote');
        }
        $(widget).attr("avg", avg);
    }
    $('._ratings_stars').bind('click', function () {
        var star = this;
        var widget = $(this).parent();
        var RatingId = $(star).data('rating');
        var RatingCostant = $(star).data('ratingconstant');
        $('._save-stage').attr('RatingId', RatingId);
        Set_Rating("._rate_widget", RatingCostant);
    });
    $('#AddStagebutton').click(function () {
        $('#txtnewstage').val('');
        $('#txtstagesort').val('');
        //$("#hdnStageId").val("")
        $('#LeadStageDuration').val("");
        $('#Rating').val("");
        $('._save-stage').removeAttr("stageid");
        $('._save-stage').removeAttr("ratingid");
        $('#Ratingdiv').css("display", "block");
        $('#StageDurationDiv').css('display', 'block');
        $('._stageHeader').text(ErucaCRM.Messages.Stage.StageAddHeaderText);
        ErucaCRM.Framework.Common.OpenPopup('#addnewstage');
        Set_Rating("._rate_widget", 0);

    });


    self.getStageList = function () {
        ErucaCRM.Framework.Core.getJSONData(
            controllerUrl + "StagesList",
            function onSuccess(response) {
                $('._save-stage').removeAttr('disabled')
                self.StageHierarchy = ko.observableArray([]);
                ko.utils.arrayForEach(response, function (list) {
                    self.CompanyStageList.push(new ErucaCRM.User.Stages.CompanyStageListQueryModel(list));
                    self.StageHierarchy.push(new ErucaCRM.User.Stages.CompanyStageListQueryModel(list));
                });
                ErucaCRM.Framework.Common.ApplyPermission('._roleselector');

            }, function onError(err) {
                self.status(err.Message);
            });

    }


    $('._save-stage').click(function () {
        $(this).attr('disabled', 'disabled')
        var StageName = $.trim($('#txtnewstage').val());
        var StageId = $(this).attr("stageid");
        var StageLeadduration = $('#LeadStageDuration').val();
        if (StageId == "" || StageId == "0" || StageId == undefined)
            var StageOrder = self.NewStageOrder();
        else
            var StageOrder = $(this).attr("stageorder");
        var islaststage = $(this).attr("islaststage");
        var isInitialstage = $(this).attr("isinitialstage");
      
        if (islaststage != "true" && isInitialstage != "true") {
            var StageRating = $(this).attr("ratingid");
            if (StageName == "") {
                $(this).removeAttr('disabled');
                alert(ErucaCRM.Messages.Stage.StageFieldRequired);
                return;
            } else if (StageRating == undefined || StageRating == "") {
                $(this).removeAttr('disabled');
                alert(ErucaCRM.Messages.Stage.StageRatingRequired);
                return;
            } else if (StageLeadduration == undefined || StageLeadduration == "") {
                $(this).removeAttr('disabled');
                alert(ErucaCRM.Messages.Stage.StageLeaddurationRequired);
                return;
            }
        }
        else {
            if (StageName == "") {
                $(this).removeAttr('disabled');
                alert(ErucaCRM.Messages.Stage.StageFieldRequired);
                return;
            }
        }
        if ($('#LeadStageDuration').val() != "") {
            var value = $('#LeadStageDuration').val().replace(/^\s\s*/, '').replace(/\s\s*$/, '');
            var intRegex = /^\d+$/;
            if (!intRegex.test(value)) {
                alert(ErucaCRM.Messages.Stage.StageLeadDurationNumericValidation);
                $(this).removeAttr('disabled');
                return;
            } else if (StageLeadduration > 365) {
                alert(ErucaCRM.Messages.Stage.StageLeadDurationValidation);
                $(this).removeAttr('disabled');
                return;
            }
        }
        var StageInfo = new Object();
        StageInfo.StageName = StageName;
        StageInfo.StageOrder = StageOrder.toString();
        StageInfo.DefaultRatingId_encrypted = StageRating;
        StageInfo.StageId_encrypted = StageId;
        StageInfo.StageLeadduration = StageLeadduration;
        self.SaveNewStage(StageInfo);
    });


    self.NewStageOrder = function () {
        var count = 0;
        $('#configstages tr').each(function () {
            count++;
        })
        return count - 1;
    }
    self.SaveNewStage = function (StageInfo) {
        ErucaCRM.Framework.Core.doPostOperation
            (
                controllerUrl + "CreateNewStage",
                StageInfo,
                function onSuccess(response) {
                    if (response.Status == "Success") {
                        $('#addnewstage').fadeOut("slow");
                        Set_Rating("._rate_widget", 0);
                        $('#CompanyStages').children().remove();
                        $('._save-stage').attr('RatingId', "");
                        $('._save-stage').attr('islaststage', "false");
                        $('._save-stage').attr('isinitialstage', "false");
                        self.getStageList();
                    } else if (response.Status == "NameExist") {
                        $('._save-stage').removeAttr('disabled');
                        alert(ErucaCRM.Messages.Stage.StageNameExist);
                        return;
                    }

                });
    }


    self.UpdateStageOrder = function (SortArray) {
        ErucaCRM.Framework.Core.doPostOperation
            (
                "/User/" + "UpdateStageOrder",
                SortArray,
                function onSuccess(response) {
                }
                );
    }

    self.DeleteStage = function (data) {
        if (confirm(ErucaCRM.Messages.Stage.ConfirmStageDeleteAction)) {
            var StageId = ko.utils.unwrapObservable(data.StageId);
            if (true) {
                self.BindStageListDropDownForDeleteOperation(StageId);
                ErucaCRM.Framework.Common.OpenPopup('div#moveleadspop');
            }
        }
    }

    self.bindAllChildreen = function (array, obj) {
        var allChild_objects = array.filter(function (v) {
            var returnVal = false;
            if (obj == v.StageId && v.StageId != null) {
                returnVal = true;
            }
            return returnVal;
        });
    }
    self.BindStageListDropDownForDeleteOperation = function (stageId) {
        $('._stageListdropdown').empty();
        $('._stageListdropdown').append("<option value='0'>" + ErucaCRM.Messages.DropDowns.SelectOption + "</option>");
        $("._submitbtn").attr("stageId", stageId);
        $.each(self.StageHierarchy(), function (index, element) {
            if (element.IsLastStage != true && element.StageId != stageId) {
                $('._stageListdropdown').append("<option value='" + element.StageId + "'>" + element.StageName + "</option>");
            }
        });
    }

    self.GetStageDetail = function (stage) {
        var StageId_encrypted = ko.utils.unwrapObservable(stage.StageId);
        var stageInfo = new Object();
        stageInfo.StageId_encrypted = StageId_encrypted;
        ErucaCRM.Framework.Core.doPostOperation
            (
                "/User/" + "GetStageDetail",
                stageInfo,
                function onSuccess(response) {
                    $('#txtnewstage').val(response.StageName);
                    $('#LeadStageDuration').val(response.StageLeadDuration == 0 ? '' : response.StageLeadDuration);

                    if (response.IsInitialStage || response.IsLastStage) {
                        $('#StageDurationDiv').css('display', 'block');
                        $('#Ratingdiv').css("display", "none");
                        if (response.IsLastStage)
                            $('#StageDurationDiv').css('display', 'none');
                    }
                    else {
                        $('#Ratingdiv').css("display", "block");
                        $('#StageDurationDiv').css('display', 'block');
                    }
                    $('._save-stage').attr("stageorder", response.StageOrder);
                    $('._save-stage').attr('RatingId', response.DefaultRatingId);
                    $('._save-stage').attr('IsInitialStage', response.IsInitialStage);
                    $('._save-stage').attr('IsLastStage', response.IsLastStage);
                    $('._save-stage').attr('StageId', response.StageId);
                    Set_Rating("._rate_widget", response.Rating.RatingConstant);
                    $('._stageHeader').text(ErucaCRM.Messages.Stage.StageEditHeaderText);
                    ErucaCRM.Framework.Common.OpenPopup('#addnewstage');
                });
    }
    $('._close').click(function () {
        $('._save-stage').removeAttr("stageid");
        $('._save-stage').removeAttr("ratingid");
        Set_Rating("._rate_widget", 0);
    });
    self.getStageList();

    $('._submitbtn').bind('click', function () {
        var newstageid = $('._stageListdropdown').val();
        var oldstageid = $(this).attr("stageid");
        if (newstageid == "0" || newstageid == "" || newstageid == undefined) {
            alert(ErucaCRM.Messages.Stage.StageFieldRequired);
            return;
        }
        moveallstages = new Object();
        moveallstages.OldStageId_encrypted = oldstageid;
        moveallstages.NewStageId_encrypted = newstageid;
        ErucaCRM.Framework.Core.doPostOperation
         (
             "/User/" + "DeleteStageAfterMoveLeads",
             moveallstages,
             function onSuccess(response) {
                 if (response.Status == "Success") {
                     $('#moveleadspop').fadeOut("slow");
                     $('#CompanyStages').children().remove();
                     //self.RemoveStagebyId(oldstageid);
                     //self.RemoveStageFromList(oldstageid);
                 }
                 self.getStageList();
             });
    });


    $("#CompanyStages").sortable({
        items: 'tr._Stage,tr._Stage',
        stop: function (event, ui) {
            var SortArray = [];
            var stageorder = new Object();
            $("#CompanyStages tr._Stage").each(function (i, el) {
                var IsInitialStage = $(el).attr('isinitialstage');
                var IsLastStage = $(el).attr('islaststage');
                if (IsInitialStage != "true" && IsLastStage != "true") {
                    stageorder.StageId_encrypted = $(el).attr('id');
                    stageorder.StageIndex = $(el).index();
                    SortArray.push(stageorder);
                }
                stageorder = new Object();
            });
            self.UpdateStageOrder(SortArray);
        }
    });

    self.RemoveStageFromList = function (stageId) {
        var removestagelist = ko.utils.arrayFilter(self.StageHierarchy(), function (item) {
            if (item != undefined && item != null) {
                if (item.StageId == stageId) {
                    self.StageHierarchy.remove(item);
                    isRemoved = true;
                }
            }
        });
    }

    self.RemoveStagebyId = function (stageId) {
        isRemoved = false;
        var removestagelist = ko.utils.arrayFilter(self.CompanyStageList(), function (item) {
            if (item != undefined && item != null) {
                if (item.StageId == stageId) {
                    self.CompanyStageList.remove(item);
                    isRemoved = true;
                    return false;
                }
            }
        });
    }

    //Set_Rating(".rate_widget", 0);
    $('._Stagedragdropnotification').text(ErucaCRM.Messages.Stage.StageDragNotification);
    $("#CompanyStages").disableSelection();
}