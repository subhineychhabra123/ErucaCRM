
(function ($) {

    $.support.touch = typeof Touch === 'object';

    if (!$.support.touch) {
        return;
    }

    var proto = $.ui.mouse.prototype,
    _mouseInit = proto._mouseInit;

    $.extend(proto, {
        _mouseInit: function () {
            this.element
            .bind("touchstart." + this.widgetName, $.proxy(this, "_touchStart"));
            _mouseInit.apply(this, arguments);
        },

        _touchStart: function (event) {
            if (event.originalEvent.targetTouches.length != 1) {
                return false;
            }

            this.element
            .bind("touchmove." + this.widgetName, $.proxy(this, "_touchMove"))
            .bind("touchend." + this.widgetName, $.proxy(this, "_touchEnd"));

            this._modifyEvent(event);

            $(document).trigger($.Event("mouseup")); //reset mouseHandled flag in ui.mouse
            this._mouseDown(event);

            return false;
        },

        _touchMove: function (event) {
            this._modifyEvent(event);
            this._mouseMove(event);
        },

        _touchEnd: function (event) {
            this.element
            .unbind("touchmove." + this.widgetName)
            .unbind("touchend." + this.widgetName);
            this._mouseUp(event);
        },

        _modifyEvent: function (event) {
            event.which = 1;
            var target = event.originalEvent.targetTouches[0];
            event.pageX = target.clientX;
            event.pageY = target.clientY;
        }

    });

})(jQuery);




// include jQuery framework first.

// Namespace resolution
var PermissionArray = [];
var innerPageArray = [["Home", "UserList", "Roles", "ProfileTypes", "ProfileType", "ProfileDetail", "UserProfile", "EditUserProfile"],["Dashboard"], ["Tags", "TagDetail"], ["Accounts", "Account", "AccountDetail"], ["Contacts", "AddContact", "ContactView", "EditContact"], ["Activities", "Tasks", "TaskItem", "ViewTaskItemDetail"], ["Quotes", "ViewQuoteDetail", "CreateQuote", "EditQuote"], ["SaleOrders", "Sale Orders", "CreateSalesOrder", "EditSalesOrder", "ViewSalesOrderDetail"], ["Invoices", "CreateInvoice", "EditInvoice", "ViewInvoiceDetail"], ["ContentManagement", "ManageLanguage", "ContentManagement", "EditApplicationPage", "AddCustomPage"], ["Leads", "Lead"], ["AccountCases", "AccountCase", "AccountCaseDetail"], ["Tasks", "TaskItem", "ViewTaskItemDetail"]]
var outerPageArray = [["Home", "Register"], ["Feature"], ["Aboutus"], ["Contactus"], ["Login"], ["MobileCRM"]]
jQuery.namespace = function () {
    var a = arguments, o = null, i, j, d;
    for (i = 0; i < a.length; i = i + 1) {
        d = a[i].split(".");
        o = window;
        for (j = 0; j < d.length; j = j + 1) {
            o[d[j]] = o[d[j]] || {};
            o = o[d[j]];
        }
    }
    return o;
};
//Register name space
jQuery.namespace('ErucaCRM.Framework.Core');
var PermissionArray = [];
var CultureSpecificDateFormat = "";
$(document).ready(function () {
    ErucaCRM.Framework.Core.Config.CultureSpecificDateFormat = "";
    $.ajaxSetup(
    {
        beforeSend: function () {
            ErucaCRM.Framework.Core.ShowMessage(ErucaCRM.Framework.Core.Config.ajaxProcessingText, false);
        },
        complete: function (jqXHR) {
            var errmessage = "";
            if (jqXHR.status == 200) {
                errmessage = ErucaCRM.Framework.Core.Config.ajaxProcessedText;
                ErucaCRM.Framework.Core.ShowMessage(errmessage, false);
                setTimeout('$("._messagediv").css({ "height": "25px" }).slideUp("slow",function(){$(this).remove()})', 4000);
                return;
            } else if (jqXHR.status == 404) {
                errmessage = "oops! Something went wrong. (Requested URL not found).";
            } else if (jqXHR.status == 500) {
                var response = $.parseJSON(jqXHR.responseText);
                errmessage = "oops! Something went wrong. (Internel Server Error Occurred).";
                //alert(response.ExceptionMessage);
            } else {
                errmessage = "oops! Something went wrong. (Unknown Error Occurred).";
            }
            ErucaCRM.Framework.Core.ShowMessage(errmessage, true);
            setTimeout('$("._messagediv").css({ "height": "25px" }).slideUp("slow",function(){$(this).remove()})', 6000);
        }
    });

    ErucaCRM.Framework.Core.ApplyCultureValidations = function () {
        $("[data-val]").each(function (i, e) {
            var $e = $(e);
            if ($("[data-valmsg-for='" + $e.attr("name") + "']").length == 0) {
                $($e.parent()).append('<span data-valmsg-replace="true" data-valmsg-for="' + $e.attr("name").replace(/_/g, ".") + '" class="field-validation-valid"></span>');
            }

            if (ErucaCRM.Messages != undefined) {
                // Set the validation messages specific to culture from culture specific JSON
                $($e).each(function (index, element) {
                    //loop through all type of model  validations like validation for  required, datatype specific etc.
                    $.each(element.attributes, function (inx, att) {
                        //if attribute 'data-val' found then it mean its for model validation
                        var name = this.name + "";
                        if (this.specified && (name.indexOf('data-val')) != -1) {
                            var jsonExpression;
                            if (this.name.indexOf('data-val-equalto-other') != -1) {
                                jsonExpression = "ErucaCRM.Messages.PasswordConfirmPasswordDoesNotMatches";
                            }
                            else if (this.name.indexOf('data-val-number') != -1) {
                                jsonExpression = "ErucaCRM.Messages.Lead.AmountNotValid"
                            }
                            else {
                                jsonExpression = "ErucaCRM.Messages." + this.value;
                            }
                            //skip the attribute having spaces in value
                            if (jsonExpression.indexOf(' ') < 0) {
                                try {
                                    var CultureSpecificValidationMessage = eval(jsonExpression);
                                }
                                catch (e) { }
                                //set the cultureSpecific  message in attribute 
                                if (CultureSpecificValidationMessage != undefined)
                                    $(element).attr(this.name, CultureSpecificValidationMessage);
                            }
                        }
                    });
                })
            }
        });
    }

    ErucaCRM.Framework.Core.ApplyCultureValidations();
   
    var returnUrl = GetUrlVars()["returnurl"];
    returnUrl = returnUrl == undefined || returnUrl == '' ? 'javascript:window.history.back()' : returnUrl;
    $("._back").attr("href", returnUrl);
    $("._backtohere").mouseenter(function () {       
        var $this = $(this);
        //if ($this.attr("href").contains('?')) {
        if ($this.attr("href").indexOf('?') > -1) {
          

            var href = $this.attr("href") + "&returnurl=" + (window.location.href.replace("&", "|and|").replace("=", "|equalsto|"));

        } else {
            var href = $this.attr("href") + "?returnurl=" + (window.location.href.replace("&","|and|").replace("=", "|equalsto|"));
        }
        $this.attr("href", href);
        $this.unbind("mouseenter");
       
    });
    //function GetParameterByName(name) {
    //    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    //    var regex = new RegExp("[\\?&]" + name + "=([^&]*)"),
    //        results = regex.exec(location.search);
    //    return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
    //}
    function GetUrlVars() {
        var vars = [], hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            if (hash.length % 3 == 0) {
                var param = hash[1] + '=' + hash[2];
                if (param != null) {
                    vars[hash[0]] = param.replace("|and|", "&").replace("|equalsto|", "=");
                }
            }
            else if(hash[1]!=null) {
                vars[hash[0]] = hash[1].replace("|and|", "&").replace("|equalsto|", "=");
            }
        }
       
        return vars;
    }
    ErucaCRM.Framework.Core.GetUrlVars = function () {
        var vars = [], hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
       
        for (var i = 0; i < hashes.length; i++) {

            hash = hashes[i].split('=');
            vars.push(hash[0]);
            if (hash.length % 3 == 0) {
                vars[hash[0]] = hash[1] + '=';;
            }
            else { vars[hash[0]] = hash[1]; }
        }
       
        return vars;
    }
   
    var inner = SelectMainMenu();
    if (inner != undefined) {
        $('ul li').removeClass('selected');

        $('ul li a[menu=' + inner + ']').parent().addClass('selected');
    }
    else {
        var outer = SelectOuterMenu();

        $('ul li').removeClass('selected');

        $('ul li a[menu=' + outer + ']').parent().addClass('selected');
        //$('ul li').removeClass('selected');
        //$('ul li:contains(' + outer + ')').addClass('selected');
    }

    var inner = SelectMainMenu();
    if (inner != undefined) {
        $('ul li a').removeClass('active');

        $('ul li a[menu=' + inner + ']').addClass('active');
    }
    else {
        var outer = SelectOuterMenu();

        $('ul li a').removeClass('active');

        $('ul li a[menu=' + outer + ']').addClass('active');
        //$('ul li').removeClass('selected');
        //$('ul li:contains(' + outer + ')').addClass('selected');
    }


    if (ErucaCRM.Framework.Common) {
        var d = new Date();
        var v = d.getTime() + "|" + ErucaCRM.Framework.Common.GetCookie('Permissions');
        PermissionArray = v.split('|')
        for (var i = 0; i < PermissionArray.length; i++) {
            {
                PermissionArray[i] = PermissionArray[i].toString().toLowerCase();
            }
        }
        ErucaCRM.Framework.Common.ApplyPermission();
    }

    $("#ddlGlobalCulture").on("change", function () {

        var cultureName = $(this).val();
        if (cultureName != undefined && cultureName != "undefined")
            ErucaCRM.Framework.Core.SetCulture(cultureName);
    })

    function OneClickSubmitButton() {
        $('._one-click-submit-button').each(function () {
            var buttonType = $(this).prop('type');
            if (buttonType.toLowerCase() != 'submit') return;
            var $theButton = $(this);
            var $theForm = $theButton.closest('form');

            //disabled the button and submit the form
            function tieButtonToForm() {
                $theButton.one('click', function () {
                    $theButton.prop('disabled', true);
                    $theForm.submit();
                });
            }

            tieButtonToForm();

            // This handler will re-wire the event when the form is invalid.
            $theForm.submit(function (event) {
                if (!$(this).valid()) {
                    $theButton.prop('disabled', false);
                    event.preventDefault();
                    tieButtonToForm();
                }
            });
        });
    }

    OneClickSubmitButton();

    




});



var SelectMainMenu = function () {
    var pathArray
    if (window.location.href.toString().indexOf("?") > 0) {
        pathArray = window.location.href.split(/[\/?]+/)

    }
    else {
        pathArray = window.location.href.split('/')

    }
    var returnvalue = false;

    for (i = 0; i < innerPageArray.length; i++) {

        var v = innerPageArray[i];

        for (j = 0; j < v.length; j++) {

            if (jQuery.inArray(v[j], pathArray) > 0) {
                returnvalue = true;

                return v[0];

            }
        }
    }
}

var SelectOuterMenu = function () {

    var pathArray = window.location.href.split('/')
    if (pathArray[3] == "") {
        return "Home";
    }
    for (i = 0; i < outerPageArray.length; i++) {
        var v = outerPageArray[i];

        for (j = 0; j < v.length; j++) {

            if (jQuery.inArray(v[j], pathArray) > 0) {

                return v[0];

            }
        }
    }

}
ErucaCRM.Framework.Core.Config = {
    APIBaseUrl: "",
    PageSize: 10,
    NotificationListPageSize:5,
    ajaxProcessingText: "Loading....",
    ajaxProcessedText: "Loaded",
    CultureSpecificDateFormat: ""


    //$("#" + dateElement).datepicker({ dateFormat: '@Html.jQueryDatePickerFormat()' });

}



ErucaCRM.Framework.Core.SetGlobalDatepicker = function (dateElement) {

    $("#" + dateElement).datepicker({
        onSelect: function(dateText, inst) {
            var errorContainer = $(this).attr('errorMsgContainer');
            if (errorContainer != undefined && errorContainer != null) {
                $(errorContainer).text('');
            }
        },
        dateFormat: ErucaCRM.Framework.Core.Config.CultureSpecificDateFormat
    });
}
ErucaCRM.Framework.Core.SetCulture = function (cultureName) {
    //document.cookie = "Culture=" + $(cultureName).attr("Culture") + ";{ expires: 7 }";
   // document.cookie = "Culture=" + cultureName + ";{ expires: 7 }";
    createCookie("Culture", cultureName, 365);
    window.location.reload()
    // window.location.href = window.location.href + "?reload=true";
}
function createCookie(name, value, days) {
    var expires;

    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toGMTString();
    } else {
        expires = "";
    }
    document.cookie = escape(name) + "=" + escape(value) + expires + "; path=/";
}
//Web api - Http get operation - Data fetch
ErucaCRM.Framework.Core.getJSONData = function (url, successCallBack, failureCallBack) {
    // alert('Ho');
    $.ajaxSetup({ cache: false });
    $.getJSON(this.Config.APIBaseUrl + url)

    .success(function (data) {
        successCallBack(data);

    })
        .error(function OnError(xhr, textStatus, err) {

            if (failureCallBack != null) {
                var obj = jQuery.parseJSON(xhr.responseText);
                var errObj = new Object();
                errObj.Message = obj.Message;
                errObj.status = xhr.status;
                errObj.statusText = xhr.statusText;
                failureCallBack(errObj)

            };
        });
}

//Web api - Http get operation - Data fetch
ErucaCRM.Framework.Core.getJSONDataBySearchParam = function (url, object, successCallBack, failureCallBack, beforeSendCallBack, onCompleteCallBack) {
    $.ajax({
        url: this.Config.APIBaseUrl + url,
        cache: false,
        type: 'GET',
        data: object,
        beforeSend: beforeSendCallBack == undefined ? undefined : beforeSendCallBack,
        complete: onCompleteCallBack == undefined ? undefined : onCompleteCallBack
    })
     .success(function (data) { successCallBack(data); })
     .error(function OnError(xhr, textStatus, err) {
         if (failureCallBack != null) {
             var obj = jQuery.parseJSON(xhr.responseText);
             var errObj = new Object();
             errObj.Message = obj.Message;

             if (obj.ModelState != null)
                 errObj.ModelState = obj.ModelState;

             errObj.status = xhr.status;
             errObj.statusText = xhr.statusText;
             failureCallBack(errObj)
         }
     });
}

// Web api - Http put operation - record update
ErucaCRM.Framework.Core.doPutOperation = function (url, object, successCallBack, failureCallBack) {
    $.ajax({
        url: this.Config.APIBaseUrl + url,
        cache: false,
        type: 'PUT',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(object)
    })
    .success(function (data) { successCallBack(data); })
    .error(function OnError(xhr, textStatus, err) {

        if (failureCallBack != null) {
            var obj = jQuery.parseJSON(xhr.responseText);
            var errObj = new Object();
            errObj.Message = obj.Message;

            if (obj.ModelState != null)
                errObj.ModelState = obj.ModelState;

            errObj.status = xhr.status;
            errObj.statusText = xhr.statusText;
            failureCallBack(errObj)
        }
    });
}

// Web api - Http post operation - create record
ErucaCRM.Framework.Core.doPostOperation = function (url, object, successCallBack, failureCallBack) {
    $.ajax({
        url: this.Config.APIBaseUrl + url,
        cache: false,
        type: 'POST',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(object),
        statusCode: {
            200 /*Created*/: function (data) {
                successCallBack(data)
            }
        }
    })
      .error(function OnError(xhr, textStatus, err) {

          if (failureCallBack != null) {
              var obj = jQuery.parseJSON(xhr.responseText);
              var errObj = new Object();
              errObj.Message = obj.Message;

              if (obj.ModelState != null)
                  errObj.ModelState = obj.ModelState;

              errObj.status = xhr.status;
              errObj.statusText = xhr.statusText;
              failureCallBack(errObj)
          }
      });


}

// Web api - Http delete operation - delete a record
ErucaCRM.Framework.Core.doDeleteOperation = function (url, object, successCallBack, failureCallBack) {
    $.ajax({
        url: this.Config.APIBaseUrl + url,
        cache: false,
        type: 'DELETE',
        data: JSON.stringify(object),
        contentType: 'application/json; charset=utf-8'
    })
    .success(function (data) { successCallBack(data); })
    .fail(
        function (xhr, textStatus, err) {
            if (failureCallBack != null)
                failureCallBack(xhr, textStatus, err);
        });
}

// Validation message
ErucaCRM.Framework.Core.showErrors = function (submitForm, err) {
    var validator = $("#" + submitForm).validate();
    var iCnt = 0;
    errors = [];
    $.each(err.ModelState, function (key, value) {
        var pieces = key.split('.');
        key = pieces[pieces.length - 1];
        errors[key] = value[0];

    });
    //var validator = $("#frmUserCreate").validate();
    //validator.showErrors({
    //    "FirstName": "I know that your firstname is Pete, Pete!"
    //});
    validator.showErrors(errors);
}

ErucaCRM.Framework.Core.ShowMessage = function (msg, iserror, removeOtherMessages) {
    if (msg == ErucaCRM.Framework.Core.Config.ajaxProcessedText && $('div._messagediv:not(:contains(' + ErucaCRM.Framework.Core.Config.ajaxProcessingText + '))').length > 0) {
        return;
    }
    var messageBoxid = "messagediv" + Math.round(Math.random() * 1000);
    if ($("#" + messageBoxid).length == 0) {
        if (removeOtherMessages != false) {
            $("._messagediv").remove();
        }
        var message = $("<div id='" + messageBoxid + "' class='_messagediv messagediv displaynone'><div class='statusMessage rb-a-4 _status'><span class='_message'></span></div></div>");
        $(document.body).append(message);
    }
    if (iserror != undefined && iserror == false) {
        $("#" + messageBoxid + " ._status").removeClass("error").addClass("message");
    }
    else {
        $("#" + messageBoxid + " ._status").removeClass("message").addClass("error");
    }
    var width = $(window).width() - $("#" + messageBoxid + "").width();

    $("#" + messageBoxid + " ._message").html(msg);
    $("#" + messageBoxid).slideDown("slow", function () { $(this).css({ "height": "1px" }) });

    //setTimeout('$("#MessageDiv").css({ "height": "25px" }).slideUp("slow",function(){$(this).remove()})', 5000);
}

ErucaCRM.Framework.Core.GetPagger = function (TotalRecords, currentPage, methodName, uniquePagerId) {
    currentPage = currentPage == undefined || parseInt(currentPage) <= 0 ? 1 : currentPage;
    var totalRecords = TotalRecords;
    var pageSize = ErucaCRM.Framework.Core.Config.PageSize;

    if (methodName == "GetLeadHistorybyPaging") {
        methodName = "viewLeadModel." + methodName;
    }
    else if (methodName.indexOf('.') > 0) {// if already passed (view model + methodname) example MyViewModel.MyMethodName (By Rakesh Rana)
        methodName = methodName;
        
    }
    else {
        methodName = "viewModel." + methodName;
    }
    var totalPages = parseInt(totalRecords / pageSize);

    if (totalRecords % pageSize > 0) {
        totalPages = totalPages + 1;
    }
    if (uniquePagerId == undefined || uniquePagerId == null) {
        uniquePagerId = "divPager";
    }
    if (totalPages <= 1) {
        $("#" + uniquePagerId).html('');
        $('#showallLess').hide();
        return false;
    }

    var sbPagger = '';
    sbPagger = sbPagger + "<div id='" + uniquePagerId + "' class='divPager'>";
    if (totalPages > 5) {
        if (currentPage == 1) {
            sbPagger = sbPagger + "<span class='_selectedPage selectedGridPage'>First</span>&nbsp;";
        }
        else {
            sbPagger = sbPagger + "<a pageno='1' StartRowIndex='1' href='javascript:" + methodName + "(1);'>First</a>&nbsp;";
        }
    }
    var index = currentPage - 3;
    var tmppage = 0;
    if (index < 0) {
        tmppage = index;
        index = 0;
    }
    var tmptotalPages = currentPage + 2 - (tmppage);
    if (tmptotalPages < 5) {
        tmptotalPages = 5;
    }
    if (tmptotalPages > totalPages) {
        index -= (tmptotalPages - totalPages);
        tmptotalPages = totalPages;
    }
    if (index < 0) {
        index = 0;
    }
    for (; index < tmptotalPages; index++) {
        var pageNo = index + 1;

        if (currentPage == pageNo)
            sbPagger = sbPagger + "<span class='_selectedPage selectedGridPage'>" + pageNo + "</span>&nbsp;";
        else {
            sbPagger = sbPagger + "<a pageno='" + pageNo + "' StartRowIndex='" + ((pageSize * index) + 1)
            + "'href='javascript:" + methodName + "(" + pageNo + ");' " + (currentPage == pageNo ? "class='divpager'" : "") + ">" + pageNo + "</a>&nbsp;";
        }
    }
    if (totalPages > 5) {

        if (currentPage == tmptotalPages) {
            sbPagger = sbPagger + "<span class='_selectedPage selectedGridPage' >Last</span>&nbsp;";
        }
        else {
            sbPagger = sbPagger + "<a pageno='" + tmptotalPages + "' StartRowIndex='" + ((pageSize * (tmptotalPages - 1)) + 1) + "' href='javascript:" + methodName + "(" + parseInt(totalPages) + ");'>Last</a>&nbsp;";
        }
    }
    sbPagger = sbPagger + "</div>";

    return sbPagger;
}

ErucaCRM.Framework.Core.OpenRolePopup = function (opendivSelector) {

    var hiddenSection = $(opendivSelector);
    $(opendivSelector + " .error").text('').removeClass("msgssuccess");
    hiddenSection.fadeIn("fast")
        .css({ 'display': 'block' })
        .css({ width: $(window).width() + 'px', height: $(window).height() + 'px' })
        .css({
            top: ($(window).height() - hiddenSection.height()) / 2 + 'px',
            left: ($(window).width() - hiddenSection.width()) / 2 + 'px'
        })
        .css({ 'background-color': 'rgba(0,0,0,0.5)' })
        .appendTo('body');
    $(opendivSelector + ' ._close').click(function () { $(hiddenSection).fadeOut("fast"); });

}
