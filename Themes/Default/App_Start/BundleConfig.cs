using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Optimization;

namespace Atomia.Store.Themes.Default
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/shared").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery.validate.js",
                "~/Scripts/jquery.validate.unobtrusive.js",
                "~/Scripts/jquery.unobtrusive-ajax.js",
                "~/Scripts/knockout-{version}.js",
                "~/Scripts/underscore.js",
                "~/Scripts/amplify.js",
                "~/Scripts/lib/api-definitions.js",
                "~/Scripts/lib/ko-binding-submitvalid.js",
                "~/Scripts/viewmodels/root.js"));

            bundles.Add(new ScriptBundle("~/bundles/domains").Include(
                "~/Scripts/lib/api-domains.js",
                "~/Scripts/viewmodels/domains.js",
                "~/Scripts/viewmodels/domainreg.js",
                "~/Scripts/viewmodels/domaintransfer.js"));

            bundles.Add(new StyleBundle("~/Content/css/skeleton").Include(
                "~/Content/css/skeleton/base.css",
                "~/Content/css/skeleton/layout.css",
                "~/Content/css/skeleton/skeleton.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/css/style.css"));
        }
    }
}
