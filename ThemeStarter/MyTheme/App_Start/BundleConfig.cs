using System.Web.Optimization;


namespace $MyTheme$
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            /* This configuration assumes you want to extend, override or add to the default scripts and styles 
             * To start over from scratch instead, call bundles.Clear(); at the top of this method.
             */

            var scriptBundle = bundles.GetBundleFor(Atomia.Store.Themes.Default.BundleConfig.DEFAULT_SCRIPTS_BUNDLE);
            scriptBundle.IncludeDirectory("~/Themes/$MyTheme$/Scripts", "*.js");

            var styleBundle = bundles.GetBundleFor(Atomia.Store.Themes.Default.BundleConfig.DEFAULT_STYLES_BUNDLE);
            styleBundle.Include("~/Themes/$MyTheme$/Content/css/theme.css");

            // Re-register bundles
            bundles.Clear();
            bundles.Add(scriptBundle);
            bundles.Add(styleBundle);
        }
    }
}
