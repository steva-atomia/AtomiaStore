using Atomia.Store.Core;

namespace Atomia.Store.Fakes.Adapters
{
    public class FakeLanguagePreferenceProvider : ILanguagePreferenceProvider
    {
        private readonly IResourceProvider resourceProvider;

        public FakeLanguagePreferenceProvider(IResourceProvider resourceProvider)
        {
            this.resourceProvider = resourceProvider;
        }

        public Language GetCurrentLanguage()
        {
            return Language.CreateLanguage(resourceProvider, "EN");
        }

        public void SetPreferredLanguage(Language language)
        {
            
        }
    }
}
