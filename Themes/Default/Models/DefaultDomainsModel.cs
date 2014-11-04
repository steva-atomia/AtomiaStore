using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atomia.OrderPage.Core.Models;

namespace Atomia.OrderPage.Themes.Default.Models
{
    public class DefaultDomainsModel : DomainsModel
    {
        public DefaultDomainsModel()
        {
            AllowedNumberOfDomains = 5;
            AllowedDomainLength = 61;
        }

        public int AllowedNumberOfDomains { get; set; }
        public int AllowedDomainLength { get; set; }
    }
}
