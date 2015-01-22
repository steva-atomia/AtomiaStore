using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.Core;

namespace Atomia.Store.Fakes.Adapters
{
    public class FakeLanguageProvider : ILanguageProvider
    {
        public IList<Language> GetAvailableLanguages()
        {
            return new List<Language>
            {
                new Language
                {
                    Name = "English",
                    Code = "EN"
                },
                new Language
                {
                    Name = "Swedish",
                    Code = "SE"
                },
                new Language
                {
                    Name = "German",
                    Code = "DE"
                },
                new Language
                {
                    Name = "French",
                    Code = "FR"
                }
            };
        }

        public Language GetDefaultLanguage()
        {
            return new Language
            {
                Name = "English",
                Code = "EN"
            };
        }

        public Language GetCurrentLanguage()
        {
            return new Language
            {
                Name = "English",
                Code = "EN"
            };
        }
    }
}
