jQuery.namespace('ErucaCRM.User.ViewLead');
var viewLeadModel;
var currentPage = 1;
var filterBy = "Active";
ErucaCRM.User.ViewLead.pageLoad = function () {
    currentPage = 1;
    filterBy = "Active";
    viewLeadModel = new ErucaCRM.User.ViewLead.pageViewModel();
    //ko.cleanNode(document.getElementById("formleadDetails"));
    ko.applyBindings(viewModel, document.getElementById("InnerContentContainer"));
}


// A view model that represent a Test report query model.
ErucaCRM.User.ViewLead.ProductsModel = function (data) {
    var self = this;
    self.RowId = "row_" + data.ProductId;
    self.ProductId = data.ProductId;
    self.ProductName = data.ProductName;
    self.ProductCode = data.ProductCode;
    return self;
}

// A view model that represent a Test report query model.
ErucaCRM.User.ViewLead.TaskItemListQueryModel = function (data) {

    var self = this;
    self.TaskId = data.TaskId;
    self.Subject = data.Subject;
    self.DueDate = data.TaskDueDate;
    self.TaskStatus = data.TaskStatus;
    self.Priority = data.PriorityName;

    var LeadID = $("._leadId").find("input[type=hidden]").val();

    self.EditUrl = "/User/TaskItem?taskID_encrypted=" + data.TaskId + "&mod=Lead&val_encrypted=" + LeadID;
    self.DetailUrl = "/User/ViewTaskItemDetail?mod=Lead&taskID_encrypted=" + data.TaskId;
}
ErucaCRM.User.ViewLead.LeadListHistoryQueryModel = function (data) {
    var self = this;
    self.StageName = data.StageName;
    self.Proability = data.Proability;
    self.StageId = data.StageId;
    self.Amount = data.Amount;
    self.ExpectedRevenue = data.ExpectedRevenue;
    self.ClosingDate = data.LeadClosingDate;
    self.Duration = data.Duration;
    self.FromDate = data.LeadStageFromDate;
}
ErucaCRM.User.ViewLead.LeadRatingQueryModel = function (data) {
    var self = this;
    self.RatingId = data.RatingId;
    self.Icons = "/Content/images/" + data.Icons + ".png";
}
//Page view model


ErucaCRM.User.ViewLead.pageViewModel = function () {
    //Class variables
    var self = this;
    self.Core = ErucaCRM.Framework.Core;
    var objLeadInfo = new Object();
    objLeadInfo.LeadId = $("#LeadId").val();
    objLeadInfo.CurrentPage = currentPage;
    objLeadInfo.FilterParameters = new Array();
    var PagingMethodName = "GetPageData";

    var controllerUrl = "/User/";
    var Message = {
        Failure: 'Failure',
        Success: 'Success'
    }
    ErucaCRM.Framework.Core.ApplyCultureValidations();
    self.LeadHistoryList = ko.observableArray([]);
    self.LeadListHistoryQueryModel = ko.observableArray();
    self.GetPaging = function (Rowcount, currentPage, methodName) {
        return ErucaCRM.Framework.Core.GetPagger(Rowcount, currentPage, methodName);
    }
    $("._addProduct").click(function () {

        self.AddProduct($(this).data("productid"));
    });
    $("._editLead").click(function () {

        var leadId = $(this).attr("data-leadId");
        self.EditLead(leadId);

    })
    $("._deleteContact").click(function () {

        var contactId = $(this).attr("data-contactId");
        var LeadId = $(this).attr("data-leadId");

        if (confirm(ErucaCRM.Messages.Account.ConfirmAccountContactDeleteAction) == true) {
            self.DeleteContact(contactId, LeadId, this);
        }

    });


    $('._deleteTask').click(function () {
        var taskId = $(this).attr("data-taskId");
        var leadId = $('#LeadId').val();

        if (confirm(ErucaCRM.Messages.Account.ConfirmAccountContactDeleteAction) == true) {
            self.DeleteContact(taskId);
        }



    });


    $("#LeadDetailEditingMode").on("click", "._backToDetail", function () {
        $("#LeadDetailEditingMode").css("display", "none");
        $("#LeadDetail").css("display", "block");
    });
    $("._removeProduct").click(function () {
        self.RemoveProduct($(this).data("productid"));
    });
    $("._deleteQuote").click(function () {
        if (confirm(ErucaCRM.Messages.Quote.ConfirmQuoteDeleteAction)) {
            self.RemoveQuote($(this).attr("quoteId"), this);
        }
    });
    $('._close-Popup-Movestage').click(function () {
        $('._MoveStagePopup').fadeOut("slow");
    });
    $("._OpenMoveStagePop").click(function () {
        var position = $(this).position();
        $('._RatingPopup').fadeOut("slow");
        $("._MoveStagePopup").fadeIn("slow");
    });

    self.DeleteContact = function (contactId, leadId, e) {

        var PostData = new Object();
        PostData.contactId_encrypted = contactId;
        PostData.leadId_encrypted = leadId;
        ErucaCRM.Framework.Core.doDeleteOperation(controllerUrl + "DeleteLeadContact", PostData,
        function SuccessCallBack(response) {
            if (response.Status == "Success") {
                ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Account.RecordDeletedSuccess, false);
                $(e).parent().parent().remove();

                if ($("#AccountContacts").children("tbody").children("tr [rowType=data]").length == 0) {
                    var HtmlNoRecords = "<span><b> " + ErucaCRM.Messages.Account.NoAccountContactRecordFound + "</b></span>";
                    $("#AccountContacts").children("tbody").children("tr [id=trNoAccountContactsRecord]").children("td").html(HtmlNoRecords);
                }
            }
            else {
                ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Account.RecordDeletionFailure, true);
            }
        }, function FailureCallback() { })
    }

    self.DeleteTaskItem = function (taskID) {
        if (confirm(ErucaCRM.Messages.Account.ConfirmAccountActivityDeleteAction)) {
            var LeadId = $('#LeadId').val();
            var PostData = new Object();
            PostData.taskId_encrypted = taskID;
            PostData.LeadId_encrypted = LeadId;
            ErucaCRM.Framework.Core.doDeleteOperation(controllerUrl + "DeleteLeadActivity", PostData,
            function SuccessCallBack(response) {
                if (response.Status == "Success") {
                    ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Account.RecordDeletedSuccess, false);
                    self.getTaskItemList(objLeadInfo);
                }
                else {
                    ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Account.RecordDeletionFailure, true);
                }
            }, function FailureCallback() { })


        }
    }

    self.EditLead = function (leadId) {

        var PostData = new Object();
        PostData.Id_encrypted = leadId;
        ErucaCRM.Framework.Core.doPostOperation
             (
                 "/User/" + "EditLeadDetail",
                 PostData,
                 function onSuccess(response) {
                     $.validator.unobtrusive.parse('._leadDetails');
                     $("#LeadDetail").css("display", "none");
                     $("#LeadDetailEditingMode").css("display", "block");
                     $("#LeadDetailEditingMode").html(response);

                     $.validator.unobtrusive.parse('#LeadDetailEditingMode');
                     ErucaCRM.Framework.Common.ApplyPermission();
                 });
        self.changeleadBydropdownstage = function (ChangeStageInfo) {
            ErucaCRM.Framework.Core.doPostOperation
                 (
                     controllerUrl + "ChangeLeadStage",
                     ChangeStageInfo,
                     function onSuccess(response) {
                         if (response.response.Status == "Success") {
                             $("._LeadList").find("div[leadid='" + ChangeStageInfo.leadId_encrypted + "']").remove();
                             self.LeadListDetailQueryModel = ko.observableArray([]);
                             var val = new Object();
                             val.Name = ChangeStageInfo.LeadName;
                             val.LeadId = ChangeStageInfo.leadId_encrypted;
                             val.StageId = ChangeStageInfo.toStage_encrypted;
                             val.Rating = response.LeadRating;
                             var LeadsArrary = ko.utils.arrayFilter(self.LeadList(), function (item) {
                                 if (item.StageId == ChangeStageInfo.toStage_encrypted) {
                                     return item;
                                 }
                             });
                             LeadsArrary[0].Leads.splice(0, 0, new ErucaCRM.User.Leads.LeadListDetailQueryModel(val));
                             $('._IsLastStage').css("display", "none");
                             ErucaCRM.Framework.Core.ShowMessage("Stage has been changed Successfully.", false);
                         } else { ErucaCRM.Framework.Core.ShowMessage("Sorry try again.", false); }
                     },
            function onError(err) {
                self.status(err.Message);
            }
            );
        }
    }

    $('._leadstatus').click(function () {
        var leadstatus = $(this).data("leadstatus");
        var moveStageId = $('._stagedropdown').val();
        var leadId = $('#LeadId').val();
        var leadName = $('#Name').val();
        var FromStageId = $('#StageId').val();
        var ChangeStageInfo = new Object();
        var IslastStage = $('#MainStageDiv').find('div[id= "' + moveStageId + '"]').attr("islaststage");
        ChangeStageInfo.leadId_encrypted = leadId;
        ChangeStageInfo.LeadName = leadName;
        ChangeStageInfo.fromStage_encrypted = FromStageId;
        ChangeStageInfo.toStage_encrypted = moveStageId;
        self.changeleadBydropdownstage(ChangeStageInfo);
        $('._draggable').draggable({
            revert: true, helper: 'clone',
            tolerance: 'fit'
        });
    });

    $('._close-Popup-RatingPop').click(function () {
        $('._RatingPopup').fadeOut("slow");
    });

    $("._OpenReatingPop").click(function () {
        $("._MoveStagePopup").fadeOut("slow");
        $('._RatingPopup').fadeIn("slow");

    });


    //$('._stars').click(function () {
      
    //    var Rating = $(this).data("rating");
    //    var ExpectedRevenuePer = $(this).data("expectedrevenue");
    //    var Amount = $('#Amount').val();
    //    var ExpectedRevenue = Amount * (ExpectedRevenuePer / 100);
        
    //    var NewImage = $(this).html();
    //    var RatingConstant = undefined;
    //    var RatingInfo = new Object();
    //    RatingInfo.leadId_encrypted = $("#LeadId").val();
    //    RatingInfo.ratingId_encrypted = Rating;
    //    RatingConstant = $(this).data('ratingconstant');
    //    if (RatingConstant == 5) {//5 is for final
    //        ErucaCRM.Framework.Common.OpenPopup('div._IsLeadLastRating');
    //        $('._leadWinFromRating').show();
    //        $('._leadFromStageMove').hide();
    //        $('._IsLeadLastRating').attr({ "LeadId": $("#LeadId").val(), "ratingId": Rating });
    //    }
    //    else {
    //         self.ChangeLeadRating(RatingInfo)
    //      }
         
    //            }    
    //   );      


    $('._stars').click(function () {
        var Rating = $(this).data("rating");
        var ExpectedRevenuePer = $(this).data("expectedrevenue");
        var Amount = $('#Amount').val();
        var ExpectedRevenue = Amount * (ExpectedRevenuePer / 100);
        var NewImage = $(this).html();
        var RatingConstant = undefined;
        var RatingInfo = new Object();
        RatingInfo.leadId_encrypted = $("#LeadId").val();
        RatingInfo.ratingId_encrypted = Rating;
        RatingConstant = $(this).data('ratingconstant');
        if (RatingConstant == 5) {//5 is for final
            ErucaCRM.Framework.Common.OpenPopup('div._IsLeadLastRating');
            $('._leadratingstatus').attr({ "LeadId": $("#LeadId").val(), "RatingId": Rating ,"NewImage":NewImage,"StageId":$('#StageId').val() ,"FinalStageId":$('#FinalStageId').val(),"LeadTitle":$('#Title').val()});
        }
        else {
            self.ChangeLeadRating(RatingInfo, NewImage);
            $('#ExpectedRevenueAmount').text(ExpectedRevenue);
        }

    })
  


   

    self.ChangeLeadRating = function (RatingInfo,NewImage) {

        ErucaCRM.Framework.Core.doPostOperation
         (
         controllerUrl + "ChangeLeadRating",
                 RatingInfo,
              function onSuccess(response) {

                  if (response.Status == "Success") {
                      $('._SingleRating').children().remove();
                      $('._SingleRating').append('<div class="stars">' + NewImage + '</div>');
                      $("._LeadList").find("div[leadid='" + $("#LeadId").val() + "']").find("._ratingicon").remove();
                      $("._LeadList").find("div[leadid='" + $("#LeadId").val() + "']").find('._leadiconcontainer').prepend(NewImage).find("img").addClass("ratingicon _ratingicon");
                      $('._RatingPopup').fadeOut("slow");
                      var LeadInfo = new Object();
                      LeadInfo.leadId_encrypted = $("#LeadId").val();
                      LeadInfo.CurrentPageNo = 1;
                      self.GetLeadHistory(LeadInfo);
                  }
              }
     );


    }


    $('._deleteInvoice').click(function () {
        if (confirm(ErucaCRM.Messages.Invoice.ConfirmInvoiceDeleteAction)) {
            self.DeleteInvoice($(this).attr("invoiceId"), this);
        }
    });
    $('._deleteSalesOrder').click(function () {
        if (confirm(ErucaCRM.Messages.SalesOrder.ConfirmSalesOrderDeleteAction)) {
            self.DeleteSalesOrder($(this).attr("salesOrderId"), this);
        }

    })

    //$('._deleteLeadDocs').click(function () {
    //    var DocId = $(this).data("docId");
    //    self.DeleteLeadDocs(DOcId)
    //});

    this.AssociatedProductsModels = ko.observableArray([]);
    this.AllProductsModels = ko.observableArray([]);

    //
    self.TaskItemListQueryModel = ko.observableArray();
    self.queryModel = ko.observable();
    self.status = ko.observable();
    this.TaskItemList = ko.observableArray([]);



    self.changeleadBydropdownstage = function (ChangeStageInfo) {
        ErucaCRM.Framework.Core.doPostOperation
             (
                 controllerUrl + "ChangeLeadStage",
                 ChangeStageInfo,
                 function onSuccess(response) {

                     if (response.response.Status == "Success") {
                         $("._LeadList").find("div[leadid='" + ChangeStageInfo.leadId_encrypted + "']").remove();
                         self.LeadListDetailQueryModel = ko.observableArray([]);
                         var val = new Object();
                         val.Name = ChangeStageInfo.LeadName;
                         val.LeadId = ChangeStageInfo.leadId_encrypted;
                         val.StageId = ChangeStageInfo.toStage_encrypted;
                         val.Rating = response.LeadRating;
                         var LeadsArrary = ko.utils.arrayFilter(self.LeadList(), function (item) {
                             if (item.StageId == ChangeStageInfo.toStage_encrypted) {
                                 return item;
                             }
                         });
                         LeadsArrary[0].Leads.splice(0, 0, new ErucaCRM.User.Leads.LeadListDetailQueryModel(val));
                         $('._IsLastStage').fadeOut("slow");
                         ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Lead.StageChangeSuccessfully, false);
                     } else { ErucaCRM.Framework.Core.ShowMessage("Sorry try again.", false); }
                 }
             );
    }

    self.DeleteLeadDocs = function () {


    }

    self.getTaskItemList = function (objLeadInfo) {
        self.TaskListList = ko.observableArray([]);
        ErucaCRM.Framework.Core.getJSONDataBySearchParam(controllerUrl + "GetLeadTasks", objLeadInfo, function onSuccess(response) {
            self.RenderTaskItems(response);
            ErucaCRM.Framework.Common.ApplyPermission('._roleselector');

        }, function onError(err) {
            ErucaCRM.Framework.Core.ShowMessage(err.Message, true);
        });

    }
    self.RenderTaskItems = function (TaskItemListData) {

        $("#divNoRecord").hide();
        $("#TaskItemListData").children().remove();
        if (TaskItemListData.List.length == 0) {
            $("#divNoRecord").html("<br/><b>" + ErucaCRM.Messages.Account.NoAccountActivityRecordFound + "</b>");
            $("#divNoRecord").show();

        }

        ko.utils.arrayForEach(TaskItemListData.List, function (TaskItem) {
            self.TaskItemList.push(new ErucaCRM.User.ViewLead.TaskItemListQueryModel(TaskItem));
        });

        //$("#Pager").html(self.GetPaging(TaskItemListData.TotalRecords, currentPage, PagingMethodName));
    };
    self.stars = function () {
        $('span.stars').each(function () {
            var val = parseFloat($(this).html());
            var size = Math.max(0, (Math.min(5, val))) * 16;
            var $span = $('<span />').width(size);
            $(this).html($span);
        });
    }
    //
    self.GetPageData = function (currentPageNo) {

        currentPage = currentPageNo;
        var objTaskItemInfo = new Object();
        objTaskItemInfo.CurrentPage = currentPageNo;
        objTaskItemInfo.FilterParameters = new Array();
        self.getTaskItemList(objTaskItemInfo);

    }
    self.AddProduct = function (ProductId) {
        var postData = new Object();
        postData.ProductId = ProductId;
        postData.LeadId = $("input[type='hidden']#LeadId").val();
        self.Core.doPostOperation(controllerUrl + "AddProductToLead", postData, function onSuccess(response) {
            self.Core.ShowMessage(ErucaCRM.Messages.Product.ProductAddedSuccess, false);
            this.AssociatedProductsModels = ko.observableArray([]);
            var $productRow = $("#allProducts tr#row_" + ProductId);
            postData.ProductName = $.trim($productRow.find("td:eq(1)").text());
            postData.ProductCode = $.trim($productRow.find("td:eq(2)").text());
            self.AssociatedProductsModels.push(ErucaCRM.User.ViewLead.ProductsModel(postData));
            $productRow.remove();
            $("._close").click();
        });
    }

    self.RemoveProduct = function (productId) {
        var postData = new Object();
        postData.ProductId = productId;
        postData.LeadId = $("input[type='hidden']#LeadId").val();
        self.Core.doPostOperation(controllerUrl + "RemoveProductFromLead", postData, function onSuccess(response) {
            self.Core.ShowMessage(ErucaCRM.Messages.Product.ProductRemovedSuccess, false);
            this.AllProductsModels = ko.observableArray([]);
            var $productRow = $("#leadProducts tr#row_" + postData.ProductId);
            postData.ProductName = $.trim($productRow.find("td:eq(1)").text());
            postData.ProductCode = $.trim($productRow.find("td:eq(2)").text());
            self.AllProductsModels.push(ErucaCRM.User.ViewLead.ProductsModel(postData));
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
            self.AllProductsModels.push(ErucaCRM.User.ViewLead.ProductsModel(postData));
            $("#addNewProductSection ._close").click();
        })

    };
    self.RemoveQuote = function (quoteId, e) {
        var postData = new Object();
        postData.Id_encrypted = quoteId;
        self.Core.doPostOperation(controllerUrl + "DeleteQuote", postData, function onSuccess(response) {
            self.Core.ShowMessage(ErucaCRM.Messages.Quote.QuoteDeletedSuccess, false);
            $(e).parent().parent().hide();
            //this.AllProductsModels = ko.observableArray([]);
            //var $productRow = $("#leadProducts tr#row_" + postData.ProductId);
            //postData.ProductName = $.trim($productRow.find("td:eq(1)").text());
            //postData.ProductCode = $.trim($productRow.find("td:eq(2)").text());
            //self.AllProductsModels.push(ErucaCRM.User.ViewLead.ProductsModel(postData));
            //$productRow.remove();
        });


    }
    var uploadObj;
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
            //$('_Attachments').append('<tr><td><a target=_blank href="' + data.FilePath + '">' + data.FileName + '</a></td><td>' + data.AttachedBy + '</td><td> <a docid=' + data.DocId + ' class="_deleteDoc" href="javascript:void(0)">delete</a></td></tr>');

            $('._Attachments').append('<div class="attachment-thumbnail"><a class="attachment-thumbnail-preview js-real-link" ' + 'title="ContentManagement.cshtml" target="_blank" href="#"><span class="attachment-thumbnail-preview-ext">' + data.FileName + '</span></a><p class="attachment-thumbnail-details ">' + data.FileName + '<span class="attachment-thumbnail-details-options"><a class="icon-sm icon-movedown light attachment-thumbnail-details-options-item js-download download" target="_blank" href="' + data.FilePath + '" title="Download"></a><span docid=' + data.DocId + '   class="icon-sm icon-close light attachment-thumbnail-details-options-item attachment-thumbnail-details-options-item-delete js-confirm-delete _deleteLeadDocs" title="Delete">X</span></span></div>')


            $('div.hidden').fadeOut();
        },
        afterUploadAll: function () {

        },
        onError: function (files, status, errMsg) {

        }
    });
    $("._adddoc").bind("click", function () {
        $('._RatingPopup').fadeOut("slow");
        $("._MoveStagePopup").fadeOut("slow");
        ErucaCRM.Framework.Core.OpenRolePopup("#FileUploadSection");
    });

    $("#btnAddProduct").bind("click", function () {
        $('._RatingPopup').fadeOut("slow");
        $("._MoveStagePopup").fadeOut("slow");
        ErucaCRM.Framework.Core.OpenRolePopup("#AddProductSection");
    });

    $("#btnAddNewProduct").bind("click", function () {
        $(':text').each(function () { $(this).val(""); })
        ErucaCRM.Framework.Core.OpenRolePopup("#addNewProductSection");
    });



    $("._attachDoc").bind("click", function () {
        uploadObj.startUpload();
    });
    $("#LeadDocs").on("click", "._deleteLeadDocs", function () {

        var response = confirm(ErucaCRM.Messages.Document.ConfirmDocumentDeleteAction);
        if (!response)
            return false;

        var actionName = "RemoveLeadDocument";
        $td = this;
        var docid = $($td).attr("docid");
        var obj = new Object();
        obj.DocumentId = docid;
        ErucaCRM.Framework.Core.doPostOperation
               (
                   controllerUrl + actionName,
                   obj,
                   function onSuccess(response) {
                       if (response.Status == Message.Failure) { ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Messages.Document.DocumentNotRemoved, true); }
                       else if (response.Status == Message.Success) {
                           var CurrentDocDiv = $($td).parent('span').parent('p').parent('div');//closest('tr');
                           CurrentDocDiv.css("background-color", "#FF3700");
                           CurrentDocDiv.fadeOut(400, function () {
                               CurrentDocDiv.remove();
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
        obj.LeadId = $("._leadId").find("input[type=hidden]").val();
        return obj;
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
    self.GetLeadHistory = function (LeadInfo) {
        ErucaCRM.Framework.Core.doPostOperation
            (
                controllerUrl + "GetLeadHistory",
                LeadInfo,
                 function onSuccess(response) {
                     if (response.Response == "Success") {
                         $('#LeadHistoryData').children().remove();
                         ko.utils.arrayForEach(response.LeadHistory, function (list) {
                             self.LeadHistoryList.push(new ErucaCRM.User.ViewLead.LeadListHistoryQueryModel(list));
                         });
                         $("#Pager").html(self.GetPaging(response.TotalRecords, LeadInfo.CurrentPageNo, 'GetLeadHistorybyPaging'));
                     }
                 }
            );
    }

    self.GetLeadHistorybyPaging = function (pageno) {
        var LeadInfo = new Object();
        LeadInfo.leadId_encrypted = $("#LeadId").val();
        LeadInfo.CurrentPageNo = pageno;
        self.GetLeadHistory(LeadInfo);
    }

    self.GetLeadRating = function () {
        ErucaCRM.Framework.Core.doPostOperation
            (
            controllerUrl + "GetLeadRating",
                    "",
                 function onSuccess(response) {
                     if (response.Response == "Success") {

                         ko.utils.arrayForEach(response.Ratings, function (list) {
                             self.LeadRatingList.push(new ErucaCRM.User.ViewLead.LeadRatingQueryModel(list));
                         });
                     }
                 }
        );
    }

    var LeadInfo = new Object();
    LeadInfo.leadId_encrypted = $("#LeadId").val();
    LeadInfo.CurrentPageNo = 1;
    self.GetLeadHistory(LeadInfo);
    self.getTaskItemList(objLeadInfo);
}

