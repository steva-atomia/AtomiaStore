using Atomia.Store.AspNetMvc.Helpers;
using Atomia.Store.Core;
using System.Web;

namespace Atomia.Store.AspNetMvc.Adapters
{
    /// <summary>
    /// Session backed <see cref="Atomia.Store.Core.IResellerIdentifierProvider"/>
    /// </summary>
    public sealed class ResellerIdentifierProvider : IResellerIdentifierProvider
    {
        /// <summary>
        /// Get <see cref="Atomia.Store.Core.ResellerIdentifier"/> from session, or default with empty hash and current base URI.
        /// </summary>
        public ResellerIdentifier GetResellerIdentifier()
        {
            var identifier = HttpContext.Current.Session["ResellerIdentifier"] as ResellerIdentifier;

            if (identifier == null)
            {
                identifier = new ResellerIdentifier
                {
                    AccountHash = "",
                    BaseUrl = BaseUriHelper.GetBaseUriString()
                };
            }

            return identifier;
        }

        /// <summary>
        /// Save <see cref="ResellerIdentifier"/> in session
        /// </summary>
        public void SetResellerIdentifier(ResellerIdentifier identifier)
        {
            HttpContext.Current.Session["ResellerIdentifier"] = identifier;
        }
    }
}
