
namespace Atomia.Store.Core
{
    /// <summary>
    /// Persistence provider for <see cref="ContactData"/>.
    /// </summary>
    public interface IContactDataProvider
    {
        /// <summary>
        /// Get all persisted <see cref="ContactData"/> as an <see cref="IContactDataCollection"/> for the current user
        /// </summary>
        IContactDataCollection GetContactData();

        /// <summary>
        /// Persist a <see cref="ContactData"/> instance for the current user
        /// </summary>
        void SaveContactData(IContactDataCollection contactData);

        /// <summary>
        /// Throw away all persisted <see cref="ContactData"/> for the current user
        /// </summary>
        void ClearContactData();
    }
}
