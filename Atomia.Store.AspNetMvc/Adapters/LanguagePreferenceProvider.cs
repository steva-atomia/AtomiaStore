using Atomia.Store.Core;
using System;
using System.Web;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Adapters
{
    /// <summary>
    /// Session backed <see cref="Atomia.Store.Core.ILanguagePreferenceProvider"/>
    /// </summary>
    public sealed class LanguagePreferenceProvider : ILanguagePreferenceProvider
    {
        private readonly ILanguageProvider languageProvider = DependencyResolver.Current.GetService<ILanguageProvider>();

        /// <inheritdoc/>
        public void SetPreferredLanguage(Language language)
        {
            if (language == null)
            {
                throw new ArgumentException("language");
            }

            HttpContext.Current.Session["PreferredLanguage"] = language;
        }

        /// <inheritdoc/>
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
