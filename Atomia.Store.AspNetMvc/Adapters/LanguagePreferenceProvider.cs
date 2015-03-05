using Atomia.Store.Core;
using System;
using System.Web;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Adapters
{
    public sealed class LanguagePreferenceProvider : ILanguagePreferenceProvider
    {
        private readonly ILanguageProvider languageProvider = DependencyResolver.Current.GetService<ILanguageProvider>();

        public void SetPreferredLanguage(Language language)
        {
            if (language == null)
            {
                throw new ArgumentException("language");
            }

            HttpContext.Current.Session["PreferredLanguage"] = language;
        }

        public Language GetCurrentLanguage()
        {
            var language = HttpContext.Current.Session["PreferredLanguage"] as Language;

            if (language == null)
            {
                language = languageProvider.GetDefaultLanguage();
            }

            return language;
        }
    }
}
