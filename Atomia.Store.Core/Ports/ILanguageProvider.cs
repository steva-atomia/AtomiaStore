using System.Collections.Generic;

namespace Atomia.Store.Core
{
    /// <summary>
    /// Get default and available languages. <see cref="Language"/>
    /// </summary>
    public interface ILanguageProvider
    {
        /// <summary>
        /// Get all available languages, <see cref="Language"/>
        /// </summary>
        IList<Language> GetAvailableLanguages();

        /// <summary>
        /// Get the default <see cref="Language"/>
        /// </summary>
        Language GetDefaultLanguage();
    }
}
