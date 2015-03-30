using Atomia.Common.Validation;
using Atomia.Web.Plugin.Validation.ValidationAttributes;

namespace Atomia.Store.AspNetMvc.Models
{
    /// <summary>
    /// Slightly customized implementation of the standard <see cref="ContactModel" />
    /// </summary>
    public class MainContactModel : ContactModel
    {
        /// <summary>
        /// Unique <see cref="Atomia.Store.Core.ContactData"/> identifier.
        /// </summary>
        public override string Id
        {
            get { return "MainContact"; }
        }

        /// <summary>
        /// Override the default from <see cref="ContactModel"/> to add AtomiaUsername validation.
        /// </summary>
        [AtomiaRequired("ValidationErrors,ErrorEmptyField")]
        [CustomerValidation(CustomerValidationType.Email, "CustomerValidation,Email")]
        [AtomiaUsername("ValidationErrors,ErrorUsernameNotAvailable")]
        public override string Email { get; set; }
    }
}
