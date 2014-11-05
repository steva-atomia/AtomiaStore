using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atomia.OrderPage.UI.ViewModels;

namespace Atomia.OrderPage.Themes.Default.ViewModels
{
    public class DefaultDomainsViewModel : DomainsViewModel
    {
        public DefaultDomainsViewModel()
        {
            AllowedNumberOfDomains = 5;
            AllowedDomainLength = 61;
        }

        public int AllowedNumberOfDomains { get; set; }
        public int AllowedDomainLength { get; set; }
    }
}
