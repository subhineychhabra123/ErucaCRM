jQuery.namespace('ErucaCRM.User.Leads');
var ContentpageUrl = "/User/Leads"
var IsLeadsLastStage = false;
var IsLeadWinClose = false;
var tagcnt = 0;
var TagList = new Array();
var SearchTagName = "";
var CurrentTagObject = new Object();
var FinalRatingConstant = 5;
var ContactCurrentPage = 1;
var viewModel;
var CurrentPage1 = 1;
var filterBy = "Active";
var Visibility = {
    visible: 'visible',
    hidden: 'hidden'
}
var ContentpageUrl = "/User/Leads"
var CurrentLeadTaskItemsPage = 1,
    LeadTaskItemsMethodName = "viewModel.GetTaskItemList";
var CurrentLeadProductsPage = 1,
    LeadProductsMethodName = "viewModel.GetLeadProductsList";
var CurrentAllProductsPage = 1,
    AllProductsMethodName = "viewModel.GetAllProductsList";
var CurrentLeadContactPage = 1,
    LeadContactsMethodName = "viewModel.GetLeadContactsList";
var CurrentLeadDocumentPage = 1,
    LeadDocumentsMethodName = "viewModel.GetLeadDocumentsList";
var CurrentLeadCommentsPage = 1,
    LeadCommentsMethodName = "viewModel.GetLeadCommentsList";
ErucaCRM.User.Leads.pageLoad = function () {
    //$.connection.hub.logging = true;
    //chat = $.connection.realTimeNotificationHub;
    viewModel = new ErucaCRM.User.Leads.pageViewModel();
    ko.applyBindings(viewModel, document.getElementById("InnerContentContainer"));
}

ErucaCRM.User.Leads.DisplayFormSubmitQueryModel = function (isVisible) {
    var self = this;
    self.Hidden = ko.observable(isVisible);
    return self;
}
ErucaCRM.User.Leads.LeadDocumentsQueryModel = function (data) {
    var self = this;
    self.DocumentId = data.DocumentId;
    self.DocumentName = data.DocumentName;
    self.DocumentPath = data.LeadFilePath;
    self.AttachedBy = data.AttachedBy;
    self.ShowDocRemoveLink = ko.observable('none');
    return self;
}
ErucaCRM.User.Leads.LeadRatingQueryModel = function (data) {
    var self1 = this;
    self1.RatingId = data.RatingId;
    self1.RatingConstant = data.RatingConstant;
    var ratingClass = data.IsRatingViewOnly == true ? "ratings_stars-readonly " : "ratings_stars "
    self1.RatingClass = ratingClass + " star_" + data.RatingConstant;
    return self1;
}
ErucaCRM.User.Leads.LeadDetailQueryModel = function (data) {
    var self = this;
    self.AccountId = data.AccountId;
    self.LeadId = data.LeadId;
    self.RatingId = data.RatingId;
    self.RatingConstant = data.Rating.RatingConstant;
    self.IsClosedWin = data.IsClosedWin;
    self.IsLastStage = data.Stage.IsLastStage;
    self.FinalStageId = data.FinalStageId;
    self.StageId = data.StageId;
    self.Title = data.Title;
    self.LeadCompanyName = data.LeadCompanyName;
    self.IndustryId = data.IndustryId;
    self.LeadSourceId = data.LeadSourceId;
    self.LeadOwnerId = data.LeadOwnerId;
    return self;
}

ErucaCRM.User.Leads.TagListQueryModel = function (data) {

    var self = this;
    self.TagId = data.TagId;
    self.TagName = data.TagName;

}

ErucaCRM.User.Leads.LeadContactsQueryModel = function (data) {

    var self = this;
    self.ContactId = data.ContactId;
    self.ContactName = data.FirstName
    if (data.LastName != null)
        self.ContactName = self.ContactName + " " + data.LastName;
    self.ContactPhone = "";
    if (data.Phone != null)
        self.ContactPhone = data.Phone;//{new @userId=" + data.UserId + "}";
    self.Company = data.ContactCompanyName;
    self.ContactEmail = data.EmailAddress;
    self.UrlEdit = "EditContact/" + data.ContactId + "?returnurl=" + window.location.href;
    self.UrlViewContact = "ContactView/" + data.ContactId;
    self.LeadId = data.LeadId;
    self.Mobile = "";
    if (data.Mobile != null) {
        self.Mobile = data.Mobile;
    }
    self.JobPosition = "";
    if (data.JobPosition != null) {
        self.JobPosition = data.JobPosition;
    }

}

ErucaCRM.User.Leads.LeadCommentsQueryModel = function (data) {
    var self = this;
    self.LeadCommentId = data.LeadCommentId;
    self.Comment = data.Comment;
    //   self.CreatedDate = dateFormat(Date(data.LeadCommentCreatedTime), "CisoUtcDateTime");
    self.CreatedDate = data.LeadCommentCreatedTime == null ? null : data.LeadCommentCreatedTime;
    self.UserName = data.UserName;
    self.UserDetail = "/User/UserProfile/" + data.UserId;
 //   self.UserImg = "/Uploads/users/" + data.UserImg;
    self.UserImg = data.UserImg;
    self.fileExists = data.AudioFileName == null || data.AudioFileName == "" ? false : true;
    self.filePath = data.AudioPathName;
}

ErucaCRM.User.Leads.LeadProductsQueryModel = function (data) {

    var self = this;
    self.RowId = "row_" + data.ProductId;
    self.ProductId = data.ProductId;
    self.ProductName = data.ProductName;
    self.ProductCode = data.ProductCode;
    self.ShowLink = ko.observable('none');
    return self;
}
ErucaCRM.User.Leads.LeadTaskItemQueryModel = function (data) {
    var self = this;
    self.TaskId = data.TaskId;
    self.Subject = data.Subject;
    self.DueDate = data.TaskDueDate;
    self.TaskStatus = data.TaskStatus;
    self.Priority = data.PriorityName;
    var LeadID = viewModel.GetSelectedLeadId();

    self.EditUrl = "/User/TaskItem?taskID_encrypted=" + data.TaskId + "&mod=Lead&val_encrypted=" + LeadID + "&returnurl=" + window.location.href;;
    self.DetailUrl = "/User/ViewTaskItemDetail?mod=Lead&taskID_encrypted=" + data.TaskId;
}
ErucaCRM.User.Leads.LeadHistoryQueryModel = function (data) {
    var self = this;
    self.StageName = data.StageName;
    self.Proability = data.Proability;
    self.StageId = data.StageId;
    // self.Amount = data.Amount;
    self.Amount = data.CultureSpcificAmount;
    self.ExpectedRevenue = data.ExpectedRevenue;
    self.ClosingDate = data.LeadClosingDate;
    self.Duration = data.Duration;
    self.ActivityType = data.HistoryActivityType;
    self.FromDate = data.LeadStageFromDate;
}

ErucaCRM.User.Leads.LeadHistoryChartDetailsQueryModel = function (data) {
    var self = this;
    self.StageId = data.StageId;
    self.StageName = data.StageName;
    self.Duration = data.Duration;
    self.NumberOfMinutes = data.NumberOfMinutes;
    self.TotalNumberOfMinutes = data.TotalNumberOfMinutes;
}

ErucaCRM.User.Leads.OnloadRenderTagsQueryModel = function (data) {
    var self = this;
    self.TagId = data.TagId;
    self.TagName = data.TagName;
    self.TagLink = data.TagId != 0 ? "/User/TagDetail/" + data.TagId : "";
    var EditPermission = ErucaCRM.Framework.Common.CookieExists("LeadEdit");
    if (EditPermission == false) {
        var ViewPermission = ErucaCRM.Framework.Common.CookieExists("LeadView");
        if (ViewPermission == false)
            return;
        else {
            self.IsEditable = false;
        }
    } else {
        self.IsEditable = true;
    }

}

ErucaCRM.User.Leads.LeadListDetailQueryModel = function (data) {
    var self = this;
    self.LeadId = data.LeadId;
    self.LeadName = data.Name;
    self.Title = data.Title;
    self.IsClosedWin = data.IsClosedWin;
    self.StageId = data.StageId;
    self.LeadOwnerName = data.LeadOwnerName
    self.LeadOwnerImage = "/Uploads/users/" + data.LeadOwnerImage;
    self.DetailUrl = "#" + data.LeadId;
    self.Icons = data.Rating.Icons != null ? "/Content/images/" + data.Rating.Icons + ".png" : "";
    self.ViewLeadDetail_Click = function (data) {
        $("#LeadHistoryData").children().remove();
        $("#TaskItemListData").children().remove();
        $("#AccountCaseAttachments").children().remove();
        $("#leadProduct").children().remove();
        $("#ContactListData").children().remove();
        $("#CommentsListData").children().remove();
        $("#allProduct").children().remove();
        $("#allContacts").children().remove();
        $("#Filterpopup1").hide();
        viewModel.LeadDetailClickHandler(data);
    }
    return self;
}
// A view model that represent a Test report query model.
ErucaCRM.User.Leads.LeadListQueryModel = function (data) {
    var self = this;
    var pagesize = ErucaCRM.Framework.Core.Config.PageSize;
    self.StageName = data.StageName;
    self.LeadContainerId = data.StageName.split(' ').join('');
    self.StageId = data.StageId;
    self.loaderId = data.StageId;
    self.IsLastStage = data.IsLastStage;
    self.IsInitialStage = data.IsInitialStage;
    self.Leads = ko.observableArray([]);
    $.each(data.Leads, function (index, val) {
        val.StageId = data.StageId;
        self.Leads.push(new ErucaCRM.User.Leads.LeadListDetailQueryModel(val));
    })
};
//Page view model


ErucaCRM.User.Leads.pageViewModel = function (currentPage, filterBy) {
    //Class variables

    var CompanyName = "Add company name..";
    var LeadName = ErucaCRM.Messages.Lead.AddLeadWatermark;
    var self = this;
    var objLeadInfo = new Object();
    objLeadInfo.CurrentPage = CurrentPage1;
    objLeadInfo.FilterParameters = new Array();
    var PagingMethodName = "GetPageData";
    var controllerUrl = "/User/";
    var Message = {
        Failure: 'Failure',
        Success: 'Success'
    }
    self.Core = ErucaCRM.Framework.Core;
    self.DisplaySubmitButton = ko.observableArray([]);
    self.LeadCompanyName = ko.observable();
    self.LeadAmount = ko.observable();
    self.LeadOwner = ko.observable();
    self.HasContact = ko.observable();
    self.ContactName = ko.observable();
    self.ContactEmail = ko.observable();
    this.ContactList = ko.observableArray([]);
    this.LeadContactList = ko.observableArray([]);
    //self.ContactListQueryModel = ko.observableArray();
    self.ShowContactsDropDown = ko.observable(false);
    self.LeadListQueryModel = ko.observableArray();
    self.queryModel = ko.observable();
    self.status = ko.observable();
    self.LeadList = ko.observableArray([]);
    self.LeadHistoryList = ko.observableArray([]);
    self.OnTagList = ko.observableArray([]);
    self.LeadHistoryChartDetailList = ko.observableArray([]);
    self.TaskItemList = ko.observableArray([]);
    self.AllProductsList = ko.observableArray([]);
    self.LeadProductsList = ko.observableArray([]);
    self.LeadDocumentsList = ko.observableArray([]);
    self.LeadContactsList = ko.observableArray([]);
    self.LeadCommentsList = ko.observableArray([]);
    self.LeadRatingList = ko.observableArray([]);
    self.TagList = ko.observableArray([]);
    self.AddLeadContactUrl = ko.observable();
    self.AddLeadActivityUrl = ko.observable();
    //===================Lead Filter Info Variables===============
    self.ShowLeadFilterInfo = ko.observable(false);
    self.ShowAddLeadOnFilter = ko.observable(true);
    self.FilterByLeadTitle = ko.observable(false);
    self.FilterLeadTitle = ko.observable('');
    self.FilterByLeadTag = ko.observable(false);
    self.FilterLeadTag = ko.observable('');
    self.FilterByTitleAndTag = ko.observable(false);

    self.TitleSearchTag = ko.observable(ErucaCRM.Messages.Lead.MostUsedTagsToolTip);

    //==================End Lead Filter=========================
    self.ShowNoLeadHistoryFoundMsg = ko.observable(Visibility.hidden);
    self.ShowNoLeadActivityFoundMsg = ko.observable(Visibility.hidden);
    self.ShowNoLeadDocFoundMsg = ko.observable(Visibility.hidden);
    self.ShowNoLeadProductFoundMsg = ko.observable(Visibility.hidden);
    self.ShowNoLeadContactFoundMsg = ko.observable(Visibility.hidden);
    self.ShowNoLeadCommentFoundMsg = ko.observable(Visibility.hidden);
    var NoLeadHistoryRecordFound = ErucaCRM.Messages.Lead.NoLeadHistoryRecordFound;
    self.NoLeadHistoryFoundMsg = ko.observable(NoLeadHistoryRecordFound == undefined ? "No Record Found" : NoLeadHistoryRecordFound);
    self.NoLeadActivityFoundMsg = ko.observable();
    var NoLeadDocumentRecordFound = ErucaCRM.Messages.Lead.NoLeadDocumentRecordFound;
    var NoLeadProductRecordFound = ErucaCRM.Messages.Lead.NoLeadProductRecordFound;
    var NoLeadContactRecordFound = ErucaCRM.Messages.Lead.NoLeadContactRecordFound;
    self.NoLeadDocFoundMsg = ko.observable(NoLeadDocumentRecordFound == undefined ? "No Record Found" : NoLeadDocumentRecordFound);
    self.NoLeadProductFoundMsg = ko.observable(NoLeadProductRecordFound == undefined ? "No Record Found" : NoLeadProductRecordFound);
    self.NoLeadContactFoundMsg = ko.observable(NoLeadContactRecordFound == undefined ? "No Record Found" : NoLeadContactRecordFound);

    self.CurrentLeadDetailObject = null;
    self.AddNewLeadInArray = function (val) {
        var LeadsArrary = ko.utils.arrayFilter(self.LeadList(), function (item) {
            if (item.StageId == val.StageId) {
                item.Leads.unshift(new ErucaCRM.User.Leads.LeadListDetailQueryModel(val));
                return item;
            }

        });
        self.MakeElementDraggable();

    }

    self.LeadDetailClickHandler = function (data) {


        location.href = data.DetailUrl;
        var LeadArray = self.GetLeadObjectByStageId(data.StageId);
        IsLeadsLastStage = LeadArray[0].IsLastStage;
        IsLeadWinClose = data.IsClosedWin;
        self.Initialize();

    }
    self.LeadDetail = function (e) {

        var pageurl = $(e).attr('href');
        var v = pageurl.split("#");
        if (pageurl != window.location) {

            self.ViewLeadDetail(v[1]);
            window.history.pushState({ path: pageurl }, '', pageurl);
        }
        return false;
    }

    self.DeleteLead = function (leadId, StageId) {
        if (confirm(ErucaCRM.Messages.Lead.ConfirmLeadDeleteAction)) {
            var data = new Object();
            data.LeadId = leadId;
            ErucaCRM.Framework.Core.doPostOperation(controllerUrl + "DeleteLead", data, function onSuccess(response) {

                self.RemoveLeadById(leadId, StageId);
            });
        }
    }




    self.AddLeadDetail = function (accountId) {
        var PostData = new Object();
        ErucaCRM.Framework.Core.getJSONDataBySearchParam
           (
               "/User/" + "CreateLead",
               PostData,
                 function onSuccess(response) {
                     ErucaCRM.Framework.Common.OpenPopup('div._leadDetail');
                     // $("._AddProductSection").remove();
                     // $("._addNewProductSection").remove();
                     $("._FileUploadSection").remove();
                     $('#LeadDetail').html(response);
                     $('#LeadDetailEditingMode').hide();
                     $('#LeadDetail').show();
                     $.validator.unobtrusive.parse('._leadDetail');
                     if (accountId != undefined) {
                         $("#AccountId").val(accountId);
                     }
                 });
        ErucaCRM.Framework.Core.ApplyCultureValidations();

    }

    self.RenderLeadDetail = function (data) {
        $('.leadpopup input[type="text"] ,.leadpopup select ,.leadpopup textarea').prop("disabled", false);// enable all lead detail controls
        self.DisplaySubmitButton.removeAll();
        self.DisplaySubmitButton.push(new ErucaCRM.User.Leads.DisplayFormSubmitQueryModel(data.Stage.IsLastStage));
        $("#AccountId").val(data.AccountId);
        $("#LeadId").val(data.LeadId);
        $("#RatingId").val(data.RatingId);
        $("#hdnIsClosedWin").val(data.IsClosedWin);
        $("#LeadRatingConstant").val(data.Rating.RatingConstant)
        $("#Title").val(data.Title);
        $("#LeadCompanyName").val(data.LeadCompanyName);
        $("#hdnIsLastStage").val(data.Stage.IsLastStage);
        $("#hdnIsInitialStage").val(data.Stage.IsInitialStage);
        $("#FinalStageId").val(data.FinalStageId);
        $("#StageId").val(data.StageId);
        var IndustryId = data.IndustryId;
        IndustryId = (IndustryId == null || IndustryId == "") ? 0 : IndustryId;
        $("#IndustryId").val(IndustryId);
        var LeadSourceId = data.LeadSourceId;
        LeadSourceId = (LeadSourceId == null || LeadSourceId == "") ? 0 : LeadSourceId;
        $("#LeadSourceId").val(LeadSourceId);
        var LeadOwnerId = data.LeadOwnerId;
        LeadOwnerId = (LeadOwnerId == null || LeadOwnerId == "") ? 0 : LeadOwnerId;
        $("#LeadOwnerId").val(LeadOwnerId);
        var LeadStatusId = data.LeadStatusId;
        LeadStatusId = (LeadStatusId == null || LeadStatusId == "") ? 0 : LeadStatusId;
        $("#LeadStatusId").val(LeadStatusId)
        var Description = data.Description;
        $("#Description").val(Description);
        $("#Amount").val(data.Amount);
        var isLeadInLastStage = data.Stage.IsLastStage == null ? true : data.Stage.IsLastStage;
        data.FileName = data.FileName == null ? "" : data.FileName;
        if (data.FileName != "") {

            $('audio[id=resource_audio]').children('source').attr("src", data.AudioPath);
            $('div[id=_audio]').children('a').attr("href", data.AudioPath);
            $('._audio').show();
            $('._downloadaudio').show();
        }
        else {
            $('._audio').hide();
            $('._downloadaudio').hide();

        }

        //====================Expected Revenue Section===================================
        $("#ExpectedRevenue").text('');
        $("#ExpectedRevenueContainer").css("display", "none")
        if (!isLeadInLastStage) {
            $("#ExpectedRevenueContainer").css("display", "block")
            $("#ExpectedRevenue").text(data.ExpectedLeadRevenue);
        }
        //====================End Expected Revenue Section===================================

        //====================Rating Section===================================

        self.LeadRatingList.removeAll();
        $("#RatingContainer").css("display", "none")
        if (!data.Stage.IsInitialStage) {
            $("#RatingContainer").css("display", "block")
            ko.utils.arrayForEach(data.RatingList, function (rating) {
                rating.IsRatingViewOnly = data.IsRatingViewOnly;
                self.LeadRatingList.push(new ErucaCRM.User.Leads.LeadRatingQueryModel(rating));

            });
            self.InitialiseRatings();

        }
        //====================End Rating Section===================================
    }

    self.ViewLeadDetail = function (v) {
        //remove error message when we see other stage
        $("#LeadDetailSection").find(".field-validation-error").empty();
        $("#Commenttxt").val("");
        var ratingViewOnly = false;
        var methodName = "";

        var EditPermission = false;
        EditPermission = ErucaCRM.Framework.Common.CookieExists("LeadE");
        if (EditPermission == false) {
            var ViewPermission = ErucaCRM.Framework.Common.CookieExists("LeadVe");
            if (ViewPermission == false)
                return;
            else {
                methodName = "GetLeadDetail";
                ratingViewOnly = true;

            }
        } else {
            methodName = "GetLeadDetail";
        }
        //if (IsLeadsLastStage) {
        //    methodName = "_ViewLeadDetail";
        //}

        var PostData = new Object();
        PostData.Id_encrypted = v;
        ErucaCRM.Framework.Core.doPostOperation
             (
                 "/User/" + methodName,
                 PostData,
                 function onSuccess(response) {
                     if (response.StatusCode == 401) {
                         location.href = "/Login";
                     }
                     if (response.lead.RecordDeleted == true) {
                         $("._resetUrl").click();

                         ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Lead.LeadDeleted, true);
                     }
                     else {
                         ErucaCRM.Framework.Common.OpenPopup('div._leadDetail');


                         self.SetSelectedTab(0);
                         //$("._AddProductSection").remove();
                         // $("._addNewProductSection").remove();
                         $("._FileUploadSection").remove();
                         if (response.lead != undefined) {
                             response.lead.IsRatingViewOnly = ratingViewOnly;
                             self.RenderLeadDetail(response.lead);
                             if (!EditPermission) {
                                 $('.leadpopup input[type="text"] ,.leadpopup select ,.leadpopup textarea').prop("disabled", true);

                                 // disable all lead detail controls
                             }
                         }
                         //$('#LeadDetail').html("");
                         //$('#LeadDetail').html(response);
                         //if (IsLeadsLastStage) 
                         //    $('._commonButton').hide();
                         //else
                         //    $('._commonButton').show();
                         self.RenderLeadTagsOnLoad(response.lead.LeadTags);
                         $("#LeadDetailEditingMode").css("display", "none");
                         $("#LeadDetail").css("display", "block");

                         if (IsLeadsLastStage) {
                             $('._commonButton').css("display", "none");
                         }
                         else
                             $('._commonButton').css("display", "block");
                         $.validator.unobtrusive.parse('#LeadDetail');
                         // ko.applyBindings(viewModel, document.getElementById('sectionRating'));
                     }

                 },
                 function onError(err) {
                     self.status(err.Message);
                 }
             );
    }
    self.Initialize = function () {

        var v = location.hash;

        if (v.length > 0) {

            var param = v.split('#')[1];
            var createLeadParam = param.split("&");

            if (createLeadParam.length == 2) {

                if (createLeadParam[0] == "CreateLead") {
                }
            }
            if (param == "CreateLead") {

            }
            if (param == "RecentActivity") {

            }
            else {

                self.ViewLeadDetail(param);
            }
        }

    }
    $('._resetUrl').click(function () {
        $('div._leadDetail').fadeOut("fast");
        window.history.pushState({ path: ContentpageUrl }, '', ContentpageUrl);
    });

    $(window).resize(function () {

        SetLeadListLayout();
        $('.leadsouter').perfectScrollbar('update');
        $("._leadsContainer").each(function () {
            $(this).perfectScrollbar('update');
        });

    });
    $(window).mousewheel(function () {

        SetLeadListLayout();
        $('.leadsouter').perfectScrollbar('update');
        $("._leadsContainer").each(function () {
            $(this).perfectScrollbar('update');
        });

    });
    self.MakeElementDraggable = function () {

        $('._draggable').draggable({
            containment: "#leadslayout",
            //revert: true,
            helper: 'clone',
            appendTo: "body",
            start: function (event, ui) {

                ui.helper.addClass("lead").css("width", "220px");
            },
            drag: function (e, ui) {
                var xPos = ui.position.left;
                var yPos = ui.position.top;
                $('.leadsouter').scrollLeft(xPos - 100);
                $('.leadsouter').perfectScrollbar('update');
            },

            tolerance: 'fit'
        });
    }
    self.AddNewLead = function (e) {

        if ($('._addnewlead').val() != LeadName && $('._addnewlead').val() != "") {
            var NewLead = new Object();
            $(e).addClass('disabled');
            NewLead.Title = $('._addnewlead').val();
            self.CreateNewLead(NewLead);
        }
        //chat.server.sendNotification();
    }
    self.CreateNewLead = function (NewLeadObject) {

        ErucaCRM.Framework.Core.doPostOperation
              (
        controllerUrl + "CreateLead",
                 NewLeadObject,
                 function onSuccess(response) {
                     if (response.Status == "Success") {
                         self.LeadListDetailQueryModel = ko.observableArray([]);
                         var val = new Object();
                         val.Title = response.NewLead.Title;
                         val.LeadId = response.NewLead.LeadId;
                         val.StageId = response.NewLead.StageId;
                         val.Rating = response.NewLead.Rating;
                         val.LeadOwnerImage = response.NewLead.LeadOwnerImage;
                         val.LeadOwnerName = response.NewLead.LeadOwnerName;
                         val.IsClosedWin = false;
                         self.AddNewLeadInArray(val);

                         //self.MakeElementDraggable();
                     }
                     $('._companyName').val(CompanyName);
                     $('._companyName').toggle();
                     $('._close-Popup').css("display", "none");

                     //$("._lead").click(function () {
                     //    self.LeadDetail($(this));
                     //});
                     $('._btnAddLead').removeClass('disabled');
                     $('._btnAddLead').css("display", "none");
                     var returnurl = "";
                     var sPageURL = window.location.search.substring(1);
                     var sURLVariables = sPageURL.split('&');
                     for (var i = 0; i < sURLVariables.length; i++) {
                         //var sParameterName = sURLVariables[i].split('=');
                         if (sURLVariables.length > 1) {
                             if (i == sURLVariables.length - 1)
                                 returnurl = sURLVariables[i].replace("returnurl=", "");
                         } else {
                             if (i == 0)
                                 returnurl = sURLVariables[i].replace("returnurl=", "");

                         }
                     }

                     if (returnurl != "" && returnurl != undefined) {
                         window.location.href = returnurl;
                     }
                 }
        );
    };


    //to change the browser URL to 'pageurl'
    //self.ViewLeadDetail();
    function SetLeadListLayout() {

        var totalStages = $("._stage").length;
        var stageWidth = (($(window).width() - 100) / 4);
        if (stageWidth < 210) {
            stageWidth = 210;
        }
        $("._stage").width(stageWidth - 26);
        $("#MainStageDiv").width(stageWidth * totalStages);
        var ofset = $("._stage:first").offset();

        var hight = $(window).height() - ((ofset != undefined) ? ofset.top : 0);
        $("#leadslayout").height(hight);

        ofset = $("._leadsContainer:first").offset();
        var LeadContainerheight = $(window).height() - (ofset != undefined ? ofset.top : 0);
        $("._leadsContainer:first").height(LeadContainerheight - 25);

        $("._leadsContainer:not(:first)").height(LeadContainerheight);

    }


    self.RenderLeadTagsOnLoad = function (TagList) {
        $('.tagmain').html("");
        ko.utils.arrayFilter(TagList, function (item) {
            self.OnTagList.push(new ErucaCRM.User.Leads.OnloadRenderTagsQueryModel(item.TagVM));
        });

    }


    self.getLeadList = function (objLeadInfo) {
        $('._leadDetail').hide();
        $('#LeadListData').html("");
        ErucaCRM.Framework.Core.doPostOperation
              (
                  controllerUrl + "GetLeadsByStageGroup",
                  objLeadInfo,
                  function onSuccess(response) {
                      //$('#divFilterInfo').hide();
                      self.FilterLeadTag('');
                      self.FilterLeadTitle('');
                      self.DisplayLeadFilterInfo();
                      self.LeadListQueryModel = ko.observableArray([]);
                      ko.utils.arrayForEach(response.List, function (list) {
                          self.LeadList.push(new ErucaCRM.User.Leads.LeadListQueryModel(list));
                          ErucaCRM.Framework.Common.ApplyPermission('._roleselector');
                      });

                      self.GetLeadsbyStageId();
                      SetLeadListLayout();

                      $('._loadmore').click(function () {

                          var stageid = $(this).parent('._loadmorecontainer').attr("StageId");
                          var lastLeadId = $('._leadsContainer[stageid="' + stageid + '"]').find('._LeadList').children('._draggable ').last().attr('leadid');
                          if (lastLeadId == undefined)
                              var lastLeadId = $('._leadsContainer[stageid="' + stageid + '"]').find('._LeadList').children('.lead ').last().attr('leadid');
                          var stageinfo = new Object();
                          stageinfo.CurrentPage = 1;
                          stageinfo.LastLeadId_encrypted = lastLeadId;
                          stageinfo.IsLoadMore = true;
                          stageinfo.StageId_encrypted = stageid;
                          //if ($('#divFilterInfo').is(':visible')) {
                          stageinfo.tagName = $("#divFilterInfo").attr('tagname') == undefined ? "" : $("#divFilterInfo").attr('tagname');
                          stageinfo.SearchLeadName = $("#divFilterInfo").attr('searchLeadName') == undefined ? "" : $("#divFilterInfo").attr('searchLeadName');
                          //}
                          self.LoadMoreLeads(stageinfo);
                      });


                      self.MakeElementDraggable();





                      $('._addLeadDetail').click(function () {

                          self.AddLeadDetail();
                      });
                      $("._addnewlead").val(LeadName);
                      $('._companyName').val(CompanyName);
                      $("._addnewlead").focus(function () {
                          if ($(this).val() == LeadName) {
                              $(this).val("");
                          }
                          $('._companyName').css("display", "block");
                          $('._btnAddLead').css("display", "block");
                          $('._close-Popup').css("display", "block");
                      });
                      $("._companyName").focus(function () {
                          if ($(this).val() == CompanyName) {
                              $(this).val("");
                          }
                      });

                      $("._stage").on("click", "._deletelead", function () {
                          var leadId = $(this).attr("LeadId");
                          var StageId = $(this).attr("StageId");
                          self.DeleteLead(leadId, StageId);

                      });

                      $("._addnewlead").blur(function () {
                          if ($(this).val() == "") {
                              $(this).val(LeadName);
                          }
                      });
                      $("._companyName").blur(function () {
                          if ($(this).val() == "") {
                              $(this).val(CompanyName)
                          }
                      });

                      $('._closewinlostpop').click(function () {
                          $('._IsLeadLastStage').fadeOut("slow");
                      });

                      $('._close-Popup').click(function () {
                          $('._companyName').val(CompanyName).css("display", "none");
                          $('._addnewlead').val(LeadName);
                          $('._btnAddLead').css("display", "none");
                          $(this).css("display", "none");
                      });
                      $('._addnewlead').keypress(function (event) {

                          if (event.keyCode == 13) {
                              self.AddNewLead($('._btnAddLead'));
                              $('._addnewlead').val("");
                              self.ClearTagFilter();

                          }
                      });
                      $('._btnAddLead').click(function (event) {
                          self.AddNewLead(this);
                          $('._addnewlead').val(LeadName);
                          self.ClearTagFilter();
                      });

                      $('.closeLead-statuspop').click(function () {
                          $('._IsLeadLastStage').fadeOut("slow");
                      });
                      $('._ExistingContacts').click(function () {
                          self.ContactEmail("");
                          self.ContactName("");
                          self.HasContact(false);
                          self.ShowContactsDropDown(true);

                      });

                      //$("._leadsContainer").scroll(function () {
                      //    alert();
                      //    var top = $(window).height();

                      //    var menu = $(".side-buttons");

                      //    if ($(this).scrollTop() >= $(window).height() - menu.height()) {
                      //        menu.css("top", 0);
                      //    }
                      //    else {

                      //        menu.css("top", 210);
                      //        menu.removeClass('top').addClass('bottom');
                      //    }
                      //});


                      //$('._close').click(function () {

                      //    self.ShowContactsDropDown(false);
                      //    $('._leadAdditionalInformation').hide();
                      //})


                      $('._btnAddContacts').click(function () {
                          self.ContactEmail("");
                          self.ContactEmail("");
                          self.HasContact(true);
                          self.ShowContactsDropDown(false);

                      });


                      $('._droppable').droppable({
                          hoverClass: "drop-here",
                          //drag: function (event, ui) {
                          //    clone = ui.helper.clone();                             
                          //    clone.unbind("draggable");
                          //    var leadid = $(clone).find('._lead').attr("LeadId");
                          //    alert(leadid);
                          //},
                          drop: function (event, ui) {
                              clone = ui.helper.clone();
                              ui.helper.remove();

                              clone.unbind("draggable");
                              var leadid = $(clone).find('._lead').attr("LeadId");
                              var title = $(clone).find('._lead').attr("Title")
                              var Ownername = $(clone).find('._owner').text();
                              var NewStageId = $(this).attr("id");
                              var OldStageId = $(clone).find('._lead').attr("StageId");
                              clone.remove();

                              var IslastStage = $('#MainStageDiv').find('div[id= "' + NewStageId + '"]').attr("islaststage");
                              //  $("._LeadList").find("a[leadid='" + leadid + "']").removeClass('_draggable');
                              if (IslastStage == undefined || IslastStage == null) {
                                  IslastStage = "false";
                              }
                              if (IslastStage == "true") {


                                  ErucaCRM.Framework.Common.OpenPopup('div._IsLeadLastStage');
                                  $('._finalstageheader').html(ErucaCRM.Messages.Lead.FinalStageHeader)
                                  $('._winlossheader').html(ErucaCRM.Messages.Lead.WinLossOptionHeader)
                                  $('._IsLeadLastStage').attr({ "LeadId": leadid, "title": title, "FromStage": OldStageId, "ToStage": NewStageId });
                              } else {
                                  if (NewStageId != OldStageId) {
                                      var ChangeStageInfo = new Object();
                                      ChangeStageInfo.leadId_encrypted = leadid;
                                      ChangeStageInfo.Title = title;
                                      ChangeStageInfo.fromStage_encrypted = OldStageId;
                                      ChangeStageInfo.toStage_encrypted = NewStageId;
                                      ChangeStageInfo.IsClosedWin = false;
                                      self.changeleadstage(ChangeStageInfo, false, Ownername);
                                      //  chat.server.sendNotification(ChangeStageInfo.leadId_encrypted);
                                  }
                              }

                              self.MakeElementDraggable();
                          }
                      });
                      //
                      //$('._droppable[islaststage="true"]').children("leadlst").find("a[class='_draggable']").removeClass("_draggable")

                  },
                  function onError(err) {
                      self.status(err.Message);
                  }
              );
    }
    var WinCLoseChangeStageInfo = undefined;


    self.GetLeadsbyStageId = function () {
        ko.utils.arrayFilter(self.LeadList(), function (item) {
            var stagename = '#' + item.StageName.split(' ').join('');
            var stageinfo = new Object();
            stageinfo.stageId_encrypted = item.StageId;
            stageinfo.CurrentPage = 1;
            self.RenderLeadsInStage(stageinfo, item.StageId, stagename);
        });

    }



    self.RenderLeadsInStage = function (stageinfo, stageId, stagename) {
        var loderobj = undefined;
        $('._stageloader').each(function () {
            if ($(this).attr('loader_id') == stageId) {
                loderobj = this;
                $(this).show();

            }
        })

        ErucaCRM.Framework.Core.doPostOperation
         (
         controllerUrl + "RenderLeadsInStage",
                 stageinfo,
                  function onSuccess(response) {

                      ko.utils.arrayFilter(self.LeadList(), function (item) {
                          if (item.StageId == stageId) {
                              var pagesize = ErucaCRM.Framework.Core.Config.PageSize;

                              if (response.TotalRecord > pagesize) {
                                  $('._loadmorecontainer[stageid="' + stageId + '"]').css('display', 'block');
                              } else {
                                  $('._loadmorecontainer[stageid="' + stageId + '"]').css('display', 'none');
                              }
                              item.Leads.removeAll();
                              $.each(response.list, function (index, value) {
                                  item.Leads.push(new ErucaCRM.User.Leads.LeadListDetailQueryModel(value));
                              });
                          }

                      });
                      $(loderobj).hide();

                      self.MakeElementDraggable();
                      $(stagename).perfectScrollbar({
                          suppressScrollX: true
                      });
                      $('.leadsouter').perfectScrollbar({
                          suppressScrollY: true
                      });

                  });
    }


    $('._leadstagestatus').click(function () {
        var leadstatus = $(this).data("leadstatus");
        var Ownername = $(clone).find('._owner').text();
        var leadid = $(this).parent().parent(".popup").parent("._IsLeadLastStage").attr("LeadId");
        var Title = $(this).parent().parent(".popup").parent("._IsLeadLastStage").attr("Title");
        var FromStage = $(this).parent().parent(".popup").parent("._IsLeadLastStage").attr("FromStage");
        var ToStage = $(this).parent().parent(".popup").parent("._IsLeadLastStage").attr("ToStage");

        var ChangeStageInfo = new Object();
        ChangeStageInfo.leadId_encrypted = leadid;
        ChangeStageInfo.Title = Title;
        ChangeStageInfo.OwnerName = Ownername;
        ChangeStageInfo.fromStage_encrypted = FromStage;
        ChangeStageInfo.toStage_encrypted = ToStage;
        ChangeStageInfo.IsClosedWin = leadstatus;
        ChangeStageInfo.IsLastStage = true;

        if (ChangeStageInfo.IsClosedWin == "True") {

            self.GetLeadCompanyAndContactStatus(leadid);
            WinCLoseChangeStageInfo = ChangeStageInfo;
        }
        else {
            self.changeleadstage(ChangeStageInfo, true, Ownername);
        }
    });

    //self.RefreshLeadListbyStageId = function () {
    //    //$("._leadsContainer").each(function (index, value) {
    //    //    var lastLeadId = $(this).find('._LeadList').children('._draggable ').first().attr('leadid');
    //    //    var stageinfo = new Object();
    //    //    stageinfo.CurrentPage = 1;
    //    //    stageinfo.LastLeadId_encrypted = lastLeadId;
    //    //    stageinfo.IsLoadMore = false;
    //    //    stageinfo.StageId_encrypted = $(this).attr('StageId');
    //        self.LoadMoreLeads(stageinfo);
    //    });
    //}

    //setInterval(function () {
    //    var stageinfo = new Object();
    //    //if ($('#divFilterInfo').is(':visible')) {
    //    stageinfo.tagName = $("#divFilterInfo").attr('tagname') == undefined ? "" : $("#divFilterInfo").attr('tagname');
    //    stageinfo.SearchLeadName = $("#divFilterInfo").attr('SearchLeadName') == undefined ? "" : $("#divFilterInfo").attr('SearchLeadName');
    //    //}
    //    self.AutoRefreshLeads(stageinfo);
    //}, 60000);


    //self.AutoRefreshLeads = function (stageinfo) {
    //    ErucaCRM.Framework.Core.getJSONDataBySearchParam
    //                         (
    //                         controllerUrl + "AutoRefreshLeads",
    //                          stageinfo,
    //                        function onSuccess(response) {
    //                            if (response.Status == "Success") {
    //                                $.each(response.LeadList, function (index, value) {
    //                                    var $lead = $("._LeadList").find('div[leadid="' + value.LeadId_encrypted + '"]').find('._lead'); if (value.ActivityText == "LeadStageChanged") {

    //                                        self.LeadListDetailQueryModel = ko.observableArray([]);
    //                                        var val = new Object();
    //                                        val.Title = $lead.attr('title');
    //                                        val.LeadId = $lead.attr('leadid');
    //                                        val.StageId = value.StageId_encrypted;
    //                                        val.RatingId = value.RatingId_encrypted;
    //                                        var Rating = new Object();
    //                                        if (value.IsClosed == true) {
    //                                            Rating.Icons = value.IsClosedWin == true ? "winlead" : "lostlead";
    //                                        } else { Rating.Icons = value.RatingIcon; }
    //                                        val.Rating = Rating;
    //                                        val.IsClosedWin = value.IsClosedWin == undefined ? false : value.IsClosedWin;
    //                                        $("._LeadList").find("div[leadid='" + $lead.attr('leadid') + "']").remove();
    //                                        val.LeadOwnerName = value.OwnerName;
    //                                        self.AddNewLeadInArray(val);
    //                                        $('._IsLeadLastStage').css("display", "none");

    //                                    }
    //                                    else if (value.ActivityText == "LeadDeleted") {
    //                                        var leadId = $lead.attr("LeadId");
    //                                        var StageId = $lead.attr("StageId");
    //                                        $("._LeadList").find("div[leadid='" + leadId + "']").remove();

    //                                    } else if (value.ActivityText == "LeadRatingChanged") {
    //                                        var RatingInfo = new Object();
    //                                        RatingInfo.leadId_encrypted = $lead.attr("LeadId");
    //                                        RatingInfo.ratingId_encrypted = value.RatingId_encrypted;
    //                                        var ImgSrc = value.RatingIcon != null ? "/Content/images/" + value.RatingIcon + ".png" : "";
    //                                        $("._LeadList").find('div[leadid="' + $lead.attr("LeadId") + '"]').children('._leadiconcontainer').find('._ratingicon').attr('src', ImgSrc);

    //                                    } else if (value.ActivityText == "LeadAdded") {

    //                                        //if lead is alreadi has been added then skip
    //                                        if ($("._LeadList").find("div[leadid='" + value.LeadId_encrypted + "']").length == 0) {

    //                                            var val = new Object();
    //                                            val.Title = value.Title;
    //                                            val.LeadId = value.LeadId_encrypted;
    //                                            val.StageId = value.StageId_encrypted;
    //                                            val.RatingId = value.RatingId_encrypted;
    //                                            val.IsClosedWin = value.IsClosedWin == undefined ? false : value.IsClosedWin
    //                                            val.LeadOwnerName = value.OwnerName;
    //                                            var Rating = new Object();
    //                                            Rating.Icons = value.RatingIcon;
    //                                            val.Rating = Rating;
    //                                            self.AddNewLeadInArray(val);
    //                                            $('._IsLeadLastStage').css("display", "none");
    //                                        }
    //                                    }
    //                                });
    //                                self.MakeElementDraggable();
    //                            }

    //                        }, function onError(err) {
    //                            ErucaCRM.Framework.Core.ShowMessage(err.Message, true);
    //                        }, function () {

    //                        }, function () {
    //                        });
    //}

    ErucaCRM.User.Leads.autoRefreshLeads = function (response) {
        if (response != "" || response != null) {
            var $lead = $("._LeadList").find('div[leadid="' + response.RecentActivities[0].LeadId + '"]').find('._lead');
            if (response.RecentActivities[0].ActivityType == 4) {

                self.LeadListDetailQueryModel = ko.observableArray([]);
                var val = new Object();
                val.Title = $lead.attr('title');
                val.LeadId = $lead.attr('leadid');
                val.StageId = response.RecentActivities[0].StageId;
                val.RatingId = response.RecentActivities[0].RatingId;
                var Rating = new Object();
                if (response.RecentActivities[0].IsClosedWin == true) {
                    Rating.Icons = response.RecentActivities[0].IsClosedWin == true ? "winlead" : "lostlead";
                } else { Rating.Icons = response.RecentActivities[0].RatingIcon; }
                val.Rating = Rating;
                val.IsClosedWin = response.RecentActivities[0].IsClosedWin == undefined ? false : response.RecentActivities[0].IsClosedWin;
                $("._LeadList").find("div[leadid='" + $lead.attr('leadid') + "']").remove();
                val.LeadOwnerName = response.RecentActivities[0].OwnerName;
                self.AddNewLeadInArray(val);
                $('._IsLeadLastStage').css("display", "none");

            }
            else if (response.RecentActivities[0].ActivityType == 5) {
                var leadId = $lead.attr("LeadId");
                var StageId = $lead.attr("StageId");
                $("._LeadList").find("div[leadid='" + leadId + "']").remove();

            } else if (response.RecentActivities[0].ActivityType == 3) {
                var RatingInfo = new Object();
                RatingInfo.leadId_encrypted = $lead.attr("LeadId");
                RatingInfo.ratingId_encrypted = response.RecentActivities[0].RatingId;
                var ImgSrc = response.RecentActivities[0].RatingIcon != null ? "/Content/images/" + response.RecentActivities[0].RatingIcon + ".png" : "";
                $("._LeadList").find('div[leadid="' + $lead.attr("LeadId") + '"]').children('._leadiconcontainer').find('._ratingicon').attr('src', ImgSrc);

            } else if (response.RecentActivities[0].ActivityType == 1) {
                //if lead is alreadi has been added then skip
                if ($("._LeadList").find("div[leadid='" + response.RecentActivities[0].LeadId + "']").length == 0) {

                    var val = new Object();
                    val.Title = response.RecentActivities[0].Title;
                    val.LeadId = response.RecentActivities[0].LeadId;
                    val.StageId = response.RecentActivities[0].StageId;
                    val.RatingId = response.RecentActivities[0].RatingId;
                    val.IsClosedWin = response.RecentActivities[0].IsClosedWin == undefined ? false : response.RecentActivities[0].IsClosedWin
                    val.LeadOwnerName = response.RecentActivities[0].OwnerName;
                    var Rating = new Object();
                    Rating.Icons = response.RecentActivities[0].RatingIcon;
                    val.Rating = Rating;
                    self.AddNewLeadInArray(val);
                    $('._IsLeadLastStage').css("display", "none");
                }
            }
        }
        self.MakeElementDraggable();
    }

    self.LoadMoreLeads = function (stageinfo) {


        ErucaCRM.Framework.Core.getJSONDataBySearchParam
                       (
                       controllerUrl + "GetLeadsByStageId",
                        stageinfo,
                            function onSuccess(response) {
                                if (response.List.length != 0) {

                                    self.Leads = ko.observableArray([]);
                                    var LeadsArrary = ko.utils.arrayFilter(self.LeadList(), function (item) {
                                        if (item.StageId == stageinfo.StageId_encrypted) {

                                            ko.utils.arrayForEach(response.List, function (list) {

                                                var alreadyAdded = false;
                                                ko.utils.arrayForEach(item.Leads(), function (val) {

                                                    if (val.LeadId == list.LeadId) {
                                                        alreadyAdded = true;


                                                    }

                                                });
                                                if (!alreadyAdded) {

                                                    item.Leads.push(new ErucaCRM.User.Leads.LeadListDetailQueryModel(list));
                                                }

                                            });
                                        }
                                    });

                                    self.MakeElementDraggable();

                                    $("._leadsContainer").each(function () {
                                        $(this).perfectScrollbar('update');
                                    });
                                }
                                var pagesize = ErucaCRM.Framework.Core.Config.PageSize;
                                if (response.TotalRecords > pagesize) {

                                    $('._leadsContainer[stageid="' + stageinfo.StageId_encrypted + '"]').find('._loadmorecontainer').css('display', 'block');
                                } else { $('._leadsContainer[stageid="' + stageinfo.StageId_encrypted + '"]').find('._loadmorecontainer').css('display', 'none'); }


                            });

    }


    self.OpenPopUpForConfirmation = function (e) {
        var Rating = $(e).data("ratingid");
        var ExpectedRevenuePer = $(e).data("expectedrevenue");
        var Amount = $('#Amount').val();
        var ExpectedRevenue = Amount * (ExpectedRevenuePer / 100);

        var RatingInfo = new Object();
        RatingInfo.leadId_encrypted = $("#LeadId").val();
        RatingInfo.ratingId_encrypted = Rating;
        RatingConstant = $(e).data('ratingconstant');
        if (RatingConstant == FinalRatingConstant) {//5 is for final
            ErucaCRM.Framework.Common.OpenPopup('div._IsLeadLastRating');
            $('._leadWonHeading').html(ErucaCRM.Messages.Lead.LeadWonHeader);
            $('._optionYesNoHeading').html(ErucaCRM.Messages.Lead.YesNoOptionHeader);

            $('._leadratingstatus').attr({ "LeadId": $("#LeadId").val(), "RatingId": Rating, "StageId": $('#StageId').val(), "FinalStageId": $('#FinalStageId').val(), "LeadTitle": $('#Title').val() });
        }
        else {
            self.ChangeLeadRating(RatingInfo);
            $("#LeadRatingConstant").val(RatingConstant)
            $('#ExpectedRevenueAmount').text(ExpectedRevenue);
        }



    }

    self.GetLeadObjectByStageId = function (stageId) {
        var LeadsArrary = ko.utils.arrayFilter(self.LeadList(), function (item) {
            if (item.StageId == stageId) {
                return item;
            }
        });
        return LeadsArrary;
    }
    self.ChangeRatingIcon = function (leadObj) {
        var imageSrc = leadObj.Rating.Icons != null ? "/Content/images/" + leadObj.Rating.Icons + ".png" : "";
        $("img[ratingImageLeadId='" + leadObj.LeadId + "']").attr('src', imageSrc);
    }
    self.ChangeLeadRating = function (RatingInfo) {

        ErucaCRM.Framework.Core.doPostOperation
         (
         controllerUrl + "ChangeLeadRating",
                 RatingInfo,
              function onSuccess(response) {
                  $('#RatingId').val(RatingInfo.ratingId_encrypted);
                  if (response.Status == "Success") {
                      self.ChangeRatingIcon(response.Lead);
                      ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Lead.RatingChangedSuccessfully, false);

                  }
              }
     );


    }
    //Code for opening popup when last star is clicked


    $('._leadratingstatus').click(function () {
        var RatingInfo = new Object();
        RatingInfo.leadId_encrypted = $(this).attr('LeadId');
        RatingInfo.ratingId_encrypted = $(this).attr('RatingId');
        var image = $(this).attr('NewImage');
        // self.ChangeLeadRating(RatingInfo, image);
        $('._close').click();
        self.GetLeadCompanyAndContactStatus(RatingInfo.leadId_encrypted);
        var ChangeStageInfo = new Object();
        ChangeStageInfo.leadId_encrypted = RatingInfo.leadId_encrypted;
        ChangeStageInfo.Title = $(this).attr("leadtitle");
        ChangeStageInfo.fromStage_encrypted = $(this).attr("StageId");
        ChangeStageInfo.toStage_encrypted = $(this).attr("FinalStageId");
        ChangeStageInfo.IsClosedWin = "True";
        WinCLoseChangeStageInfo = ChangeStageInfo;

    })


    self.GetLeadCompanyAndContactStatus = function (leadId) {
        $('._IsLeadLastStage').fadeOut();




        var objLeadInfo = new Object();

        objLeadInfo.leadId_encrypted = leadId;

        ErucaCRM.Framework.Core.doPostOperation
            (
                controllerUrl + "GetLeadCompanyAndContactStatus",
                 objLeadInfo,
                  function onSuccess(response) {

                      self.ClearField();
                      self.LeadCompanyName(response.CompanyName);
                      self.LeadAmount(response.Amount);
                      self.LeadOwner(response.LeadOwnerName);
                      if (response.HasContact == true) {

                          self.HasContact(false);
                      }
                      else {
                          self.HasContact(true);
                          $('#drpContactList').html("");
                          $('#drpContactList').append("<option value=0>" + ErucaCRM.Messages.DropDowns.SelectOption + "</option>");
                          $.each(response.ContactList, function (index, element) {
                              $('#drpContactList').append("<option value='" + element.Value + "'>" + element.Text + "</option>");
                          });
                      }

                      ErucaCRM.Framework.Common.OpenPopup('div#leadAdditionalInformation');

                      $('._headerAddditionalInfo').html(ErucaCRM.Messages.Lead.LeadAdditionalInformationHeader)
                  });

    }

    self.ClearField = function () {
        self.LeadCompanyName("");
        self.LeadAmount("")
        self.LeadOwner("")
        self.HasContact("")
        self.ContactName("")
        self.ContactEmail("");
        self.ShowContactsDropDown(false)
        self.HasContact(true);

    }

    self.RemoveLeadById = function (leadId, stageId) {
        var isRemoved = false;
        var LeadsArrary = ko.utils.arrayFilter(self.LeadList(), function (item) {
            if (item.StageId == stageId) {
                if (isRemoved == false)
                    ko.utils.arrayFilter(item.Leads(), function (leadItem) {
                        if (leadItem != undefined && leadItem != null)
                            if (leadItem.LeadId == leadId) {
                                item.Leads.remove(leadItem);
                                isRemoved = true;
                                return false;
                            }
                    });
                return false;
            }
        });
    }


    self.changeleadstage = function (ChangeStageInfo, IslastStage, Ownername) {
        ErucaCRM.Framework.Core.doPostOperation
             (
                 controllerUrl + "ChangeLeadStage",
                 ChangeStageInfo,
                 function onSuccess(response) {
                     if (response.response.Status == "Success") {
                         $("._LeadList").find("div[leadid='" + ChangeStageInfo.leadId_encrypted + "']").remove();
                         self.LeadListDetailQueryModel = ko.observableArray([]);
                         var val = new Object();
                         val.Title = ChangeStageInfo.Title;
                         val.LeadId = ChangeStageInfo.leadId_encrypted;
                         val.StageId = ChangeStageInfo.toStage_encrypted;
                         val.Rating = response.LeadRating;
                         val.LeadOwnerName = Ownername == undefined ? ChangeStageInfo.OwnerName : Ownername;
                         val.IsClosedWin = ChangeStageInfo.IsClosedWin == undefined ? false : ChangeStageInfo.IsClosedWin
                         self.RemoveLeadById(ChangeStageInfo.leadId_encrypted, ChangeStageInfo.fromStage_encrypted);
                         self.AddNewLeadInArray(val);
                         $('._IsLeadLastStage').css("display", "none");
                         ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Lead.StageChangeSuccessfully, false);
                         //self.MakeElementDraggable();
                         //$("._lead").click(function () {
                         //    self.LeadDetail($(this));
                         //});

                         if (IslastStage) {
                             $("._LeadList").find("a[leadid='" + ChangeStageInfo.leadId_encrypted + "']").removeClass('_draggable');

                         }
                     } else { ErucaCRM.Framework.Core.ShowMessage("Sorry try again.", false); }
                 }
             );
    }

    self.RenderLeads = function (LeadListData) {
        $("#divNoRecord").hide();
        $("#LeadListData").children().remove();
        if (LeadListData.List.length == 0) {
            $("#divNoRecord").html("<b>" + ErucaCRM.Messages.Lead.NoLeadFound + "</b>");
            $("#divNoRecord").show();
        }
        self.LeadList.removeAll();
        ko.utils.arrayForEach(LeadListData.List, function (Lead) {
            self.LeadList.push(new ErucaCRM.User.Leads.LeadListQueryModel(Lead));
        });

        $("#Pager").html(self.GetPaging(LeadListData.TotalRecords, currentPage, PagingMethodName));
    };

    self.GetPageData = function (currentPage) {

        currentPage = currentPage;
        var objLeadInfo = new Object();
        objLeadInfo.CurrentPage = currentPage;
        objLeadInfo.FilterParameters = new Array();
        self.getLeadList(objLeadInfo);

    }

    self.GetPaging = function (Rowcount, currentPage, methodName) {

        return ErucaCRM.Framework.Core.GetPagger(Rowcount, currentPage, methodName);
    }
    self.getLeadList(objLeadInfo);
    self.Initialize();
    self.InitializeTabControl = function () {
        $("#leadDetailTabs").tabs({
            select: function (event, ui) {
                $("#leadDetailTabs").find("._tabstrip li").removeClass("active");
                $(ui.tab).parents('li').addClass('active');
                var selectedTab = ui.panel.id;
                switch (selectedTab) {
                    case "LeadGeneralTab":
                        break;
                    case "LeadHistoryTab":
                        self.GetLeadHistory(1);
                        self.GetLeadHistoryChartDetails();
                        break;
                    case "LeadActivitiesTab":
                        self.GetTaskItemList(1);

                        break;
                    case "LeadProductsTab":
                        self.GetLeadProductsList(1);
                        break;
                    case "LeadDocumentsTab":
                        self.GetLeadDocumentsList(1);
                        break;
                    case "LeadContactsTab":
                        self.GetLeadContactsList(1);
                        break;
                    case "LeadCommentsTab":
                        self.GetLeadCommentsList(1);
                        break;
                }
            }

        });

    }
    self.SetSelectedTab = function (index) {
        self.LeadHistoryList.removeAll();
        self.TaskItemList.removeAll();
        self.AllProductsList.removeAll();
        self.LeadProductsList.removeAll();
        $('#leadDetailTabs').tabs("option", "active", index);
    }
    self.SaveAdditionalInformation = function () {
        if (self.LeadCompanyName() == null || $.trim(self.LeadCompanyName()) == "") {
            alert(ErucaCRM.Messages.Lead.CompanyNameRequired)
            return false;
        }


        else if (self.LeadAmount() == null || $.trim(self.LeadAmount()) == "") {
            alert(ErucaCRM.Messages.Lead.AmountRequired)
            return false;

        }

        if (self.HasContact() == true) {
            if (self.ContactEmail() == null || $.trim(self.ContactEmail()) == "") {
                alert(ErucaCRM.Messages.Lead.EmailRequired)
                return false;

            }
            else if (self.ContactName() == null || $.trim(self.ContactName()) == "") {
                alert(ErucaCRM.Messages.Lead.ContactNameRequired)
                return false;
            }
            var email = self.ContactEmail();
            var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
            if (!filter.test(email)) {
                alert(ErucaCRM.Messages.Lead.InvalidEmailAddress)
                return false;
            }
        }
        else if (self.ShowContactsDropDown() == true) {

            if ($('#drpContactList').val() == "0") {
                alert(ErucaCRM.Messages.Lead.ContactNameRequired);
                return false;
            }
        }


        if (isNaN(self.LeadAmount())) {
            alert(ErucaCRM.Messages.Lead.AmountNotValid);
            return false;
        }

        var objLeadInfo = new Object();
        objLeadInfo.leadId_encrypted = WinCLoseChangeStageInfo.leadId_encrypted;

        objLeadInfo.CompanyName = self.LeadCompanyName();
        objLeadInfo.Amount = self.LeadAmount();

        if (self.ShowContactsDropDown() == true) {
            objLeadInfo.id_encrypted = $('#drpContactList').val();
        }
        else {
            objLeadInfo.ContactName = self.ContactName();
            objLeadInfo.ContactEmail = self.ContactEmail();
        }
        ErucaCRM.Framework.Core.doPostOperation
               (
                   controllerUrl + "UpdateLeadCompanyAndContactStatus",
                    objLeadInfo,
                     function onSuccess(response) {
                         if (WinCLoseChangeStageInfo != undefined) {

                             self.changeleadstage(WinCLoseChangeStageInfo, true, self.LeadOwner());
                             $('._resetUrl').click();
                             WinCLoseChangeStageInfo = undefined;
                             self.HasContact(false);
                             self.ShowContactsDropDown(false);
                             $('._close').click();
                         }


                     });


    }
    self.GetSelectedLeadId = function () {
        return $("#LeadId").val();
    }

    self.InitialiseRatings = function () {

        IsLeadWinClose = $('#hdnIsClosedWin').val() == "true" ? true : false;
        IsLeadsLastStage = $('#hdnIsLastStage').val() == "true" ? true : false;
        if (!IsLeadsLastStage) {

            $('.ratings_stars').hover(
        function () {
            $(this).prevAll().andSelf().addClass('ratings_over');
            $(this).nextAll().removeClass('ratings_vote');
        },

        function () {
            $(this).prevAll().andSelf().removeClass('ratings_over');
            Set_Rating($(this).parent(), $(this).parent().attr('avg'));
        }
      );
            $('.ratings_stars').bind('click', function () {

                self.OpenPopUpForConfirmation(this);
                Set_Rating(".rate_widget", $(this).data('ratingconstant'));
            });
        }
        function Set_Rating(widget, RatingCostant) {

            var ratingClass = "ratings_vote"
            if (IsLeadsLastStage && IsLeadWinClose) {
                $(widget).find('.ratings_stars, .ratings_stars-readonly').removeClass(ratingClass);
                ratingClass = "ratings_over"
                RatingCostant = 5;
                $('.leadpopup input[type="text"],.leadpopup select ,.leadpopup textarea').attr("disabled", "disabled");

            } else if (IsLeadsLastStage && !IsLeadWinClose) {
                $(widget).find('.ratings_stars, .ratings_stars-readonly').removeClass(ratingClass);
                ratingClass = "ratings_loss"
                RatingCostant = 5;
                $('.leadpopup input[type="text"] ,.leadpopup select ,.leadpopup textarea').attr("disabled", "disabled");
            }
            else {
                $('.leadpopup input[type="text"] ,.leadpopup select ,.leadpopup textarea').prop("disabled", false);
            }
            var avg = RatingCostant;

            $(widget).find('.star_' + avg).prevAll().andSelf().addClass(ratingClass);
            $(widget).find('.star_' + avg).nextAll().removeClass(ratingClass);
            $(widget).attr("avg", avg);
        }

        Set_Rating(".rate_widget", $("#LeadRatingConstant").val());
        $('._close').click(function () {
            Set_Rating(".rate_widget", $("#LeadRatingConstant").val());

        })
    }




    //===================================================Lead Hisotry section==========================================
    self.GetLeadHistory = function (pageNo) {
        var LeadInfo = new Object();
        LeadInfo.leadId_encrypted = self.GetSelectedLeadId();
        LeadInfo.CurrentPageNo = pageNo;
        ErucaCRM.Framework.Core.doPostOperation
            (
                controllerUrl + "GetLeadHistory",
                LeadInfo,
                 function onSuccess(response) {
                     if (response.Response == "Success") {

                         var LeadStages = new Array();
                         var LeadDuration = new Array();
                         var objSeries = new Object()
                         objSeries.data = [];
                         objSeries.Mint = []
                         self.LeadHistoryList.removeAll();
                         ko.utils.arrayForEach(response.LeadHistory, function (list) {

                             self.LeadHistoryList.push(new ErucaCRM.User.Leads.LeadHistoryQueryModel(list));

                             LeadStages.unshift(list.StageName);
                             var stageDurationPercentage = list.StageDurationPercentage == null ? 0 : list.StageDurationPercentage;
                             objSeries.stack = 'Stage';
                             var objDuration = new Object();
                             objDuration.Key = stageDurationPercentage + list.StageName;
                             objDuration.Minutes = list.Duration
                             objSeries.Mint.unshift(objDuration)
                             //objSeries.Duration.Minutes = list.Duration;



                             objSeries.data.unshift(stageDurationPercentage);


                         }); LeadDuration.push(objSeries);

                         $("#LeadHistoryPager").html(self.GetPaging(response.TotalRecords, LeadInfo.CurrentPageNo, 'viewModel.GetLeadHistory'));



                     }
                     $("#containerLeadHistoryChart .highcharts-legend").css("display", "none");
                     //$("#containerLeadHistoryChart .highcharts-tooltip").css("display", "none");                   
                     if (self.LeadHistoryList().length == 0) { self.ShowNoLeadHistoryFoundMsg(Visibility.visible) }
                     else { self.ShowNoLeadHistoryFoundMsg(Visibility.hidden) }
                 }
            );



    }

    self.GetLeadHistoryChartDetails = function () {
        var LeadInfo = new Object();
        LeadInfo.leadId_encrypted = self.GetSelectedLeadId();
        //  LeadInfo.CurrentPageNo = pageNo;

        ErucaCRM.Framework.Core.doPostOperation
       (
           controllerUrl + "GetLeadHistoryChartDetails",
           LeadInfo,
                function onSuccess(response) {
                    if (response.Response == "Success") {

                        var LeadStages = new Array();
                        var LeadDuration = new Array();
                        var objSeries = new Object()
                        objSeries.data = [];
                        objSeries.Mint = []

                        ko.utils.arrayForEach(response.LeadHistoryChartDetail, function (LeadHistoryChartDetail) {

                            self.LeadHistoryChartDetailList.push(new ErucaCRM.User.Leads.LeadHistoryChartDetailsQueryModel(LeadHistoryChartDetail));

                            LeadStages.push(LeadHistoryChartDetail.StageName);
                            var stageDurationPercentage = LeadHistoryChartDetail.StageDurationPercentage == null ? 0 : LeadHistoryChartDetail.StageDurationPercentage;
                            objSeries.stack = 'Stage';
                            var objDuration = new Object();
                            objDuration.Key = stageDurationPercentage + LeadHistoryChartDetail.StageName;
                            objDuration.Minutes = LeadHistoryChartDetail.Duration
                            objSeries.Mint.push(objDuration)

                            objSeries.data.push(stageDurationPercentage);


                        }); LeadDuration.push(objSeries);


                        $('#containerLeadHistoryChart').highcharts({

                            chart: {
                                type: 'column'
                            },

                            title: {
                                text: ErucaCRM.Messages.Lead.LeadHistoryChartHeaderText
                            },

                            xAxis: {
                                categories: LeadStages
                            },

                            yAxis: {
                                allowDecimals: false,
                                min: 0,
                                max: 100,
                                step: 1,
                                title: {
                                    text: ErucaCRM.Messages.Lead.LeadHistoryChartYAxisText
                                }
                            },

                            tooltip: {
                                formatter: function () {
                                    var stage = this.y;
                                    stage = stage.toString() + this.key;
                                    var miutesArrary = this.series.userOptions.Mint;
                                    var mint = self.GetKeyVaue(miutesArrary, stage);
                                    return '<b>' + this.x + '</b><br/> Total' +
                                      ': ' + mint  //+ '<br/>' +
                                    //  'Total: ' + this.point.stackTotal;

                                }
                            },

                            plotOptions: {
                                column: {
                                    stacking: 'normal'
                                }
                            },

                            series: LeadDuration
                        });
                        $("#containerLeadHistoryChart .highcharts-legend").css("display", "none");
                    }

                }
                    );

        $("#spanDeletedStageInfo").html(ErucaCRM.Messages.Lead.LeadHistoryChartLegendText);

    }

    //===================================================End History section===========================================

    //===================================================Lead Activities Section========================================
    self.GetKeyVaue = function (arrary, key) {
        var minutes = '0';
        $.each(arrary, function (key1, val) {
            if (val.Key == key) {
                minutes = val.Minutes; return;
            }
        })
        if (minutes == undefined || minutes == null || minutes == '') minutes = 0;
        return minutes;
    }
    self.GetTaskItemList = function (pageNo) {
        var leadId = self.GetSelectedLeadId();
        //self.AddLeadActivityUrl("/User/taskitem?mod=Lead&val_encrypted=" + leadId);
        self.AddLeadActivityUrl("/User/taskitem?mod=Lead&val_encrypted=" + leadId);
        var LeadInfo = new Object();
        CurrentLeadTaskItemsPage = pageNo;
        LeadInfo.leadId_encrypted = leadId;
        LeadInfo.CurrentPageNo = pageNo;
        CurrentLeadTaskItemsPage = pageNo;
        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetLeadTasks", LeadInfo, function onSuccess(response) {
            self.RenderTaskItems(response);


        }, function onError(err) {
            ErucaCRM.Framework.Core.ShowMessage(err.Message, true);
        });

    }
    self.RenderTaskItems = function (TaskItemListData) {

        $("#divNoRecord").hide();
        self.TaskItemList.removeAll();
        if (TaskItemListData.List.length == 0) {
            $("#divNoRecord").html("<br/><b>" + ErucaCRM.Messages.Account.NoAccountActivityRecordFound + "</b>");
            $("#divNoRecord").show();

        }

        ko.utils.arrayForEach(TaskItemListData.List, function (TaskItem) {
            self.TaskItemList.push(new ErucaCRM.User.Leads.LeadTaskItemQueryModel(TaskItem));
        });
        ErucaCRM.Framework.Common.ApplyPermission('._roleselector');
        $("#Pager").html(self.GetPaging(TaskItemListData.TotalRecords, CurrentLeadTaskItemsPage, LeadTaskItemsMethodName));

    };

    self.DeleteTaskItem = function (task) {

        if (confirm(ErucaCRM.Messages.TaskItem.ConfirmTaskDeleteAction)) {
            var data = new Object();
            data.id_encrypted = task.TaskId;
            ErucaCRM.Framework.Core.doDeleteOperation(controllerUrl + "DeleteTaskItem", data, function onSuccess(response) {

                self.TaskItemList.remove(task);
                if (self.TaskItemList().length == 0) {
                    self.GetTaskItemList(CurrentLeadTaskItemsPage - 1);
                }
                else {
                    if (CurrentLeadTaskItemsPage == 1) {
                        self.GetTaskItemList(1);
                    }
                    else {
                        self.GetTaskItemList(CurrentLeadTaskItemsPage);
                    }
                }
            });
        }
    }
    //===================================================End Activities Section===========================================

    //===================================================Lead Products Section===========================================

    self.GetLeadProductsList = function (pageNo) {

        var LeadInfo = new Object();
        LeadInfo.leadId_encrypted = self.GetSelectedLeadId();
        LeadInfo.CurrentPageNo = pageNo;
        CurrentLeadProductsPage = pageNo;
        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetLeadProducts", LeadInfo, function onSuccess(response) {
            self.RenderLeadProducts(response);


        }, function onError(err) {
            ErucaCRM.Framework.Core.ShowMessage(err.Message, true);
        });

    }
    self.RenderLeadProducts = function (LeadProducts) {
        $("#divNoRecord").hide();
        self.LeadProductsList.removeAll();

        if (LeadProducts.LeadProducts.length == 0) {
            $("#divNoRecord").html("<br/><b>" + ErucaCRM.Messages.Account.NoAccountActivityRecordFound + "</b>");
            $("#divNoRecord").show();

        }
        ko.utils.arrayForEach(LeadProducts.LeadProducts, function (leadProduct) {
            self.LeadProductsList.push(new ErucaCRM.User.Leads.LeadProductsQueryModel(leadProduct));
        });

        $("#LeadProductsPager").html(self.GetPaging(LeadProducts.TotalRecords, CurrentLeadProductsPage, LeadProductsMethodName));
        if (self.LeadProductsList().length == 0) { self.ShowNoLeadProductFoundMsg(Visibility.visible) }
        else { self.ShowNoLeadProductFoundMsg(Visibility.hidden) }
    };
    self.GetAllProductsList = function (pageNo) {
        var LeadInfo = new Object();
        LeadInfo.leadId_encrypted = self.GetSelectedLeadId();
        LeadInfo.CurrentPageNo = pageNo;
        CurrentAllProductsPage = pageNo;
        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetAllProducts", LeadInfo, function onSuccess(response) {
            self.RenderAllProducts(response);


        }, function onError(err) {
            ErucaCRM.Framework.Core.ShowMessage(err.Message, true);
        });

    }
    self.RenderAllProducts = function (Products) {

        $("#divNoRecord").hide();
        self.AllProductsList.removeAll();
        if (Products.Products.length == 0) {
            $("#divNoRecord").html("<br/><b>" + ErucaCRM.Messages.Account.NoAccountActivityRecordFound + "</b>");
            $("#divNoRecord").show();
        }

        ko.utils.arrayForEach(Products.Products, function (Product) {
            self.AllProductsList.push(new ErucaCRM.User.Leads.LeadProductsQueryModel(Product));
        });

        $("#AllProductsPager").html(self.GetPaging(Products.TotalRecords, CurrentAllProductsPage, AllProductsMethodName));
    };
    self.AllProductListPopup = function () {
        ErucaCRM.Framework.Core.OpenRolePopup("#AddProductSection");
        self.GetAllProductsList(1);
    }
    self.AddNewProductPopup = function () {
        $('#addNewProductSection input[type=text]').val("");
        ErucaCRM.Framework.Core.OpenRolePopup("#addNewProductSection");
    }

    self.AllProductsMouserEnterHandler = function (data) {
        data.ShowLink('block');
    },
    self.AllProductsMouserOutHandler = function (data) {
        data.ShowLink('none');
    }
    self.AddProduct = function (product) {
        var postData = new Object();
        postData.ProductId = product.ProductId;
        postData.LeadId = self.GetSelectedLeadId();
        ErucaCRM.Framework.Core.doPostOperation(controllerUrl + "AddProductToLead", postData, function onSuccess(response) {
            ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Product.ProductAddedSuccess, false);
            postData.ProductName = product.ProductName;
            postData.ProductCode = product.ProductCode;
            self.GetLeadProductsList(1);
            self.GetAllProductsList(1);
            self.AllProductsList.remove(product);
            $("#AddProductSection ._close").click();
            self.ShowNoLeadProductFoundMsg(Visibility.hidden)
        });
    }

    self.RemoveProduct = function (product) {
        var postData = new Object();
        postData.ProductId = product.ProductId;
        postData.LeadId = self.GetSelectedLeadId();
        ErucaCRM.Framework.Core.doPostOperation(controllerUrl + "RemoveProductFromLead", postData, function onSuccess(response) {
            ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Product.ProductRemovedSuccess, false);
            this.AllProductsList = ko.observableArray([]);
            postData.ProductName = product.ProductName;
            postData.ProductCode = product.ProductCode;
            self.AllProductsList.push(new ErucaCRM.User.Leads.LeadProductsQueryModel(postData));
            self.LeadProductsList.remove(product)
            //  self.GetLeadProductsList(1);           
            if (self.LeadProductsList().length == 0 && CurrentLeadProductsPage != 1) {
                self.GetLeadProductsList(CurrentLeadProductsPage - 1);
            }
            else {
                if (CurrentLeadProductsPage == 1) {
                    self.GetLeadProductsList(1);
                }
                else {
                    self.GetLeadProductsList(CurrentLeadProductsPage);
                }
            }
        });
    }

    self.SaveNewProduct = function () {
        var postData = new Object();
        postData.ProductName = $('#ProductName').val();
        postData.ProductCode = $('#ProductCode').val();
        postData.UnitPrice = $('#UnitPrice').val();
        if (postData.ProductCode == '' || postData.ProductName == '' || postData.UnitPrice == '') {
            alert(ErucaCRM.Messages.Product.AllFieldRequierd);
            return;
        }

        if (isNaN(postData.UnitPrice)) {
            alert(ErucaCRM.Messages.Product.UnitPriceNotValid);
            return;
        }

        ErucaCRM.Framework.Core.doPostOperation(controllerUrl + "AddNewProduct", postData, function onSuccess(response) {
            ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Product.ProductAddedSuccess, false);
            postData.ProductId = response.ProductId;
            self.GetAllProductsList(1);
            $("#addNewProductSection ._close").click();
        })

    };


    //======================================================End Products Section=============================================================

    //=====================================================Lead Contacts Section============================================================
    self.GetLeadContactsList = function (pageNo) {

        var leadId = self.GetSelectedLeadId();
        //self.AddLeadContactUrl("/User/AddContact?id_encrypted=" + leadId + "&mode=Lead");

        self.AddLeadContactUrl("/User/AddContact?id_encrypted=" + leadId + "&mode=Lead&returnurl=" + window.location.href.replace('#', '@'));
        CurrentLeadContactPage = pageNo;
        var LeadInfo = new Object();
        LeadInfo.leadId_encrypted = self.GetSelectedLeadId();
        LeadInfo.CurrentPageNo = pageNo;
        CurrentLeadContactPage = pageNo;
        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetLeadContacts", LeadInfo, function onSuccess(response) {
            self.RenderLeadContacts(response);


        }, function onError(err) {
            ErucaCRM.Framework.Core.ShowMessage(err.Message, true);
        });

    }
    self.RenderLeadContacts = function (LeadContacts) {

        $("#divNoRecord").hide();
        self.LeadContactsList.removeAll();

        if (LeadContacts.LeadContacts.length == 0) {
            $("#divNoRecord").html("<br/><b>" + ErucaCRM.Messages.Account.NoAccountActivityRecordFound + "</b>");
            $("#divNoRecord").show();

        }
        ko.utils.arrayForEach(LeadContacts.LeadContacts, function (leadProduct) {
            self.LeadContactsList.push(new ErucaCRM.User.Leads.LeadContactsQueryModel(leadProduct));
        });
        ErucaCRM.Framework.Common.ApplyPermission('._roleselector');
        $("#LeadContactsPager1").html(self.GetPaging(LeadContacts.TotalRecords, CurrentLeadContactPage, LeadContactsMethodName));
        if (self.LeadContactsList().length == 0) { self.ShowNoLeadContactFoundMsg(Visibility.visible) }
        else { self.ShowNoLeadContactFoundMsg(Visibility.hidden) }
    };
    self.DeleteContact = function (Contact) {
        if (confirm(ErucaCRM.Messages.Contact.ConfirmContactDeleteAction)) {
            var data = new Object();
            data.contactId_encrypted = Contact.ContactId;
            data.leadId_encrypted = Contact.LeadId;
            ErucaCRM.Framework.Core.doDeleteOperation(controllerUrl + "DeleteLeadContact", data, function onSuccess(response) {
                if (Message.Success == response.Status) {
                    ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Contact.RecordDeletedSuccess, false);
                    self.LeadContactsList.remove(Contact);
                    //currentPageNo = 1;
                    //self.GetLeadContactsList(currentPageNo);

                    if (self.LeadContactsList().length == 0 && CurrentLeadContactPage != 1) {
                        self.GetLeadContactsList(CurrentLeadContactPage - 1);
                    }
                    else {
                        if (CurrentLeadContactPage == 1) {
                            self.GetLeadContactsList(1);
                        }
                        else {
                            self.GetLeadContactsList(CurrentLeadContactPage);
                        }

                    }


                }
                else {
                    ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Contact.ContactDeletedFailure, true);
                }
            });
        }

    }
    //======================================================End Contacts Section==============================================================

    //=====================================================Lead Comments Section============================================================
    self.GetLeadCommentsList = function (pageNo) {
        CurrentLeadCommentsPage = pageNo;
        var LeadInfo = new Object();
        LeadInfo.leadId_encrypted = self.GetSelectedLeadId();
        LeadInfo.CurrentPageNo = pageNo;
        CurrentLeadCommentsPage = pageNo;
        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetLeadComments", LeadInfo, function onSuccess(response) {
            self.RenderLeadComments(response);
        }, function onError(err) {
            ErucaCRM.Framework.Core.ShowMessage(err.Message, true);
        });

    };

    self.AddCommentInLead = function () {
        var comment = $('#Commenttxt').val().trim();
        if (comment == undefined || comment == "") {
            alert(ErucaCRM.Messages.Lead.CommentRequired);
            return;
        }
        var LeadInfo = new Object();
        LeadInfo.leadId = self.GetSelectedLeadId();
        LeadInfo.comment = comment;
        ErucaCRM.Framework.Core.doPostOperation(controllerUrl + "AddLeadComment", LeadInfo, function onSuccess(response) {
            self.LeadCommentsList.unshift(new ErucaCRM.User.Leads.LeadCommentsQueryModel(response.LeadComment));

            $('#Commenttxt').val("");
        }, function onError(err) {
            ErucaCRM.Framework.Core.ShowMessage(err.Message, true);
        });
    };

    self.RenderLeadComments = function (LeadComments) {

        $("#divNoRecord").hide();
        self.LeadCommentsList.removeAll();
        if (LeadComments.LeadComment.length == 0) {
            $("#divNoRecord").html("<br/><b>" + ErucaCRM.Messages.Account.NoAccountActivityRecordFound + "</b>");
            $("#divNoRecord").show();

        }
        ko.utils.arrayForEach(LeadComments.LeadComment, function (leadComment) {
            self.LeadCommentsList.unshift(new ErucaCRM.User.Leads.LeadCommentsQueryModel(leadComment));
        });
        $("#LeadCommentsPager").html(self.GetPaging(LeadComments.TotalRecords, CurrentLeadCommentsPage, LeadCommentsMethodName));
        if (self.LeadCommentsList().length == 0) { self.ShowNoLeadCommentFoundMsg(Visibility.visible) }
        else { self.ShowNoLeadCommentFoundMsg(Visibility.hidden) }
    };

    //======================================================End Comments Section==============================================================

    //=====================================================Lead Documents Section============================================================
    self.AddNewLeadDocument = function () {
        ErucaCRM.Framework.Core.OpenRolePopup("._LeadFileUploadSection");
    }
    self.setPostData = function () {
        var obj = new Object;
        obj.LeadId = self.GetSelectedLeadId();
        return obj;
    }
    var uploadObj;
    self.InitializeLeadFileUploader = function () {
        uploadObj = $("#fileuploader").uploadFile({
            url: '/User/UploadLeadDocument',
            multiple: true,
            autoSubmit: false,
            fileName: "docs",
            maxFileCount: 5,
            maxFileSize: 1024 * 10000,
            onSelect: function (files) {
                return true; //to allow file submission.
            },
            dynamicFormData: function () {
                var data = self.setPostData();
                return data;
            },
            showStatusAfterSuccess: false,
            multiDragErrorStr: "Multiple File Drag &amp; Drop is not allowed.",
            extErrorStr: "is not allowed. Allowed extensions: ",
            onSubmit: function (files) {
            },
            onSuccess: function (files, data, xhr) {

                ErucaCRM.Framework.Core.ShowMessage(data.response.Message, false);
                //var docObject = new Object();
                //docObject.DocumentId = data.DocId;
                //docObject.DocumentName = data.FileName;
                //docObject.DocumentPath = data.FilePath;
                //docObject.AttachedBy = data.AttachedBy;
                //self.LeadDocumentsList.unshift(ErucaCRM.User.Leads.LeadDocumentsQueryModel(docObject));
                $('#FileUploadSection').fadeOut();
                self.ShowNoLeadDocFoundMsg(Visibility.hidden)
            },
            afterUploadAll: function () {
                $('._LeadFileUploadSection').fadeOut("slow");
                self.GetLeadDocumentsList(1);
            },
            onError: function (files, status, errMsg) {

            }
        });
    }

    $('._close').click(function () {

        uploadObj.cancelAll();
        uploadObj.errorLog.empty();

    });
    self.AttachLeadDocument = function () {
        if (uploadObj != null && uploadObj.selectedFiles > 0) {
            uploadObj.startUpload();
            uploadObj.selectedFiles = 0;

        }
        else {
            alert(ErucaCRM.Messages.Lead.PleaseSelectFile);
            return;
        }
    }
    self.LeadDocumentMouserEnterHandler = function (data) {
        data.ShowDocRemoveLink('block');
    },
   self.LeadDocumentMouserOutHandler = function (data) {
       data.ShowDocRemoveLink('none');
   }
    self.GetLeadDocumentsList = function (pageNo) {

        var leadId = self.GetSelectedLeadId();
        var LeadInfo = new Object();
        CurrentLeadDocumentPage = pageNo;
        LeadInfo.leadId_encrypted = leadId;
        LeadInfo.CurrentPageNo = pageNo;
        CurrentLeadContactPage = pageNo;
        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetLeadDocuments", LeadInfo, function onSuccess(response) {
            self.RenderLeadDocuments(response);


        }, function onError(err) {
            ErucaCRM.Framework.Core.ShowMessage(err.Message, true);
        });

    }
    self.RenderLeadDocuments = function (LeadDocuments) {
        $("#divNoRecord").hide();
        self.LeadDocumentsList.removeAll();

        if (LeadDocuments.LeadDocuments.length == 0) {
            $("#divNoRecord").html("<br/><b>" + ErucaCRM.Messages.Account.NoAccountActivityRecordFound + "</b>");
            $("#divNoRecord").show();

        }
        ko.utils.arrayForEach(LeadDocuments.LeadDocuments, function (leadDocument) {
            leadDocument.AttachedBy = leadDocument.UserName;
            self.LeadDocumentsList.push(new ErucaCRM.User.Leads.LeadDocumentsQueryModel(leadDocument));
        });
        ErucaCRM.Framework.Common.ApplyPermission('._roleselector');
        $("#LeadDocumentsPager").html(self.GetPaging(LeadDocuments.TotalRecords, CurrentLeadDocumentPage, LeadDocumentsMethodName));
        if (self.LeadDocumentsList().length == 0) { self.ShowNoLeadDocFoundMsg(Visibility.visible) }
        else { self.ShowNoLeadDocFoundMsg(Visibility.hidden) }
    };
    self.RemoveDocument = function (document) {
        var response = confirm(ErucaCRM.Messages.Document.ConfirmDocumentDeleteAction);
        if (!response)
            return false;
        var actionName = "RemoveLeadDocument";
        var obj = new Object();
        obj.DocumentId = document.DocumentId;
        ErucaCRM.Framework.Core.doPostOperation
               (
                   controllerUrl + actionName,
                   obj,
                   function onSuccess(response) {
                       if (response.Status == Message.Failure) { ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Document.DocumentNotRemoved, true); }
                       else if (response.Status == Message.Success) {
                           self.LeadDocumentsList.remove(document);
                           ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Document.DocumentRemoved, false);
                           //self.GetLeadDocumentsList(1);
                           self.LeadDocumentsList.remove(document);
                           if (self.LeadDocumentsList().length == 0) {
                               self.GetLeadDocumentsList(CurrentLeadDocumentPage - 1);
                           }
                           else {
                               if (CurrentLeadDocumentPage == 1) {
                                   self.GetLeadDocumentsList(1);
                               }
                               else {
                                   self.GetLeadDocumentsList(CurrentLeadDocumentPage);
                               }
                           }
                       }
                   },
                   function onError(err) {
                       self.status(err.Message);
                   }
               );

    }
    //=====================================================End Documents Section============================================================
    //================Section  Add Contact Added By Mahesh Bhatt===========================//

    self.AssociateContact = function () {

        var listObjContact = new Array();


        $('._contact').each(function (i) {
            var contact = new Object();
            if ($(this).is(':checked')) {
                contact.LeadId = $('#LeadId').val();
                contact.ContactId = $(this).attr("ContactId");
                listObjContact.push(contact);
            }
        })

        if (listObjContact.length > 0) {
            ErucaCRM.Framework.Core.doPostOperation(controllerUrl + "AssociateLeadContact", listObjContact, function onSuccess(response) {
                self.GetLeadContactsList(1);

                //$.each(listObjContact, function (key, value) {
                //    var postData = new Object();

                //    var $contactRow = $('[id="row_' + value.ContactId + '"]')
                //    postData.ContactId = value.ContactId;
                //    postData.FirstName = $.trim($contactRow.find("td:eq(1)").text());
                //    postData.EmailAddress = $.trim($contactRow.find("td:eq(3)").text());
                //    postData.Phone = $.trim($contactRow.find("td:eq(2)").text());
                //    self.LeadContactsList.push(new ErucaCRM.User.Leads.LeadContactsQueryModel(postData));
                //})
                $('._NoContactFound').parent().parent().hide();
                $('#AddContactSection ._close').click();
            }, function onError(err) {
                self.status(err.Message);
            });

        }
        else {
            alert(ErucaCRM.Messages.Account.SelectOneOrMoreOption)
        }

    }



    self.SaveNewContact = function (ProductId) {
        var postData = new Object();
        postData.ContactName = $('#ContactName').val();
        postData.EmailAddress = $('#ContactEmail').val();
        if (postData.ContactName == "" || postData.EmailAddress == "") {
            alert(ErucaCRM.Messages.Product.AllFieldRequierd)
            return false;
        }
        var email = postData.EmailAddress;
        var filter = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        if (!filter.test(email)) {
            alert(ErucaCRM.Messages.Lead.InvalidEmailAddress)
            return false;
        }

        self.Core.doPostOperation(controllerUrl + "SaveNewContact", postData, function onSuccess(response) {
            self.Core.ShowMessage(ErucaCRM.Messages.Account.ContactAddedSuccess, false);

            postData.ContactId = response.ContactId;
            self.GetPageDataForContact(1);
            //self.ContactList.push(new ErucaCRM.User.Leads.ContactListQueryModel(postData));
            $("#addNewContactSection ._close").click();
        });
    }



    ErucaCRM.User.Leads.ContactListQueryModel = function (data) {

        var self = this;
        self.RowId = "row_" + data.ContactId;
        self.ContactId = data.ContactId;
        self.ContactName = data.ContactName;
        self.ContactEmail = data.EmailAddress;
        self.PhoneNo = data.Phone;

        self.LeadId = $('#LeadId').val();
    }
    self.AddContact = function () {

        var objContactInfo = new Object();
        ContactCurrentPage = 1;
        objContactInfo.FilterBy = "LeadContacts"
        objContactInfo.LeadId = $('#LeadId').val();
        objContactInfo.CurrentPageNo = 1 //ContactCurrentPage;

        $("#btnAddNewContact").bind("click", function () {
            $("#ContactName").val("");
            $("#ContactEmail").val("");
            ErucaCRM.Framework.Core.OpenRolePopup("#addNewContactSection");
        });


        ErucaCRM.Framework.Core.OpenRolePopup("._AddContactSection");
        self.GetContactList(objContactInfo);
        self.GetPageDataForContact(1);



    }
    self.GetPageDataForContact = function (CurrentPageNo) {

        var objContactInfo = new Object();
        ContactCurrentPage = CurrentPageNo;
        objContactInfo.FilterBy = "LeadContacts"
        objContactInfo.LeadId = $('#LeadId').val();
        objContactInfo.CurrentPageNo = ContactCurrentPage;
        self.GetContactList(objContactInfo);
    }
    self.RenderContacts = function (ContactItemListData) {
        self.ContactList.removeAll();
        $("#divNoRecord").hide();
        $("#TaskItemListData").children().remove();
        if (ContactItemListData.ListContacts.length == 0) {
            $("#divNoRecord").html("<br/><b>" + ErucaCRM.Messages.Account.NoAccountActivityRecordFound + "</b>");
            $("#divNoRecord").show();

        }

        ko.utils.arrayForEach(ContactItemListData.ListContacts, function (TaskItem) {
            self.ContactList.push(new ErucaCRM.User.Leads.ContactListQueryModel(TaskItem));
        });

        $("#AllContactPager").html(self.GetPaging(ContactItemListData.TotalRecords, ContactCurrentPage, "viewModel.GetPageDataForContact"));

    };

    self.GetContactList = function (objContactInfo) {
        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "NonAssociatedContactList", objContactInfo, function onSuccess(response) {

            self.RenderContacts(response);

            ErucaCRM.Framework.Common.ApplyPermission('._roleselector');

        }, function onError(err) {
            ErucaCRM.Framework.Core.ShowMessage(err.Message, true);
        });


    }

    //=====================================================================================//





    //===========================================Tag Leads==================================//

    $("#TagIds").val("");
    CurrentTagObject = new Object();
    CurrentTagObject.TagId = "";
    CurrentTagObject.TagName = "";
    self.Common = ErucaCRM.Framework.Common;
    TagList = new Array();
    $("#Tag").bind("keydown", function (event) {
        $("#CurrentTagId").val("");
        $("#CurrentTagName").val("");
        SearchTagName = "";
        if (event.keyCode === $.ui.keyCode.TAB &&
        $(this).data("ui-autocomplete").menu.active) {
            event.preventDefault();
        }
    }).autocomplete({
        appendTo: '#LeadDetail',
        //open: function () { $('#LeadDetail .ui-menu').height(10); $('#LeadDetail .ui-menu').css('overflow', 'scroll') },
        open: function () { $('#LeadDetail .ui-menu').height(10); $('#LeadDetail .ui-menu').css({ 'overflow': 'scroll', 'width': '203px' }) }, minLength: 0,
        source: function (request, response) {

            //self.GetSearchTags(request.term);
            // delegate back to autocomplete, but extract the last term

            self.GetSearchTags(request, response)
        },
        focus: function () {
            // prevent value inserted on focus
            return false;
        },
        select: function (event, ui) {

            var terms = self.Common.Split(this.value);

            $("#CurrentTagId").val(ui.item.value);
            $("#CurrentTagName").val(ui.item.label)

            var CurrentTagId = $("#CurrentTagId").val();
            var CurrentTagName = $("#CurrentTagName").val();


            var objTag = new Object();

            if (ui.item.label == "") {

                objTag.TagId = "0";
                objTag.TagName = SearchTagName;

            }
            else {
                objTag.TagId = CurrentTagId;
                objTag.TagName = CurrentTagName;
            }
            CurrentTagObject.TagId = "";
            CurrentTagObject.TagName = "";

            TagList.push(objTag);


            // remove the current input
            terms.pop();
            // add the selected item
            terms.push(ui.item.label);
            // add placeholder to get the comma-and-space at the end
            terms.push("");
            this.value = terms.join(",");
            // alert($("#TagIds").val());
            return false;
        }
    }).css("position", "relative !impotant");

    self.GetTaggedLeads = function (tagObj) {
        $('#TextBoxSearchbyTag').val(tagObj.TagName);
        // self.FilterLeadTag(tagObj.TagName);
        $('#LeadTags').show();

    }

    self.GetSearchTags = function (request, response) {

        SearchTagName = jQuery.trim(request.term.substring(request.term.lastIndexOf(',') + 1));

        var taglist = jQuery.trim($("#Tag").val()).split(',');

        //if user find the exact match in and did not selected it and enter the comma to start entering other tag
        //then first loop the through the taglist contain selected tag through autocomplete if did not find the match
        // then llo into CurrentTagObject which contain last exact match user selected or not.


        if (SearchTagName == "") {

            var lastTagName = taglist[taglist.length - 2];

            var lastTagSelecetd = false

            if (lastTagName != undefined && CurrentTagObject.TagName.toLowerCase() == lastTagName.toLowerCase()) {

                for (var j = 0; j < TagList.length; j++) {
                    if (TagList[j].TagName.toLowerCase() == lastTagName.toLowerCase()) {

                        lastTagSelecetd = true;
                        break;
                    }
                }

                if (lastTagSelecetd == false) {

                    var objTag = new Object();



                    objTag.TagId = CurrentTagObject.TagId;
                    objTag.TagName = CurrentTagObject.TagName;

                    TagList.push(objTag);

                }

            }


        }

        ErucaCRM.Framework.Core.getJSONData(controllerUrl + "GetSearchTagList/?searchText=" + SearchTagName, function onSuccess(data) {

            //if exact match found then caputre the tag info
            var objFilteredList = $.ui.autocomplete.filter(
                      data.ListTags, self.Common.ExtractLast(request.term));

            if (objFilteredList.length == 1) {

                CurrentTagObject.TagId = objFilteredList[0].value;
                CurrentTagObject.TagName = objFilteredList[0].label;



            }

            response(objFilteredList);
            return;

        }, function onError(err) {
            self.status(err.Message);
        });
    }


    self.AddTagToLead = function () {
        if ($("#Tag").val() == "") {
            $("#Tag").focus();
            alert(ErucaCRM.Messages.Lead.TagRequired);

            return false;
        }

        var AllTagList = jQuery.trim($("#Tag").val()).split(',');

        var AddedTags = new Array();

        var html = "";
        var tagId = "0";
        var tagName = "";
        var tagAlreadyExist = false;
        for (var i = 0; i < AllTagList.length; i++) {

            tagId = "0";
            tagName = "";
            tagAlreadyExist = false;

            tagName = jQuery.trim(AllTagList[i]);
            if (tagName != "") {
                for (var j = 0; j < TagList.length; j++) {
                    if (TagList[j].TagName.toLowerCase() == tagName.toLowerCase()) {
                        tagId = TagList[j].TagId;
                        break;
                    }

                }

                for (var k = 0; k < AddedTags.length; k++) {
                    if (tagName.toLowerCase() == AddedTags[k])
                        tagAlreadyExist = true;
                }

                $(".tagmain").children('div').each(function () {
                    if ($(this).attr("tagName").toLowerCase() == tagName.toLowerCase()) {
                        tagAlreadyExist = true;

                    }

                });



                //if for last tag autocomplete find the exact match but user did not select it and did not appened the comma then capture last entered tag's id in the system

                if (tagAlreadyExist == false) {
                    if (tagId == "0" && tagName.toLowerCase() == CurrentTagObject.TagName.toLowerCase() && CurrentTagObject.TagId != "") {
                        tagId = CurrentTagObject.TagId;
                    }
                    var obj = new Object();
                    obj.TagId = tagId;
                    obj.TagName = tagName;
                    self.OnTagList.push(new ErucaCRM.User.Leads.OnloadRenderTagsQueryModel(obj));

                    //html = html + " <div class='tag tagwidth _tag' tagId='" + tagId + "' tagName='" + tagName + "'><div><div class='tagname'> <a target='_blank' href='/User/TagDetail/" + tagId + "'> <span>" + tagName + "</span></a><a onclick='viewModel.RemoveTag(this);'>&nbsp;&nbsp;X</a></div></div></div>";


                    AddedTags.push(tagName.toLowerCase());
                }
            }

        }

        $(".tagmain").append(html);
        $("#Tag").val("");
        TagList = new Array();

        return false;
    }


    self.RemoveTag = function (tag) {
        if (confirm(ErucaCRM.Messages.Lead.TagRemoveConfirmAction)) {
            $(tag).parent('div').parent('div').parent('div').remove();
        }
        return false;

    }

    self.getTagList = function () {
        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetLeadTags", null, function onSuccess(response) {

            self.RenderTagList(response);
            ErucaCRM.Framework.Common.ApplyPermission('._roleselector');
            //  $('._resetUrl').click();
        }, function onError(err) {
            self.status(err.Message);
        });
        //  $('._resetUrl').click();
    }

    self.RenderTagList = function (TagListData) {
        self.TagList.removeAll();
        ko.utils.arrayForEach(TagListData.listTags, function (Tag) {
            self.TagList.push(new ErucaCRM.User.Leads.TagListQueryModel(Tag));
        });

    };

    self.CancelLeadDetailPopUp = function () {
        $('#Tag').val(''); $('._leadDetail').hide(); $('._resetUrl').click();
    }

    self.SaveTagToLead = function () {
        //Keep pop active when validation error occur 
        var titlename = $("#Title").val();
        var amountvalid = $(".amount").children(".field-validation-error");
        if (titlename.length > 0 && amountvalid.length == 0) {
            var TagIds = "";
            var NewTagNames = "";
            $("#Tag").val("");
            $(".tagmain").children('div').each(function () {


                if (parseInt($(this).attr("tagId")) != "" && $(this).attr("tagName") != "") {
                    if (TagIds == "")
                        TagIds = TagIds + $(this).attr("tagId");
                    else
                        TagIds = TagIds + "," + $(this).attr("tagId");
                }
                else if (parseInt($(this).attr("tagId")) == 0) {

                    if (NewTagNames == "")
                        NewTagNames = NewTagNames + $(this).attr("tagName");
                    else
                        NewTagNames = NewTagNames + "," + $(this).attr("tagName");
                }


            });


            $("#LeadTagIds").val(TagIds);
            $("#NewTagNames").val(NewTagNames);
            //self.getTagList();
            $('._resetUrl').click();
        }

    }

    $('#Searchtagbutton').click(function (e) {
        $("#tagsearch").toggle();
        $("#Filterpopup2").hide();
        $("#Filterpopup1").show();
        e.stopPropagation();
        e.preventDefault();

    });

    //if ($('#divTagSearch:visible').size() > 0) {
    //    $("#Filterpopup1").hide();
    //}
    //else {
    //    $("._popup").hide();
    //    $("#Filterpopup1").show();
    //}

    $('#TextBoxSearchbyLeadName').keypress(function (event) {

        if (event.keyCode == 13) {
            self.GetSearchLeads();
        }
    });

    $('#Tag').keypress(function (event) {

        if (event.keyCode == 13) {
            self.AddTagToLead();
            event.preventDefault();
        }
    });
    self.GetSearchLeads = function () {
        if ($("#TextBoxSearchbyLeadName").val() == "" && $('#TextBoxSearchbyTag').val() == "") {

            alert(ErucaCRM.Messages.Lead.SearchTagRequired)
            return false;

        }
        ko.utils.arrayFilter(self.LeadList(), function (item) {
            currentPage = 1;
            TagSearchName = "";
            var objLeadInfo = new Object();
            objLeadInfo.tagName = $('#TextBoxSearchbyTag').val();
            objLeadInfo.stageId_encrypted = item.StageId;
            objLeadInfo.IsLoadMore = false;
            objLeadInfo.SearchLeadName = $("#TextBoxSearchbyLeadName").val();

            objLeadInfo.CurrentPage = currentPage;
            self.GetAllLeadTags(objLeadInfo);
        });
        $('#divFilterInfo').attr({ 'tagname': $('#TextBoxSearchbyTag').val(), 'SearchLeadName': $("#TextBoxSearchbyLeadName").val() });
        // var htm = $('#TextBoxSearchbyTag').val();
        // htm += $("#TextBoxSearchbyLeadName").val() != "" ? "," + $("#TextBoxSearchbyLeadName").val() : '';
        // $("#spanCurrentTagName").html(htm);
        //$("#spanCurrentTagName").html($("#TextBoxSearchbyLeadName").val());
        //$("#TextBoxSearchbyLeadName").val("");
        $("#divTagSearch").hide();
        $('#LeadTags').hide();
        //$("#divFilterInfo").show();
        self.DisplayLeadFilterInfo();
    };
    self.DisplayLeadFilterInfo = function () {
        self.FilterLeadTitle($("#TextBoxSearchbyLeadName").val());
        self.FilterLeadTag($('#TextBoxSearchbyTag').val());
        if (self.FilterLeadTitle() != '' || self.FilterLeadTag() != '') {
            self.ShowLeadFilterInfo(true);
            if (self.FilterLeadTitle() != '') { self.FilterByLeadTitle(true); }
            else { self.FilterByLeadTitle(false); }
            if (self.FilterLeadTag() != '') { self.FilterByLeadTag(true); }
            else { self.FilterByLeadTag(false); }
            if (self.FilterLeadTitle() != '' && self.FilterLeadTag() != '') { self.FilterByTitleAndTag(true); }
            else { self.FilterByTitleAndTag(false); }
        }
        else {
            self.ShowLeadFilterInfo(false);
            $('._addNewLeadSection').show();
        }

    }



    $('#ClearTagFilter').click(function () { self.ClearTagFilter(); })


    self.ClearTagFilter = function () {
        self.FilterLeadTag('');
        self.FilterLeadTitle('');
        self.DisplayLeadFilterInfo();
        $("#divFilterInfo").removeAttr('tagname').removeAttr('searchleadname');
        self.GetLeadsbyStageId();
        return false;
    }

    $('#ShowLeadTags').click(function () {
        self.getTagList();
        $('#LeadTags').toggle();
    });


    self.GetAllLeadTags = function (objLeadInfo) {
        var loderobj = undefined;
        $('._stageloader').each(function () {
            if ($(this).attr('loader_id') == objLeadInfo.stageId_encrypted) {
                loderobj = this;
                $(this).show();
            }
        })


        ErucaCRM.Framework.Core.getJSONDataBySearchParam
                     (
                     controllerUrl + "GetLeadsByStageId",
                      objLeadInfo,
                          function onSuccess(response) {
                              var pagesize = ErucaCRM.Framework.Core.Config.PageSize;
                              ko.utils.arrayFilter(self.LeadList(), function (item) {
                                  if (item.StageId == objLeadInfo.stageId_encrypted) {
                                      if (response.List.length > pagesize - 1) {
                                          $('._leadsContainer[stageid="' + item.StageId + '"]').find('._loadmorecontainer').css('display', 'block');
                                      } else { $('._leadsContainer[stageid="' + item.StageId + '"]').find('._loadmorecontainer').css('display', 'none'); }
                                      item.Leads.removeAll();
                                      $.each(response.List, function (index, value) {
                                          item.Leads.push(new ErucaCRM.User.Leads.LeadListDetailQueryModel(value));
                                      });
                                  }
                              });
                              $(loderobj).hide();
                              self.MakeElementDraggable();
                          }, function onError(err) {
                              ErucaCRM.Framework.Core.ShowMessage(err.Message, true);
                          }

                          );

    }


    self.HideTagFilterMenu = function () {
        $("#TextBoxSearchbyLeadName").val("");
        $("#TextBoxSearchbyTag").val("");
        $("#divTagSearch").hide();
        $("#LeadTags").hide();
    }
    //======================================================================================//


    //self.getTagList();


    self.InitializeTabControl();
    self.InitializeLeadFileUploader();
    if (/chrom(e|ium)/.test(navigator.userAgent.toLowerCase())) {
        $("#LeadTags").css({ left: "252px" });
    }

    self.hideMenu = function () {
        $("#tagsearch").hide();
    }
    self.showFilterLeadsmenu = function () {
        self.getTagList();
        $('#Filterpopup1').hide();
        $('#Filterpopup2').show();

    }
    self.hideFilterMenu = function () {
        $('#Filterpopup2').hide();
        $('#Filterpopup1').show();

    }
    self.ClearFields = function () {
        $('#TextBoxSearchbyLeadName').val("");
        $('#TextBoxSearchbyTag').val("");
        self.FilterLeadTag('');
        self.ClearTagFilter();
    }
}







