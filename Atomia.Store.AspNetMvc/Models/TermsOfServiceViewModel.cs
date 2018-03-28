
namespace Atomia.Store.AspNetMvc.Models
{
    /// <summary>
    /// View model for the /Checkout/TermsOfService page.
    /// </summary>
    public class TermsOfServiceViewModel
    {
        /// <summary>
        /// Unique id of the terms of service
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Localized name of the terms of service
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The actual (localized) text of the terms of service.
        /// </summary>
        public string Terms { get; set; }

        /// <summary>
        /// Link to external terms of service.
        /// </summary>
        public string Link { get; set; }
    }
}
