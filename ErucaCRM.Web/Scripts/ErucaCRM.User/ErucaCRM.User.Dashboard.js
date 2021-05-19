jQuery.namespace('ErucaCRM.User.Dashboard');
ErucaCRM.User.Dashboard.pageLoad = function () {

    currentPage = 1;
    TagId = "";
    TagSearchName = "";
    CurrentTagName = "";
    var PeriodText = ErucaCRM.Messages.DashBoard.PeriodsYearly;

    viewHomeModel = new ErucaCRM.User.Dashboard.pageViewModel();
    ko.applyBindings(viewModel, document.getElementById("InnerContentContainer"));

}

var common = jQuery.namespace('ErucaCRM.Framework.Common');

ErucaCRM.User.Dashboard.pageViewModel = function (data) {



    var controllerUrl = "/User/";
    //==================================Home Activtity section=============
    var self = this;

    var ReportLeadsInPipleLineInfo = new Object();
    ReportLeadsInPipleLineInfo.dateFilterOption = "Y";
    PeriodText = ErucaCRM.Messages.DashBoard.PeriodsYearly;

    var PagingMethodName = "GetPageData";

    var controllerUrl = "/User/";

    var Message = {
        Failure: 'Failure',
        Success: 'Success'
    }
    var FilterParam = new Object();
    FilterParam.Month = "Month";
    FilterParam.Year = "Year";

    self.FunnelChartQueryModel = ko.observableArray();
    self.queryModel = ko.observable();
    self.status = ko.observable();
    // this.LeadsInPipeLineList = ko.observableArray([]);
    $('._yearWise').click(function () {

        $('._LeadsInPipeLine').find('li').removeClass('active');

        $(this).parent().addClass('active');
        self.GetLeadsInPipeLineByThisYear();

    });
    $('._monthwise').click(function () {
        
        $('._LeadsInPipeLine').find('li').removeClass('active');
        $(this).parent().addClass('active');
       
        self.GetLeadsInPipeLineByThisMonth();

    });
    $('._forAll').click(function () {
        $('._LeadsInPipeLine').find('li').removeClass('active');

        $(this).parent().addClass('active');
        self.GetOverAllLeadsInPipeLine();

    });

    $('._monthwiseAllLead').click(
        function () {
            $('._AllLeadRecord').find('li').removeClass('active');
            $(this).parent().addClass('active');
            $('#containerLeadYearCountChartMonthbasis').html("");
            self.GetYearMonthWiseLeadCount(FilterParam.Month);
        }
        )
    $('._yearWiseAllLead').click(
       function () {
           $('._AllLeadRecord').find('li').removeClass('active');
           $(this).parent().addClass('active');
           $('#containerLeadYearCountChartMonthbasis').html("");
           self.GetYearMonthWiseLeadCount(FilterParam.Year);
       }
       )

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

    self.GetMonthWiseAccountSaleRevenue = function () {

        //  LeadInfo.CurrentPageNo = pageNo;

        ErucaCRM.Framework.Core.doPostOperation
       (
           controllerUrl + "GetMonthWiseAccountSaleRevenue",
           null,
                function onSuccess(response) {
                    if (response.Response == "Success") {

                        var Months = new Array();
                        var SaleRevenue = new Array();
                        var objSeriesNew = new Object()
                        objSeriesNew.data = [];

                        ko.utils.arrayForEach(response.ListMonthWiseAccountSaleRevenue, function (AccountSaleRevenue) {


                            Months.push(AccountSaleRevenue.Month);
                            var accountSaleRevenue = AccountSaleRevenue.TotalAccountSaleRevenue == null ? 0 : AccountSaleRevenue.TotalAccountSaleRevenue;
                            //    objSeriesNew.stack = 'Sales';

                            //  objSeries.Mint.push(objDuration)

                            objSeriesNew.data.push(accountSaleRevenue);
                            Months = common.ConvertMonthsToCultureSpecific(Months);

                        });

                        SaleRevenue.push(objSeriesNew);


                        $('#containerMonthWiseAccountSaleRevenueChart').highcharts({

                            chart: {
                                type: 'column'
                            },

                            title: {
                                text: ErucaCRM.Messages.DashBoard.MonthWiseAccountSaleRevenueTitle

                            },

                            xAxis: {
                                categories: Months,
                                title: {
                                    text: ErucaCRM.Messages.DashBoard.MonthWiseAccountSaleRevenueXAxisText
                                }
                            },

                            yAxis: {

                                allowDecimals: false,
                                min: 0,

                                step: 1,
                                title: {
                                    text: ErucaCRM.Messages.DashBoard.MonthWiseAccountSaleRevenueYAxisText
                                }
                            },

                            tooltip: {
                                formatter: function () {
                                    //var stage = this.y;
                                    //stage = stage.toString() + this.key;
                                    //var miutesArrary = this.series.userOptions.Mint;
                                    //var mint = self.GetKeyVaue(miutesArrary, stage);
                                    return '<b>' + this.x + '</b><br/> ' + ErucaCRM.Messages.DashBoard.MonthWiseAccountSaleRevenueToolTip +
                                      ': ' + this.y;  //+ '<br/>' +
                                    //  'Total: ' + this.point.stackTotal;

                                }
                            },

                            plotOptions: {
                                column: {
                                    stacking: 'normal'
                                }
                            },

                            series: SaleRevenue
                        });
                        $("#containerMonthWiseAccountSaleRevenueChart .highcharts-legend").css("display", "none");
                    }

                }

                    );


    }


    self.GetAccountByTopHighestSaleRevenue = function () {

        ErucaCRM.Framework.Core.doPostOperation
      (
          controllerUrl + "GetAccountByTopHighestSaleRevenue",
          null,
                function onSuccess(response) {
                    if (response.Response == "Success") {

                        var Accounts = new Array();
                        var SaleRevenue = new Array();
                        var objSeriesNew = new Object()
                        objSeriesNew.data = [];

                        ko.utils.arrayForEach(response.ListAccountByTopHighestSaleRevenue, function (AccountSaleRevenue) {

                            Accounts.push(AccountSaleRevenue.AccountName);
                            var accountSaleRevenue = AccountSaleRevenue.TotalAccountSaleRevenue == null ? 0 : AccountSaleRevenue.TotalAccountSaleRevenue;
                            objSeriesNew.stack = 'Sales';

                            objSeriesNew.data.push(accountSaleRevenue);


                        });

                        SaleRevenue.push(objSeriesNew);

                        $('#containerAccountByTopHighestSaleRevenueChart').highcharts({
                            chart: {
                                type: 'bar'
                            },
                            title: {
                                text: ErucaCRM.Messages.DashBoard.AccountByTopHighestSaleRevenueTitle
                            },
                            subtitle: {
                                text: ''
                            },
                            xAxis: {
                                categories: Accounts,//['Africa', 'America', 'Asia', 'Europe', 'Oceania'],
                                title: {
                                   // align:'center',
                                    text: ErucaCRM.Messages.DashBoard.AccountByTopHighestSaleRevenueYAxisText
                                }
                            },
                            yAxis: {
                                min: 0,
                                title: {
                                    text: ErucaCRM.Messages.DashBoard.AccountByTopHighestSaleRevenueXAxisText
                                  //  align: 'high'
                                },
                                labels: {
                                    overflow: 'justify'
                                }
                            },
                            tooltip: {
                                formatter: function () {
                                    //var stage = this.y;
                                    //stage = stage.toString() + this.key;
                                    //var miutesArrary = this.series.userOptions.Mint;
                                    //var mint = self.GetKeyVaue(miutesArrary, stage);
                                    return '<b>' + this.x + '</b><br/> ' + ErucaCRM.Messages.DashBoard.AccountByTopHighestSaleRevenueToolTip +
                                      ': ' + this.y;  //+ '<br/>' +
                                    //  'Total: ' + this.point.stackTotal;

                                }
                                //valueSuffix: ' millions'
                            },
                            plotOptions: {
                                bar: {
                                    dataLabels: {
                                        enabled: true
                                    }
                                }
                            },
                            legend: {
                                layout: 'vertical',
                                align: 'right',
                                verticalAlign: 'top',
                                x: -40,
                                y: 100,
                                floating: true,
                                borderWidth: 1,
                                backgroundColor: '#FFFFFF',
                                shadow: true
                            },
                            credits: {
                                enabled: false
                            },
                            series: SaleRevenue// [{
                            //    name: 'Year 1800',
                            //    data: [107, 31, 635, 203, 2]
                            //}, {
                            //    name: 'Year 1900',
                            //    data: [133, 156, 947, 408, 6]
                            //}, {
                            //    name: 'Year 2008',
                            //    data: [973, 914, 4054, 732, 34]
                            //}]
                        });
                        $("#containerAccountByTopHighestSaleRevenueChart .highcharts-legend").css("display", "none");


                    }

                });

        //=========================================================================
        self.GetYearWiseLeadCount = function () {
            var LeadCount = new Array();
            var Year = new Array();
            var objSeries = new Object()
            objSeries.name = 'Total Lead';
            objSeries.data = [];
            ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetYearWiseLeadCount", "", function onSuccess(response) {
                if (response.data != null) {
                    ko.utils.arrayForEach(response.data, function (list) {
                        Year.push(list.CreatedYear);
                        objSeries.data.push(list.LeadCount);
                    });
                }
                LeadCount.push(objSeries);

                $('#containerLeadYearCountChart').highcharts({

                    chart: {
                        type: 'line'
                    },

                    title: {
                        text: ErucaCRM.Messages.DashBoard.LeadHistoryYearWiseChartTitle
                    },

                    xAxis: {
                        categories: Year,
                        title: {
                            text: ErucaCRM.Messages.DashBoard.LeadHistoryYearWiseChartXAxisText
                        }
                    },

                    yAxis: {
                        min: 0,
                        title: {
                            text: ErucaCRM.Messages.DashBoard.LeadHistoryYearWiseChartYAxisText
                        }

                    },

                    plotOptions: {
                        column: {
                            stacking: 'normal'
                        }
                    },

                    series: LeadCount
                });

            }, function onError(err) {
                ErucaCRM.Framework.Core.ShowMessage(err.Message, true);
            });


        }
        self.RenderYearWiseLead = function (LeadItem) {

        }

        //==================================Home Account Case section=============

     

        self.GetYearMonthWiseLeadCount = function (Interval) {
            var title = undefined;
            var xAxisTitle = undefined;
            var yAxisTitle = undefined;
            var LeadCount = new Array();
            var Year = new Array();
            var objSeries = new Object()
            objSeries.data = [];

            if (Interval == FilterParam.Month) {

                title = ErucaCRM.Messages.DashBoard.LeadHistoryMonthWiseChartTitle;
                xAxisTitle = ErucaCRM.Messages.DashBoard.LeadHistoryMonthWiseChartXAxisText;
                yAxisTitle = ErucaCRM.Messages.DashBoard.LeadHistoryMonthWiseChartYAxisText;
                ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetMonthWiseLeadCount", "", function onSuccess(response) {

                    if (response.data != null) {

                        ko.utils.arrayForEach(response.data, function (list) {
                            objSeries.name = 'Total Lead';
                            Year.push(list.Month);
                            objSeries.data.push(list.LeadCount);
                        });
                    }

                    LeadCount.push(objSeries);
                    Year = common.ConvertMonthsToCultureSpecific(Year);
                    $('#containerLeadYearCountChartMonthbasis').highcharts({

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
            else {
                title = ErucaCRM.Messages.DashBoard.LeadHistoryYearWiseChartTitle;
                xAxisTitle = ErucaCRM.Messages.DashBoard.LeadHistoryMonthWiseChartXAxisText;
                yAxisTitle = ErucaCRM.Messages.DashBoard.LeadHistoryYearWiseChartYAxisText;
                ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetYearWiseLeadCount", "",

                function onSuccess(response) {
                    if (response.data != null) {
                        ko.utils.arrayForEach(response.data, function (list) {
                            Year.push(list.CreatedYear);
                            objSeries.name = 'Total Lead';
                            objSeries.data.push(list.LeadCount);


                        });
                    }
                    LeadCount.push(objSeries);

                    $('#containerLeadYearCountChartMonthbasis').highcharts({

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



                });

            }



        }

    }

    self.getLeadsInPipeLine(ReportLeadsInPipleLineInfo);
    self.GetMonthWiseAccountSaleRevenue();
    self.GetAccountByTopHighestSaleRevenue();

    self.GetLeadsInPipeLineByThisYear = function () {


        ReportLeadsInPipleLineInfo = new Object();
        PeriodText = ErucaCRM.Messages.DashBoard.PeriodsYearly;
        ReportLeadsInPipleLineInfo.dateFilterOption = "Y";
        self.getLeadsInPipeLine(ReportLeadsInPipleLineInfo);

    }

    self.GetLeadsInPipeLineByThisMonth = function () {

        ReportLeadsInPipleLineInfo = new Object();
        PeriodText = ErucaCRM.Messages.DashBoard.PeriodsMonthly;

        ReportLeadsInPipleLineInfo.dateFilterOption = "M";
        self.getLeadsInPipeLine(ReportLeadsInPipleLineInfo);

    }

    self.GetOverAllLeadsInPipeLine = function () {
        ReportLeadsInPipleLineInfo = new Object();
        PeriodText = ErucaCRM.Messages.DashBoard.PeriodsOverAll;
        ReportLeadsInPipleLineInfo.dateFilterOption = "";
        self.getLeadsInPipeLine(ReportLeadsInPipleLineInfo);

    }



    //=========================================Leads Star Rating ================================//
    self.GetLeadsByStarRatingPercentage = function () {

        var LeadCount = new Array();
        var StageName = new Array();
        var objSeries = new Object()
        objSeries.name = '';
        objSeries.data = [];
        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetLeadsByStarRatingPercentage", "", function onSuccess(response) {

            if (response.data != null) {
                StageName = response.data.StageName;


                for (var i = 0; i < response.data.RatingArray.length; i++) {
                    objSeries = new Object();
                    objSeries.name = response.data.RatingName[i];
                    objSeries.data = response.data.RatingArray[i];

                    LeadCount.push(objSeries);
                }

            }


            $('#containerLeadsByStarRatingPercentage').highcharts({

                chart: {
                    type: 'column'
                },

                title: {
                    text: ErucaCRM.Messages.DashBoard.LeadsByStagRatingPercentageTitle

                },

                xAxis: {
                    categories: StageName,
                    title: {
                        text: ErucaCRM.Messages.DashBoard.LeadsByStagRatingPercentageXAxisText
                    }
                },

                yAxis: {
                    min: 0,
                    title: {
                        text: ErucaCRM.Messages.DashBoard.LeadsByStagRatingPercentageYAxisText
                    }

                },
                tooltip: {
                    pointFormat: '<span style="color:{series.color}">{series.name}</span>: <b>{point.y}</b> ({point.percentage:.0f}%)<br/>',
                    shared: true
                },
                plotOptions: {
                    column: {
                        stacking: 'percent'
                    }
                },

                series: LeadCount
            });

        }, function onError(err) {
            ErucaCRM.Framework.Core.ShowMessage(err.Message, true);
        });



    }

    //=========================================================================

    //self.GetYearWiseLeadCount();

    self.GetYearMonthWiseLeadCount(FilterParam.Month);
    self.GetLeadsByStarRatingPercentage();

}