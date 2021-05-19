//Register name space
jQuery.namespace('ErucaCRM.User.CreateQuote');
var viewModel;

ErucaCRM.User.CreateQuote.pageLoad = function () {

    ErucaCRM.Framework.Core.SetGlobalDatepicker('ValidTill');
    viewModel = new ErucaCRM.User.CreateQuote.pageViewModel();

    ko.applyBindings(viewModel, document.getElementById("InnerContentContainer"));
}

// A view model that represent a Test report query model.
ErucaCRM.User.CreateQuote.createQuoteViewModel = function (queryModel) {

    var self = this;
    self.OwnerId = queryModel.OwnerId;
    self.Carrier = queryModel.Carrier;
    self.ValidTill = queryModel.ValidTill;
    self.Subject = queryModel.Subject;
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
    self.LeadId = queryModel.LeadId;
    self.Address = new Object();
    self.Address.Street = queryModel.Street;
    self.Address.City = queryModel.City;
    self.Address.State = queryModel.State;
    self.Address.ZipCode = queryModel.ZipCode;
    self.Address.CountryId = queryModel.CountryId;
    self.Description = queryModel.Description;
    self.ProductQuoteAssociations = new Object();
    //self.ProductQuoteAssociations.AssociatedProduct = new Object();

}

//Page view model
ErucaCRM.User.CreateQuote.pageViewModel = function (hasAccess) {
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
    $('._infoheader').on("click", "._addproduct", function () {
        self.CustomeRowCounter += 1;
        self.ProductQuoteAssociations.push(self.createQuoteProductDetailViewModel());
    });
    $('#quoteProduct').on("change", "._productList", function () {
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
    $("#quoteProduct").on("blur", "._txtQty", function () {
        var txt = $(this);
        var $Row = txt.closest('tr');
        self.doAllCalculations($Row);
    });
    $("#quoteProduct").on("blur", "._txtListPrice", function () {
        var txt = $(this);
        var $Row = txt.closest('table').closest('tr');
        self.doAllCalculations($Row);
    });
    $("#quoteProduct").on("blur", "._caltxtdiscount, ._txtdiscount", function () {
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
    //$("#SalesOrderId").on("change", function () {

    //    var selectedvalue = $(this).val();

    //    self.getInvoiceDetail(selectedvalue);
    //});
    $("#maintotal").on("change", "#operator", function () {

        self.doAllCalculations();
    });
    $("#quoteProduct").on("change", "._drpdiscount", function () {
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
    $("#quoteProduct").on("click", "._chksalestax", function () {
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
    $("#quoteProduct").on("click", "._chkvat", function () {
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
    $("#quoteProduct").on("blur", "._txtvat, ._txtsalestax", function () {
        var $Row = $(this).closest('table').closest('tr');
        self.doAllCalculations($Row);

    });
    $("#Subject").on("blur", function () {
        if ($.trim($(this).val()) == "") {
            $("._subjectError").text("Subject is required");
            return;
        }
        else { $("._subjectError").text(""); }

    });
    self.doAllCalculations = function ($Row) {
        var rows = $("#quoteProduct tbody .parentRow ");
        $(rows).each(function () {
            $Row = $(this);
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
        var rows = $("#quoteProduct tbody .parentRow ");
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
        if ($("#LeadId option:selected").text().trim() == "---Select---") {
            alert(ErucaCRM.Messages.Lead.LeadRequired);
            return;
        }
        self.getQuote();
        if (self.ProductQuoteAssociations != undefined && self.ProductQuoteAssociations != null && self.ProductQuoteAssociations.length > 0) {
            ErucaCRM.Framework.Core.doPostOperation
                    (
                        controllerUrl + "CreateQuote",
                        self,
                        function onSuccess(response) {
                            if (response.Status == Message.Failure) { $(".error").text(response.Message).removeClass("msgssuccess") }
                            else if (response.Status == Message.Success) {
                                $(".error").text(response.Message).addClass("msgssuccess");

                                if ($("#QuoteLeadId").length == 0) {
                                    window.location.href = '/User/Quotes';
                                }
                                else {
                                    window.location.href = '/User/ViewLeadDetail/' + $("#QuoteLeadId").val();
                                }

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

        self.QuoteId = $("#QuoteId").val();
        self.OwnerId = $("#OwnerId").val();
        var validTill = $("#ValidTill").datepicker('getDate');//.datepicker({ dateFormat: 'mm-dd-yy' }).val()
        self.ValidTill = validTill;
        self.Subject = $("#Subject").val();
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
        self.LeadId = $("#LeadId").val();

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
        self.ProductQuoteAssociations = self.getQuoteProducts();
        //self.InvoiceId = $("#InvoiceId").val();
        //self.OwnerId = $("#OwnerId").val();
        //self.LeadId = $("#LeadId").val();

        //if (parseInt(self.LeadId) == 0)
        //    self.LeadId = null;

        //var validTill = $("#DueDate").datepicker('getDate');
        //self.DueDate = validTill;
        //self.InvoiceDate = $("#InvoiceDate").datepicker('getDate');
        //self.Subject = $("#Subject").val();

        //self.SalesOrderId = $("#SalesOrderId").val();

        //if (parseInt(self.SalesOrderId) == 0)
        //    self.SalesOrderId = null;

        //self.StatusId = $("#StatusId").val();
        //self.SalesCommission = $("#SalesCommission").val();
        //self.Carrier = $("#Carrier").val();
        //self.SubTotal = $("#txtSubTotal").val();
        //var discountType = $("#ddlDiscount").val();
        //if (discountType == self.Ddldiscount.FixedPrice) {
        //    self.DiscountAmount = $("#txtMainDiscount").val();
        //}
        //else if (discountType == self.Ddldiscount.PercOfPrice) {
        //    self.DiscountAmount = $("#txtCalMainDiscount").val();
        //}
        //else {
        //    self.DiscountAmount = 0;
        //}
        //self.DiscountType = $("#ddlDiscount").val();
        //self.TaxApplied = $("#chkSaleTax").is(":checked");
        //self.TaxAmount = $("#txtSalesTax").val();
        //self.VatApplied = $("#chkVat").is(":checked");
        //self.VatAmount = $("#txtVat").val();
        //self.AdjustmentType = $("#operator").val();
        //self.AdjustmentAmount = $("#txtAdjustment").val();
        //self.GrandTotal = $("#txtGrandTotal").val();
        //self.SubTotal = $("#txtSubTotal").val();
        //// self.LeadId = $("#LeadId").val();
        //self.Terms = $("#Terms").val();
        //self.Address = new Object();
        //self.Address.Street = $("#Address_Street").val();
        //self.Address.City = $("#Address_City").val();
        //self.Address.State = $("#Address_State").val();
        //self.Address.ZipCode = $("#Address_Zipcode").val();
        //self.Address.CountryId = $("#Address_CountryId").val();

        ////self.Address1 = new Object();
        ////self.Address1.Street = $("#Address1_Street").val();
        ////self.Address1.City = $("#Address1_City").val();
        ////self.Address1.State = $("#Address1_State").val();
        ////self.Address1.Zipcode = $("#Address1_Zipcode").val();
        ////self.Address1.CountryId = $("#Address1_CountryId").val();
        ////self.Description = $("#Description").val();
        //self.ProductQuoteAssociations = self.getQuoteProducts();
    }
    self.getQuoteProducts = function () {
        self.ProductQuoteAssociations = new Array();

        var rows = $("#quoteProduct tbody .parentRow ");
        $(rows).each(function () {

            var $Tr = $(this);
            var productId = $Tr.find("._productList").val();
            var listPrice = $Tr.find("._txtListPrice").val();
            listPrice = $.isNumeric(listPrice) ? listPrice : 0;
            var qty = $Tr.find("._txtQty").val();
            qty = $.isNumeric(qty) ? qty : 0;
            if (productId != undefined && productId != null && productId > 0 && listPrice > 0 && qty > 0) {
                var objQuoteProduct = new Object();

                objQuoteProduct.AssociationSalesOrderId = $Tr.find("._hiddenAssociationQuoteId").val();
                objQuoteProduct.AssociatedProductId = $Tr.find("._hiddenAssociatedProductId").val();
                objQuoteProduct.QuoteId = $("#QuoteId").val();

                objQuoteProduct.AssociatedProduct = new Object;

                objQuoteProduct.AssociatedProduct.AssociatedProductId = $Tr.find("._hiddenAssociatedProductId").val();

                objQuoteProduct.AssociatedProduct.ProductId = productId;
                objQuoteProduct.AssociatedProduct.QtyInStock = $Tr.find("._txtQtyStock").val();
                objQuoteProduct.AssociatedProduct.Quantity = qty;
                objQuoteProduct.AssociatedProduct.UnitPrice = $Tr.find("._txtUnitPrice").val();
                objQuoteProduct.AssociatedProduct.ListPrice = listPrice;
                var discountType = $Tr.find("._drpdiscount").val();
                if (discountType == self.Ddldiscount.FixedPrice) {
                    objQuoteProduct.AssociatedProduct.DiscountAmount = $Tr.find("._txtdiscount").val();
                }
                else if (discountType == self.Ddldiscount.PercOfPrice) {
                    objQuoteProduct.AssociatedProduct.DiscountAmount = $Tr.find("._caltxtdiscount").val();
                }
                else {
                    objQuoteProduct.AssociatedProduct.DiscountAmount = 0;
                }
                objQuoteProduct.AssociatedProduct.DiscountType = discountType;
                objQuoteProduct.AssociatedProduct.TaxApplied = $Tr.find("._chksalestax").is(":checked");
                objQuoteProduct.AssociatedProduct.TaxAmount = $Tr.find("._txtsalestax").val();
                objQuoteProduct.AssociatedProduct.VatApplied = $Tr.find("._chkvat").is(":checked");
                objQuoteProduct.AssociatedProduct.VatAmount = $Tr.find("._txtvat").val();
                self.ProductQuoteAssociations.push(objQuoteProduct);
            }
        });
        return self.ProductQuoteAssociations;
    }
    self.setRowData = function (rowData, $Row) {
       
        $Row.find("._txtQtyStock").val(rowData.QuantityInStock);
        $Row.find("._txtUnitPrice").val(rowData.UnitPrice);
    }
    //self.getInvoiceDetail = function (saleOrderId) {
    //    var obj = new Object();
    //    obj.Id_encrypted = saleOrderId;
    //    ErucaCRM.Framework.Core.getJSONDataBySearchParam
    //           (
    //               controllerUrl + "GetSalesOrderDetail", obj,
    //               function onSuccess(response) {
    //                   self.bindProducts(response);
    //               },
    //               function onError(err) {
    //                   self.status(err.Message);
    //               }
    //           );
    //}
    self.gettablerow = function () {

        var row = '<tr>'
                       + ' <td valign="top" style="padding: 3px;" class="tableData">'
                         + '   <select class="_productList">' + $("#hdnProduct").html() + '</select>'
                           + ' <input type="hidden" id="hdnProductId1" value=""></td>'
                        + '<td valign="top" style="padding: 3px;" class="tableData">'
                          + '  <input type="text"  size="6" class="_txtQtyStock"></td>'
                        + '<td valign="top" style="padding: 3px;" class="tableData">'
                          + '  <input type="text"  class="_txtQty">'
                        + '</td>'
                        + '<td valign="top" style="padding: 3px;" class="tableData">'
                          + '  <input type="text"  size="7" class="_txtUnitPrice"></td>'
                        + '<td valign="top" class="tableData">'
                          + '  <div class="control-group">'
                            + '    <table class="childtable">'
                              + '      <tbody>'
                                + '        <tr>'
                                  + '          <td valign="top">'
                                    + '            <input type="text" class="_txtListPrice"  size="10">'
                                      + '      </td>'
                                        + '</tr>'
                                        + '<tr>'
                                          + '  <td>'
                                            + '    <div class="alignright">'
                                              + '      (-)<b><a class="link" href="javascript:;">Discount:</a></b>'
                                                + '    <select class="drpdiscount margin-bottom-0 _drpdiscount">'
                                                  + '      <option value="1">No Discount</option>'
                                                    + '    <option value="2">% of Price</option>'
                                                      + '  <option value="3">Fixed Price</option>'
                                                    + '</select>'
                                                + '</div>'
                                            + '</td>'
                                        + '</tr>'
                                        + '<tr>'
                                          + '  <td class="alignright">Total After Discount:</td>'
                                        + '</tr>'
                                        + '<tr>'
                                          + '  <td>'
                                            + '    <div class="alignright">'
                                              + '      (+)<b>'
                                                + '        <a class="link" href="javascript:;">Tax:</a></b>'
                                                  + '  Sales Tax<input type="checkbox" class="_chksalestax" /><input type="text" class="txtbox20px _txtsalestax" />'
                                                    + 'Vat<input type="checkbox" class="_chkvat" /><input type="text" class="txtbox20px _txtvat" />'
                                                + '</div>'
                                            + '</td>'
                                        + '</tr>'
                                        + '<tr>'
                                          + '  <td class="alignright">Net Total:</td>'
                                        + '</tr>'
                                    + '</tbody>'
                                + '</table>'
                            + '</div>'
                        + '</td>'
                        + '<td style="padding: 3px;" class="tableData">'
                          + '  <div class="control-group">'
                            + '    <table>'
                              + '      <tbody>'
                                + '        <tr>'
                                  + '          <td>'
                                    + '            <input type="text" readonly="true" size="10" class="_total"></td>'
                                      + '  </tr>'
                                        + '<tr>'
                                          + '  <td valign="bottom">'
                                            + '    <input type="text" class="txtbox20px _caltxtdiscount" />'
                                              + '  <input type="text"  class="width114px _txtdiscount"></td>'
                                        + '</tr>'
                                        + '<tr>'
                                          + '  <td valign="bottom">'
                                            + '    <input type="text"  class="_txttotalAfterDiscount"></td>'
                                        + '</tr>'
                                        + '<tr>'
                                          + '  <td valign="bottom">'
                                            + '    <input type="text"  class="_txttax"></td>'
                                        + '</tr>'
                                        + '<tr>'
                                          + '  <td valign="bottom">'
                                            + '    <input type="text"  class="_txtnetTotal"></td>'
                                        + '</tr>'
                                    + '</tbody>'
                                + '</table>'
                            + '</div>'
                        + '</td>'
                        + '<td width="15" align="center" class="tableData" style="padding: 3px;">&nbsp;</td>'
                    + '</tr>'
        return row;
    }
    self.ProductQuoteAssociations = ko.observableArray([]);
    self.ProductQuote = ko.observableArray([]);
    self.bindProducts = function (quoteData) {
        //self.ProductQuoteAssociations.removeAll();
        self.ProductQuote.removeAll();
        if (self.ProductQuoteAssociations.length > 0)
            self.ProductQuoteAssociations.remove(function (item) { return item.Identity == "Identity" })

        var quoteId = $("#QuoteId").val();

        if (quoteId == undefined || quoteId == null || parseInt(quoteId) == 0 || quoteId == "") {

            self.ProductQuoteAssociations.removeAll();
            self.ProductQuote.push(self.createQuoteMainCalViewModel(quoteData));
        }

        //  self.ProductQuote.push(self.createInvoiceMainCalViewModel(quoteData));
        $.each(quoteData.ProductSalesOrderAssociations, function (idx, item) {
            self.ProductQuoteAssociations.push(self.createQuoteProductDetailViewModel(item));
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
    self.createQuoteProductDetailViewModel = function (item) {

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
    self.createQuoteMainCalViewModel = function (modeldata) {

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
    var QuoteId = $("#QuoteId").val();

    if (QuoteId == undefined || QuoteId == null || $("#QuoteId").length == 0 || QuoteId == "") {

        self.ProductQuote.push(self.createQuoteMainCalViewModel());
    }
    self.ProductQuoteAssociations.push(self.createQuoteProductDetailViewModel());
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