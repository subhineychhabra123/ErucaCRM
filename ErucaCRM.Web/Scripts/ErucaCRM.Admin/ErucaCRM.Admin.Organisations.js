jQuery.namespace('ErucaCRM.Admin.Organisations');
var viewModel;
var currentPage = 1;
var userStatus = "Active";
var Activate = "";
ErucaCRM.Admin.Organisations.pageLoad = function () {

	currentPage = 1;

	CompanyStatus = "Active";
	viewModel = new ErucaCRM.Admin.Organisations.pageViewModel(currentPage, CompanyStatus);
	ko.applyBindings(viewModel);

	$("#ActiveInActive").html("Active")
	$("#ActiveInActive").hide();
	$("#ActiveInActive").html($("#ActiveInActive").attr("Activate"));
	$("#DropDownListUserStatus option[value=Active]").text("Active");
	$("#DropDownListUserStatus option[value=Inactive]").text("Inactive");

}

ErucaCRM.Admin.Organisations.GetGridData = function (currentPage, userStatus) {

    viewModel = new ErucaCRM.Admin.Organisations.pageViewModel(currentPage, CompanyStatus);
	ko.applyBindings(viewModel);

}


// A view model that represent a Test report query model.
ErucaCRM.Admin.Organisations.OrganisationListQueryModel = function (data) {
	var self = this;
	var d = new Date();
	self.CompanyId = data.CompanyId;
	
	self.CompanyName =data.CompanyName
	self.EmailId = data.EmailId;
	var date = Date(data.CreatedOn);
	
	self.CreatedOn = dateFormat(date, "fullDate");
	self.Designation = "";
	if (data.Role != undefined) {
		self.Designation = data.Role.RoleName;
	}
	
	
	self.Url = self.Url + "?=" + d.getUTCMilliseconds();
	self.CompanyDetail = "/Admin/OrganizationDetail/" + data.CompanyId;//{new @userId=" + data.UserId + "}";
	self.UrlError = "this.onerror = null; this.src = '/Uploads/users/no_image.gif'";
	//  self.ActivateText = ErucaCRM.Messages.User.ActivateText;
	//  self.DeactivateText = ErucaCRM.Messages.User.DeactivateText;
	self.MoreDetailsText = "more detail";
}
//Page view model

ErucaCRM.Admin.Organisations.pageViewModel = function (currentPage, CompanyStatus) {
	//Class variables
	var self = this;
	var objCompanyInfo = new Object();
	objCompanyInfo.CurrentPage = currentPage;
	objCompanyInfo.CompanyStatus = CompanyStatus;
	var PagingMethodName = "GetPageData";

	var controllerUrl = "/Admin/";
	var Message = {
		Failure: 'Failure',
		Success: 'Success'
	}

	self.UserListQueryModel = ko.observableArray();
	self.queryModel = ko.observable();
	self.status = ko.observable();
	this.UserList = ko.observableArray([]);
	self.getUserList = function (objCompanyInfo) {

		ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetOrganisations", objCompanyInfo, function onSuccess(response) {
			self.SuccessfullyRetrievedModelsFromAjax(response);

		}, function onError(err) {
			self.status(err.Message);
		});

	}



	self.getUserList(objCompanyInfo);

	self.SuccessfullyRetrievedModelsFromAjax = function (CompanyListData) {
		$("#divNoRecord").hide();
		$("#CompanyListData").children().remove();
		if (CompanyListData.listCompany.length == 0) {
			$("#divNoRecord").html("<b>" + "No Company Found" + "</b>");
			$("#divNoRecord").show();
		}

		ko.utils.arrayForEach(CompanyListData.listCompany, function (company) {
		    self.UserList.push(new ErucaCRM.Admin.Organisations.OrganisationListQueryModel(company));
		});
		$(".detail").html("more detail");
		$("#Pager").html(self.GetPaging(CompanyListData.TotalRecords, currentPage, PagingMethodName));
	};

	self.GetPageData = function (currentPageNo) {

		currentPage = currentPageNo;
		var objCompanyInfo = new Object();
		objCompanyInfo.CurrentPage = currentPageNo;
		objCompanyInfo.CompanyStatus = $("#DropDownListUserStatus").val();
        
		self.getUserList(objCompanyInfo);

	}

	$("#ActiveInActive").click(function () {

		var listObjUsers = new Array();
		var objUser = new Object();

		$("#CompanyListData input:checked").each(function () {
			currentPage = 1
			objUser = new Object();
			objUser.CompanyId = $(this).attr("companyId");
			objUser.CurrentPage = currentPage;
			objUser.CompanyStatus = $("#DropDownListUserStatus").val();
			listObjUsers.push(objUser);
		});

		self.UpdateCompanyStatus(listObjUsers);

	});

	self.UpdateCompanyStatus = function (listObjUsers) {

	    ErucaCRM.Framework.Core.doPostOperation(controllerUrl + "UpdateCompanyStatus", listObjUsers, function onSuccess(response) {
			self.SuccessfullyUpdateUserStatus(response);

		}, function onError(err) {
			self.status(err.Message);
		});
	};

	self.SuccessfullyUpdateUserStatus = function (result) {

		if (result.status == "Success") {
			$("#ActiveInActive").hide();
			ErucaCRM.Framework.Core.ShowMessage("Status has been updated successfully.", false);

			var objCompanyInfo = new Object();
			objCompanyInfo.CurrentPage = currentPage;
			objCompanyInfo.CompanyStatus = userStatus;

			self.getUserList(objCompanyInfo);
		}
		else {
			alert("User Status Not Updated");
			return false;
		}


	}

	$("#DropDownListUserStatus").change(function () {
		currentPage = 1;
		$("#ActiveInActive").hide();

		userStatus = $("#DropDownListUserStatus").val();
		var objCompanyInfo = new Object();
		objCompanyInfo.CurrentPage = currentPage;
		objCompanyInfo.CompanyStatus = userStatus;
		self.getUserList(objCompanyInfo);


	});


	self.ActivateDeactivate = function (obj) {

		if ($("#CompanyListData input:checked").length > 0)
			$("#ActiveInActive").show();
		else
			$("#ActiveInActive").hide();

		if ($("#DropDownListUserStatus").val() == "Active") {
			if ($("#ActiveInActive").html() == "Active") {
				$("#ActiveInActive").html("Inactive");
			}
		}
		else {
			if ($("#ActiveInActive").html() == "Inactive") {
				$("#ActiveInActive").html("Active");
			}

		}
	}


	self.GetPaging = function (Rowcount, currentPage, methodName) {
		return ErucaCRM.Framework.Core.GetPagger(Rowcount, currentPage, methodName);
	}

}

