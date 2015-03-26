using System.Collections.Generic;

namespace Atomia.Store.Core
{
    /// <summary>
    /// An enumerable collection of <see cref="ContactData"/>
    /// </summary>
    public interface IContactDataCollection
    {
        /// <summary>
        /// Get the <see cref="ContactData"/> collection
        /// </summary>
        IEnumerable<ContactData> GetContactData();
    }
}
