// include jQuery framework first.

// Namespace resolution
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
jQuery.namespace('ErucaCRM.Framework.Common');
var helpviewModel;


ErucaCRM.Framework.Common.ApplyHelp = function (Page) {
    helpviewModel = new ErucaCRM.Framework.Common.helpModel(Page);
    ko.applyBindings(helpviewModel, document.getElementById("HelpTips"));
    ko.applyBindings(helpviewModel, document.getElementById("Helplinks"));
}


ErucaCRM.Framework.Common.OpenPopup = function (opendivSelector) {
    var hiddenSection = $(opendivSelector);

    $(opendivSelector + " .error").text('').removeClass("msgssuccess");
    hiddenSection.fadeIn("fast")
        .css({ 'display': 'block' })
        .css({ width: $(window).width() + 'px', height: $(window).height() + 'px' })
        .css({
            top: ($(window).height() - hiddenSection.height()) / 2 + 'px',
            left: ($(window).width() - hiddenSection.width()) / 2 + 'px'
        })
        //.css({ "width": "100%", "height": "100%", "position": "fixed", "overflow": "hidden","top":"0" })
        .css({ 'background-color': 'rgba(0,0,0,0.5)' })
        .appendTo('body');
    $(opendivSelector + ' ._close').click(function () { $(hiddenSection).fadeOut("fast"); });
}

ErucaCRM.Framework.Common.Split = function (val) {
    return val.split(/,\s*/);
}

ErucaCRM.Framework.Common.ExtractLast = function (term) {
    return ErucaCRM.Framework.Common.Split(term).pop();
}

if (!('filter' in Array.prototype)) {
    Array.prototype.filter = function (filter, that /*opt*/) {
        var other = [], v;
        for (var i = 0, n = this.length; i < n; i++)
            if (i in this && filter.call(that, v = this[i], i, this))
                other.push(v);
        return other;
    };
}
ErucaCRM.Framework.Common.helpModel = function (Page) {
    var self = this;
    self.HelpPage = Page;
    self.HelpList = ko.observableArray();
}

ErucaCRM.Framework.Common.HelpQueryModel = function (key, data) {
    var self = this;
    self.Title = data.Title;
    self.Description = data.Description;
    self.ElementId = key.toLowerCase();
    self.Links = ko.observableArray();
    if (data.Links != null)
    {
        var link = new Array();
        link = data.Links.split('|');
        for (var i = 0; i < link.length - 1; i++)
        {
            var title = link[i].split(',');
            self.Links.push(new ErucaCRM.Framework.Common.HelpLinkModel(title[0], title[1]))
        }
    }
    self.TipType = data.TipType;
    self.Class = data.TipType == "window" ? 'window' : 'tooltip';
    self.highlightThis = function (obj, element) {
        //$("._helptip").animate({ "opacity": .5 }, {
        //    duration: 500,
        //    complete: function () {
        //        $(element.target).css("opacity", 1);
        //    }
        //});
        $("._helptip").css("opacity", .2);
        $(element.currentTarget).css("opacity", 1);
    }
    self.showall = function (obj, element) {
        $("._helptip").css("opacity", 1);
        //$("._helptip").animate({ "opacity": 1 }, {
        //    duration: 500
        //});
    }
}
ErucaCRM.Framework.Common.HelpLinkModel = function (title,href) {
    var self = this;
    self.LinkTitle = title;
    self.Linkaddress = href;
}

ErucaCRM.Framework.Common.ApplyPermission = function (selector) {
    if (selector != undefined) {
        $(selector + ' [data-permission]').each(function () {
            if ($.inArray($(this).attr('data-permission').toLowerCase(), PermissionArray) >= 0) {
                $(this).removeClass('permissionbased');
                $(this).addClass('permissiongranted');
            }
        });
    }
    else {
        $('[data-permission]').each(function () {

            if ($.inArray($(this).attr('data-permission').toLowerCase(), PermissionArray) >= 0) {
                $(this).removeClass('permissionbased');
                $(this).addClass('permissiongranted');
            }
            else {
                var display = $(this).attr('data-showalways');
                if (display == 'True') {
                    $(this).removeClass('permissionbased');
                    $(this).addClass('permissiongranted');
                    $(this).attr("href", 'javascript:void(0)');
                    $(this).removeAttr("onclick");
                }
            }
        });

    }
}

ErucaCRM.Framework.Common.SetCookie = function (cname, cvalue) {
    document.cookie = cname + "=" + cvalue + "; "

}
//Code added for implementing permission on ui based on cookie
ErucaCRM.Framework.Common.GetCookie = function (cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i].split(' ').join('');
        if (c.indexOf(name) == 0) return c.substring(name.length, c.length);
    }
    return "";
}


ErucaCRM.Framework.Common.CookieExists = function (permissionname) {
    if ($.inArray(permissionname.toLowerCase(), PermissionArray) >= 0) {
        return true;
    }
    else
        return false;
}


ErucaCRM.Framework.Common.ConvertMonthsToCultureSpecific = function (monthArray) {
    var dashboard = "ErucaCRM.Messages.DashBoard."
    for (i = 0; i < monthArray.length; i++) {
        monthArray[i] = eval(dashboard + monthArray[i]);

    }
    return monthArray;

}


ErucaCRM.Framework.Common.DefaultIntervalForChart = function () {
    return "Week";
}

ErucaCRM.Framework.Common.ConvertWeekToCultureSpecific = function (weekArray) {
    var dashboard = "ErucaCRM.Messages.DashBoard."
    for (i = 0; i < weekArray.length; i++) {
        weekArray[i] = eval(dashboard + weekArray[i]);

    }

    return weekArray;

}

