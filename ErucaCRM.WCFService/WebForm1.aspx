﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="ErucaCRM.WCFService.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="http://localhost:2399/Scripts/jquery-1.10.2.min.js"></script>
    <script type="text/javascript">
        var counter = "Hello";

        function CallMyService() {
            var user = new Object();
                
          
            //user.UserId = "GXk1CaiuPU8=";
            
            //user.TimeZoneId = 6;
            //user.CultureInformationId = 6;
            //user.LastName = "test";
            //user.CountryId = 9;
            //user.ZipCode = '7234678';
            //user.Street = "testStreetUpdated";
            //user.EmailId = "EmailAfterEdit@ggmail.com";
            //user.City="testCity"
            //user.CountryId = 12;
            //user.CultureId = 50;
            //user.UserId="GXk1CaiuPU8="
            //user.CurrentPage=1;
            //user.CompanyId = 9;
            //user.PageSize = 10;
            //user.FilterBy = "Allcontacts";
            //user.IsSearchByTag = false;
            //user.SearchTags="asd"
            user.DocumentId = 
            $.ajax({
                type: "POST",
                url: "ErucaCRMService.svc/RemoveLeadDocument/50",
                data: JSON.stringify(user),
                contentType: "application/json",
                success: ServiceSucceeded,
                error: ServiceFailed,
                beforeSend: function (request) {
                    request.setRequestHeader("token", "xDKSlTxHh0C1ZtLbNMVacg_mBB4Zb9hpto=");
                }
            });
          
        }
        function ServiceFailed(result) {
            Log('Service call failed: ' + result.status + '  ' + result.statusText);
        }
        function ServiceSucceeded(result) {
            var resultObject = result.MyFunctionResult;
            Log("Success: " + result);
        }

        function Log(msg) {
            $("#logdiv").append(msg + "<br />");
        }
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <input id="Button1" type="button" value="Execute" onclick="CallMyService();" />
            <div id="logdiv"></div>
        </div>
        <asp:Label ID="lblmsg" runat="server" />
   <iframe class="_downloadFile"  style="display:none"></iframe>
         </form>
</body>
</html>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=Edge,chrome=1">
    <meta name="description" content="Organize anything, together. Trello is a collaboration tool that organizes your projects into boards. In one glance, know what's being worked on, who's working on what, and where something is in a process.">
    <meta name="viewport" content="maximum-scale=1.0,width=device-width,initial-scale=1.0,user-scalable=0">
    <meta name="apple-itunes-app" content="app-id=461504587" />
    <meta name="robots" content="noarchive">
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>Trello</title>
    <script>
        var page = location.pathname;
        if (page === "/")
            page = (document.cookie.match(/token=[a-f0-9]+%2F[a-f0-9]+/) ? "/boards" : "/landing");
        (function (i, s, o, g, r, a, m)
        { i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () { (i[r].q = i[r].q || []).push(arguments) }, i[r].l = 1 * new Date(); a = s.createElement(o), m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m) })(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga'); ga('create', 'UA-225715-29', 'trello.com'); ga('send', 'pageview', page); var _failed = function (a) { a = a.src || a.href; var b = function (a) { if (a = /^https?:\/\/[^\/]+/.exec(a)) return a[0] }, b = b(a) || b(location.href) || "unknown"; ga('send', 'event', 'Load Failure', b, a) }; (function () {
            var m, g, n, p, q; g = "addAttachmentToCard,addChecklistToCard,addMemberToCard,commentCard,copyCommentCard,convertToCardFromCheckItem,createCard,copyCard,deleteAttachmentFromCard,emailCard,moveCardFromBoard,moveCardToBoard,removeChecklistFromCard,removeMemberFromCard,updateCard:idList,updateCard:closed,updateCheckItemStateOnCard";
            [[g, "addMemberToBoard,addToOrganizationBoard,copyBoard,createBoard,createList,deleteCard,disablePowerUp,enablePowerUp,makeAdminOfBoard,makeNormalMemberOfBoard,makeObserverOfBoard,moveListFromBoard,moveListToBoard,removeFromOrganizationBoard,unconfirmedBoardInvitation,unconfirmedOrganizationInvitation,updateBoard,updateList:closed"].join(),
     "updateMember"].join(); m = { fields: "all" }; g = { actions: g, action_memberCreator_fields: "fullName,initials,memberType,username,avatarHash,bio,bioData,confirmed,products,idPremOrgsAdmin", members: !0, member_fields: "fullName,initials,memberType,username,avatarHash,bio,bioData,confirmed,products,status", actions_limit: 1E3, attachments: !0, fields: "", checklists: "all" }; n = {
         lists: "all", cards: "visible", card_attachments: "cover", card_stickers: !0, card_fields: "badges,closed,dateLastActivity,desc,descData,due,idAttachmentCover,idList,idBoard,idMembers,idShort,labels,name,pos,shortUrl,shortLink,subscribed,url",
         card_checklists: "none", members: "all", member_fields: "fullName,initials,memberType,username,avatarHash,bio,bioData,confirmed,products,status", membersInvited: "all", membersInvited_fields: "fullName,initials,memberType,username,avatarHash,bio,bioData,confirmed,products", checklists: "none", organization: !0, organization_fields: "name,displayName,desc,descData,url,website,prefs,memberships,logoHash,products", myPrefs: !0, fields: "name,closed,dateLastActivity,dateLastView,idOrganization,prefs,shortLink,shortUrl,url,desc,descData,invitations,invited,labelNames,memberships,pinned,powerUps,subscribed"
     };
     p = {
         boards: "open,starred", board_fields: "name,closed,dateLastActivity,dateLastView,idOrganization,prefs,shortLink,shortUrl,url,subscribed", boardStars: !0, boardsInvited: "all", boardsInvited_fields: "name,closed,dateLastActivity,dateLastView,idOrganization,prefs,shortLink,shortUrl,url,subscribed", board_organization: !0, board_organization_fields: "name,displayName,products,prefs,logoHash", credits: "invitation,promoCode", organizations: "all", organization_fields: "name,displayName,products,prefs,logoHash", organizationsInvited: "all",
         organizationsInvited_fields: "name,displayName,products,prefs,logoHash", paid_account: !0
     }; q = { notifications: "all", notifications_limit: 5, notification_memberCreator_fields: "fullName,initials,memberType,username,avatarHash,bio,bioData,confirmed,products", organizations: "all", organization_paid_account: !0, organization_fields: "name,displayName", paid_account: !0 };
     var r, l, s; window.websocketPercent = 100; s = function (b) {
         var c; if (null != (c = window.JSON) && c.parse) try { return window.JSON.parse(b) } catch (a) { return null } else return (new Function("return " +
         b))()
     }; r = function (b, c) { var a; a = window.XMLHttpRequest ? new XMLHttpRequest : new ActiveXObject("Microsoft.XMLHTTP"); a.open("GET", b, !0); a.onreadystatechange = function () { 4 === a.readyState && (200 !== a.status ? c(a.responseText) : c(null, s(a.responseText))) }; a.send(null) }; l = function (b, c) {
         var a, f, e, d, t; null == c && (c = {}); a = []; for (f = /invite-token-[-a-f0-9]*=([^;]+)/g; null != (d = null != (t = f.exec(document.cookie)) ? t[1] : void 0) ;) a.push(unescape(d)); 0 < a.length && (c.invitationTokens = a.join(",")); f = "" + b + "?"; d = []; for (e in c) a = c[e],
         d.push("" + e + "=" + encodeURIComponent(a)); return f + d.join("&")
     }; g = function () {
         var b, c, a, f, e; e = window; c = location; a = c.pathname.substr(1); b = c.hash; b = /^#[^\/]+/.test(b) && !/^#gaso=/.test(b) ? "/" + b.substr(1) : null; f = {
             init: function () { f.verifyPushState() && (f.preloads = {}, /token/.test(document.cookie) && (f.preload(l("/1/Members/me", q)), f.preload(f.getDataUrl()))) }, verifyPushState: function () {
                 var d; d = e.history && e.history.pushState && !/android(\s)*[3-4]\.[0-1][^0-9]/i.test(navigator.userAgent); return b && d ? (c.replace(b),
                 !1) : 1 < a.length && !d ? (c.replace("/#" + a), !1) : !0
             }, getDataUrl: function () { var d; b && (a = b); return "" === a ? l("/1/Members/me", p) : (d = /^board\/[^\/]+\/([^\/]+)/.exec(a)) ? f.getBoardDataUrl(d[1]) : (d = /^b\/([^\/]+)/.exec(a)) ? f.getBoardDataUrl(d[1]) : (d = /^c\/([^\/]+)/.exec(a)) ? f.getCardDataUrl(d[1]) : !1 }, getBoardDataUrl: function (d) { return l("/1/Boards/" + d, n) }, getCardDataUrl: function (d) { return l("/1/Cards/" + d, m) }, preload: function (d) {
                 var a; d && (a = { isLoading: !0, callbacks: [], errorCallbacks: [] }, f.preloads[d] = a, r(d, function (c,
                 b) { var e, h, g, k; a.isLoading = !1; if (c) { a.errorMessage = c; k = a.errorCallbacks; h = 0; for (g = k.length; h < g; h++) e = k[h], e(c) } else { a.data = b; k = a.callbacks; h = 0; for (g = k.length; h < g; h++) e = k[h], e(b); setTimeout(function () { return f.removePreload(d) }, 1E4) } }))
             }, removePreload: function (a) { return delete f.preloads[a] }, load: function (a, c, b) {
                 var e, g; e = f.preloads[a]; null != e ? e.isLoading ? (e.callbacks.push(c), b && e.errorCallbacks.push(b)) : (null != e.errorMessage || null == e.data ? "function" === typeof b && b(null != (g = e.errorMessage) ? g : "No data on quickload") :
                 c(e.data), f.removePreload(a)) : $.ajax(a, { dataType: "json" }).success(c).error(function (a, c, d) { return "function" === typeof b ? b(a.responseText) : void 0 })
             }
         }; f.init(); return f
     }(); window.QuickLoad = g.load
 })();
    </script>
    <script>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    window._abPercentagesAtLoad = { "recommend": 100, "status": 100, "gethelp": 50, "ipad_sidebar_promo": 100, "bc_waitingOnGoogle": 100, "bc_google": 100, "bc_launch": 100, "powerUps": 100, "boardEmails": 100, "gold_launch": 100, "BC_boards_list_ad": 50, "BC_google_apps_ad": 50, "newSockets": 100, "BC_observers_ad": 50, "trellocdn_warning": 100, "BC_manage_boards_ad": 50, "elastic_search": 100, "client_profiler_reports": 5 };</script>
    <script type="text/javascript">                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    (function (e, b) { if (!b.__SV) { var a, f, i, g; window.mixpanel = b; a = e.createElement("script"); a.type = "text/javascript"; a.async = !0; a.src = ("https:" === e.location.protocol ? "https:" : "http:") + '//cdn.mxpnl.com/libs/mixpanel-2.2.min.js'; f = e.getElementsByTagName("script")[0]; f.parentNode.insertBefore(a, f); b._i = []; b.init = function (a, e, d) { function f(b, h) { var a = h.split("."); 2 == a.length && (b = b[a[0]], h = a[1]); b[h] = function () { b.push([h].concat(Array.prototype.slice.call(arguments, 0))) } } var c = b; "undefined" !== typeof d ? c = b[d] = [] : d = "mixpanel"; c.people = c.people || []; c.toString = function (b) { var a = "mixpanel"; "mixpanel" !== d && (a += "." + d); b || (a += " (stub)"); return a }; c.people.toString = function () { return c.toString(1) + ".people (stub)" }; i = "disable track track_pageview track_links track_forms register register_once alias unregister identify name_tag set_config people.set people.set_once people.increment people.append people.track_charge people.clear_charges people.delete_user".split(" "); for (g = 0; g < i.length; g++) f(c, i[g]); b._i.push([a, e, d]) }; b.__SV = 1.2 } })(document, window.mixpanel || []); mixpanel.init("b43256b2c2de6b32b77fc0a2c4f63e7a", { secure_cookie: true });</script>
    <script type="text/javascript">                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    window.channels = { "active": "stable", "allowed": ["stable"], "dev": false }</script>
    <script type="text/javascript" src="https://d2k1ftgv7pobq7.cloudfront.net/js/e2d061ffe94a187f2cbf2c159a565d79/all.js" onerror="_failed(this)"></script>
    <script>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    (function (b, c) { var a = b.createElement("link"); a.href = c; a.rel = "stylesheet"; a.type = "text/css"; b.getElementsByTagName("head")[0].appendChild(a) })(document, "https://d2k1ftgv7pobq7.cloudfront.net/css/5836d866e05374a78de375c300fc92eb/images.css");</script>
    <link rel="stylesheet" href="https://d2k1ftgv7pobq7.cloudfront.net/css/97e0031dcd3cfccb2e5153c3657d6b22/core.css" onerror="_failed(this)">
    <link rel="apple-touch-icon-precomposed" sizes="152x152" href="https://d2k1ftgv7pobq7.cloudfront.net/images/0307bc39ec6c9ff499c80e18c767b8b1/apple-touch-icon-152x152-precomposed.png">
    <link rel="apple-touch-icon-precomposed" sizes="144x144" href="https://d2k1ftgv7pobq7.cloudfront.net/images/0dbf6daf256eb7678e5e2185d5146165/apple-touch-icon-144x144-precomposed.png">
    <link rel="apple-touch-icon-precomposed" sizes="120x120" href="https://d2k1ftgv7pobq7.cloudfront.net/images/a68051b5c9144abe859ba11e278bbceb/apple-touch-icon-120x120-precomposed.png">
    <link rel="apple-touch-icon-precomposed" sizes="114x114" href="https://d2k1ftgv7pobq7.cloudfront.net/images/7f4a80b64fd8fd99840b1c08d9b45a04/apple-touch-icon-114x114-precomposed.png">
    <link rel="apple-touch-icon-precomposed" sizes="72x72" href="https://d2k1ftgv7pobq7.cloudfront.net/images/91a3a04ec68a6090380156f847c082bf/apple-touch-icon-72x72-precomposed.png">
    <link rel="apple-touch-icon-precomposed" href="https://d2k1ftgv7pobq7.cloudfront.net/images/8de2074e8a785dd5d498f8f956267478/apple-touch-icon-precomposed.png">
    <link type="application/opensearchdescription+xml" rel="search" href="/osd.xml" />
</head>
<body class="page-index unknown-window ">
    <div id="nocss">Your browser was unable to load all of Trello's resources. They may have been blocked by your firewall, proxy or browser configuration.<br>
        Press Ctrl+F5 or Ctrl+Shift+R to have your browser try again.
        <hr>
    </div>
    <div id="surface" class="clearfix">
        <div id="header"></div>
        <p id="notification" />
        <div id="content" class="clearfix"></div>
        <noscript class="big-message"><h1>To use Trello, please enable JavaScript.</h1></noscript>
    </div>
    <div class="window-overlay">
        <div class="window"><a class="focus-dummy" href="#"></a>
            <div class="window-wrapper clearfix"></div>
        </div>
    </div>
    <div class="pop-over clearfix fancy-scrollbar">
        <div class="header clearfix"><a class="back-btn js-back-view" href="#"><span class="icon-sm icon-leftarrow"></span></a><span class="header-title"></span><a class="close-btn js-close-popover" href="#"><span class="icon-sm icon-close"></span></a></div>
        <div class="content clearfix"></div>
    </div>
    <div class="tooltip-container"></div>
    <div id="clipboard-container"></div>
    <a href="#" class="sticker-edit-button js-edit-sticker" id="edit-sticker"><span class="icon-sm icon-sticker"></span><span class="text">Edit sticker.</span></a>
    <script>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    var _qevents = _qevents || []; (function () { var a = document.createElement("script"); a.src = ("https:" == document.location.protocol ? "https://secure" : "http://edge") + ".quantserve.com/quant.js"; a.async = !0; a.type = "text/javascript"; var b = document.getElementsByTagName("script")[0]; b.parentNode.insertBefore(a, b) })(); _qevents.push({ qacct: "p-9tnwrxnK1azF1" }); </script>
    <noscript><div style="display:none;"><img src="//pixel.quantserve.com/pixel/p-9tnwrxnK1azF1.gif" border="0" height="1" width="1"/></div></noscript>
</body>
</html>
