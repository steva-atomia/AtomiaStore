
namespace Atomia.Store.Core
{
    /// <summary>
    /// Base for contact data input classes.
    /// </summary>
    /// <remarks>
    /// Used to connect input of contact data with contact data handling.
    /// </remarks>
    public abstract class ContactData
    {
        /// <summary>
        /// Id of the contact data.
        /// </summary>
        public abstract string Id { get; }
    }
}
