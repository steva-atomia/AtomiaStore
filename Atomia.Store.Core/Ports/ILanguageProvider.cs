using System.Collections.Generic;

namespace Atomia.Store.Core
{
    public interface ILanguageProvider
    {
        IList<Language> GetAvailableLanguages();

        Language GetDefaultLanguage();
    }
}
