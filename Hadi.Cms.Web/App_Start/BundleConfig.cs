using System.Web.Optimization;

namespace Hadi.Cms.Web.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Scripts
            bundles.Add(new ScriptBundle("~/bundles/jquery211").Include(
                    "~/Content/RadaTemplate/js/jquery-2.1.1.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                    "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                    "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                    "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                    "~/Scripts/bootstrap.js",
                    "~/Scripts/respond.js",
                    "~/Scripts/modal.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-rtl").Include(
                    "~/Scripts/bootstrap.rtl.js",
                    "~/Scripts/respond.js",
                    "~/Scripts/modal.js"));

            bundles.Add(new ScriptBundle("~/bundles/signalr").Include(
                    "~/Scripts/jquery.signalR-2.4.1.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/common").Include(
                    "~/Scripts/Namespace.js",
                    "~/Scripts/String.js",
                    "~/Scripts/Custom/custom.icommon.js",
                    "~/Scripts/Custom/custom.showTreeObjectPicker.js",
                    "~/Scripts/Custom/custom.locationPicker.js"));

            bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
                    "~/Scripts/KendoUI/kendo.all.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/kendo-fa").Include(
                    "~/Scripts/KendoUI/messages/kendo.messages.fa-IR.js",
                    "~/Scripts/kendo.messages.fa-IR.Override.js"));

            bundles.Add(new ScriptBundle("~/bundles/DatePicker").Include(
                "~/Scripts/DatePicker/bootstrap-datepicker.min.js",
                "~/Scripts/DatePicker/bootstrap-datepicker.fa.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/radaJs").Include(
                //Mainly scripts
                "~/Content/RadaTemplate/js/bootstrap.min.js",
                "~/Content/RadaTemplate/js/plugins/metisMenu/jquery.metisMenu.js",
                "~/Content/RadaTemplate/js/plugins/slimscroll/jquery.slimscroll.min.js",
                "~/Content/RadaTemplate/js/plugins/flot/jquery.flot.js",
                "~/Content/RadaTemplate/js/plugins/flot/jquery.flot.tooltip.min.js",
                "~/Content/RadaTemplate/js/plugins/flot/jquery.flot.spline.js",
                "~/Content/RadaTemplate/js/plugins/flot/jquery.flot.resize.js",
                "~/Content/RadaTemplate/js/plugins/flot/jquery.flot.pie.js",
                "~/Content/RadaTemplate/js/plugins/flot/jquery.flot.symbol.js",
                "~/Content/RadaTemplate/js/plugins/flot/curvedLines.js",
                "~/Content/RadaTemplate/js/plugins/peity/jquery.peity.min.js",
                //persian number
                "~/Scripts/persianumber.min.js",
                //Custom and plugin javascript
                "~/Content/RadaTemplate/js/rada.js",
                "~/Content/RadaTemplate/js/plugins/pace/pace.min.js",
                "~/Content/RadaTemplate/js/plugins/jquery-ui/jquery-ui.min.js",
                "~/Content/RadaTemplate/js/plugins/jvectormap/jquery-jvectormap-2.0.2.min.js",
                "~/Content/RadaTemplate/js/plugins/jvectormap/jquery-jvectormap-world-mill-en.js",
                "~/Content/RadaTemplate/js/plugins/sparkline/jquery.sparkline.min.js",
                //Sparkline demo data
                "~/Content/RadaTemplate/js/plugins/chartJs/Chart.min.js",
                //Toastr
                "~/Content/RadaTemplate/js/plugins/toastr/toastr.min.js",

                /// Select2
                "~/Scripts/select2.min.js"
                ));

            bundles.Add(new ScriptBundle("~/Scripts/Hadi/AllProject").Include(
                "~/Scripts/Hadi/Project/allProject.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Hadi/AProject").Include(
                "~/Scripts/Hadi/Project/aProject.js"));

                /// Index Pages Grid Scripts
            bundles.Add(new ScriptBundle("~/Content/RadaTemplate/js/plugins").Include(
                "~/Content/RadaTemplate/js/plugins/footable/footable.all.min.js",
                "~/Content/RadaTemplate/js/plugins/sweetalert/sweetalert.min.js",
                "~/Content/RadaTemplate/js/plugins/chosen/chosen.jquery.js"));


            /// For CKEditor and chekbox
            bundles.Add(new ScriptBundle("~/Content/RadaTemplate/js/plugins/summernote").Include(
            /// for checkbox and radios
            "~/Content/RadaTemplate/js/plugins/iCheck/icheck.min.js",

            /// for CKEditor
            "~/Content/RadaTemplate/js/plugins/summernote/summernote.min.js"
            ));

            bundles.Add(new ScriptBundle("~/Scripts/Hadi/MainJS").Include(
                "~/Scripts/Hadi/main.js",
                "~/Scripts/jquery-1.10.2.js"
            ));

            /// Style Sheets
            bundles.Add(new StyleBundle("~/Content/Stylesheet/css").Include(
                    "~/Content/Stylesheet/bootstrap.css",
                    "~/Content/Stylesheet/bootstrap.overrides.css",
                    "~/Content/Stylesheet/Site.css",
                    "~/Content/Stylesheet/Custom/custom.locationpicker.css",
                    "~/Content/Stylesheet/glyphicons.css",
                    "~/Content/Stylesheet/Views.TaskList.css",
                    "~/Content/Stylesheet/htdf/css/utility/slick.css",
                    "~/Content/Stylesheet/htdf/css/utility/slick-theme.css"));

            bundles.Add(new StyleBundle("~/Content/Stylesheet/css-rtl").Include(
                    "~/Content/RadaTemplate/css/bootstrap.min.css",
                    "~/Content/RadaTemplate/css/bootstrap.rtl.min.css",
                    "~/Content/RadaTemplate/font-awesome/css/font-awesome.css",
                    "~/Content/RadaTemplate/css/animate.css",
                    "~/Content/RadaTemplate/css/style.rtl.css"));

            bundles.Add(new StyleBundle("~/Content/Stylesheet/css-ltr").Include(
                    "~/Content/Stylesheet/bootstrap.rtl.css",
                    "~/Content/Stylesheet/bootstrap.overrides.css",
                    "~/Content/Stylesheet/Site.css",
                    "~/Content/Stylesheet/glyphicons.css",
                    "~/Content/Stylesheet/Views.TaskList.css"));

            bundles.Add(new StyleBundle("~/Content/RadaTemplate/css-main-rtl").Include(
                   "~/Content/RadaTemplate/css/bootstrap.min.css",
                    "~/Content/RadaTemplate/css/bootstrap.rtl.min.css",
                    "~/Content/RadaTemplate/font-awesome/css/font-awesome.css",
                    "~/Content/RadaTemplate/css/plugins/morris/morris-0.4.3.min.css",
                    "~/Content/RadaTemplate/css/plugins/toastr/toastr.min.css",
                    "~/Content/RadaTemplate/css/animate.css",
                    "~/Content/RadaTemplate/css/style.rtl.css",

                    ///Select2
                    
                    "~/Content/Stylesheet/select2-bootstrap.css"

                    ));


            bundles.Add(new StyleBundle("~/Content/Stylesheet/kendo").Include(
                 "~/Content/Stylesheet/KendoUI/kendo.common.min.css",
                 "~/Content/Stylesheet/KendoUI/kendo.bootstrap.min.css",
                 "~/Content/Stylesheet/KendoUI/kendo.rtl.min.css"));

            bundles.Add(new StyleBundle("~/Content/Stylesheet/DatePicker").Include(
                "~/Content/Stylesheet/DatePicker/bootstrap-datepicker.css",
                "~/Content/Stylesheet/DatePicker/bootstrap-datepicker.min.css"));

            /// htdf template
            bundles.Add(new StyleBundle("~/Content/Stylesheet/htdf/css").Include(
               "~/Content/Stylesheet/htdf/css/utility/jquery-ui-1.12.1.css",
               "~/Content/Stylesheet/htdf/css/utility/hadi-pack.css",
               "~/Content/Stylesheet/htdf/css/bootstrap.min.css",
               "~/Content/Stylesheet/htdf/css/styles.css"));


            /// Index Pages Grid Styles
            bundles.Add(new StyleBundle("~/Content/RadaTemplate/css/plugins").Include(
               "~/Content/RadaTemplate/css/plugins/sweetalert/sweetalert.css",
               "~/Content/RadaTemplate/css/plugins/footable/footable.core.css",
               "~/Content/RadaTemplate/css/plugins/chosen/chosen.css"));


            /// for CKEditor and checkBox
            bundles.Add(new StyleBundle("~/Content/RadaTemplate/css/plugins/summernot").Include(
                "~/Content/RadaTemplate/css/plugins/summernote/summernote.css",
                "~/Content/RadaTemplate/css/plugins/summernote/summernote-bs3.css",
                "~/Content/RadaTemplate/css/plugins/awesome-bootstrap-checkbox/awesome-bootstrap-checkbox.rtl.css",
                "~/Content/RadaTemplate/css/plugins/iCheck/custom.css"
            ));

            #if DEBUG
            BundleTable.EnableOptimizations = false;
            #else
            BundleTable.EnableOptimizations = true;
            #endif
        }
    }
}
