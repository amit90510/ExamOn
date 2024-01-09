using System.Web;
using System.Web.Optimization;

namespace ExamOn
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-3.7.1.min.js", "~/Scripts/JqueryPost.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new Bundle("~/bundles/bootstraps").Include(
                      "~/Scripts/bootstrap.min.js", "~/Scripts/bootstrap.bundle.min.js", "~/Scripts/sweetalert2.all.min.js", "~/Scripts/jquery.signalR-2.4.3.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css", "~/Content/bootstrap-grid.min.css","~/Content/font-awesome.css", "~/Content/sweetalert2.min.css","~/Content/Animate.css"));

            bundles.Add(new StyleBundle("~/Content/Dashboardcss").Include(
                     "~/Content/Dashboard.css"));

        }
    }
}
