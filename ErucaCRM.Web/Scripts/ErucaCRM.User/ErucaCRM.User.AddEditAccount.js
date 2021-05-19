//Register name space
jQuery.namespace('ErucaCRM.User.AddEditAccount');
var viewModel;
var tagcnt = 0;
var TagList = new Array();
var SearchTagName = "";
var CurrentTagObject = new Object();
ErucaCRM.User.AddEditAccount.pageLoad = function () {
    SearchTagName = "";
    TagList = new Array();
    CurrentTagObject = new Object();
    CurrentTagObject.TagId = "";
    CurrentTagObject.TagName = "";
    viewModel = new ErucaCRM.User.AddEditAccount.pageViewModel();
    ko.applyBindings(viewModel, document.getElementById("InnerContentContainer"));
    tagcnt = 0;
}
ErucaCRM.User.AddEditAccount.pageViewModel = function (emailId) {
    //Class variables
    var controllerUrl = "/User/";
    var self = this;
    $("#TagIds").val("");
    CurrentTagObject = new Object();
    CurrentTagObject.TagId = "";
    CurrentTagObject.TagName = "";
    self.availableTags = ["Bussiness Manager", "Potential", "IMPORTANT", "Jan", "Feb", "March", "April", "May", "June", "July", "August", "Sept", "Oct", "Nov", "Dec", "Seminar", "Webinar", "Interested", "Soon", "Account"];

    self.Common = ErucaCRM.Framework.Common;
    TagList = new Array();

    $("#Tag").bind("keydown", function (event) {
        $("#CurrentTagId").val("");
        $("#CurrentTagName").val("");
        SearchTagName = "";
        if (event.keyCode === $.ui.keyCode.TAB &&
        $(this).data("ui-autocomplete").menu.active) {
            event.preventDefault();
        }
    }).autocomplete({

        minLength: 0,
        source: function (request, response) {

            //self.GetSearchTags(request.term);
            // delegate back to autocomplete, but extract the last term

            self.GetSearchTags(request, response)
        },
        focus: function () {
            // prevent value inserted on focus
            return false;
        },
        select: function (event, ui) {

            var terms = self.Common.Split(this.value);

            $("#CurrentTagId").val(ui.item.value);
            $("#CurrentTagName").val(ui.item.label)

            var CurrentTagId = $("#CurrentTagId").val();
            var CurrentTagName = $("#CurrentTagName").val();


            var objTag = new Object();

            if (ui.item.label == "") {

                objTag.TagId = "0";
                objTag.TagName = SearchTagName;

            }
            else {
                objTag.TagId = CurrentTagId;
                objTag.TagName = CurrentTagName;
            }
            CurrentTagObject.TagId = "";
            CurrentTagObject.TagName = "";

            TagList.push(objTag);


            // remove the current input
            terms.pop();
            // add the selected item
            terms.push(ui.item.label);
            // add placeholder to get the comma-and-space at the end
            terms.push("");
            this.value = terms.join(",");
            // alert($("#TagIds").val());
            return false;
        }
    });



    self.GetSearchTags = function (request, response) {

        SearchTagName = jQuery.trim(request.term.substring(request.term.lastIndexOf(',') + 1));

        var taglist = jQuery.trim($("#Tag").val()).split(',');

        //if user find the exact match in and did not selected it and enter the comma to start entering other tag
        //then first loop the through the taglist contain selected tag through autocomplete if did not find the match
        // then llo into CurrentTagObject which contain last exact match user selected or not.


        if (SearchTagName == "") {

            var lastTagName = taglist[taglist.length - 2];

            var lastTagSelecetd = false

            if (CurrentTagObject.TagName.toLowerCase() == lastTagName.toLowerCase()) {

                for (var j = 0; j < TagList.length; j++) {
                    if (TagList[j].TagName.toLowerCase() == lastTagName.toLowerCase()) {

                        lastTagSelecetd = true;
                        break;
                    }
                }

                if (lastTagSelecetd == false) {

                    var objTag = new Object();



                    objTag.TagId = CurrentTagObject.TagId;
                    objTag.TagName = CurrentTagObject.TagName;

                    TagList.push(objTag);

                }

            }


        }

        ErucaCRM.Framework.Core.getJSONData(controllerUrl + "GetSearchTagList/?searchText=" + SearchTagName, function onSuccess(data) {

            //if exact match found then caputre the tag info
            var objFilteredList = $.ui.autocomplete.filter(
                      data.ListTags, self.Common.ExtractLast(request.term));

            if (objFilteredList.length == 1) {

                CurrentTagObject.TagId = objFilteredList[0].value;
                CurrentTagObject.TagName = objFilteredList[0].label;



            }

            response(objFilteredList);
            return;

        }, function onError(err) {
            self.status(err.Message);
        });
    }



    self.AddTagToAccount = function () {     

        if ($("#Tag").val() == "") {
            $("#Tag").focus();
            alert(ErucaCRM.Messages.Account.TagRequired);

            return false;
        }

        var AllTagList = jQuery.trim($("#Tag").val()).split(',');

        var AddedTags = new Array();

        var html = "";
        var tagId = "0";
        var tagName = "";
        var tagAlreadyExist = false;
        for (var i = 0; i < AllTagList.length; i++) {

            tagId = "0";
            tagName = "";
            tagAlreadyExist = false;
  
            tagName = jQuery.trim(AllTagList[i]);
            if (tagName != "") {
                for (var j = 0; j < TagList.length; j++) {
                    if (TagList[j].TagName.toLowerCase() == tagName.toLowerCase()) {
                        tagId = TagList[j].TagId;
                        break;
                    }

                }

                for (var k = 0; k < AddedTags.length; k++) {
                    if (tagName.toLowerCase() == AddedTags[k])
                        tagAlreadyExist = true;
                }

                $(".tagmain").children('div').each(function () {
                    if ($(this).attr("tagName").toLowerCase() == tagName.toLowerCase()) {
                        tagAlreadyExist = true;

                    }

                });

             

                    //if for last tag autocomplete find the exact match but user did not select it and did not appened the comma then capture last entered tag's id in the system

                    if (tagAlreadyExist == false) {
                        if (tagId == "0" && tagName.toLowerCase() == CurrentTagObject.TagName.toLowerCase() && CurrentTagObject.TagId != "") {
                            tagId = CurrentTagObject.TagId;
                        }

                        if (tagId == "0") {
                            html = html + " <div class='tag tagwidth _tag' tagId='" + tagId + "' tagName='" + tagName + "'><div><div class='tagname'>  <span>" + tagName + "</span><a onclick='viewModel.RemoveTag(this)'>&nbsp;&nbsp;X</a></div></div></div>";
                        }
                        else {
                            html = html + " <div class='tag tagwidth _tag' tagId='" + tagId + "' tagName='" + tagName + "'><div><div class='tagname'> <a target='_blank' href='/User/TagDetail/" + tagId + "'> <span>" + tagName + "</span></a><a onclick='viewModel.RemoveTag(this)'>&nbsp;&nbsp;X</a></div></div></div>";
                        }

                        AddedTags.push(tagName.toLowerCase());
                    }
            }

        }

        $(".tagmain").append(html);
        $("#Tag").val("");
        TagList = new Array();

        return false;
    }
    $('#Tag').keypress(function (event) {

        if (event.keyCode == 13) {
            self.AddTagToAccount();
            event.preventDefault();
        }
    });

    self.RemoveTag = function (tag) {

        if (confirm(ErucaCRM.Messages.Account.TagRemoveConfirmAction)) {
            $(tag).parent('div').parent('div').parent('div').remove();
        }
        return false;

    }


    self.SaveTagToAccount = function () {
        var TagIds = "";
        var NewTagNames = "";
        $(".tagmain").children('div._tag').each(function () {

            if ($(this).attr("tagId").toString() != '0' && $(this).attr("tagName") != "") {
                if (TagIds == "")
                    TagIds = TagIds + $(this).attr("tagId");
                else
                    TagIds = TagIds + "," + $(this).attr("tagId");
            }
            else if (parseInt($(this).attr("tagId")) == 0) {

                if (NewTagNames == "")
                    NewTagNames = NewTagNames + $(this).attr("tagName");
                else
                    NewTagNames = NewTagNames + "," + $(this).attr("tagName");
            }


        });

        $("#AccountTagIds").val(TagIds);
        $("#NewTagNames").val(NewTagNames);


    }

}


