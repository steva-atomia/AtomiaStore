using Atomia.Store.AspNetMvc.Infrastructure;
using Atomia.Store.Core;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Controllers
{
    /// <summary>
    /// Language JSON API.
    /// </summary>
    public sealed class LanguageController : Controller
    {
        private readonly ILanguageProvider languageProvider = DependencyResolver.Current.GetService<ILanguageProvider>();
        private readonly ILanguagePreferenceProvider languagePreferenceProvider = DependencyResolver.Current.GetService<ILanguagePreferenceProvider>();

        /// <summary>
        /// Get current and available langugages
        /// </summary>
        public JsonResult GetLanguages()
        {
            return JsonEnvelope.Success(new
            {
                Languages = languageProvider.GetAvailableLanguages(),
                CurrentLanguage = languagePreferenceProvider.GetCurrentLanguage()
            });
        }
    }
}
