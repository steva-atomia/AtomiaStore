
namespace Atomia.Store.Core
{
    public interface IContactDataProvider
    {
        IContactDataCollection GetContactData();

        void SaveContactData(IContactDataCollection contactData);
    }
}
