using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atomia.OrderPage.Core.Models;
using Atomia.OrderPage.Core.Services;

namespace Atomia.OrderPage.Services.WebPluginDomainSearch
{
    public class DomainSearchService : IDomainSearchService
    {
        public DomainSearchResult FindDomains(DomainSearchQuery searchQuery) 
        {
            return new DomainSearchResult
            {
                DomainNames = new List<string> { "example.com", "example.net", "example.org" }
            };
        }
    }
}
