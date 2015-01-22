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
                "~/Themes/Default/Scripts/atomia/polyfills.js",
                "~/Themes/Default/Scripts/atomia/atomia.utils.js",
                "~/Themes/Default/Scripts/atomia/atomia.api.definitions.js",
                "~/Themes/Default/Scripts/atomia/atomia.ko.submitvalid-binding.js",
                "~/Themes/Default/Scripts/atomia/atomia.ko.slidevisible-binding.js",
                "~/Themes/Default/Scripts/atomia/atomia.api.cart.js",
                "~/Themes/Default/Scripts/atomia/atomia.viewmodels.languageselector.js",
                "~/Themes/Default/Scripts/atomia/atomia.viewmodels.cart.js",
                "~/Themes/Default/Scripts/atomia/atomia.viewmodels.progress.js",
                "~/Themes/Default/Scripts/atomia/atomia.viewmodels.domainconnection.js"));

            bundles.Add(new ScriptBundle("~/bundles/domains").Include(
                "~/Themes/Default/Scripts/atomia/atomia.api.domains.js",
                "~/Themes/Default/Scripts/atomia/atomia.viewmodels.domainspage.js",
                "~/Themes/Default/Scripts/atomia/atomia.viewmodels.domainregistration.js",
                "~/Themes/Default/Scripts/atomia/atomia.viewmodels.domaintransfer.js"));

            bundles.Add(new ScriptBundle("~/bundles/hostingpackages").Include(
                "~/Themes/Default/Scripts/atomia/atomia.viewmodels.productslisting.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Themes/Default/Content/css/atomia_2/style.css",
                "~/Themes/Default/Content/fonts/atomia_2/atomicons*"));
        }
    }
}
