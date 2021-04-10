using System.Web;
using System.Web.Optimization;

namespace Trendyol_Integration
{
    public class BundleConfig
    {

        public static void RegisterBundles(BundleCollection bundles)
        {
            //Scripts
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-3.6.0.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap.bundle").Include(
                       "~/Scripts/jquery.validate*"));
            bundles.Add(new ScriptBundle("~/bundles/popper").Include(
                      "~/Scripts/bootstrap.bundle.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/jquery.easing").Include(
                      "~/Scripts/jquery.easing.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/jquery.datatables").Include(
                      "~/Scripts/DataTables/jquery.dataTables.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/datatables").Include("~/Scripts/DataTables/dataTables.bootstrap4.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/sb-admin-2.min").Include(
                     "~/Scripts/sb-admin-2.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/datatables.demo").Include(
                    "~/Scripts/datatables-demo.js"));
            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
                    "~/Scripts/custom.js"));

            //Styles
            bundles.Add(new StyleBundle("~/styles/bootstrap").Include(
                    "~/Content/bootstrap.min.css"));
            bundles.Add(new StyleBundle("~/styles/sb-admin-2").Include(
                   "~/Content/sb-admin-2.min.css"));
            bundles.Add(new StyleBundle("~/styles/fontawesome").Include(
                   "~/Content/all.min.css"));
            bundles.Add(new StyleBundle("~/styles/datatables").Include(
                   "~/Content/DataTables/css/dataTables.bootstrap4.min.css"));
        }
    }
}
