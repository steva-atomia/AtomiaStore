using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.Core;
using System.Web;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Adapters
{
    public class LanguagePreferenceProvider : ILanguagePreferenceProvider
    {
        private readonly ILanguageProvider languageProvider = DependencyResolver.Current.GetService<ILanguageProvider>();

        public void SetPreferredLanguage(Language language)
        {
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
