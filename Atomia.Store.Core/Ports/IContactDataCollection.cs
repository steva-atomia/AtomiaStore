using System.Collections.Generic;

namespace Atomia.Store.Core
{
    public interface IContactDataCollection
    {
        IEnumerable<ContactData> GetContactData();
    }
}
