using Atomia.Store.AspNetMvc.Services;
using System.Linq;

namespace Atomia.Store.Services.Fakes
{
    public class FakeItemPresenter: IItemPresenter
    {
        public string GetName(IPresentableItem item)
        {
            var domainNameAttribute = item.CustomAttributes.Find(ca => ca.Name == "DomainName");

            if (domainNameAttribute != null)
            {
                return domainNameAttribute.Values[0];
            }

            return item.ArticleNumber;
        }

        public string GetDescription(IPresentableItem item)
        {
            return "Description of " + item.ArticleNumber;
        }
    }
}
