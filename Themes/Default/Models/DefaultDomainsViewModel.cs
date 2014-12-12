using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Atomia.Store.AspNetMvc.Models;

namespace Atomia.Store.Themes.Default.Models
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
