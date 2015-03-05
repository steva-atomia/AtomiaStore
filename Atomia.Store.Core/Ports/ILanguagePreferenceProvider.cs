
namespace Atomia.Store.Core
{
    public interface ILanguagePreferenceProvider
    {
        void SetPreferredLanguage(Language language);

        Language GetCurrentLanguage();
    }
}
