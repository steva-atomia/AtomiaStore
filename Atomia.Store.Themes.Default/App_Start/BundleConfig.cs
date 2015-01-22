using System.Web.Optimization;

namespace Atomia.Store.Themes.Default
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/shared").Include(
                "~/Themes/Default/Scripts/jquery-{version}.js",
                "~/Themes/Default/Scripts/jquery.validate.js",
                "~/Themes/Default/Scripts/jquery.validate.unobtrusive.js",
                "~/Themes/Default/Scripts/jquery.unobtrusive-ajax.js",
                "~/Themes/Default/Scripts/knockout-{version}.js",
                "~/Themes/Default/Scripts/underscore.js",
                "~/Themes/Default/Scripts/amplify.js",
                "~/Themes/Default/Scripts/lib/polyfills.js",
                "~/Themes/Default/Scripts/lib/utils.js",
                "~/Themes/Default/Scripts/lib/api-definitions.js",
                "~/Themes/Default/Scripts/lib/ko-binding-submitvalid.js",
                "~/Themes/Default/Scripts/lib/ko-binding-slidevisible.js",
                "~/Themes/Default/Scripts/lib/api-cart.js",
                "~/Themes/Default/Scripts/viewmodels/language-selector.js",
                "~/Themes/Default/Scripts/viewmodels/cart.js",
                "~/Themes/Default/Scripts/viewmodels/progress.js",
                "~/Themes/Default/Scripts/viewmodels/domainconnection.js"));

            bundles.Add(new ScriptBundle("~/bundles/domains").Include(
                "~/Themes/Default/Scripts/lib/api-domains.js",
                "~/Themes/Default/Scripts/viewmodels/domains-index.js",
                "~/Themes/Default/Scripts/viewmodels/domains-domainregistration.js",
                "~/Themes/Default/Scripts/viewmodels/domains-domaintransfer.js"));

            bundles.Add(new ScriptBundle("~/bundles/hostingpackages").Include(
                "~/Themes/Default/Scripts/viewmodels/category-hostingpackages.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Themes/Default/Content/css/atomia_2/style.css",
                "~/Themes/Default/Content/fonts/atomia_2/atomicons*"));
        }
    }
}
