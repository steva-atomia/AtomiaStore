using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Atomia.Web.Base.Helpers.General;

namespace Atomia.Store.PublicOrderHandlers.Configuration
{
    public class PublicOrderHandlersDataManager : ConfigurationSection
    {
        public const string SectionName = "publicOrderHandlersData";

        [ConfigurationProperty("mailOnOrderSettings")]
        [ConfigurationCollection(typeof(MailOnOrderSettingsCollection), AddItemName = "add")]
        public MailOnOrderSettingsCollection mailOnOrderSettings { get { return (MailOnOrderSettingsCollection)base["mailOnOrderSettings"]; } }
    }

    public class MailOnOrderSettingsCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new MailOnOrderSettingsElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MailOnOrderSettingsElement)element).ProductId;
        }
    }

    public class MailOnOrderSettingsElement : ConfigurationElement
    {
        [ConfigurationProperty("productId", IsRequired = true)]
        public string ProductId
        {
            get { return (string)this["productId"]; }
            set { this["productId"] = value; }
        }

        [ConfigurationProperty("cccEmail", IsRequired = true)]
        public string CccEmail
        {
            get { return (string)this["cccEmail"]; }
            set { this["cccEmail"] = value; }
        }

        [ConfigurationProperty("email", IsRequired = true)]
        public string Email
        {
            get { return (string)this["email"]; }
            set { this["email"] = value; }
        }

    }

    public class LocalConfigurationHelper
    {
        public static List<MailOnOrderSettingsElement> GetMailOnOrderSettings()
        {
            var result = new List<MailOnOrderSettingsElement>(); ;
            try
            {
                string currentDir = Directory.GetCurrentDirectory();
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;

                System.Configuration.Configuration config = PluginLoaderHelper.GetConfigFile(
                    PluginConfigurationHelper.GetAssemblyFilePath(codeBase),
                    PluginConfigurationHelper.GetConfigFilePath(codeBase));
            
                var customConfig = config.GetSection(PublicOrderHandlersDataManager.SectionName) as PublicOrderHandlersDataManager;
                if (customConfig != null) result.AddRange(customConfig.mailOnOrderSettings.Cast<MailOnOrderSettingsElement>());

                Directory.SetCurrentDirectory(currentDir);
            }
            catch (Exception)
            {
                
            }

            return result;
        }
    }
}
