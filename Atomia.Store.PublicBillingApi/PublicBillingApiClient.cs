
namespace Atomia.Store.PublicBillingApi
{
    public abstract class PublicBillingApiClient
    {
        private PublicBillingApiProxy billingApi;

        public PublicBillingApiClient(PublicBillingApiProxy billingApi)
        {
            this.billingApi = billingApi;
        }

        public PublicBillingApiProxy BillingApi
        {
            get
            {
                return billingApi;
            }
            set
            {
                billingApi = value;
            }
        }
    }
}
