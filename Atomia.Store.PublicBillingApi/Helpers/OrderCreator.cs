using Atomia.Store.Core;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;

namespace Atomia.Store.PublicBillingApi
{
    public interface IOrderCreator
    {
        PublicOrder CreateBasicOrder();
    }

    public class OrderCreator : IOrderCreator
    {
        private readonly IResellerProvider resellerProvider;
        private readonly ICurrencyProvider currencyProvider; 
        private readonly ICountryProvider countryProvider;

        public OrderCreator(IResellerProvider resellerProvider, ICurrencyProvider currencyProvider, ICountryProvider countryProvider)
        {
            this.resellerProvider = resellerProvider;
            this.currencyProvider = currencyProvider;
            this.countryProvider = countryProvider;
        }

        public PublicOrder CreateBasicOrder()
        {
            return new PublicOrder
            {
                ResellerId = resellerProvider.GetResellerId(),
                Country = countryProvider.GetCountryCode(),
                Currency = currencyProvider.GetCurrencyCode()
            };
        }
    }
}
