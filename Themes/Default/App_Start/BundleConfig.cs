using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Optimization;

namespace Atomia.OrderPage.Themes.Default
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bundles/shared").Include(
                        "~/Scripts/knockout-{version}.js"));

            bundles.Add(new StyleBundle("~/Content/css/skeleton").Include(
                "~/Content/css/skeleton/base.css",
                "~/Content/css/skeleton/layout.css",
                "~/Content/css/skeleton/skeleton.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
               "~/Content/css/style.css"));
        }
    }
}
