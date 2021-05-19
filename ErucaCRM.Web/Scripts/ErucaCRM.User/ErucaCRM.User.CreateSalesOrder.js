//Register name space
jQuery.namespace('ErucaCRM.User.CreateSalesOrder');
var viewModel;
var updateOrderAmount = false;
ErucaCRM.User.CreateSalesOrder.pageLoad = function () {
    updateOrderAmount = false;
    ErucaCRM.Framework.Core.SetGlobalDatepicker('DueDate');
    viewModel = new ErucaCRM.User.CreateSalesOrder.pageViewModel();
    ko.applyBindings(viewModel, document.getElementById("InnerContentContainer"));


}

// A view model that represent a Test report query model.
ErucaCRM.User.CreateSalesOrder.createSalesOrderViewModel = function (queryModel) {
    var self = this;
    self.OwnerId = queryModel.OwnerId;
    self.DueDate = queryModel.DueDate;
    self.Subject = queryModel.Subject;
    self.SalesCommission = queryModel.SalesCommission;
    self.Carrier = queryModel.Carrier;
    self.SubTotal = queryModel.SubTotal;
    self.DiscountAmount = queryModel.DiscountAmount;
    self.DiscountType = queryModel.DiscountType
    self.TaxApplied = queryModel.TaxApplied;
    self.TaxAmount = queryModel.TaxAmount;
    self.VatApplied = queryModel.VatApplied;
    self.VatAmount = queryModel.VatAmount;
    self.AdjustmentType = queryModel.AdjustmentType;
    self.AdjustmentAmount = queryModel.AdjustmentAmount;
    self.GrandTotal = queryModel.GrandTotal;
    self.QuoteId = queryModel.QuoteId;
    self.LeadId = queryModel.LeadId;
    self.Address = new Object();
    self.Address.Street = queryModel.Street;
    self.Address.City = queryModel.City;
    self.Address.State = queryModel.State;
    self.Address.ZipCode = queryModel.ZipCode;
    self.Address.CountryId = queryModel.CountryId;
    self.Description = queryModel.Description;
    self.ProductSalesOrderAssociations = new Object();
    //self.ProductQuoteAssociations.AssociatedProduct = new Object();

}

//Page view model
ErucaCRM.User.CreateSalesOrder.pageViewModel = function (hasAccess) {
    //Class variables
    var self = this;
    var controllerUrl = "/User/";
    var Message = {
        Failure: 'Failure',
        Success: 'Success'
    }
    self.Ddldiscount =
        {
            NoDiscount: '1',
            PercOfPrice: '2',
            FixedPrice: '3'
        }
    self.Operator = {

        Plus: '1',
        Minus: '2'
    }
    self.CustomeRowCounter = 0;
    updateOrderAmount = false;
    $('._infoheader').on("click", "._addproduct", function () {
        self.CustomeRowCounter += 1;
        self.ProductQuoteAssociations.push(self.createSalesOrderProductDetailViewModel());

    });



    $('#salesOrderProduct').on("change", "._productList", function () {

        var ddlproduct = $(this);
        var selectedProduct = ddlproduct.val();
        if (selectedProduct > 0) {
            var obj = new Object();
            obj.Id = selectedProduct;
            ErucaCRM.Framework.Core.getJSONDataBySearchParam
                (
                    controllerUrl + "GetProduct", obj,
                    function onSuccess(response) {
                        var $Row = ddlproduct.closest('tr');
                        self.setRowData(response, $Row);
                    },
                    function onError(err) {
                        self.status(err.Message);
                    }
                );
        }
    });
    $("._saveQuote").on("click", function () {
        //var obj = self.GetPermissions();
        self.saveQuote();
    });
    $("#salesOrderProduct").on("blur", "._txtQty", function () {
        var txt = $(this);
        var $Row = txt.closest('tr');
        self.doAllCalculations($Row);
    });
    $("#salesOrderProduct").on("blur", "._txtListPrice", function () {
        var txt = $(this);
        var $Row = txt.closest('table').closest('tr');
        self.doAllCalculations($Row);
    });
    $("#salesOrderProduct").on("blur", "._caltxtdiscount, ._txtdiscount", function () {
        var $Row = $(this).closest('table').closest('tr');
        self.doAllCalculations($Row);
    });
    $("#maintotal").on("blur", "#txtCalMainDiscount, #txtMainDiscount, #txtAdjustment, #txtTotalAfterDiscount", function () {
        self.doAllCalculations();
    }); //
    $("#maintotal").on("change", "#ddlDiscount", function () {
        var ddldiscount = $(this);
        var selectedvalue = ddldiscount.val();
        switch (selectedvalue) {
            case self.Ddldiscount.PercOfPrice:
                $("#txtCalMainDiscount").attr("readonly", false);
                break;
            default:
                $("#txtCalMainDiscount").attr("readonly", true);
                $("#txtCalMainDiscount").val(0);
                $("#txtMainDiscount").val($("#txtSubTotal").val());
        }
        self.doAllCalculations();
    });
    $("#QuoteId").on("change", function () {

        var selectedvalue = $(this).val();
        self.getQuoteDetail(selectedvalue);
    });
    $("#maintotal").on("change", "#operator", function () {
        self.doAllCalculations();
    });
    $("#salesOrderProduct").on("change", "._drpdiscount", function () {
        var ddldiscount = $(this);
        var selectedvalue = ddldiscount.val();
        var $Row = ddldiscount.closest('table').closest('tr');
        switch (selectedvalue) {
            case self.Ddldiscount.PercOfPrice:
                $Row.find("._caltxtdiscount").attr("readonly", false);
                break;
            default:
                $Row.find("._caltxtdiscount").attr("readonly", true);
                $Row.find("._caltxtdiscount").val(0);
                $Row.find("._txttotalAfterDiscount").val($Row.find("._total").val());
        }
        self.doAllCalculations($Row);
    });
    $("#maintotal").on("click", "#chkSaleTax", function () {
        var chk = $(this);
        var checked = chk.is(":checked");
        var txtSaletax = $("#txtSalesTax");
        if (checked) {

            txtSaletax.attr("readonly", false);
        }
        else {
            txtSaletax.val(0);
            txtSaletax.attr("readonly", true);

        }
        self.doAllCalculations();
    });
    $("#maintotal").on("click", "#chkVat", function () {
        var chk = $(this);
        var checked = chk.is(":checked");
        var txtSaletax = $("#txtVat");
        if (checked) {

            txtSaletax.attr("readonly", false);
        }
        else {
            txtSaletax.val(0);
            txtSaletax.attr("readonly", true);

        }
        self.doAllCalculations();
    });
    $("#OrderAmount").on("change", function () {
        if ($("#txtGrandTotal").val() == 0) {
            $("#txtGrandTotal").val($("#OrderAmount").val());
        }

    })
    $("#salesOrderProduct").on("click", "._chksalestax", function () {
        var chk = $(this);
        var checked = chk.is(":checked");
        var $Row = chk.closest('table').closest('tr');
        if (checked) {
            var txtSaletax = $Row.find("._txtsalestax");
            txtSaletax.attr("readonly", false);
        }
        else {
            var txtSaletax = $Row.find("._txtsalestax");
            txtSaletax.val(0);
            txtSaletax.attr("readonly", true);

        }
        self.doAllCalculations($Row);
    });
    $("#salesOrderProduct").on("click", "._chkvat", function () {
        var chk = $(this);
        var checked = chk.is(":checked");
        var $Row = chk.closest('table').closest('tr');
        if (checked) {
            var txtSaletax = $Row.find("._txtvat");
            txtSaletax.attr("readonly", false);
        }
        else {
            var txtSaletax = $Row.find("._txtvat");
            txtSaletax.val(0);
            txtSaletax.attr("readonly", true);

        }
        self.doAllCalculations($Row);
    });
    $("#maintotal").on("blur", "#txtVat, #txtSalesTax", function () {

        self.doAllCalculations();

    });
    $("#salesOrderProduct").on("blur", "._txtvat, ._txtsalestax", function () {
        var $Row = $(this).closest('table').closest('tr');
        self.doAllCalculations($Row);

    });
    $("#Subject").on("blur", function () {
        if ($.trim($(this).val()) == "") {
            $("._subjectError").text(ErucaCRM.Messages.Quote.SubjectRequired);
            return;
        }
        else { $("._subjectError").text(""); }
    });
    //$("#DueDate").on("blur", function () {

    //    if ($.trim($(this).val()) == "") {
    //        $("._orderdateError").text(ErucaCRM.Messages.SalesOrder.OrderDateRequired);
    //        return;
    //    }
    //    else { $("._orderdateError").text(""); }
    //});

    self.doAllCalculations = function ($Row) {
        var rows = $("#salesOrderProduct tbody .parentRow ");
        $(rows).each(function () {
            var $Row = $(this);
            var productId = $Row.find("._productList").val();
            var listPrice = $Row.find("._txtListPrice").val();
            listPrice = $.isNumeric(listPrice) ? listPrice : 0;
            var qty = $Row.find("._txtQty").val();
            qty = $.isNumeric(qty) ? qty : 0;
            if (productId != undefined && productId != null && productId > 0 && listPrice > 0 && qty > 0) {
                self.calTotal($Row);
                self.calDiscount($Row);
                self.calTax($Row);
            }
        });
        self.calMainTotal();

    }
    self.calTotal = function ($Row) {
        var listPrice = parseFloat($Row.find("._txtListPrice").val());
        var qty = parseFloat($Row.find("._txtQty").val());
        qty = $.isNumeric(qty) == true ? qty : 0;
        listPrice = $.isNumeric(listPrice) == true ? listPrice : 0;
        var total = listPrice * qty
        $Row.find("._total").val(total.toFixed(2));
        $Row.find("._txttotalAfterDiscount").val(total.toFixed(2));
        $Row.find("._txtnetTotal").val(total.toFixed(2));

    }
    self.calDiscount = function ($Row) {
        var ddldiscount = $Row.find("._drpdiscount");
        var totalPrice = parseFloat($Row.find("._total").val());
        totalPrice = $.isNumeric(totalPrice) ? totalPrice : 0;
        var selectedvalue = ddldiscount.val();
        var discountValue = 0;

        var discount = 0;
        var totalAfterDiscount = 0;
        switch (selectedvalue) {
            case self.Ddldiscount.PercOfPrice:
                var txtDiscount = $Row.find("._caltxtdiscount");
                var discountValue = parseFloat(txtDiscount.val());
                discountValue = $.isNumeric(discountValue) ? discountValue : 0;
                discount = totalPrice * (discountValue / 100);
                totalAfterDiscount = totalPrice - discount;
                $Row.find("._txtdiscount").val(discount.toFixed(2));
                $Row.find("._txttotalAfterDiscount").val(totalAfterDiscount.toFixed(2));
                break;
            case self.Ddldiscount.FixedPrice:
                discountValue = parseFloat($Row.find("._txtdiscount").val());
                discountValue = $.isNumeric(discountValue) ? discountValue : 0;
                totalAfterDiscount = totalPrice - discountValue;
                $Row.find("._caltxtdiscount").val(0);
                $Row.find("._txttotalAfterDiscount").val(totalAfterDiscount.toFixed(2));
                break;
            default:
                $Row.find("._caltxtdiscount").attr("readonly", true);
        }
    }
    self.calTax = function ($Row) {
        var chkSale = $Row.find('._chksalestax');
        var chkVat = $Row.find('._chkvat');
        var isSaleChecked = chkSale.is(":checked");
        var isVatChecked = chkVat.is(":checked");
        var vatValue = parseFloat($Row.find('._txtvat').val());
        var saleTaxValue = parseFloat($Row.find('._txtsalestax').val());
        var totalAfterDiscount = parseFloat($Row.find('._txttotalAfterDiscount').val());
        totalAfterDiscount = $.isNumeric(totalAfterDiscount) ? totalAfterDiscount : 0;
        vatValue = $.isNumeric(vatValue) ? vatValue : 0;
        saleTaxValue = $.isNumeric(saleTaxValue) ? saleTaxValue : 0;
        var netTotal = 0;
        var totalVat = 0;
        var totalSaleTax = 0;
        var totalTax = 0;
        if (isVatChecked) {
            totalVat = totalAfterDiscount * (vatValue / 100);
            totalVat = $.isNumeric(totalVat) ? totalVat : 0;

        }
        if (isSaleChecked) {
            totalSaleTax = totalAfterDiscount * (saleTaxValue / 100);
            totalSaleTax = $.isNumeric(totalSaleTax) ? totalSaleTax : 0;
        }
        totalTax = totalVat + totalSaleTax;
        netTotal = totalAfterDiscount + totalTax;
        $Row.find("._txttax").val(totalTax.toFixed(2));
        $Row.find("._txtnetTotal").val(netTotal.toFixed(2));
    }
    self.calSubTotal = function () {

        var grandSubTotal = 0;
        var netTotal = 0;
        var rows = $("#salesOrderProduct tbody .parentRow ");
        $(rows).each(function () {
            var $Row = $(this);
            netTotal = parseFloat($Row.find("._txtnetTotal").val());
            netTotal = $.isNumeric(netTotal) == true ? netTotal : 0;
            grandSubTotal += netTotal;
        });
        $("#maintotal").find("#txtSubTotal").val(grandSubTotal.toFixed(2));

        $("#maintotal").find("#txtTotalAfterDiscount").val(grandSubTotal.toFixed(2));
        $("#maintotal").find("#txtGrandTotal").val(grandSubTotal.toFixed(2));


    }
    self.calMainDiscount = function () {
        var ddldiscount = $("#ddlDiscount");
        var totalPrice = parseFloat($("#txtSubTotal").val());
        totalPrice = $.isNumeric(totalPrice) ? totalPrice : 0;
        var selectedvalue = ddldiscount.val();
        var discountValue = 0;
        var discount = 0;
        var totalAfterDiscount = 0;
        switch (selectedvalue) {
            case self.Ddldiscount.PercOfPrice:
                var txtDiscount = $("#txtCalMainDiscount");

                discountValue = parseFloat(txtDiscount.val());
                discountValue = $.isNumeric(discountValue) ? discountValue : 0;
                discount = totalPrice * (discountValue / 100);
                totalAfterDiscount = totalPrice - discount;
                $("#txtMainDiscount").val(discount.toFixed(2));
                $("#txtTotalAfterDiscount").val(totalAfterDiscount.toFixed(2));
                break;
            case self.Ddldiscount.FixedPrice:
                discountValue = parseFloat($("#txtMainDiscount").val());
                discountValue = $.isNumeric(discountValue) ? discountValue : 0;
                totalAfterDiscount = totalPrice - discountValue;
                $("#txtCalMainDiscount").val(0);
                $("#txtTotalAfterDiscount").val(totalAfterDiscount.toFixed(2));
                break;
            default:
                $("#txtCalMainDiscount").attr("readonly", true);
        }
    }
    self.calMainTax = function () {
        var chkSale = $('#chkSaleTax');
        var chkVat = $('#chkVat');
        var isSaleChecked = chkSale.is(":checked");
        var isVatChecked = chkVat.is(":checked");
        var vatValue = parseFloat($('#txtVat').val());
        var saleTaxValue = parseFloat($('#txtSalesTax').val());
        var totalAfterDiscount = parseFloat($('#txtTotalAfterDiscount').val());
        totalAfterDiscount = $.isNumeric(totalAfterDiscount) ? totalAfterDiscount : 0;
        vatValue = $.isNumeric(vatValue) ? vatValue : 0;
        saleTaxValue = $.isNumeric(saleTaxValue) ? saleTaxValue : 0;
        var netTotal = 0;
        var totalVat = 0;
        var totalSaleTax = 0;
        var totalTax = 0;
        if (isVatChecked) {
            totalVat = totalAfterDiscount * (vatValue / 100);
            totalVat = $.isNumeric(totalVat) ? totalVat : 0;
        }
        if (isSaleChecked) {
            totalSaleTax = totalAfterDiscount * (saleTaxValue / 100);
            totalSaleTax = $.isNumeric(totalSaleTax) ? totalSaleTax : 0;
        }
        totalTax = totalVat + totalSaleTax;
        netTotal = totalAfterDiscount + totalTax;
        $("#txtTax").val(totalTax.toFixed(2));
        $("#txtGrandTotal").val(netTotal.toFixed(2));
    }
    self.calAdjustment = function () {
        var grandTotal = parseFloat($("#txtGrandTotal").val());
        grandTotal = $.isNumeric(grandTotal) ? grandTotal : 0;
        var adjustment = parseFloat($("#txtAdjustment").val());
        adjustment = $.isNumeric(adjustment) ? adjustment : 0;
        var selectedOperator = $("#operator").val();
        switch (selectedOperator) {
            case self.Operator.Plus:
                grandTotal = grandTotal + adjustment;
                $("#txtGrandTotal").val(grandTotal.toFixed(2));
                break;
            case self.Operator.Minus:
                grandTotal = grandTotal - adjustment;
                $("#txtGrandTotal").val(grandTotal.toFixed(2));
                break;
            default:

        }

    }
    self.calMainTotal = function () {
        self.calSubTotal();
        self.calMainDiscount();
        self.calMainTax();
        self.calAdjustment();

        if (updateOrderAmount == true) {
            $("#OrderAmount").val($("#txtGrandTotal").val());
        }
        else
            $("#txtGrandTotal").val($("#OrderAmount").val());
        updateOrderAmount = true;
    }
    self.queryModel = ko.observable();
    self.status = ko.observable();

    self.saveQuote = function () {
        if ($("#OwnerId").val() == 0) {
            alert(ErucaCRM.Messages.Quote.OwnerRequired);
            return;
        }

        if ($.trim($("#Subject").val()) == "") {

            $("._subjectError").text(ErucaCRM.Messages.Quote.SubjectRequired);
            return;
        }
        //if ($.trim($("#DueDate").val()) == "") {

        //    $("._orderdateError").text(ErucaCRM.Messages.SalesOrder.OrderDateRequired);
        //    return;
        //}
        if ($("#AccountId option:selected").text().trim() == "---Select---") {
            alert(ErucaCRM.Messages.Lead.LeadRequired);
            return;
        }
        self.getQuote();
        if (self.ProductSalesOrderAssociations != undefined && self.ProductSalesOrderAssociations != null && self.ProductSalesOrderAssociations.length > 0 || (parseFloat($("#OrderAmount").val()) > 0.0)) {
            ErucaCRM.Framework.Core.doPostOperation
                    (
                        controllerUrl + "CreateSalesOrder",
                        self,
                        function onSuccess(response) {
                            if (response.Status == Message.Failure) { $(".error").text(response.Message).removeClass("msgssuccess") }
                            else if (response.Status == Message.Success) {
                                $(".error").text(response.Message).addClass("msgssuccess");
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
                                //if ($("#SaleOrderAccountId").length == 0) {

                                //    window.location.href = '/User/SaleOrders';
                                //}
                                //else {

                                //    window.location.href = '/User/AccountDetail/' + $("#SaleOrderAccountId").val();
                                //}

                            }
                        },
                        function onError(err) {
                            self.status(err.Message);
                        }
                    );
        }
        else {
            alert(ErucaCRM.Messages.Quote.SelectProductAndFillAllField);
        }
    }
    self.getQuote = function () {

        self.SalesOrderId = $("#SalesOrderId").val();
        self.OwnerId = $("#OwnerId").val();
        var validTill = $("#DueDate").datepicker('getDate');
        self.DueDate = validTill;
        self.Subject = $("#Subject").val();
        self.DueDate = $("#DueDate").val();
        self.QuoteId = $("#QuoteId").val();
        self.OrderAmount = $("#OrderAmount").val();
        if (parseInt(self.QuoteId) == 0)
            self.QuoteId = null;

        self.SalesCommission = $("#SalesCommission").val();
        self.Carrier = $("#Carrier").val();
        self.SubTotal = $("#txtSubTotal").val();
        var discountType = $("#ddlDiscount").val();
        if (discountType == self.Ddldiscount.FixedPrice) {
            self.DiscountAmount = $("#txtMainDiscount").val();
        }
        else if (discountType == self.Ddldiscount.PercOfPrice) {
            self.DiscountAmount = $("#txtCalMainDiscount").val();
        }
        else {
            self.DiscountAmount = 0;
        }
        self.DiscountType = $("#ddlDiscount").val();
        self.TaxApplied = $("#chkSaleTax").is(":checked");
        self.TaxAmount = $("#txtSalesTax").val();
        self.VatApplied = $("#chkVat").is(":checked");
        self.VatAmount = $("#txtVat").val();
        self.AdjustmentType = $("#operator").val();
        self.AdjustmentAmount = $("#txtAdjustment").val();
        self.GrandTotal = $("#txtGrandTotal").val();
        //self.LeadId = $("#LeadId").val();
        self.AccountId = $('#AccountId').val();

        if (parseInt(self.LeadId) == 0)
            self.LeadId = null;

        self.Address = new Object();
        self.Address.Street = $("#Address_Street").val();
        self.Address.City = $("#Address_City").val();
        self.Address.State = $("#Address_State").val();
        self.Address.ZipCode = $("#Address_Zipcode").val();
        self.Address.CountryId = $("#Address_CountryId").val();

        self.Address1 = new Object();
        self.Address1.Street = $("#Address1_Street").val();
        self.Address1.City = $("#Address1_City").val();
        self.Address1.State = $("#Address1_State").val();
        self.Address1.Zipcode = $("#Address1_Zipcode").val();
        self.Address1.CountryId = $("#Address1_CountryId").val();
        self.Description = $("#Description").val();
        self.ProductSalesOrderAssociations = self.getQuoteProducts();
    }
    self.getQuoteProducts = function () {
        self.ProductSalesOrderAssociations = new Array();

        var rows = $("#salesOrderProduct tbody .parentRow ");
        $(rows).each(function () {

            var $Tr = $(this);
            var productId = $Tr.find("._productList").val();
            var listPrice = $Tr.find("._txtListPrice").val();
            listPrice = $.isNumeric(listPrice) ? listPrice : 0;
            var qty = $Tr.find("._txtQty").val();
            qty = $.isNumeric(qty) ? qty : 0;
            if (productId != undefined && productId != null && productId > 0 && listPrice > 0 && qty > 0) {
                var objSalesOrderProduct = new Object();
                objSalesOrderProduct.AssociationSalesOrderId = $Tr.find("._hiddenAssociationSalesOrderId").val();
                objSalesOrderProduct.AssociatedProductId = $Tr.find("._hiddenAssociatedProductId").val();
                objSalesOrderProduct.SalesOrderId = $("#SalesOrderId").val();
                objSalesOrderProduct.AssociatedProduct = new Object;
                objSalesOrderProduct.AssociatedProduct.AssociatedProductId = $Tr.find("._hiddenAssociatedProductId").val();
                objSalesOrderProduct.AssociatedProduct.ProductId = productId;
                objSalesOrderProduct.AssociatedProduct.QtyInStock = $Tr.find("._txtQtyStock").val();
                objSalesOrderProduct.AssociatedProduct.Quantity = qty;
                objSalesOrderProduct.AssociatedProduct.UnitPrice = $Tr.find("._txtUnitPrice").val();
                objSalesOrderProduct.AssociatedProduct.ListPrice = listPrice;
                var discountType = $Tr.find("._drpdiscount").val();
                if (discountType == self.Ddldiscount.FixedPrice) {
                    objSalesOrderProduct.AssociatedProduct.DiscountAmount = $Tr.find("._txtdiscount").val();
                }
                else if (discountType == self.Ddldiscount.PercOfPrice) {
                    objSalesOrderProduct.AssociatedProduct.DiscountAmount = $Tr.find("._caltxtdiscount").val();
                }
                else {
                    objSalesOrderProduct.AssociatedProduct.DiscountAmount = 0;
                }
                objSalesOrderProduct.AssociatedProduct.DiscountType = discountType;
                objSalesOrderProduct.AssociatedProduct.TaxApplied = $Tr.find("._chksalestax").is(":checked");
                objSalesOrderProduct.AssociatedProduct.TaxAmount = $Tr.find("._txtsalestax").val();
                objSalesOrderProduct.AssociatedProduct.VatApplied = $Tr.find("._chkvat").is(":checked");
                objSalesOrderProduct.AssociatedProduct.VatAmount = $Tr.find("._txtvat").val();
                self.ProductSalesOrderAssociations.push(objSalesOrderProduct);
            }
        });
        return self.ProductSalesOrderAssociations;
    }
    self.setRowData = function (rowData, $Row) {
        $Row.find("._txtQtyStock").val(rowData.QuantityInStock);
        $Row.find("._txtUnitPrice").val(rowData.UnitPrice);
    }
    self.getQuoteDetail = function (quoteId) {
        var obj = new Object();
        obj.Id_encrypted = quoteId;
        ErucaCRM.Framework.Core.getJSONDataBySearchParam
               (
                   controllerUrl + "GetQuoteDetail", obj,
                   function onSuccess(response) {

                       self.bindProducts(response);

                   },
                   function onError(err) {
                       self.status(err.Message);
                   }
               );
    }

    self.ProductQuoteAssociations = ko.observableArray([]);
    self.ProductQuote = ko.observableArray([]);
    self.bindProducts = function (quoteData) {
        //self.ProductQuoteAssociations.removeAll();
        self.ProductQuote.removeAll();
        self.ProductQuoteAssociations.remove(function (item) { return item.Identity == "Identity" })
        var saleOrderId = $("#SalesOrderId").val();
        if (saleOrderId == undefined || saleOrderId == null || parseInt(saleOrderId) == 0 || saleOrderId == "") {
            self.ProductQuoteAssociations.removeAll();
            self.ProductQuote.push(self.createSalesOrderMainCalViewModel(quoteData));
        }

        $.each(quoteData.ProductQuoteAssociations, function (idx, item) {
            ;
            self.ProductQuoteAssociations.push(self.createSalesOrderProductDetailViewModel(item));
        });
        self.doAllCalculations();
    }

    self.getProductList = function () {
        var list = new Array();
        $('#hdnProduct option').each(function () {
            var option = this;
            var obj = new Object();
            obj.text = option.text;
            obj.value = option.value;
            list.push(obj);
        });
        return list;
    }

    self.createSalesOrderProductDetailViewModel = function (item) {
        ;
        self.AssociatedProduct = new Object();
        self.AssociatedProduct.RowId = "Row_" + (item == undefined ? self.CustomeRowCounter : self.CustomeRowCounter + item.AssociatedProduct.AssociatedProductId);
        self.AssociatedProduct.Identity = item == undefined ? "IdentityX" : "Identity";
        self.AssociatedProduct.ProductsList = self.getProductList();
        self.AssociatedProduct.ProductId = item == undefined ? 0 : item.AssociatedProduct.ProductId;
        self.AssociatedProduct.QtyInStock = item == undefined ? 0 : item.AssociatedProduct.QtyInStock;
        self.AssociatedProduct.UnitPrice = item == undefined ? 0 : item.AssociatedProduct.UnitPrice
        self.AssociatedProduct.Quantity = item == undefined ? 0 : item.AssociatedProduct.Quantity;
        self.AssociatedProduct.ListPrice = item == undefined ? 0 : item.AssociatedProduct.ListPrice;
        self.AssociatedProduct.TaxApplied = item == undefined ? 0 : item.AssociatedProduct.TaxApplied;
        self.AssociatedProduct.TaxAmount = item == undefined ? 0 : item.AssociatedProduct.TaxAmount;
        self.AssociatedProduct.VatApplied = item == undefined ? 0 : item.AssociatedProduct.VatApplied;
        self.AssociatedProduct.VatAmount = item == undefined ? 0 : item.AssociatedProduct.VatAmount;
        self.AssociatedProduct.DiscountType = item == undefined ? 0 : item.AssociatedProduct.DiscountType;
        var discountType = item == undefined ? 0 : item.AssociatedProduct.DiscountType;
        if (discountType == self.Ddldiscount.FixedPrice) {
            self.AssociatedProduct.PercDiscount = 0;
            self.AssociatedProduct.FixedDiscount = item == undefined ? 0 : item.AssociatedProduct.DiscountAmount;
        }
        else if (discountType == self.Ddldiscount.PercOfPrice) {
            self.AssociatedProduct.PercDiscount = item == undefined ? 0 : item.AssociatedProduct.DiscountAmount;
            self.AssociatedProduct.FixedDiscount = 0;
        }
        else {
            self.AssociatedProduct.PercDiscount = 0;
            self.AssociatedProduct.FixedDiscount = 0;
        }
        return self.AssociatedProduct;
    }
    self.createSalesOrderMainCalViewModel = function (modeldata) {

        var percDiscount = 0;
        var fixedDiscount = 0;
        var discountTypeMain = modeldata == undefined ? 0 : modeldata.DiscountType;
        if (discountTypeMain == self.Ddldiscount.FixedPrice) {
            percDiscount = 0;
            fixedDiscount = modeldata.DiscountAmount;
        }
        else if (discountTypeMain == self.Ddldiscount.PercOfPrice) {
            percDiscount = modeldata.DiscountAmount;
            fixedDiscount = 0;
        }
        self = this;
        self.PercDiscount = percDiscount;
        self.FixedDiscount = fixedDiscount;
        self.SubTotal = modeldata == undefined ? 0 : modeldata.SubTotal;
        self.DiscountAmount = modeldata == undefined ? 0 : modeldata.DiscountAmount;
        self.DiscountType = modeldata == undefined ? 0 : modeldata.DiscountType;
        self.TaxApplied = modeldata == undefined ? 0 : modeldata.TaxApplied;
        self.TaxAmount = modeldata == undefined ? 0 : modeldata.TaxAmount;
        self.VatApplied = modeldata == undefined ? 0 : modeldata.VatApplied;
        self.VatAmount = modeldata == undefined ? 0 : modeldata.VatAmount;
        self.AdjustmentType = modeldata == undefined ? 0 : modeldata.AdjustmentType;
        self.AdjustmentAmount = modeldata == undefined ? 0 : modeldata.AdjustmentAmount;
        self.GrandTotal = modeldata == undefined ? 0 : modeldata.GrandTotal;
        return self;
    }

    var saleOrderId = $("#SalesOrderId").val();
    if (saleOrderId == undefined || saleOrderId == null || parseInt(saleOrderId) == 0 || saleOrderId == "") {

        self.ProductQuote.push(self.createSalesOrderMainCalViewModel());
    }
    self.ProductQuoteAssociations.push(self.createSalesOrderProductDetailViewModel());
    self.removeProduct = function (RowId) {

        self.ProductQuoteAssociations.remove(function (item) { return item.RowId == RowId })
        self.doAllCalculations();
    }

    self.removetablerow = function (v) {
        $('._tablerow').each(function () {
            if ($(this).attr('rowindex') == $(v).attr('buttonindex')) {
                $(this).remove();
                self.doAllCalculations();
            }
        });
    }
}