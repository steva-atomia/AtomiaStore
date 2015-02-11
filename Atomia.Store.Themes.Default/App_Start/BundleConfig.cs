using System.Web.Optimization;

namespace Atomia.Store.Themes.Default
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                "~/Themes/Default/Scripts/jquery-{version}.js",
                "~/Themes/Default/Scripts/jquery.validate.js",
                "~/Themes/Default/Scripts/MicrosoftMvcJQueryValidation.js",
                "~/Themes/Default/Scripts/knockout-{version}.js",
                "~/Themes/Default/Scripts/underscore.js",
                "~/Themes/Default/Scripts/amplify.js",
                "~/Themes/Default/Scripts/atomia/AtomiaValidation.js",
                "~/Themes/Default/Scripts/atomia/atomia.utils.js",
                "~/Themes/Default/Scripts/atomia/atomia.api.definitions.js",
                "~/Themes/Default/Scripts/atomia/atomia.api.cart.js",
                "~/Themes/Default/Scripts/atomia/atomia.api.domains.js",
                "~/Themes/Default/Scripts/atomia/atomia.ko.submitvalid-binding.js",
                "~/Themes/Default/Scripts/atomia/atomia.ko.slidevisible-binding.js",
                "~/Themes/Default/Scripts/atomia/atomia.viewmodels.languageselector.js",
                "~/Themes/Default/Scripts/atomia/atomia.viewmodels.cart.js",
                "~/Themes/Default/Scripts/atomia/atomia.viewmodels.progress.js",
                "~/Themes/Default/Scripts/atomia/atomia.viewmodels.domainconnection.js",
                "~/Themes/Default/Scripts/atomia/atomia.viewmodels.domainspage.js",
                "~/Themes/Default/Scripts/atomia/atomia.viewmodels.domainregistration.js",
                "~/Themes/Default/Scripts/atomia/atomia.viewmodels.domaintransfer.js",
                "~/Themes/Default/Scripts/atomia/atomia.viewmodels.productslisting.js",
                "~/Themes/Default/Scripts/atomia/atomia.viewmodels.account.js",
                "~/Themes/Default/Scripts/atomia/atomia.viewmodels.paymentselector.js"));

            bundles.Add(new StyleBundle("~/Themes/Default/Content/css/atomia_2/style").Include(
                "~/Themes/Default/Content/css/atomia_2/style.css"));
        }
    }
}
