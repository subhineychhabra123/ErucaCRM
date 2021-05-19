jQuery.namespace('ErucaCRM.User.ViewAccount');
var IsLeadsLastStage = false;
var viewModel;
var currentPage = 1;
var ContactCurrentPage = 1;
var filterBy = "Active";
ErucaCRM.User.ViewAccount.pageLoad = function () {
    currentPage = 1;
    filterBy = "Active";
    viewModel = new ErucaCRM.User.ViewAccount.pageViewModel();
    ko.applyBindings(viewModel, document.getElementById("InnerContentContainer"));
}


// A view model that represent a Test report query model.
ErucaCRM.User.ViewAccount.ProductsModel = function (data) {
    var self = this;
    self.RowId = "row_" + data.ProductId;
    self.ProductId = data.ProductId;
    self.ProductName = data.ProductName;
    self.ProductCode = data.ProductCode;
    return self;
}
ErucaCRM.User.ViewAccount.ContactListQueryModel = function (data) {

    var self = this;
    self.RowId = "row_" + data.ContactId;
    self.ContactId = data.ContactId;
    self.ContactName = data.ContactName;
    self.ContactEmail = data.EmailAddress;
    self.PhoneNo = data.Phone;
    self.EditContact = "/User/EditContact/" + data.ContactId + "/?AccountId=" + $('#AccountId').val()
    self.AccountId = $('#AccountId').val();
}
// A view model that represent a Test report query model.
ErucaCRM.User.ViewAccount.TaskItemListQueryModel = function (data) {

    var self = this;
    self.TaskId = data.TaskId;
    self.Subject = data.Subject;
    self.DueDate = data.TaskDueDate;
    self.TaskStatus = data.TaskStatus;
    self.Priority = data.PriorityName;
    var AccountID = $("._accountId").find("input[type=hidden]").val();

    self.EditUrl = "/User/TaskItem?taskID_encrypted=" + data.TaskId + "&mod=Account&val_encrypted=" + AccountID + "&returnurl=" + window.location.href;;
    self.DetailUrl = "/User/ViewTaskItemDetail?mod=Account&taskID_encrypted=" + data.TaskId;
}
//Page view model

ErucaCRM.User.ViewAccount.AccountLeadListQueryModel = function (data) {
    var self = this;
    self.RowId = data.LeadId;
    self.LeadId = data.LeadId;
    self.Title = data.Title;
    self.StageId = data.StageId;     
    self.CreatedOn = data.CreatedOn;
    self.LeadOwnerName = data.LeadOwnerName;
    
}
ErucaCRM.User.ViewAccount.AccountDocListQueryModel = function (data) {
    var self = this;
    self.FileName = data.FileName;
    self.AttachedBy = data.AttachedBy;
    self.DocId = data.DocId;
    self.FilePath = data.FilePath;
    self.UserProfileLink = "/user/userprofile/" + data.AttachById;    

}
ErucaCRM.User.ViewAccount.pageViewModel = function () {
    //Class variables
    var self = this;
    self.Core = ErucaCRM.Framework.Core;
    var objAccountInfo = new Object();
    objAccountInfo.AccountId = $("._accountId").find("input[type=hidden]").val();
    objAccountInfo.CurrentPage = currentPage;
    objAccountInfo.FilterParameters = new Array();

    var objContactInfo = new Object();
    objContactInfo.CurrentPageNo = ContactCurrentPage;
    objContactInfo.FilterBy = "Allcontacts"
    objContactInfo.AccountId = $('#AccountId').val();
    var PagingMethodName = "GetPageData";

    var controllerUrl = "/User/";
    var Message = {
        Failure: 'Failure',
        Success: 'Success'
    }

    $('._addLead').click(function () {
        $('#LeadTitle').val("");
        ErucaCRM.Framework.Core.OpenRolePopup("._addNewLeadPopup");

    });


    $('._saveLead').click(function () {
        
        var leadTitle = $('#LeadTitle').val();
        if (leadTitle.length == 0) {
            $('._error').text(ErucaCRM.Messages.Lead.LeadRequired)

            return false;
        } else {
            $('._error').text("");
        }
        var PostData = new Object();
        PostData.Title = leadTitle;
        PostData.AccountId = $('#AccountId').val();
        ErucaCRM.Framework.Core.doPostOperation(controllerUrl + "CreateLead", PostData, function onSuccess(response) {
            var PostData = new Object();
            PostData.LeadId = response.NewLead.LeadId;
            PostData.StageId = response.NewLead.StageId;
            PostData.Title = response.NewLead.Title;    
            self.CreatedOn = response.NewLead.LeadCreatedTime;
            PostData.CreatedOn = response.NewLead.LeadCreatedTime;
            PostData.LeadOwnerName = response.NewLead.LeadOwnerName;
            self.AccountLeadList.push(new ErucaCRM.User.ViewAccount.AccountLeadListQueryModel(PostData));
            $("#AssociateAccountLeads").children("tbody").children("tr._NoAssociateAccountLeadRecord").hide();
            ErucaCRM.Framework.Common.ApplyPermission('._roleselector');
        })
      
       
       
        $('._close').click();
    })
    self.DeleteLead = function (Lead) {        
        if (confirm(ErucaCRM.Messages.Account.ConfirmLeadDeleteAction) == true) {
        var data = new Object();
        data.LeadId = Lead.LeadId;       
          ErucaCRM.Framework.Core.doDeleteOperation(controllerUrl + "DeleteLead", data, function onSuccess(response) {         
              self.DisplayNoRecodsMessage();
            self.AccountLeadList.remove(Lead);
        });
        }
    }

  

    $('._deleteContact').click(function () {


        self.DeleteContact($(this).attr("contactId"), $(this).attr("accountId"), this);

    });
    $('._deleteLead').click(function () {
        if (confirm(ErucaCRM.Messages.Account.ConfirmLeadDeleteAction) == true) {
        var data = new Object();
        data.LeadId = $(this).attr("data-leadId")
        ErucaCRM.Framework.Core.doDeleteOperation(controllerUrl + "DeleteLead", data, function onSuccess(response) {
            self.DisplayNoRecodsMessage();


        });
        }
       
        $(this).parent('td').parent('tr').remove();

    });
    $('._deleteCase').click(function () {
        if (confirm(ErucaCRM.Messages.Account.ConfirmAccountCaseDeleteAction) == true) {
            self.DeleteCase($(this).attr("leadId"), $(this).attr("accountId"), this);
        }
    });


    $('._deleteSalesOrder').click(function () {
        if (confirm(ErucaCRM.Messages.SalesOrder.ConfirmSalesOrderDeleteAction)) {
            self.DeleteSalesOrder($(this).attr("salesorderid"), this);
        }

    })

    $('._deleteMemberAccount').click(function () {
        if (confirm(ErucaCRM.Messages.Account.ConfirmMemberAccountDeleteAction) == true) {
            self.DeleteMemberAccount($(this).attr("memberAccountId"), $(this).attr("parentAccountId"), this);
        }
    });



    self.DisplayNoRecodsMessage = function () {
        var HtmlNoRecords = "";
        if ($("#AccountDocs").children("tbody").children("tr._dataRow").length == 0) {

            HtmlNoRecords = "<span><b> " + ErucaCRM.Messages.Account.NoAccountAttachmentRecordFound + "</b></span>";

            $("#AccountDocs").children("tbody").children("tr._NoAccountDocRecord").children("td").html(HtmlNoRecords);
            $("#AccountDocs").children("tbody").children("tr._NoAccountDocRecord").show();
        }

        if ($("#AccountSaleOrders").children("tbody").children("tr._dataRow").length == 0) {
            HtmlNoRecords = "<span><b> " + ErucaCRM.Messages.Account.NoAccountSaleOrderRecordFound + "</b></span>";
            $("#AccountSaleOrders").children("tbody").children("tr._NoAccountSalesOrderRecord").children("td").html(HtmlNoRecords);
            $("#AccountSaleOrders").children("tbody").children("tr._NoAccountSalesOrderRecord").show();
        }

        if ($("#AssociateAccountLeads").children("tbody").children("tr._dataRow").length == 0) {
            HtmlNoRecords = "<span><b> " + ErucaCRM.Messages.Account.NoAssociateAccountLeadRecord + "</b></span>";
            $("#AssociateAccountLeads").children("tbody").children("tr._NoAssociateAccountLeadRecord").children("td").html(HtmlNoRecords);
            $("#AssociateAccountLeads").children("tbody").children("tr._NoAssociateAccountLeadRecord").show();
        }
        if ($("#AccountContacts").children("tbody").children("tr._dataRow").length == 0) {
            HtmlNoRecords = "<span><b> " + ErucaCRM.Messages.Account.NoAccountContactRecordFound + "</b></span>";
            $("#AccountContacts").children("tbody").children("tr._NoAccountContactsRecord").children("td").html(HtmlNoRecords);
            $("#AccountContacts").children("tbody").children("tr._NoAccountContactsRecord").show();
        }

        if ($("#MemberAccounts").children("tbody").children("tr._dataRow").length == 0) {
            HtmlNoRecords = "<span><b> " + ErucaCRM.Messages.Account.NoMemberAccountRecordFound + "</b></span>";
            $("#MemberAccounts").children("tbody").children("tr._NoMemberAccountRecord").children("td").html(HtmlNoRecords);
            $("#MemberAccounts").children("tbody").children("tr._NoMemberAccountRecord").show();
        }


        if ($("#AccountCases").children("tbody").children("tr._dataRow").length == 0) {

            HtmlNoRecords = "<span><b> " + ErucaCRM.Messages.Account.NoAccountCaseRecordFound + "</b></span>";
            $("#AccountCases").children("tbody").children("tr._NoAccountCaseRecord").children("td").html(HtmlNoRecords);
            $("#AccountCases").children("tbody").children("tr._NoAccountCaseRecord").show();
        }


    }

    self.DisplayNoRecodsMessage();
    this.AssociatedProductsModels = ko.observableArray([]);
    this.AllProductsModels = ko.observableArray([]);

    //
    self.TaskItemListQueryModel = ko.observableArray();
    self.queryModel = ko.observable();
    self.status = ko.observable();
    this.TaskItemList = ko.observableArray([]);
    this.AccountDocList = ko.observableArray([]);
    this.ContactList = ko.observableArray([]);
    this.AccountContactList = ko.observableArray([]);
    this.AccountLeadList = ko.observableArray([]);
    self.getTaskItemList = function (objAccountInfo) {
        self.TaskListList = ko.observableArray([]);
        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetAccountTasks", objAccountInfo, function onSuccess(response) {
            self.RenderTaskItems(response);
            ErucaCRM.Framework.Common.ApplyPermission('._roleselector');

        }, function onError(err) {
            ErucaCRM.Framework.Core.ShowMessage(err.Message, true);
        });

    }

    self.GetContactList = function (objContactInfo) {

        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "NonAssociatedContactList", objContactInfo, function onSuccess(response) {

            self.RenderContacts(response);

            ErucaCRM.Framework.Common.ApplyPermission('._roleselector');

        }, function onError(err) {
            ErucaCRM.Framework.Core.ShowMessage(err.Message, true);
        });


    }
    self.AddContact = function () {
        ErucaCRM.Framework.Core.OpenRolePopup("._AddContactSection");
        self.GetContactList(objContactInfo);
    }

    self.RenderTaskItems = function (TaskItemListData) {

        $("#divNoRecord").hide();
        $("#TaskItemListData").children().remove();
        if (TaskItemListData.List.length == 0) {
            $("#divNoRecord").html("<br/><b>" + ErucaCRM.Messages.Account.NoAccountActivityRecordFound + "</b>");
            $("#divNoRecord").show();

        }

        ko.utils.arrayForEach(TaskItemListData.List, function (TaskItem) {
            self.TaskItemList.push(new ErucaCRM.User.ViewAccount.TaskItemListQueryModel(TaskItem));
        });

      
        $("#Pager").html(self.GetPaging(TaskItemListData.TotalRecords, currentPage, PagingMethodName));
    };
    self.RenderContacts = function (ContactItemListData) {
        self.ContactList.removeAll();
        $("#divNoRecord").hide();
        $("#TaskItemListData").children().remove();
        if (ContactItemListData.ListContacts.length == 0) {
            $("#divNoRecord").html("<br/><b>" + ErucaCRM.Messages.Account.NoAccountActivityRecordFound + "</b>");
            $("#divNoRecord").show();

        }

        ko.utils.arrayForEach(ContactItemListData.ListContacts, function (TaskItem) {
            self.ContactList.push(new ErucaCRM.User.ViewAccount.ContactListQueryModel(TaskItem));
        });

        $("#AllContactPager").html(self.GetPaging(ContactItemListData.TotalRecords, ContactCurrentPage, "viewModel.GetPageDataForContact"));

    };
    self.getTaskItemList(objAccountInfo);


    self.GetPageData = function (currentPageNo) {

        currentPage = currentPageNo;
        var objTaskItemInfo = new Object();
        objTaskItemInfo.CurrentPage = currentPageNo;
        objTaskItemInfo.FilterParameters = new Array();        
        self.getTaskItemList(objTaskItemInfo);

    }
    self.AssociateContact = function () {

        var listObjContact = new Array();


        $('._contact').each(function (i) {
            var contact = new Object();
            if ($(this).is(':checked')) {
                contact.AccountId = $('#AccountId').val();
                contact.ContactId = $(this).attr("ContactId");
                listObjContact.push(contact);
            }
        })

        if (listObjContact.length > 0) {
            ErucaCRM.Framework.Core.doPostOperation(controllerUrl + "AssociateAccountContact", listObjContact, function onSuccess(response) {
                $.each(listObjContact, function (key, value) {
                    var postData = new Object();
                    var $contactRow = $('[id="row_' + value.ContactId + '"]')
                    postData.ContactId = value.ContactId;
                    postData.ContactName = $.trim($contactRow.find("td:eq(1)").text());
                    postData.EmailAddress = $.trim($contactRow.find("td:eq(3)").text());
                    postData.Phone = $.trim($contactRow.find("td:eq(2)").text());
                    self.AccountContactList.push(new ErucaCRM.User.ViewAccount.ContactListQueryModel(postData));


                })
                if (self.AccountContactList().length!= 0) {
                 
                    $("#AccountContacts").children("tbody").children("tr._NoAccountContactsRecord").hide();
                }
                ErucaCRM.Framework.Common.ApplyPermission('._roleselector');
                $('._close').click();

            }, function onError(err) {
                self.status(err.Message);
            });

        }
        else {
            alert(ErucaCRM.Messages.Account.SelectOneOrMoreOption)
        }

    }
    self.GetPageDataForContact = function (CurrentPageNo) {

        var objContactInfo = new Object();
        objContactInfo.CurrentPageNo = CurrentPageNo;
        ContactCurrentPage = CurrentPageNo;
        objContactInfo.FilterBy = "Allcontacts"
        objContactInfo.AccountId = $('#AccountId').val();
        self.GetContactList(objContactInfo);
    }
    self.GetPaging = function (Rowcount, currentPage, methodName) {
        return ErucaCRM.Framework.Core.GetPagger(Rowcount, currentPage, methodName);
    }
    //
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
     
            self.GetPageDataForContact(1);
           
            $("#addNewContactSection ._close").click();
        });
    }

    self.RemoveProduct = function (productId) {
        var postData = new Object();
        postData.ProductId = productId;
        postData.AccountId = $("input[type='hidden']#AccountId").val();
        self.Core.doPostOperation(controllerUrl + "RemoveProductFromAccount", postData, function onSuccess(response) {
            self.Core.ShowMessage(ErucaCRM.Messages.Product.ProductRemovedSuccess, false);
            this.AllProductsModels = ko.observableArray([]);
            var $productRow = $("#accountProducts tr#row_" + postData.ProductId);
            postData.ProductName = $.trim($productRow.find("td:eq(1)").text());
            postData.ProductCode = $.trim($productRow.find("td:eq(2)").text());
            self.AllProductsModels.push(ErucaCRM.User.ViewAccount.ProductsModel(postData));
            $productRow.remove();
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

        self.Core.doPostOperation(controllerUrl + "AddNewProduct", postData, function onSuccess(response) {
            self.Core.ShowMessage(ErucaCRM.Messages.Product.ProductAddedSuccess, false);
            postData.ProductId = response.ProductId;
            self.AllProductsModels.push(ErucaCRM.User.ViewAccount.ProductsModel(postData));
            $("#addNewProductSection ._close").click();
        })

    };
    self.RemoveQuote = function (quoteId, e) {
        var postData = new Object();
        postData.Id_encrypted = quoteId;
        self.Core.doPostOperation(controllerUrl + "DeleteQuote", postData, function onSuccess(response) {
            self.Core.ShowMessage(ErucaCRM.Messages.Quote.QuoteDeletedSuccess, false);
            $(e).parent().parent().hide();

        });


    }
    var uploadObj;
    uploadObj = $("#fileuploader").uploadFile({
        url: '/User/UploadAccountDocument',
        multiple: false,
        autoSubmit: false,
        fileName: "docs",
        maxFileSize: 1024 * 10000,
        dynamicFormData: function () {
            var data = self.setPostData();
            return data;
        },
        showStatusAfterSuccess: false,
        onSubmit: function (files) {

        },
        onSuccess: function (files, data, xhr) {
            ErucaCRM.Framework.Core.ShowMessage(data.response.Message, false);
            self.AccountDocList.push(new ErucaCRM.User.ViewAccount.AccountDocListQueryModel(data));
            ErucaCRM.Framework.Common.ApplyPermission('._roleselector');
           // var UserProfileLink = "/user/userprofile/" + data.AttachById;
            //$('#AccountDocs > tbody:last').append('<tr class="_dataRow"><td><a target=_blank href="' + data.FilePath + '">' + data.FileName + '</a></td><td><a href="' + UserProfileLink + '">' + data.AttachedBy + '</a></td><td> <a docid=' + data.DocId + ' class="_deleteDoc" href="javascript:void(0)">delete</a></td></tr>');
            $("#AccountDocs").children("tbody").children("tr._NoAccountDocRecord").hide();
            $('div.hidden').fadeOut();
        },
        afterUploadAll: function () {         
            $('#FileUploadSection').fadeOut("slow");

        },
        onError: function (files, status, errMsg) {

        }
    });
    $("._adddoc").bind("click", function () {
    
        ErucaCRM.Framework.Core.OpenRolePopup("#FileUploadSection");
    });
    $("#btnAddProduct").bind("click", function () {
        ErucaCRM.Framework.Core.OpenRolePopup("#AddProductSection");


    });

    $("#btnAddNewProduct").bind("click", function () {
        $(':text').each(function () { $(this).val(""); })
        ErucaCRM.Framework.Core.OpenRolePopup("#addNewContactSection");
    });



    $("._attachDoc").bind("click", function () {
        if (uploadObj.selectedFiles > 0) {
            uploadObj.startUpload();
            uploadObj.selectedFiles = 0;
          
        } else {
            alert(ErucaCRM.Messages.Account.PleaseSelectFile);
            return;
        }
    });

    $('._close').click(function () {
        $('._error').text("");
        uploadObj.cancelAll();
        uploadObj.errorLog.empty();

    });
    self.RemoveDoc = function (docs) {

        var response = confirm(ErucaCRM.Messages.Document.ConfirmDocumentDeleteAction);
        if (!response)
            return false;
        var actionName = "RemoveAccountDocument";
        var docid = docs.DocId
        var obj = new Object();
        obj.DocumentId = docid;
        obj.DocumentPath = docs.FilePath;
        ErucaCRM.Framework.Core.doPostOperation
               (
                   "/User/" + actionName,
                   obj,
                   function onSuccess(response) {
                       if (response.Status == Message.Failure) { ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Document.DocumentNotRemoved, true); }
                       else if (response.Status == Message.Success) {
                           self.AccountDocList.remove(docs);
                           ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Document.DocumentRemoved, false);
                           if ($("#docAcoountList").children("tr").length == 0 && $("#AccountDocs").find("tr._dataRow").length == 0) {
                     
                               HtmlNoRecords = "<span><b> " + ErucaCRM.Messages.Account.NoAccountAttachmentRecordFound + "</b></span>";
                               $("#AccountDocs").children("tbody").children("tr._NoAccountDocRecord").children("td").html(HtmlNoRecords);
                               $("#AccountDocs").children("tbody").children("tr._NoAccountDocRecord").show();
                           }
                       }
                   },
                   function onError(err) {
                       self.status(err.Message);
                   }
               );
    }
    $("#AccountDocs").on("click", "._deleteDoc", function () {

        var response = confirm(ErucaCRM.Messages.Document.ConfirmDocumentDeleteAction);
        if (!response)
            return false;

        var actionName = "RemoveAccountDocument";

        $td = $(this);
        var docpath = $($td).attr("docpath");
        var docid = $($td).attr("docid");

        var obj = new Object();
        obj.DocumentId = docid;
        obj.DocumentPath = docpath;
        ErucaCRM.Framework.Core.doPostOperation
               (
                   controllerUrl + actionName,
                   obj,
                   function onSuccess(response) {
                       if (response.Status == Message.Failure) { ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Document.DocumentNotRemoved, true); }
                       else if (response.Status == Message.Success) {
                           var tr = $($td).closest('tr');
                           //tr.css("background-color", "#FF3700");
                           tr.fadeOut(400, function () {
                               tr.remove();

                               var HtmlNoRecords = "";
                               if ($("#AccountDocs").find("tr._dataRow").length == 0) {
                                   HtmlNoRecords = "<span><b> " + ErucaCRM.Messages.Account.NoAccountAttachmentRecordFound + "</b></span>";
                                   $("#AccountDocs").children("tr._NoAccountDocRecord").children("td").html(HtmlNoRecords);
                                   $("#AccountDocs").children("tr._NoAccountDocRecord").show();
                               }
                           });
                           ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Document.DocumentRemoved, false);

                       }
                   },
                   function onError(err) {
                       self.status(err.Message);
                   }
               );

    });

    self.setPostData = function () {
        var obj = new Object;
        obj.AccountId = $("._accountId").find("input[type=hidden]").val();
        return obj;
    }
    self.DeleteTaskItem = function (task) {
        if (confirm(ErucaCRM.Messages.Account.ConfirmAccountActivityDeleteAction)) {
            var PostData = new Object();
            var accountId = $('#AccountId').val();
            PostData.taskId_encrypted = task.TaskId;
            PostData.accountId_encrypted = accountId;
            ErucaCRM.Framework.Core.doDeleteOperation(controllerUrl + "DeleteAccountActivity", PostData,
            function SuccessCallBack(response) {
                if (response.Status == "Success") {
                    ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Account.RecordDeletedSuccess, false);
                    ErucaCRM.User.ViewAccount.pageViewModel();
                }
                else {
                    ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Account.RecordDeletionFailure, true);
                }
            }, function FailureCallback() { })


        }
    }

    self.DeleteInvoice = function (invoiceId, e) {

        var PostData = new Object();
        PostData.Id_encrypted = invoiceId;
        ErucaCRM.Framework.Core.doDeleteOperation(controllerUrl + "DeleteInvoice", PostData,
        function SuccessCallBack(response) {
            if (response.Status == "True") {

                ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Invoice.InvoiceDeletedSuccess, false);
                $(e).parent().parent().hide();
            }
            else {
                ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Invoice.InvoiceDeletedFailure, true);
            }
        }, function FailureCallback() { })


    }

    self.DeleteSalesOrder = function (salesOrderId, e) {
        var PostData = new Object();
        PostData.Id_encrypted = salesOrderId;
        ErucaCRM.Framework.Core.doDeleteOperation(controllerUrl + "DeleteSaleOrder", PostData,
        function SuccessCallBack(response) {
            if (response.Status == "Success") {
                ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.SalesOrder.SalesOrderDeletedSuccess, false);
                $(e).parent().parent().hide();
            }
            else {
                ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.SalesOrder.SalesOrderDeletedFailure, true);
            }
        }, function FailureCallback() { })


    }




    self.DeleteMemberAccount = function (memeberAccountId, parentAccountId, e) {
        var PostData = new Object();
        PostData.memeberAcctId_encrypted = memeberAccountId;
        PostData.parentAcctId_encrypted = parentAccountId;


        ErucaCRM.Framework.Core.doDeleteOperation(controllerUrl + "DeleteMemberAccount", PostData,
        function SuccessCallBack(response) {
            if (response.Status == "Success") {

                ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Account.RecordDeletedSuccess, false);
                $(e).parent().parent().remove();

                if ($("#MemberAccounts").children("tbody").children("tr [rowType=data]").length == 0) {
                    var HtmlAccountNoRecords = "<span><b> " + ErucaCRM.Messages.Account.NoMemberAccountRecordFound + "</b></span>";
                    $("#MemberAccounts").children("tbody").children("tr [id=trNoMemberAccountRecord]").children("td").html(HtmlAccountNoRecords);
                }

            }
            else {
                ErucaCRM.Framework.Core.ShowMessage("Record not deleted", true);
            }
        }, function FailureCallback() { })
    }

    self.DeleteContact = function (contactId, accountId, e) {
        if (confirm(ErucaCRM.Messages.Account.ConfirmAccountContactDeleteAction) == true) {
            var PostData = new Object();
            PostData.contactId_encrypted = contactId;
            PostData.accountId_encrypted = accountId;
            ErucaCRM.Framework.Core.doDeleteOperation(controllerUrl + "DeleteAccountContact", PostData,
            function SuccessCallBack(response) {
                if (response.Status == "Success") {
                    ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Account.RecordDeletedSuccess, false);
                    $(e).parent().parent().remove();      
                    
                    viewModel.AccountContactList.remove(e);                 
                    if (self.AccountContactList().length == 0) {                     
                        self.DisplayNoRecodsMessage();
                    }
                   
                }
                else {
                    ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Account.RecordDeletionFailure, true);
                }
            }, function FailureCallback() { })
        }

    }
    self.DeleteCase = function (caseId, accountId, e) {
        var PostData = new Object();
        PostData.caseId_encrypted = caseId;
        PostData.accountId_encrypted = accountId;

        ErucaCRM.Framework.Core.doDeleteOperation(controllerUrl + "DeleteAccountCase", PostData,
        function SuccessCallBack(response) {
            if (response.Status == "Success") {

                ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Account.RecordDeletedSuccess, false);
                $(e).parent().parent().remove();


                if ($("#AccountCases").children("tbody").children("tr [rowType=data]").length == 0) {
                    var HtmlNoRecords = "<span><b> " + ErucaCRM.Messages.Account.NoAccountContactRecordFound + "</b></span>";
                    $("#AccountCases").children("tbody").children("tr [id=trNoAccountCaseRecord]").children("td").html(HtmlNoRecords);
                }

            }
            else {
                ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Account.RecordDeletionFailure, true);
            }
        }, function FailureCallback() { })

    }


}

