jQuery.namespace('ErucaCRM.User.Reports');

var viewModel;

ErucaCRM.User.Reports.pageLoad = function () {


    viewModel = new ErucaCRM.User.Reports.pageViewModel();
    ko.applyBindings(viewModel, document.getElementById("InnerContentContainer"));

}

// A view model that represent a Test report query model.
ErucaCRM.User.Reports.FunnelChartQueryModel = function (data) {

    var self = this;
    self.StageName = data.StageName;
    self.TotalLeadsInPipeLine = data.TotalLeadsInPipeLine;

}
//Page view model

ErucaCRM.User.Reports.pageViewModel = function () {

    //Class variables

    var self = this;

    var ReportLeadsInPipleLineInfo = new Object();
    ReportLeadsInPipleLineInfo.dateFilterOption = "Y";

    var PagingMethodName = "GetPageData";

    var controllerUrl = "/User/";

    var Message = {
        Failure: 'Failure',
        Success: 'Success'
    }

    self.FunnelChartQueryModel = ko.observableArray();
    self.queryModel = ko.observable();
    self.status = ko.observable();



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
                text: 'Sales funnel',
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
                name: 'Leads in pipe line',
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
                            objSeriesNew.stack = 'Sales';


                            objSeriesNew.data.push(accountSaleRevenue);


                        });

                        SaleRevenue.push(objSeriesNew);
                

                        $('#containerMonthWiseAccountSaleRevenueChart').highcharts({

                            chart: {
                                type: 'column'
                            },

                            title: {
                                text: ErucaCRM.Messages.Lead.LeadHistoryChartHeaderText
                            },

                            xAxis: {
                                categories: Months
                            },

                            yAxis: {

                                allowDecimals: false,
                                min: 0,

                                step: 1,
                                title: {
                                    text: ErucaCRM.Messages.Lead.LeadHistoryChartYAxisText
                                }
                            },

                            tooltip: {
                                formatter: function () {
                                 
                                    return '<b>' + this.x + '</b><br/> Revenue' +
                                      ': ' + this.y;  
                                 

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
                                text: 'Historic World Population by Region'
                            },
                            subtitle: {
                                text: ''
                            },
                            xAxis: {
                                categories: Accounts,
                                title: {
                                    text: null
                                }
                            },
                            yAxis: {
                                min: 0,
                                title: {
                                    text: 'Population (millions)',
                                    align: 'high'
                                },
                                labels: {
                                    overflow: 'justify'
                                }
                            },
                            tooltip: {
                                formatter: function () {
                                   
                                    return '<b>' + this.x + '</b><br/> Revenue' +
                                      ': ' + this.y; 
                                   

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
                            series: SaleRevenue
                        });
                        $("#containerAccountByTopHighestSaleRevenueChart .highcharts-legend").css("display", "none");


                    }

                });


    }
    self.getLeadsInPipeLine(ReportLeadsInPipleLineInfo);
    self.GetMonthWiseAccountSaleRevenue();
    self.GetAccountByTopHighestSaleRevenue();

    self.GetLeadsInPipeLineByThisYear = function () {
        ReportLeadsInPipleLineInfo = new Object();
        ReportLeadsInPipleLineInfo.dateFilterOption = "Y";
        self.getLeadsInPipeLine(ReportLeadsInPipleLineInfo);

    }

    self.GetLeadsInPipeLineByThisMonth = function () {
        ReportLeadsInPipleLineInfo = new Object();
        ReportLeadsInPipleLineInfo.dateFilterOption = "M";
        self.getLeadsInPipeLine(ReportLeadsInPipleLineInfo);

    }

    self.GetOverAllLeadsInPipeLine = function () {
        ReportLeadsInPipleLineInfo = new Object();
        ReportLeadsInPipleLineInfo.dateFilterOption = "";
        self.getLeadsInPipeLine(ReportLeadsInPipleLineInfo);

    }

}



