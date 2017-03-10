using Atomia.Web.Plugin.Validation.ValidationAttributes;

namespace Atomia.Store.AspNetMvc.Models
{
    /// <summary>
    /// View model for the DomainsController::Index page.
    /// </summary>
    public class DomainsViewModel
    {

        [AtomiaRequired("Common,ErrorEmptyField")]
        public string SearchQuery { get; set; }
    }
}
