jQuery.namespace('ErucaCRM.User.Home');
var ContentpageUrl = "/User/Home"
ErucaCRM.User.Home.pageLoad = function () {

    currentPage = 1;

    TagId = "";
    TagSearchName = "";
    CurrentTagName = "";
    viewHomeModel = new ErucaCRM.User.Home.pageViewModel();
    ko.applyBindings(viewHomeModel, document.getElementById("InnerContentContainer"));
    //var obj = new Object();
    //obj.updateNotification = false;
    //obj.maxLeadAuditId_encrypted = maxLeadAuditId
    //ErucaCRM.User.InnerLayout.GetNotification(obj);
    //var objall = new Object();
    //objall.CurrentPage = 1;
    //objall.LeadAuditId_encrypted = null;
    //objall.IsLoadMore = false;
    //ErucaCRM.User.InnerLayout.GetAllRecentActivities(objall)

}

ErucaCRM.User.Home.HomeTaskItemQueryModel = function (data) {

    var self = this;
    self.TaskId = data.TaskId;
    self.Subject = data.Subject;
    self.DueDate = data.TaskDueDate;
    self.TaskStatus = data.TaskStatus;
   // self.OwnerImage = "/Uploads/users/" + data.OwnerImage;
    self.OwnerImage =  data.OwnerImage;

    self.Priority = data.PriorityName;
    self.Description = data.Description;
    self.OwnerDetail = "/User/UserProfile/" + data.OwnerIdEncrypted + "?returnurl=" + window.location.href;
    self.DetailUrl = "/User/ViewTaskItemDetail?taskID_encrypted=" + data.TaskId;
}

ErucaCRM.User.Home.HomeAllRecentActivitiesQueryModel = function (data) {

    var self = this;
    self.ActivityText = data.ActivityText;
    self.LeadAuditId = data.LeadAuditId;
    self.ActivityCreatedTime = data.ActivityCreatedTime;//== null ? null : dateFormat(eval(data.CreatedDate.replace(/\/Date\((\d+)\)\//gi, "new Date($1)")), "HCisoUtcDateTime");
    self.OwnerDetail = "/User/UserProfile/" + data.CreatedBy + "?returnurl=" + window.location.href;
    //self.ImageURL = "/Uploads/users/" + data.ImageURL;//: "";
    self.ImageURL =  data.ImageURL;//: "";
    self.DetailUrl = "/User/AccountCaseDetail/" + data.AccountCaseId + "?returnurl=" + window.location.href;
   
}
ErucaCRM.User.Home.HomeRecentActivitiesQueryModel = function (data) {
   
    var self = this;
    self.ActivityText = data.ActivityText;
    self.LeadAuditId = data.LeadAuditId;
    self.ActivityCreatedTime = data.ActivityCreatedTime;//== null ? null : dateFormat(eval(data.CreatedDate.replace(/\/Date\((\d+)\)\//gi, "new Date($1)")), "HCisoUtcDateTime");
    self.OwnerDetail = "/User/UserProfile/" + data.CreatedBy + "?returnurl=" + window.location.href;
  //  self.ImageURL = "/Uploads/users/" + data.ImageURL;//: "";
    self.ImageURL =  data.ImageURL;//: "";
    self.DetailUrl = "/User/AccountCaseDetail/" + data.AccountCaseId + "?returnurl=" + window.location.href;
}

ErucaCRM.User.Home.HomeAccountCaseQueryModel = function (data) {
    var self = this;
    self.AccountCaseId = data.AccountCaseId;
    self.Subject = data.Subject;
    self.Description = data.Description;
    self.CaseNumber = data.CaseNumber;
    self.showhr = data.index == 1 ? false : true;
    self.CaseOwnerName = data.CaseOwnerName;
    self.OwnerDetail = "/User/UserProfile/" + data.CaseOwnerIdEncrypted + "?returnurl=" + window.location.href;
    self.CreatedDate = data.CaseCreatedTime //== null ? null : dateFormat(eval(data.CaseCreatedTime.replace(/\/Date\((\d+)\)\//gi, "new Date($1)")), "HCisoUtcDateTime");
   // self.AccountCaseOwnerImg = "/Uploads/users/" + data.OwnerImage;
    self.AccountCaseOwnerImg =data.OwnerImage;
    self.CaseMessageBoards = data.CaseMessageBoards;
    self.DetailUrl = '/User/AccountCaseDetail/' + data.AccountCaseId + "?returnurl=" + window.location.href;
    //self.MessageDetailUrl = '/User/AccountCaseDetail/' + data.AccountCaseId +'#AssociatedMessageBoardMessages'+"?returnurl=" + window.location.href;
    self.MessageDetailUrl = '/User/AccountCaseDetail/' + data.AccountCaseId +"?returnurl=" + window.location.href;
    self.CaseMessageBoards = ko.observableArray([]);
    if (data.TotalCaseMessageBoards > 3)
        self.ShowViewMoreComment = true;
    else
        self.ShowViewMoreComment = false;
    self.ShowReplies = data.TotalCaseMessageBoards > 0 ? true : false;
    $.each(data.CaseMessageBoards, function (index, val) {
        if (index <= 2) {
            self.CaseMessageBoards.push(new ErucaCRM.User.Home.AccountCaseCommentInfoQueryModel(val));
        }
    });

}
ErucaCRM.User.Home.AccountCaseCommentInfoQueryModel = function (Commentdata) {
    var self = this;
    // self.CommentedbyImg = "/Uploads/users/" + Commentdata.CreatedByUserImg;
    self.CommentedbyImg = Commentdata.CreatedByUserImg;
    self.CommentedbyName = Commentdata.CreatedByName;
    self.OwnerDetail = "/User/UserProfile/" + Commentdata.CreatedByEncrypted;
    self.CommentCreatedDate = Commentdata.CreatedDate == null ? null : dateFormat(eval(Commentdata.CreatedDate.replace(/\/Date\((\d+)\)\//gi, "new Date($1)")), "HCisoUtcDateTime");


    //self.Commenttext = 
    self.Comment = Commentdata.Description;

}


ErucaCRM.User.Home.pageViewModel = function (data) {



    var DefaultPipeLineInfo = new Object();
    DefaultPipeLineInfo.dateFilterOption = "M";
    PeriodText = ErucaCRM.Messages.DashBoard.PeriodsMonthly;
    var FilterParam = new Object();
    FilterParam.Month = "Month";
    FilterParam.Week = "Week";
    FilterParam.Year = "Year";
    var self = this;
    var DefaultInterval = 'Week'
    var controllerUrl = "/User/";
    self.AnalyticdataDataArray = ko.observableArray();
    self.TotalLead = ko.observable();
    self.WinLeadPercentage = ko.observable();
    self.LeadLost = ko.observable();
    self.LeadWin = ko.observable();
    self.LeadClose = ko.observable();    
    self.LeadClosePercentage = ko.observable();
    self.Revenue = ko.observable();
    self.RevenueIncrementPercentage = ko.observable();
    self.LeadIncrementPercentage = ko.observable();
    self.LostLeadPercentage = ko.observable();
    self.TaskItemList = ko.observableArray([]);
    self.RecentActivityList = ko.observableArray([]);
    self.AccountCaseList = ko.observableArray([]);
    self.TotalLeadTitle = ko.observable();
    self.TotalRevenueTitle = ko.observable();
    self.TotalCloseTitle = ko.observable();
    self.DisplayLeadIncrement = ko.observable();
    self.DisplayLeadClose = ko.observable();
    self.DisplayLeadRevenue = ko.observable();
    self.AllRecentActivityList = ko.observableArray([]);

    $('._analyticWeek').click(function () {
        $('.FilterButtons').find('a').removeClass('activeButton');

        $(this).addClass('activeButton');
        self.GetLeadAnalyticData(FilterParam.Week)
    });

    $('._analyticMonth').click(function () {
        $('.FilterButtons').find('a').removeClass('activeButton');
        $(this).addClass('activeButton');
        self.GetLeadAnalyticData(FilterParam.Month)

    });
    $('._analyticYear').click(function () {
        $('.FilterButtons').find('a').removeClass('activeButton');
        $(this).addClass('activeButton');
        self.GetLeadAnalyticData(FilterParam.Year)
    });
    $('._year-wise').click(function () {

        $('._filter-button').find('li').removeClass('active');

        $(this).parent().addClass('active');
        //self.GetLeadsInPipeLineByThisYear();

    });

    $('._month-wise').click(function () {

        $('._filter-button').find('li').removeClass('active');
        $(this).parent().addClass('active');

        // self.GetLeadsInPipeLineByThisMonth();

    });
    $('._week-wise').click(function () {
        $('._filter-button').find('li').removeClass('active');
        $(this).parent().addClass('active');
        //self.GetOverAllLeadsInPipeLine();

    });



    self.GetLeadAnalyticData = function (Interval) {


        ErucaCRM.Messages.Lead.TotalLeadTitle
        self.TotalLeadTitle(ErucaCRM.Messages.Lead.TotalLeadTitle + " " + Interval);
        self.TotalRevenueTitle(ErucaCRM.Messages.Lead.TotalRevenueTitle + " " + Interval);
        self.TotalCloseTitle(ErucaCRM.Messages.Lead.TotalLeadCloseTitle + " " + Interval);
        var postData = new Object();
        postData.Interval = Interval;
        ErucaCRM.Framework.Core.doPostOperation(controllerUrl + "GetLeadAnalyticData", postData, function onSuccess(response) {
            self.AssignAnalyticData(response.data[0]);


        });


        $('._NewLead').click(function () {
            location.href = '/User/Leads?returnurl=' + window.location.href;

        });
        $('._NewContact').click(function () {
            location.href = '/User/AddContact?returnurl=' + window.location.href;

        });
        $('._NewActivity').click(function () {
            location.href = '/User/TaskItem?returnurl=' + window.location.href;
        });

        self.AssignAnalyticData = function (data) {


            if (data.TotalLead == 0) {
                self.DisplayLeadIncrement(false);
                self.TotalLead(ErucaCRM.Messages.Lead.NoDataToDisplay)

                self.LeadClose(ErucaCRM.Messages.Lead.NoDataToDisplay);
                self.DisplayLeadClose(false);


                self.Revenue(ErucaCRM.Messages.Lead.NoDataToDisplay);
                self.DisplayLeadRevenue(false);
            }
            else {
                self.TotalLead(data.TotalLead)
                self.DisplayLeadIncrement(true)
                self.LeadIncrementPercentage(data.LeadIncrementPercentage + "%");

                self.LeadClose(data.LeadClose);
                self.DisplayLeadClose(true);
                self.LeadClosePercentage(data.LeadClosePercentage + "%");

                self.Revenue(data.TotalRevenue);
                self.RevenueIncrementPercentage(data.RevenueIncrement + "%");
                self.DisplayLeadRevenue(true);

            }
            self.WinLeadPercentage(data.WinLeadPercentage + "%");
            self.LeadWin(ErucaCRM.Messages.Lead.WinText + "-" + data.LeadWin);
            self.LeadLost(ErucaCRM.Messages.Lead.LostText + "-" + data.LeadLost);

            self.LostLeadPercentage(data.LostLeadPercentage + "%");

        }


    }

    //==================================Home Activtity section=============
    self.GetTaskItemList = function () {
        var obj = new Object();
        obj.CurrentPage = 1;
        self.TaskItemList.removeAll();
        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetDashBoardTasks", obj, function onSuccess(response) {
            if (response.List != null)
                self.RenderTaskItems(response.List);
        }, function onError(err) {
            ErucaCRM.Framework.Core.ShowMessage(err.Message, true);
        });

    }
    self.RenderTaskItems = function (TaskItemListData) {
        $("#NoHomeActivityFound").hide();
        self.TaskItemList.removeAll();
        if (TaskItemListData.length == 0) {
            $("#NoHomeActivityFound").html("<br/><b>" + ErucaCRM.Messages.DashBoard.NoRecentTaskFound + "</b>");
            $("#NoHomeActivityFound").show();

        }

        ko.utils.arrayForEach(TaskItemListData, function (TaskItem) {
            self.TaskItemList.push(new ErucaCRM.User.Home.HomeTaskItemQueryModel(TaskItem));
        });

    };

    //=========================================================================

    self.getLeadsInPipeLine = function (ReportLeadsInPipleLineInfo) {


        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetReportLeadsInPipleLine", ReportLeadsInPipleLineInfo, function onSuccess(response) {
            self.RenderFunnelChartLeadsInPipeLine(response);
            ErucaCRM.Framework.Common.ApplyPermission('._roleselector');
        }, function onError(err) {
            ErucaCRM.Framework.Core.ShowMessage(err.Message, true);
        });

    }

    self.RenderFunnelChartLeadsInPipeLine = function (LeadsInPipeLineData) {


        var objSeries = new Object()
        objSeries.data = [];
        ko.utils.arrayForEach(LeadsInPipeLineData.ListLeadsInPipeLine, function (objLeadInPipeLine) {

            objSeries.data.push([objLeadInPipeLine.StageName, objLeadInPipeLine.TotalLeadsInPipeLine]);
        });

        $('#containerLeadsInPipeLine').highcharts({
            chart: {
                type: 'funnel',
                marginRight: 100
            },
            title: {
                text: ErucaCRM.Messages.DashBoard.SalesInPipeLineTitle + "( " + PeriodText + " )",
                x: -50
            },

            plotOptions: {
                series: {
                    dataLabels: {
                        enabled: true,
                        format: '<b>{point.name}</b> ({point.y:,.0f})',
                        color: 'black',
                        softConnector: true
                    },
                    neckWidth: '30%',
                    neckHeight: '25%'

                    //-- Other available options
                    // height: pixels or percent
                    // width: pixels or percent
                }
            },
            legend: {
                enabled: false
            },
            series: [{
                name: ErucaCRM.Messages.DashBoard.SalesInPipeLineToolTip,
                data: objSeries.data// [
                //    //['Website visits', 15654],
                //    //['Downloads', 4064],
                //    //['Requested price list', 1987],
                //    //['Invoice sent', 976],
                //    //['Finalized', 846]
                //]
            }]
        });
    }


    //==================================Home Account Case section=============
    self.GetAccountCaseList = function () {
        var obj = new Object();
        obj.CurrentPage = 1;
        self.AccountCaseList.removeAll();
        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetDashBoardAccountCases", obj, function onSuccess(response) {

            if (response.List != null)
                self.RenderAccountCases(response.List);
        }, function onError(err) {
            ErucaCRM.Framework.Core.ShowMessage(err.Message, true);
        });

    }
    self.RenderAccountCases = function (AccountCaseListData) {
        $("#divNoCaseFound").hide();
        self.AccountCaseList.removeAll();
        if (AccountCaseListData.length == 0) {
            var message = ErucaCRM.Messages.DashBoard.NoRecentCaseFound;
            message = message == undefined ? "No cases found." : message;
            $("#divNoCaseFound").html("<b>" + message + "</b>");
            $("#divNoCaseFound").show();

        }
        var index = 0
        ko.utils.arrayForEach(AccountCaseListData, function (AccountCase) {
            index++;
            AccountCase.index = index;
            self.AccountCaseList.push(new ErucaCRM.User.Home.HomeAccountCaseQueryModel(AccountCase));
        });

    };

    //=========================================================================
    var common = jQuery.namespace('ErucaCRM.Framework.Common');
    self.GetYearMonthWiseLeadCount = function (Interval) {
        var title = undefined;
        var xAxisTitle = undefined;
        var yAxisTitle = undefined;
        var LeadCount = new Array();
        var Year = new Array();
        var objSeries = new Object()
        objSeries.data = [];

        if (Interval == FilterParam.Week) {
            title = ErucaCRM.Messages.DashBoard.LeadHistoryCurrentWeekChartTitle;
            xAxisTitle = ErucaCRM.Messages.DashBoard.LeadHistoryWeekWiseChartXAxisText;
            yAxisTitle = ErucaCRM.Messages.DashBoard.LeadHistoryMonthWiseChartYAxisText;
            ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetWeekWiseLeadCount", "", function onSuccess(response) {

                if (response.data != null) {

                    ko.utils.arrayForEach(response.data, function (list) {
                        objSeries.name = ErucaCRM.Messages.DashBoard.LeadHistoryWeekWiseLeadSeriesText;
                        Year.push(list.Week);
                        objSeries.data.push(list.LeadCount);
                    });
                }
                Year = common.ConvertWeekToCultureSpecific(Year)
                LeadCount.push(objSeries);
                $('#containerLeadYearCountChartWeekBasis').highcharts({

                    chart: {
                        type: 'line'
                    },

                    title: {
                        text: title
                    },

                    xAxis: {
                        categories: Year,
                        title: {
                            text: xAxisTitle
                        }
                    },

                    yAxis: {
                        min: 0,
                        title: {
                            text: yAxisTitle

                        }

                    },

                    plotOptions: {
                        column: {
                            stacking: 'normal'
                        }
                    },

                    series: LeadCount
                });



            }
                , function onError(err) {
                    ErucaCRM.Framework.Core.ShowMessage(err.Message, true);
                });



        }




    }


    //==================================Fetch Recent Activities =============
    self.GetRecentActivities = function (obj, ShowMessage) {

        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetRecentActivitiesForHome", obj,
            function onSuccess(response) {
              
                if (response.Response == "Success") {
                    var pagesize = ErucaCRM.Framework.Core.Config.PageSize;                    
                    if (obj.LeadAuditId_encrypted == null && response.RecentActivities.length < pagesize) {
                        $('._loadall').css('display', 'none');
                    }
                    else {
                        $('._loadall').css('display', 'inline');
                    }
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
                self.RecentActivityList.push(new ErucaCRM.User.Home.HomeRecentActivitiesQueryModel(RecentActivity));
            else if (obj.LeadAuditId_encrypted != null && obj.IsLoadMore == false) {
                self.RecentActivityList.unshift(new ErucaCRM.User.Home.HomeRecentActivitiesQueryModel(RecentActivity));
            }
            else if (obj.LeadAuditId_encrypted != null && obj.IsLoadMore == true) {
                $("#divNoRecentActivtiyFound").hide();
                self.RecentActivityList.push(new ErucaCRM.User.Home.HomeRecentActivitiesQueryModel(RecentActivity));
            }
        });

    };
    self.GetAllRecentActivities = function (obj, ShowMessage) {

        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetRecentActivitiesForHome", obj,
            function onSuccess(response) {

                if (response.Response == "Success") {

                    var pagesize = ErucaCRM.Framework.Core.Config.PageSize;
               
                    if (response.RecentActivities.length < pagesize) {
                       
                        $('._loadmore').css('display', 'none');
                    }
                    else {
                        $('._loadmore').css('display', 'inline');
                    }
                    self.RenderAllRecentActivities(response.RecentActivities, obj);
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
    self.RenderAllRecentActivities = function (RecentActivityListData, obj) {
        //self.RecentActivityList.removeAll();

        $("#alldivNoRecentActivtiyFound").hide();
        if (RecentActivityListData.length == 0 && obj.LeadAuditId_encrypted == null && obj.IsLoadMore == false) {
            $("#alldivNoRecentActivtiyFound").html("<br/><b>" + ErucaCRM.Messages.DashBoard.NoRecentActFound + "</b>");
            $("#alldivNoRecentActivtiyFound").show();
        }
        ko.utils.arrayForEach(RecentActivityListData, function (RecentActivity) {
            if (obj.LeadAuditId_encrypted == null)
                self.AllRecentActivityList.push(new ErucaCRM.User.Home.HomeAllRecentActivitiesQueryModel(RecentActivity));
            else if (obj.LeadAuditId_encrypted != null && obj.IsLoadMore == false) {
                self.AllRecentActivityList.unshift(new ErucaCRM.User.Home.HomeAllRecentActivitiesQueryModel(RecentActivity));
            }
            else if (obj.LeadAuditId_encrypted != null && obj.IsLoadMore == true) {
                $("#alldivNoRecentActivtiyFound").hide();
                self.AllRecentActivityList.push(new ErucaCRM.User.Home.HomeAllRecentActivitiesQueryModel(RecentActivity));
            }
        });

    };

    setInterval(function () {
        lastLeadId = $('._left-section').first().children('._right-img').attr('leadauditid');
        var obj = new Object();
        obj.CurrentPage = 1;
        obj.LeadAuditId_encrypted = lastLeadId;
        obj.IsLoadMore = false;
        self.GetRecentActivities(obj, false);
    }, 60000);

    $('#Refreshbutton').click(function () {
        lastLeadId = $('._left-section').first().children('._right-img').attr('leadauditid');
        var obj = new Object();
        obj.CurrentPage = 1;       
        obj.LeadAuditId_encrypted = lastLeadId;
        obj.IsLoadMore = false;
        self.GetRecentActivities(obj, true);
    });

    $('._loadall').click(function () {
        //lastLeadId = $('._left-section').last().children('._right-img').attr('leadauditid');
        //var obj = new Object();
        //obj.CurrentPage = 1;
        //obj.LeadAuditId_encrypted = lastLeadId;
        //obj.IsLoadMore = true;
        //self.GetRecentActivities(obj, true);
        window.location.hash = "#RecentActivity";
        ErucaCRM.Framework.Core.OpenRolePopup("#ViewAllRecentActivity");
    });
    $('._loadmore').click(function () {
        lastLeadId = $('._left-sectionAll').last().children('._right-img').attr('leadauditid');
        var obj = new Object();      
        obj.CurrentPage = 1;
        obj.LeadAuditId_encrypted = lastLeadId;
        obj.IsLoadMore = true;
        self.GetAllRecentActivities(obj, true);

    });
    self.showPop = function () {
        //var v = location.hash;

        //if (v.length > 0) {
        //    ErucaCRM.Framework.Core.OpenRolePopup("#ViewAllRecentActivity");
        //}
       
    }
    $('._resetUrl').click(function () {      
        window.history.pushState({ path: ContentpageUrl }, '', ContentpageUrl);
    });

    //=========================================================================



    self.GetLeadAnalyticData(DefaultInterval);
    self.GetTaskItemList();
    self.GetAccountCaseList();
    self.getLeadsInPipeLine(DefaultPipeLineInfo);
    self.GetYearMonthWiseLeadCount(FilterParam.Week);
    var obj = new Object();
    obj.CurrentPage = 1;
    obj.LeadAuditId_encrypted = null;
    obj.IsLoadMore = false;
    self.GetRecentActivities(obj);

    self.GetAllRecentActivities(obj);
    self.showPop();


    //==============================New Dashboard Charts Data===============//

    self.GetDashboardAnalytics = function (interval) {

        var ob = new Object();
        ob.interval = interval;

        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetDashboardData", ob,
                function onSuccess(response) {
                    //debugger;
                    self.RenderCircleChartsForDashboard(response);
                });
    }

    self.RenderCircleChartsForDashboard = function (response) {

        var newclientpercen = response.RecentActivities.NewClientPercentage;
        var newclients = response.RecentActivities.NewClient;

        var winlead = response.RecentActivities.WinLead;
        var winleadpercen = response.RecentActivities.WinLeadPercentage;


        var lostlead = response.RecentActivities.LostLead;
        var lostleadpercen = response.RecentActivities.LostLeadPercentage;


        var closelead = response.RecentActivities.ClosedLead
        var closedleadpercen = response.RecentActivities.ClosedLeadPercentage;

       
        var totalrevenue = response.RecentActivities.TotalSaleRevenue;
        var totalrevenuepercen = response.RecentActivities.SalePercentage;

        $("#spannewlead").text(newclients);
        $("#spanwinleads").text(winlead);
        $("#spanlostleads").text(lostlead);
        $("#spanclosedlead").text(closelead);
        $("#spantotalsales").text((totalrevenue == null ? 0 : totalrevenue));

        Circles.create({
            id: 'divFirstchart',
            percentage: newclientpercen,
            radius: 70,
            width: 15,
            // number: ,
            text: '%',
            colors: ['#E0FFA3', '#B2D44F'],
            duration: 400
        })

        Circles.create({
            id: 'divSecondchart',
            percentage: closedleadpercen == undefined || closedleadpercen == null || isNaN(closedleadpercen) || closelead == 0 ? 0 : closedleadpercen,
            radius: 70,
            width: 15,
            //  number: 7.13,
            text: '%',
            colors: ['#FFBDBD', '#B24E36'],
            duration: 400
        });

        Circles.create({
            id: 'divThirdChart',
            percentage: totalrevenuepercen == undefined || totalrevenuepercen == null || isNaN(totalrevenuepercen) ? 0 : totalrevenuepercen,
            radius: 70,
            width: 15,
            //  number: 7.13,
            text: '%',
            colors: ['#E6E673', '#DBB84D'],
            duration: 400
        });
        Circles.create({
            id: 'divFourthchart',
            percentage: winleadpercen == null || winleadpercen == undefined || isNaN(winleadpercen) ? 0 : winleadpercen,
            radius: 70,
            width: 15,
            text: '',
            colors: ['#E0FFA3', '#DBB84D'],
            duration: 400
        });



        //     var chart = new CanvasJS.Chart("divFourthchart",
        //{

        //    legend: {
        //        verticalAlign: "bottom",
        //        horizontalAlign: "center"
        //    },
        //    theme: "theme1",
        //    width: 350,
        //    height: 230,
        //    data: [
        //    {
        //        type: "pie",
        //        //indexLabelFontFamily: "Garamond",       
        //        indexLabelFontSize: 16,
        //        startAngle: -30,


        //        //showInLegend: true,
        //        // toolTipContent:"{label}",
        //        dataPoints: [
        //        { y: winleadpercen, label: winleadpercen == 0 ? "" : ErucaCRM.Messages.DashBoard.WinLeads },
        //        { y: lostleadpercen, label: lostleadpercen == 0 ? "" : ErucaCRM.Messages.DashBoard.LostLeads },
        //         { y: lostleadpercen == 0 && winleadpercen == 0 ? 1 : 0, label: lostleadpercen == 0 && winleadpercen == 0 ? ErucaCRM.Messages.DashBoard.NoLeads : "" },

        //        ]
        //    }
        //    ]
        //});
        //     //if (lostleadpercen == 0 && winleadpercen == 0)
        //     //    $('#divFourthchart').html('<span>' + ErucaCRM.Messages.DashBoard.NoLeads + '</span>')
        //     //else
        //     chart.render();


    }


    self.SaveNewCommentForAccountCase = function (AccountCaseId) {
        var messageDescription = $.trim($('textarea[data-accountcasecomment="' + AccountCaseId + '"]').val());
        if (messageDescription != '') {
            var obj = new Object;
            obj.AccountCaseId = AccountCaseId;
            obj.Description = messageDescription;
            obj.CaseMessageBoardId = null;
            self.InsertNewMessage(obj);

        }
        else {
            ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.DashBoard.MessageBoardMessageDescriptionRequired, true);
            //alert("Enter your message description");
        }

    };




    self.InsertNewMessage = function (obj) {
        ErucaCRM.Framework.Core.doPostOperation(controllerUrl + "SaveMessageBoardMessage", obj, function (response) {
            self.BindMessageBoard(response, obj.AccountCaseId);
        })
    }
    self.BindMessageBoard = function (response, AccountCaseId) {
        if (response.response.Status == "Success") {

            $.each(self.AccountCaseList(), function (index, item) {
                if (item.AccountCaseId == AccountCaseId) {
                    var val = new Object();
                    val.CreatedByUserImg = response.CreatedbyUserImage;
                    val.CreatedByName = response.CreatedbyUserName;
                    val.CreatedDate = response.CreatedDate;
                    val.CreatedByEncrypted = response.UserId;
                    val.Description = response.Description;
                    item.CaseMessageBoards.push(new ErucaCRM.User.Home.AccountCaseCommentInfoQueryModel(val));
                    $('textarea[data-accountcasecomment="' + AccountCaseId + '"]').val("").css('height', '50px');
                    return false;
                }
            });

        }
    }


    self.setHeight = function (txtdesc, event) {

        txtdesc.style.height = txtdesc.scrollHeight + "px";
        if (event.keyCode == 13) {
            var accountid = $(txtdesc).data("accountcasecomment");
            self.SaveNewCommentForAccountCase(accountid);
        }
    }

    self.GetDashboardAnalytics(ErucaCRM.Framework.Common.DefaultIntervalForChart());

}