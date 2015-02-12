using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public Language GetPreferredLanguage()
        {
            return Language.CreateLanguage(resourceProvider, "EN");
        }

        public void SetPreference(Language language)
        {
            
        }
    }
}
