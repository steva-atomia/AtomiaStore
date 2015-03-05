using System;
using System.Globalization;

namespace Atomia.Store.Core
{
    public sealed class Language
    {
        public string Name { get; set; }

        public string ShortName { get; set; }

        public string Tag { get; set; }

        public string PrimaryTag { get; set; }

        public string RegionTag { get; set; }

        public CultureInfo AsCultureInfo()
        {
            if (String.IsNullOrEmpty(RegionTag))
            {
                return CultureInfo.CreateSpecificCulture(PrimaryTag.ToLower());
            }
            else
            {
                return CultureInfo.GetCultureInfo(Tag.ToLower());
            }
        }

        public static Language CreateLanguage(IResourceProvider resourceProvider, string languageTag)
        {
            var tags = languageTag.Split('-');
            var primaryTag = languageTag.Split('-')[0];
            var regionTag = "";

            if (tags.Length == 2)
            {
                regionTag = tags[1];
            }
            
            var name = resourceProvider.GetResource(languageTag.ToUpper().Replace('-', '_') + "_name");
            var shortName = resourceProvider.GetResource(languageTag.ToUpper().Replace('-', '_') + "_shortname");

            if (String.IsNullOrEmpty(name))
            {
                name = resourceProvider.GetResource(primaryTag.ToUpper().Replace('-', '_') + "_name");
            }

            if (String.IsNullOrEmpty(shortName))
            {
                shortName = resourceProvider.GetResource(primaryTag.ToUpper().Replace('-', '_') + "_shortname");
            }
            
            return new Language
            {
                Tag = languageTag.ToUpper(),
                PrimaryTag = primaryTag.ToUpper(),
                RegionTag = regionTag.ToUpper(),
                Name = name,
                ShortName = shortName
            };
        }
    }
}
