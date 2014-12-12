using Atomia.Store.Core;
using System.Linq;

namespace Atomia.Store.Services.Fakes
{
    public class FakeItemDisplayProvider : IItemDisplayProvider
    {
        public string GetName(Item item)
        {
            var domainNameAttr = item.CustomAttributes.FirstOrDefault(ca => ca.Name == "DomainName");

            if (domainNameAttr != null)
            {
                return domainNameAttr.Values.First();
            }

            return item.ArticleNumber;
        }

        public string GetDescription(Item item)
        {
            return "Description of article " + item.ArticleNumber;
        }
    }
}
