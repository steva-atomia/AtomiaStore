using Atomia.Web.Plugin.Validation.ValidationAttributes;

namespace Atomia.Store.AspNetMvc.Models
{
    /// <summary>
    /// View model for the DomainsController::Index page.
    /// </summary>
    public class DomainsViewModel
    {

        [AtomiaRequired("ValidationErrors,ErrorEmptyField")]
        [AtomiaRegularExpression("DomainSearch", "ValidationErrors,Invalid domain name", true)]
        public string SearchQuery { get; set; }
    }
}
