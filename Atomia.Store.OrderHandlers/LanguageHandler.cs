using Atomia.Store.Core;
using Atomia.Store.PublicBillingApi.Handlers;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System;

namespace Atomia.Store.PublicOrderHandlers
{
    public class LanguageHandler : OrderDataHandler
    {
        private readonly ILanguagePreferenceProvider languagePreferenceProvider;

        public LanguageHandler(ILanguagePreferenceProvider languagePreferenceProvider)
        {
            if (languagePreferenceProvider == null)
            {
                throw new ArgumentNullException("languagePreferenceProvider");
            }

            this.languagePreferenceProvider = languagePreferenceProvider;
        }

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
