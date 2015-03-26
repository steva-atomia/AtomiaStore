
namespace Atomia.Store.Core
{
    /// <summary>
    /// Get and set user's preferred language
    /// </summary>
    public interface ILanguagePreferenceProvider
    {
        /// <summary>
        /// Set user's preferred <see cref="Language"/>
        /// </summary>
        void SetPreferredLanguage(Language language);

        /// <summary>
        /// Get user's preferred <see cref="Language"/>
        /// </summary>
        Language GetCurrentLanguage();
    }
}
