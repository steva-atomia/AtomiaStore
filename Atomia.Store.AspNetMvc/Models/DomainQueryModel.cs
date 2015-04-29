using Atomia.Web.Plugin.Validation.ValidationAttributes;

namespace Atomia.Store.AspNetMvc.Models
{
    /// <summary>
    /// Representation of a domain search query.
    /// </summary>
    public class DomainQueryModel
    {
        /// <summary>
        /// The actual domain search query string.
        /// </summary>
        [AtomiaRequired("Common,ErrorEmptyField")]
        [AtomiaRegularExpression("DomainSearch", "Common,ErrorInvalidDomain", true)]
        public string Query { get; set; }
    }
}
