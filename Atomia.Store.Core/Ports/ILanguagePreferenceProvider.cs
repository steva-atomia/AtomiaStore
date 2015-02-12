using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.Core;

namespace Atomia.Store.Core
{
    public interface ILanguagePreferenceProvider
    {
        void SetPreference(Language language);

        Language GetPreferredLanguage();
    }
}
