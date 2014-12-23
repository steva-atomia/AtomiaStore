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
                return "." + item.ArticleNumber.Substring(4).ToLowerInvariant();
            }

            return item.ArticleNumber;
        }

        public string GetDescription(IPresentableItem item)
        {
            return "Description of " + item.ArticleNumber;
        }
    }
}
