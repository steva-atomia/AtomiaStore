using Atomia.Web.Plugin.Validation.ValidationAttributes;

namespace Atomia.Store.AspNetMvc.Models
{
    /// <summary>
    /// View model for the DomainsController::Index page.
    /// </summary>
    public class DomainsViewModel
    {

        [AtomiaRequired("Common,ErrorEmptyField")]
        [AtomiaRegularExpression("DomainSearch", "Common,ErrorInvalidDomain", true)]
        public string SearchQuery { get; set; }
    }
}
