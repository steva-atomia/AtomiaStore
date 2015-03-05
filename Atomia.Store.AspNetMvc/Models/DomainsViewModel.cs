using Atomia.Web.Plugin.Validation.ValidationAttributes;

namespace Atomia.Store.AspNetMvc.Models
{
    public class DomainsViewModel
    {

        [AtomiaRequired("ValidationErrors,ErrorEmptyField")]
        [AtomiaRegularExpression("DomainSearch", "ValidationErrors,Invalid domain name", true)]
        public string SearchQuery { get; set; }
    }
}
