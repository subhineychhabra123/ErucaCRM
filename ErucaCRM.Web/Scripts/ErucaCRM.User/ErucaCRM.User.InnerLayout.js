//Register name space
jQuery.namespace('ErucaCRM.User.InnerLayout');
var controllerUrl = "/User/";
var updatenotification = false;
var maxLeadAuditId = 0;
var lastLeadId;
var viewModel;
//var chat;

ErucaCRM.User.InnerLayout.pageLoad = function () {
    var obj = new Object();
    obj.updateNotification = false;
    obj.maxLeadAuditId_encrypted = maxLeadAuditId;
    ErucaCRM.User.InnerLayout.GetNotification(obj);
    var objall = new Object();
    objall.CurrentPage = 1;
    objall.LeadAuditId_encrypted = null;
    objall.IsLoadMore = false;
    ErucaCRM.User.InnerLayout.GetAllRecentActivities(objall)
    $.connection.hub.logging = true;
    chat = $.connection.realTimeNotificationHub;
    $.connection.hub.start();
    //.done(function () {
    //    chat.server.callerNotification(0, false);
    //});

    chat.client.newNotification = function (response) {
        var arraylist = [];
        var numberofcount = $("._notify-tip").text() == "" ? 0 : $("._notify-tip").text();
        if (ErucaCRM.Framework.Common.GetCookie("MaxLeadAuditId") != response.MaxLeadAuditID) {
            $("._notify-tip").text(parseInt(numberofcount) + 1);
        }
        if (numberofcount == 0 && response == null) {
            $("._icon-globe").hide();
        }
        else {
            if ($("._notify-tip").text() != "0") {
                $("._icon-globe").show();
                $.each(response.RecentActivities, function (index, RecentActivity) {
                    var OwnerDetail = "/User/UserProfile/" + RecentActivity.CreatedBy + "?returnurl=" + window.location.href;
                    //var ImageURL = "/Uploads/users/" + RecentActivity.ImageURL;//: "";           
                    var ImageURL = RecentActivity.ImageURL;//: "";            
                    var error = "this.onerror=null;this.src='/Uploads/users/no_image.gif'";

                    arraylist.push("<div id='popupboxes' class='left-section _left-sectioninner'>" + "<a class='popclose _popclose' onclick='javascript:closeNotification(this)' style='width:8px' href='javascript:void(0)'><img src='/Content/images/cross-arrow.png'></a>" +
                                 "<div class='recentactivity-left-img '>" +
                                     "<a href='" + OwnerDetail + "' class='permissionbased' data-showalways='True' data-permission='UserVe'>" +
                                      "<img style='width: 30px;' src='" + ImageURL + "' alt=''  onerror=" + error + "></a>" +
                                    "</div>" +
                                     "<div class='right-img right-text_Notification _right-img' LeadAuditId='" + RecentActivity.LeadAuditId + "'>" +
                                     "<p class='activity-text'>" + RecentActivity.ActivityText + "</p>" +
                                     "<span class='pull-right'>" + RecentActivity.ActivityCreatedTime + "</span></div></div>");
                });
                $("#popcontainer").prepend(arraylist[0]);
                if (ErucaCRM.User.Leads && ErucaCRM.User.Leads.autoRefreshLeads) {
                    ErucaCRM.User.Leads.autoRefreshLeads(response);
                }
                ErucaCRM.Framework.Common.ApplyPermission();
            }
            maxLeadAuditId = response.MaxLeadAuditID;
            ErucaCRM.User.DeleteCookie("MaxLeadAuditId");
            ErucaCRM.Framework.Common.SetCookie("MaxLeadAuditId", maxLeadAuditId);
            updatenotification = true;
            ErucaCRM.User.InnerLayout.RenderAllRecentNotification(response.RecentActivities, true);
        }
    };
    //var hash = window.location.hash;
    //if (hash == "#RecentActivity") {
    //    ErucaCRM.Framework.Core.OpenRolePopup("#viewAllInnerRecentActivites");
    //}

    //setInterval(function () {
    //    var obj = new Object();
    //    obj.updateNotification = false;
    //    updatenotification = true;   
    //    obj.maxLeadAuditId_encrypted = 0;
    //    ErucaCRM.User.InnerLayout.GetNotification(obj);
    //}, 60000);

    $("._btn-group").click(function (e) {
        e.stopPropagation();
        $("#NotificationList").toggle();
        if ($("._notify-tip").text() != "0" && $("._notify-tip").text().length > 0) {
            var obj = new Object();
            obj.updateNotification = true;
            updatenotification = true;
            obj.maxLeadAuditId_encrypted = ErucaCRM.Framework.Common.GetCookie("MaxLeadAuditId");
            ErucaCRM.User.InnerLayout.GetNotification(obj);
        }
    })
    $("._usrpic").click(function (e) {
        e.stopPropagation();
        $("._divsubmenu").hide();
        $("._divmainmenu").hide();
        $("#divTagSearch").hide();
        $("#divaccount").toggle();

    });
    $("._helpclick").click(function () {
        var tipsData = ErucaCRM.Helps[helpviewModel.HelpPage].Tips;
        var helpScope = $(this).data("helpscope");
        helpScope = helpScope == undefined ? "" : "#" + helpScope;
        helpviewModel.HelpList.removeAll();
        $.each(tipsData, function (index, data) {
            $.each(data, function (key, value) {
                var keydata = key.toLowerCase();
                var helpkey = $(helpScope + " [data-helpkey='" + keydata + "']:visible:not(:has(permissionbased))").attr('data-helpkey');
                if (helpkey == key.toLowerCase()) {
                    helpviewModel.HelpList.push(new ErucaCRM.Framework.Common.HelpQueryModel(key, value));
                }
            })
        })
        if ($("#HelpTips div").length > 1) {
            ErucaCRM.Framework.Common.OpenPopup("#Help");
            $("._helpClose").show();
            $("#Help").css({ "position": "fixed", "height": "100%", "width": "100%", "opacity": ".25", "background": "#000" });
            $("#HelpTips").show();
            $.each($("[data-helpkey]"), function () {
                var $this = $(this);
                var helpkey = $this.attr("data-helpkey");
                $this = $(helpScope + " [data-helpkey='" + helpkey + "']:visible:first");
                switch ($this.attr("data-helppos")) {
                    case "top-left":
                        var top = $this.offset().top;
                        var left = $this.offset().left + $this.width();
                        $("#" + $this.attr("data-helpkey")).css({ "top": top - 5 + "px", "left": left + "px", "position": "absolute" });
                        //$("._helplk").css({ "top": top - 5  + "px", "left": left + "px", "position": "absolute" });
                        break;
                    case "top-right":
                        var top = $this.offset().top-5;
                        var right = ($(window).width() - $this.offset().left)+10;
                        $("#" + $this.attr("data-helpkey")).css({ "top": top + "px", "right": right + "px", "position": "absolute" });

                        $("#" + $this.attr("data-helpkey") + " div._arrow") .removeClass("userarrwl").addClass("userarrwR");
                        break;
                    //default:
                    //    var top = $this.offset().top;
                    //    var left = $this.offset().left;
                    //    $("#" + $this.attr("data-helpkey")).css({ "top": top - 5 + "px", "left": left + "px", "position": "absolute" });
                    //    break;
                }
            });
        }
    });

    $("#Help").click(function () {
        $("._helpClose").fadeOut();
    });
    $("._left-leads ul li a").click(function () {
        $('#mainsidemenus li[class=selected]').removeClass('selected');
        $(this).parent('li').addClass('selected');
    });

    $("._mainmenu").click(function (e) {

        e.stopPropagation();
        $("._divsubmenu").hide();
        $("._divaccount").hide();
        $("#divTagSearch").hide();
        $("#divMainMenu").toggle();

    });
    $("._submenu").click(function (e) {
        $("._divaccount").hide();
        $("._divmainmenu").hide();
        $("#divTagSearch").hide();
        e.stopPropagation();
        $("#divSubMenu").toggle();
    });
    $("._closepop").click(function () {
        $("._helpClose").fadeOut("fast");
    })

    $(document).click(function (e) {
        if (!$(e.target).closest('._popup').length) {
            $("._popup").hide();
        }
        if (!$(e.target).closest('#NotificationList').length) {
            $("#NotificationList").hide();
        }
        if (!$(e.target).closest('#tagsearch').length) {
            $("#tagsearch").hide();
            $("#Filterpopup2").hide();
            $("#Filterpopup1").show();
        }

    });
    ErucaCRM.User.InnerLayout.Pop = function () {
        window.location.hash = "RecentActivity";
        $("#NotificationList").hide();
        $("._viewallRecent").empty();
        var objall = new Object();
        objall.CurrentPage = 1;
        objall.LeadAuditId_encrypted = null;
        objall.IsLoadMore = false;
        ErucaCRM.User.InnerLayout.GetAllRecentActivities(objall)
        //ErucaCRM.Framework.Core.OpenRolePopup("#viewAllInnerRecentActivites");
    }

    $('._loadmoreinner').click(function () {
        lastLeadId = $('._left-sectioninner').last().children('._right-img').attr('leadauditid');
        var obj = new Object();
        obj.CurrentPage = 1;
        obj.LeadAuditId_encrypted = lastLeadId;
        obj.IsLoadMore = true;
        ErucaCRM.User.InnerLayout.GetAllRecentActivities(obj, true);
    });
}
ErucaCRM.User.DeleteCookie = function (name) {
    document.cookie = name + '=;expires=Thu, 01 Jan 1970 00:00:01 GMT;';
};
//get notification recent activites
ErucaCRM.User.InnerLayout.GetNotification = function (obj, ShowMessage) {

    ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetNotification", obj,
        function onSuccess(response) {
            if (response.Response == "Success") {

                maxLeadAuditId = response.MaxLeadAuditID;
                ErucaCRM.User.DeleteCookie("MaxLeadAuditId");
                ErucaCRM.Framework.Common.SetCookie("MaxLeadAuditId", maxLeadAuditId);
                ErucaCRM.User.InnerLayout.RenderAllRecentNotification(response.RecentActivities);
                if (response.TotalNotification == 0) {
                    $("._icon-globe").hide();
                    $("._notify-tip").text("0");
                }
                else {

                    $("._notify-tip").text(response.TotalNotification);
                    $("._icon-globe").show();
                }

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

            }
            /*On complete Nothing to do*/
        }
    );
}
//Get all recent activites 
ErucaCRM.User.InnerLayout.GetAllRecentActivities = function (obj, ShowMessage) {
    ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetRecentActivitiesForHome", obj,
           function onSuccess(response) {
               if (response.Response == "Success") {
                   var pagesize = ErucaCRM.Framework.Core.Config.PageSize;

                   if (response.RecentActivities.length < pagesize) {
                       $('._loadmoreinner').css('display', 'none');
                   }
                   else {
                       $('._loadmoreinner').css('display', 'inline');
                   }
                   ErucaCRM.User.InnerLayout.RenderRecentActivities(response.RecentActivities, obj);
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
ErucaCRM.User.InnerLayout.RenderAllRecentNotification = function (RecentActivityListData, obj) {

    var arraylist = [];
    //get notification page size 
    var listsize = ErucaCRM.Framework.Core.Config.NotificationListPageSize
    $.each(RecentActivityListData, function (index, RecentActivity) {


        var OwnerDetail = "/User/UserProfile/" + RecentActivity.CreatedBy + "?returnurl=" + window.location.href;
        //var ImageURL = "/Uploads/users/" + RecentActivity.ImageURL;//: "";           
        var ImageURL = RecentActivity.ImageURL;//: "";          
        var error = "this.onerror=null;this.src='/Uploads/users/no_image.gif'";
        $("#NotificationList ._left-sectioninner:last").remove();
        arraylist.push("<div class='left-section _left-sectioninner activityviewall'>" +
                     "<div class='recentactivity-left-img'>" +
                         "<a href='" + OwnerDetail + "' class='permissionbased' data-showalways='True' data-permission='UserVe'>" +
                          "<img style='width: 30px;' src='" + ImageURL + "' alt=''  onerror=" + error + "></a>" +
                        "</div>" +
                         "<div class='right-img right-text_Notification _right-img' LeadAuditId='" + RecentActivity.LeadAuditId + "'>" +
                         "<p class='activity-text'>" + RecentActivity.ActivityText + "</p>" +
                         "<span class='pull-right'>" + RecentActivity.ActivityCreatedTime + "</span></div></div>");
    });

    if (updatenotification == false) {
        for (var i = 0; i < listsize; i++) {

            $("#NotificationList").append(arraylist[i]);
        }
        $("#NotificationList").append("<div class='innerViewAll _innerViewAll' onclick='javascript:ErucaCRM.User.InnerLayout.Pop()'><button class='normal button  clear innerViewAllbuttom' data-toggle='modal' data-target='#myModal' type='button' style='display: inline;'>" + ErucaCRM.Messages.LayoutInner.ViewAll, +"</button>" +
                    "</div>");
    }
    else {
        for (var i = RecentActivityListData.length; i >= 0; i--) {

            $("#NotificationList").prepend(arraylist[i]);
        }
    }
    if ($("#NotificationList ._left-sectioninner").length < ErucaCRM.Framework.Core.Config.NotificationListPageSize) {
        $("._innerViewAll").css('display', 'none');
    }
    else {
        $('._innerViewAll').css('display', 'inline');
    }
    ErucaCRM.Framework.Common.ApplyPermission();
};
ErucaCRM.User.InnerLayout.RenderRecentActivities = function (RecentActivities, obj) {

    if (RecentActivities.length == 0) {
        $("#NoNotification").html("<b style='text-align:center'>" + ErucaCRM.Messages.DashBoard.NoRecentNotificationFound + "</b>").css({ "padding": "20px", "font-style": "italic" });
    }
    else {
        $("#NoNotification").hide();
    }
    $.each(RecentActivities, function (index, RecentActivity) {
        var OwnerDetail = "/User/UserProfile/" + RecentActivity.CreatedBy + "?returnurl=" + window.location.href;
        //var ImageURL = "/Uploads/users/" + RecentActivity.ImageURL;//: "";           
        var ImageURL = RecentActivity.ImageURL;//: "";     
        var error = "this.onerror=null;this.src='/Uploads/users/no_image.gif'";
        $("._viewallRecent").append("<div class='left-section _left-sectioninner'>" +
                     "<div class='recentactivity-left-img'>" +
                      "<a href='" + OwnerDetail + "' class='permissionbased' data-showalways='True'  data-permission='UserVe'>" +
                      "<img style='width: 30px' src='" + ImageURL + "' alt='' onerror=" + error + "></a>" +
                     "</div>" +
                     "<div class='right-img _right-img' LeadAuditId='" + RecentActivity.LeadAuditId + "'>" +
                      "<p class='activity-text '>" + RecentActivity.ActivityText + "</p>" +
                       "<span class='pull-right'>" + RecentActivity.ActivityCreatedTime + "</span></div></div>");
    });
    ErucaCRM.Framework.Common.ApplyPermission();
}


function closeNotification(closeButton) {

    $(closeButton).parent("._left-sectioninner").hide();
}
