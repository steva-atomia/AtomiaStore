using Atomia.Store.Core;
using Atomia.Store.PublicBillingApi.Handlers;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System;

namespace Atomia.Store.PublicOrderHandlers
{
    /// <summary>
    /// Handler to amend order with "Language" custom attribute from customer's language preference.
    /// </summary>
    public class LanguageHandler : OrderDataHandler
    {
        private readonly ILanguagePreferenceProvider languagePreferenceProvider;

        /// <summary>
        /// Create new instance with access to customer's language preference.
        /// </summary>
        public LanguageHandler(ILanguagePreferenceProvider languagePreferenceProvider)
        {
            if (languagePreferenceProvider == null)
            {
                throw new ArgumentNullException("languagePreferenceProvider");
            }

            this.languagePreferenceProvider = languagePreferenceProvider;
        }

        /// <summary>
        /// Amend order with "Language" custom attribute from customer's language preference.
        /// </summary>
        public override PublicOrder AmendOrder(PublicOrder order, PublicOrderContext orderContext)
        {
            var language = languagePreferenceProvider.GetCurrentLanguage();

            Add(order, new PublicOrderCustomData
            {
                Name = "Language",
                Value = language.Tag
            });

            return order;
        }
    }
}
