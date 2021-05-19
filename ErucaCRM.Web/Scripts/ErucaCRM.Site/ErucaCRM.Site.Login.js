jQuery.namespace('ErucaCRM.Site.Login')

ErucaCRM.Site.Login.pageLoad = function () {
   
    ErucaCRM.Site.Login.DeleteCokie("Permissions");
}

ErucaCRM.Site.Login.DeleteCokie = function (name) {
    document.cookie = name + '=;expires=Thu, 01 Jan 1970 00:00:01 GMT;';
}