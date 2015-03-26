using System;
using System.Globalization;

namespace Atomia.Store.Core
{
    /// <summary>
    /// Representation of a language
    /// </summary>
    public sealed class Language
    {
        /// <summary>
        /// Human readable name of the language.
        /// </summary>
        /// <example>"English"</example>
        public string Name { get; set; }

        /// <summary>
        /// Human readable short name of the language.
        /// </summary>
        /// <example>"EN"</example>
        public string ShortName { get; set; }

        /// <summary>
        /// IETF language tag.
        /// </summary>
        /// <example>"EN", "EN-US"</example>
        public string Tag { get; set; }

        /// <summary>
        /// Primary tag of <see cref="Tag"/>
        /// </summary>
        /// <example>"EN"</example>
        public string PrimaryTag { get; set; }

        /// <summary>
        /// Region tag, if any of <see cref="Tag"/>
        /// </summary>
        /// <example>"US"</example>
        public string RegionTag { get; set; }

        /// <summary>
        /// Get a <see cref="System.Globalization.CultureInfo"/> instance from <see cref="Tag"/>
        /// </summary>
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

        /// <summary>
        /// Static <see cref="Language"/> creator method.
        /// </summary>
        /// <param name="resourceProvider">Resource provider for human readable properties.</param>
        /// <param name="languageTag">IETF language tag.</param>
        /// <returns>The created <see cref="Language"/></returns>
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
