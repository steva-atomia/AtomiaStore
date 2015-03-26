
namespace Atomia.Store.Core
{
    /// <summary>
    /// Data that can be used to infer a <see cref="Reseller"/>
    /// </summary>
    public sealed class ResellerIdentifier
    {
        /// <summary>
        /// A hash of reseller's account id
        /// </summary>
        public string AccountHash { get; set; }

        /// <summary>
        /// Host name of reseller's store site.
        /// </summary>
        public string BaseUrl { get; set; }
    }
}
