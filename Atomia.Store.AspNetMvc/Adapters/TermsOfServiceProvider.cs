using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.Core;

namespace Atomia.Store.AspNetMvc.Adapters
{
    public class TermsOfServiceProvider : ITermsOfServiceProvider
    {
        private readonly ICartProvider cartProvider;
        private readonly IProductProvider productProvider;
        private readonly IResourceProvider resourceProvider;
        private readonly IResellerProvider resellerProvider;

        public TermsOfServiceProvider(ICartProvider cartProvider, IProductProvider productProvider, IResourceProvider resourceProvider, IResellerProvider resellerProvider)
        {
            if (cartProvider == null)
            {
                throw new ArgumentNullException("cartProvider");
            }

            if (productProvider == null)
            {
                throw new ArgumentNullException("productProvider");
            }

            if (resourceProvider == null)
            {
                throw new ArgumentNullException("resourceProvider");
            }

            if (resellerProvider == null)
            {
                throw new ArgumentNullException("resellerProvider");
            }

            this.cartProvider = cartProvider;
            this.productProvider = productProvider;
            this.resourceProvider = resourceProvider;
            this.resellerProvider = resellerProvider;
        }

        public IEnumerable<TermsOfService> GetTermsOfService()
        {
            var termsOfService = new HashSet<TermsOfService>();
            var cart = cartProvider.GetCart();
            var articleNumbers = new HashSet<string>(cart.CartItems.Select(c => c.ArticleNumber));
            var isReseller = resellerProvider.GetReseller().IsSubReseller;
            var tosResources = new HashSet<string>();

            foreach (var articleNumber in articleNumbers)
            {
                var product = productProvider.GetProduct(articleNumber);

                if (product != null)
                {
                    string tosNoSplit = null;

                    if (isReseller && product.CustomAttributes.Any(ca => ca.Name == "resellertos"))
                    {
                        tosNoSplit = product.CustomAttributes.First(ca => ca.Name == "resellertos").Value;
                    }
                    else if (product.CustomAttributes.Any(ca => ca.Name == "tos"))
                    {
                        tosNoSplit = product.CustomAttributes.First(ca => ca.Name == "tos").Value;
                    }

                    if (!string.IsNullOrEmpty(tosNoSplit))
                    {
                        tosResources.UnionWith(tosNoSplit.Split('|'));
                    }
                }
            }

            foreach (var resource in tosResources)
            {
                termsOfService.Add(new TermsOfService
                {
                    Id = resource,
                    Name = resourceProvider.GetResource(resource + "_Name"),
                    Terms = resourceProvider.GetResource(resource + "_Terms")
                });
            }

            return termsOfService;
        }
    }
}
