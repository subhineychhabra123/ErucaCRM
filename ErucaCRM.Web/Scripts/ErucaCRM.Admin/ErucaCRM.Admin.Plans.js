jQuery.namespace('ErucaCRM.Admin.Plans');

var viewModel;
var currentPage = 1;


ErucaCRM.Admin.Plans.pageLoad = function () {

    currentPage = 1;

    viewModel = new ErucaCRM.Admin.Plans.pageViewModel();
    ko.applyBindings(viewModel);
}


// A view model that represent a Test report query model.
ErucaCRM.Admin.Plans.PlanListQueryModel = function (data) {


    var self = this;
    self.PlanId = data.PlanId;
    self.PlanName = data.PlanName;

    self.Price = data.Price;
    self.NoOfUsers = data.NoOfUsers;
    self.Active = data.Active;
    self.EditPlanUrl = "Plan/" + data.PlanId;
    self.DetailPlanUrl = "PlanDetail/" + data.PlanId;
}

ErucaCRM.Admin.Plans.pageViewModel = function () {
    //Class variables
    var self = this;
    var objPlanListInfo = new Object();

    objPlanListInfo.CurrentPageNo = currentPage;

    var PagingMethodName = "GetPageRecords";

    var controllerUrl = "/Admin/";

    var Message = {
        Failure: 'Failure',
        Success: 'Success'
    }

    self.PlanListQueryModel = ko.observableArray();
    self.status = ko.observable();
    this.PlanList = ko.observableArray([]);

    self.getPlanList = function () {

        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetAllPlans", null, function onSuccess(response) {
            self.RenderPlanList(response);
            ErucaCRM.Framework.Common.ApplyPermission('._roleselector');
        }, function onError(err) {
            self.status(err.Message);
        });





    }

    self.RenderPlanList = function (PlanListData) {

        $("#divNoRecord").hide();
        $("#PlanListData").children().remove();
        if (PlanListData.ListPlans.length == 0) {
            $("#divNoRecord").html("<b> No Records Found</b>");
            $("#divNoRecord").show();
        }

        ko.utils.arrayForEach(PlanListData.ListPlans, function (Plan) {
            self.PlanList.push(new ErucaCRM.Admin.Plans.PlanListQueryModel(Plan));
        });

        // $("#Pager").html(self.GetPaging(PlanListData.TotalRecords, currentPage, PagingMethodName));
    }

    self.GetPageRecords = function (currentPageNo) {

        var objPlanListInfo = new Object();

        objPlanListInfo.CurrentPageNo = currentPage;
        getPlanList();
    }


    self.GetPaging = function (Rowcount, currentPage, methodName) {
        return ErucaCRM.Framework.Core.GetPagger(Rowcount, currentPage, methodName);
    }

    self.getPlanList();

}