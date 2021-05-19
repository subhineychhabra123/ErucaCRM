jQuery.namespace('ErucaCRM.User.ViewAllRecentActivity');
ErucaCRM.User.ViewAllRecentActivity.pageLoad = function () {

    currentPage = 1;
    viewHomeModel = new ErucaCRM.User.ViewAllRecentActivity.pageViewModel();
    ko.applyBindings(viewModel, document.getElementById("InnerContentContainer"));

}


ErucaCRM.User.ViewAllRecentActivity.HomeRecentActivitiesQueryModel = function (data) {

    var self = this;
    self.ActivityText = data.ActivityText;
    self.LeadAuditId = data.LeadAuditId;
    self.ActivityCreatedTime = data.ActivityCreatedTime;//== null ? null : dateFormat(eval(data.CreatedDate.replace(/\/Date\((\d+)\)\//gi, "new Date($1)")), "HCisoUtcDateTime");
    self.OwnerDetail = "/User/UserProfile/" + data.CreatedBy + "?returnurl=" + window.location.href;
    self.ImageURL = "/Uploads/users/" + data.ImageURL;//: "";
    self.DetailUrl = "/User/AccountCaseDetail/" + data.AccountCaseId + "?returnurl=" + window.location.href;
    self.Color = "Black";
    if (data.IsDisplay == false) {
     
        self.Color = "#74787e";
    }
}



ErucaCRM.User.ViewAllRecentActivity.pageViewModel = function (data) {
    var self = this;
    var controllerUrl = "/User/";  
    self.RecentActivityList = ko.observableArray([]);  

  



    //==================================Fetch Recent Activities =============
    self.GetRecentActivities = function (obj, ShowMessage) {

        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "AllRecentActivity", obj,
            function onSuccess(response) {
              
                if (response.Response == "Success") {
                    
                    var pagesize = ErucaCRM.Framework.Core.Config.PageSize;
                    if (obj.LeadAuditId_encrypted == null && response.RecentActivities.length < pagesize)
                        $('._loadmore').css('display', 'none');
                    else
                        $('._loadmore').css('display', 'inline');
                    self.RenderRecentActivities(response.RecentActivities, obj);
                }
            }, function onError(err) {
                ErucaCRM.Framework.Core.ShowMessage(err.Message, true);
            }, function () {

                if (ShowMessage) {
                    ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Framework.Core.Config.ajaxProcessingText, true);
                }
                /*Before send Nothing to do*/
            }, function () {
                if (ShowMessage) {
                    errmessage = ErucaCRM.Framework.Core.Config.ajaxProcessedText;
                    ErucaCRM.Framework.Core.ShowMessage(errmessage, false);
                    setTimeout('$("._messagediv").css({ "height": "25px" }).slideUp("slow",function(){$(this).remove()})', 2000);
                }
                /*On complete Nothing to do*/
            }
        );
    }
    self.RenderRecentActivities = function (RecentActivityListData, obj) {
        //self.RecentActivityList.removeAll();
   
        $("#divNoRecentActivtiyFound").hide();
        if (RecentActivityListData.length == 0 && obj.LeadAuditId_encrypted == null && obj.IsLoadMore == false) {
            $("#divNoRecentActivtiyFound").html("<br/><b>" + ErucaCRM.Messages.DashBoard.NoRecentActFound + "</b>");
            $("#divNoRecentActivtiyFound").show();
        }
        ko.utils.arrayForEach(RecentActivityListData, function (RecentActivity) {
            if (obj.LeadAuditId_encrypted == null)
                self.RecentActivityList.push(new ErucaCRM.User.ViewAllRecentActivity.HomeRecentActivitiesQueryModel(RecentActivity));
            else if (obj.LeadAuditId_encrypted != null && obj.IsLoadMore == false) {
                self.RecentActivityList.unshift(new ErucaCRM.User.ViewAllRecentActivity.HomeRecentActivitiesQueryModel(RecentActivity));
            }
            else if (obj.LeadAuditId_encrypted != null && obj.IsLoadMore == true) {
                $("#divNoRecentActivtiyFound").hide();
                self.RecentActivityList.push(new ErucaCRM.User.ViewAllRecentActivity.HomeRecentActivitiesQueryModel(RecentActivity));
            }
        });

    };


    $('._loadmore').click(function () {
        lastLeadId = $('._left-section').last().children('._right-img').attr('leadauditid');
        var obj = new Object();
        obj.CurrentPage = 1;
        obj.LeadAuditId_encrypted = lastLeadId;
        obj.IsLoadMore = true;
        self.GetRecentActivities(obj, true);
    });


    //=========================================================================

    var obj = new Object();
    obj.CurrentPage = 1;
    obj.LeadAuditId_encrypted = null;
    obj.IsLoadMore = false;
    self.GetRecentActivities(obj);
    //==============================New Dashboard Charts Data===============//


}