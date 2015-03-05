using Atomia.Store.Core;
using System.Collections.Generic;

namespace Atomia.Store.Fakes.Adapters
{
    public sealed class FakeLanguageProvider : ILanguageProvider
    {
        private readonly IResourceProvider resourceProvider;

        public FakeLanguageProvider(IResourceProvider resourceProvider)
        {
            this.resourceProvider = resourceProvider;
        }

        public IList<Language> GetAvailableLanguages()
        {
            return new List<Language>
            {
                Language.CreateLanguage(resourceProvider, "EN-US"),
                Language.CreateLanguage(resourceProvider, "SV-SE"),
                Language.CreateLanguage(resourceProvider, "FR"),
                Language.CreateLanguage(resourceProvider, "DE")
            };
        }

        public Language GetDefaultLanguage()
        {
            return Language.CreateLanguage(resourceProvider, "EN-US");
        }
    }
}
