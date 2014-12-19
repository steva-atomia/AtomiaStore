using Atomia.Store.Core;
using System.Linq;

namespace Atomia.Store.Fakes
{
    public class FakeItemPresenter: IItemPresenter
    {
        public string GetName(IPresentableItem item)
        {
            var domainNameAttribute = item.CustomAttributes.Find(ca => ca.Name == "DomainName");

            if (domainNameAttribute != null)
            {
                return domainNameAttribute.Value;
            }

            return item.ArticleNumber;
        }

        public string GetDescription(IPresentableItem item)
        {
            return "Description of " + item.ArticleNumber;
        }
    }
}
