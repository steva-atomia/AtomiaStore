
namespace Atomia.Store.PublicBillingApi
{
    /// <summary>
    /// Base for classes that need to communicate with Atomia Billing Public Service
    /// </summary>
    public abstract class PublicBillingApiClient
    {
        private PublicBillingApiProxy billingApi;

        /// <summary>
        /// Create new instance with billing api
        /// </summary>
        public PublicBillingApiClient(PublicBillingApiProxy billingApi)
        {
            this.billingApi = billingApi;
        }

        /// <summary>
        /// Property to use for calling methods in Atomia Billing Public Service
        /// </summary>
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
