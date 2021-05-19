using System.Web;
using System.Web.Optimization;
using ErucaCRM.Utility;
namespace ErucaCRM.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/BootstrapScripts").Include(
                        "~/Scripts/jquery-1.10.2.js",
                        "~/Scripts/jquery-ui-1.9.2.js",
                        "~/Scripts/knockout-3.0.0.js",
                        "~/Scripts/ErucaCRM.Framework/ErucaCRM.Framework.Core.js",
                        "~/Scripts/ErucaCRM.Framework/ErucaCRM.Framework.Common.js",
                        "~/Scripts/jquery.tinyscrollbar.js",
                        "~/Scripts/ErucaCRM.User/ErucaCRM.User.InnerLayout.js",
                         "~/Scripts/jquery.signalR-2.1.0.js",
                                            //"~/Scripts/bootstrap.js"
                "~/Scripts/bootstrap.min.js"
                //"~/Scripts/sb-admin-2.js"
                //"~/Scripts/jquery-1.11.0.js"
                      
            ));
            bundles.Add(new ScriptBundle("~/bundles/MainScripts").Include(
                        "~/Scripts/jquery-1.10.2.js",
                        "~/Scripts/jquery-ui-1.9.2.js",
                        "~/Scripts/knockout-3.0.0.js",
                        "~/Scripts/ErucaCRM.Framework/ErucaCRM.Framework.Core.js",
                        "~/Scripts/ErucaCRM.Framework/ErucaCRM.Framework.Common.js",
                        "~/Scripts/jquery.tinyscrollbar.js",
                        "~/Scripts/ErucaCRM.User/ErucaCRM.User.InnerLayout.js",
                         "~/Scripts/jquery.signalR-2.1.0.js"

                        ));
            bundles.Add(new ScriptBundle("~/bundles/ValidateScripts").Include(
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery.validate.unobtrusive.js"

                        ));


            //Add  User profile

            bundles.Add(new ScriptBundle("~/bundles/AddUserProfileScripts").Include(
                "~/Scripts/ErucaCRM.User/ErucaCRM.User.AddUser.js",
                  "~/Scripts/jquery.validate.js",
                     "~/Scripts/jquery.validate.unobtrusive.js"
                ));


            //Edit user profile scripts

            bundles.Add(new ScriptBundle("~/bundles/EditUserProfileScripts").Include(
              "~/Scripts/file-upload/jquery.fileupload.js",
              "~/Scripts/file-upload/jquery.fileupload-ui.js",
              "~/Scripts/file-upload/jquery.iframe-transport.js",
                "~/Scripts/jquery.validate.js",
                   "~/Scripts/jquery.validate.unobtrusive.js"
              ));

            //Add Edit Account scripts
            bundles.Add(new ScriptBundle("~/bundles/AddEditAccountScripts").Include(
                     "~/Scripts/ErucaCRM.User/ErucaCRM.User.AddEditAccount.js",
                      "~/Scripts/jquery.validate.js",
                     "~/Scripts/jquery.validate.unobtrusive.js"
                     ));

            //Account details scripts
            bundles.Add(new ScriptBundle("~/bundles/AccountDetailScripts").Include(
             "~/Scripts/ErucaCRM.User/ErucaCRM.User.ViewAccount.js",
              "~/Scripts/ErucaCRM.Framework/ErucaCRM.Framework.DateFormat.js",
              "~/Scripts/file-upload/jquery.uploadfile.js",
               "~/Scripts/file-upload/jquery.form.js"

             ));



            //Lead page scripts

            bundles.Add(new ScriptBundle("~/bundles/LeadScripts").Include(
                        "~/Scripts/ErucaCRM.User/ErucaCRM.User.Leads.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js",
                         "~/Scripts/file-upload/jquery.uploadfile.js",
                          "~/Scripts/file-upload/jquery.form.js",
                         "~/Scripts/HighCharts/highcharts.js",
                         "~/Scripts/HighCharts/exporting.js",
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery.validate.unobtrusive.js",
                        "~/Scripts/jquery.mousewheel.js",
                          "~/Scripts/perfect-scrollbar.js"
                        ));


            //Account Case scripts
            bundles.Add(new ScriptBundle("~/bundles/AccountCaseDetailScripts").Include(
                     "~/Scripts/ErucaCRM.User/ErucaCRM.User.CreateCaseMessageBoard.js",
                      "~/Scripts/file-upload/jquery.uploadfile.js",
                       "~/Scripts/file-upload/jquery.form.js",
                       "~/Scripts/ErucaCRM.Framework/ErucaCRM.Framework.DateFormat.js"
                     ));


            //Add Edit Contact scripts

            bundles.Add(new ScriptBundle("~/bundles/AddEditContactScripts").Include(
                 "~/Scripts/ErucaCRM.User/ErucaCRM.User.AddEditContact.js",
                   "~/Scripts/jquery.validate.js",
                    "~/Scripts/jquery.validate.unobtrusive.js"
                 ));

            //Contact view scripts

            bundles.Add(new ScriptBundle("~/bundles/ContactViewScripts").Include(
                 "~/Scripts/ErucaCRM.User/ErucaCRM.User.ContactView.js",
                   "~/Scripts/file-upload/jquery.uploadfile.js",
                    "~/Scripts/file-upload/jquery.form.js"
                 ));


            //Contact List Scripts

            bundles.Add(new ScriptBundle("~/bundles/ContactListScripts").Include(
                  "~/Scripts/ErucaCRM.User/ErucaCRM.User.Contacts.js",
                    "~/Scripts/file-upload/jquery.uploadfile.js",
                     "~/Scripts/file-upload/jquery.form.js",
                     "~/Scripts/ErucaCRM.User/ErucaCRM.User.AccountAndContact.js"
                  ));


            //Add Edit Sale order scripts

            bundles.Add(new ScriptBundle("~/bundles/AddEditSaleOrderScripts").Include(
                 "~/Scripts/ErucaCRM.User/ErucaCRM.User.CreateSalesOrder.js",
                   "~/Scripts/jquery.validate.js",
                    "~/Scripts/jquery.validate.unobtrusive.js"
                 ));

            //DashBoard Scripts

            bundles.Add(new ScriptBundle("~/bundles/DashBoardScripts").Include(
          "~/Scripts/ErucaCRM.User/ErucaCRM.User.Dashboard.js",
            "~/Scripts/HighCharts/highcharts.js",
             "~/Scripts/HighCharts/exporting.js",
              "~/Scripts/HighCharts/funnel.js"
          ));

            //Home Scripts
            bundles.Add(new ScriptBundle("~/bundles/HomeScripts").Include(
            "~/Scripts/ErucaCRM.User/ErucaCRM.User.Home.js",
            "~/Scripts/HighCharts/highcharts.js",
            "~/Scripts/HighCharts/exporting.js",
            "~/Scripts/HighCharts/funnel.js",
            "~/Scripts/circles.js",
           "~/Scripts/canvasjs.min.js",
           "~/Scripts/ErucaCRM.Framework/ErucaCRM.Framework.DateFormat.js"
    ));
            //View All RecentActivity Scripts
            bundles.Add(new ScriptBundle("~/bundles/ViewAllRecentActivity").Include(           
           "~/Scripts/ErucaCRM.Framework/ErucaCRM.Framework.DateFormat.js",
             "~/Scripts/ErucaCRM.User/ErucaCRM.User.ViewAllRecentActivity.js"
    ));



            //Role Scripts
            bundles.Add(new ScriptBundle("~/bundles/RoleScripts").Include(
            "~/Scripts/ErucaCRM.User/ErucaCRM.User.Roles.js",
            "~/Scripts/knockout.mapping.js"
    ));


            //TaskItem scripts
            bundles.Add(new ScriptBundle("~/bundles/TaskItemScripts").Include(
                "~/Scripts/ErucaCRM.User/ErucaCRM.User.TaskItem.js",
                  "~/Scripts/jquery.validate.js",
                   "~/Scripts/jquery.validate.unobtrusive.js"
                ));

            //Register Page Script
            bundles.Add(new ScriptBundle("~/bundles/RegisterScripts").Include(
              "~/Scripts/jquery-1.10.2.js",
                "~/Scripts/knockout-3.0.0.js",
                 "~/Scripts/ErucaCRM.Framework/ErucaCRM.Framework.Core.js",
                 "~/Scripts/jquery.validate.js",
                 "~/Scripts/jquery.validate.unobtrusive.js",
                 "~/Scripts/ErucaCRM.User/ErucaCRM.User.AddUser.js"
              ));

            bundles.Add(new ScriptBundle("~/bundles/TaskItemDetailScripts").Include(
             "~/Scripts/ErucaCRM.User/ErucaCRM.User.ViewActivity.js",
               "~/Scripts/file-upload/jquery.uploadfile.js",
                "~/Scripts/file-upload/jquery.form.js"
             ));


            bundles.Add(new StyleBundle("~/Content/BootstrapCss").Include(
          //"~/Content/css/Main.css",
                "~/Content/css/jquery-ui.css",
                "~/Content/css/uploadfile.css",
                "~/Content/css/perfect-scrollbar.css",
                "~/Content/css/tinyscrollbar.css" ,
                     "~/Content/css/bootstrap.min.css",
                 
                 "~/Content/css/font-awesome.min.css",
                 "~/Content/css/inner.css"
        ));
            bundles.Add(new StyleBundle("~/Content/MainCss").Include(
                "~/Content/css/main.css",
                "~/Content/css/jquery-ui.css",
                "~/Content/css/uploadfile.css",
                "~/Content/css/perfect-scrollbar.css",
                "~/Content/css/tinyscrollbar.css"

                ));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
            //            "~/Scripts/jquery-ui-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.unobtrusive*",
            //            "~/Scripts/jquery.validate*"));

            //// Use the development version of Modernizr to develop with and learn from. Then, when you're
            //// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            //bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
            //            "~/Content/themes/base/jquery.ui.core.css",
            //            "~/Content/themes/base/jquery.ui.resizable.css",
            //            "~/Content/themes/base/jquery.ui.selectable.css",
            //            "~/Content/themes/base/jquery.ui.accordion.css",
            //            "~/Content/themes/base/jquery.ui.autocomplete.css",
            //            "~/Content/themes/base/jquery.ui.button.css",
            //            "~/Content/themes/base/jquery.ui.dialog.css",
            //            "~/Content/themes/base/jquery.ui.slider.css",
            //            "~/Content/themes/base/jquery.ui.tabs.css",
            //            "~/Content/themes/base/jquery.ui.datepicker.css",
            //            "~/Content/themes/base/jquery.ui.progressbar.css",
            //            "~/Content/themes/base/jquery.ui.theme.css"));
        }
    }
}