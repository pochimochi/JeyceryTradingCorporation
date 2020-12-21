using System.Web;
using System.Web.Optimization;

namespace JeyceryTradingCorporation
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/scripts")
                    .IncludeDirectory("~/Scripts/", "*.js", true));


            

            bundles.Add(new ScriptBundle("~/Content/css")
                    .IncludeDirectory("~/Content/", "*.css", true));
        }
    }
}
