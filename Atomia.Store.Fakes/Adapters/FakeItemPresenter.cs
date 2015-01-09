using Atomia.Store.Core;
using System.Linq;

namespace Atomia.Store.Fakes.Adapters
{
    public class FakeItemPresenter: IItemPresenter
    {
        public string GetName(IPresentableItem item)
        {
            if (item.ArticleNumber.StartsWith("DMN-"))
            {
                var domainNameAttr = item.CustomAttributes.FirstOrDefault(ca => ca.Name == "DomainName");
                
                if (domainNameAttr != default(CustomAttribute)) {
                    return domainNameAttr.Value;
                }
            }

            if (item.ArticleNumber == "DNS-PK")
            {
                return "DNS Package";
            }

            if (item.ArticleNumber == "HST-GLD")
            {
                return "Gold Package";
            }

            if (item.ArticleNumber == "HST-PLT")
            {
                return "Platinum Package";
            }

            return item.ArticleNumber;
        }

        public string GetDescription(IPresentableItem item)
        {
            return "Description of " + item.ArticleNumber;
        }
    }
}
