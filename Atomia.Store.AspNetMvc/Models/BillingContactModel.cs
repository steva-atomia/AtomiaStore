
namespace Atomia.Store.AspNetMvc.Models
{
    /// <summary>
    /// Implementation of the standard <see cref="ContactModel" />
    /// </summary>
    public class BillingContactModel : ContactModel
    {
        /// <summary>
        /// Unique <see cref="Atomia.Store.Core.ContactData"/> identifier.
        /// </summary>
        public override string Id
        {
            get { return "BillingContact"; }
        }
    }
}
