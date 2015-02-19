using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.PublicBillingApi.Handlers;
using Atomia.Store.PublicBillingApi;
using Atomia.Store.Core;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;

namespace Atomia.Store.PublicOrderHandlers
{
    public class CurrencyHandler : OrderDataHandler
    {
        private readonly ICurrencyPreferenceProvider currencyPreferenceProvider;

        public CurrencyHandler(ICurrencyPreferenceProvider currencyPreferenceProvider)
        {
            if (currencyPreferenceProvider == null)
            {
                throw new ArgumentNullException("currencyPreferenceProvider");
            }

            this.currencyPreferenceProvider = currencyPreferenceProvider;
        }

        public override PublicOrder AmendOrder(PublicOrder order, PublicOrderContext orderContext)
        {
            var currency = currencyPreferenceProvider.GetCurrentCurrency();

            order.Currency = currency.Code;

            return order;
        }
    }
}
