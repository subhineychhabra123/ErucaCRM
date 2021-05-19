jQuery.namespace('ErucaCRM.User.AddUser');
ErucaCRM.User.AddUser.pageLoad = function () {
    
    viewModel = new ErucaCRM.User.AddUser.pageViewModel();
    ko.applyBindings(viewModel, document.getElementById("InnerContentContainer"));
   

}

ErucaCRM.User.AddUser.pageViewModel = function () {
   self.SelectBrowserTimeZone = function () {

        var objLocalZone = new Date();
        var strLocalZone = '' + objLocalZone;
        var mySplitResult = strLocalZone.split(" ");
        var newLocalZone = mySplitResult[5].slice(0, mySplitResult[5].length - 2) + ':' + mySplitResult[5].slice(mySplitResult[5].length - 2, mySplitResult[5].length);
      
        $("#TimeZoneId > option").each(function () {
            var option = $(this).text();
            if (option.indexOf(newLocalZone) >= 0) {
                $(this).prop('selected', true);
            }
        });
   }
   self.SelectBrowserTimeZone();
}



