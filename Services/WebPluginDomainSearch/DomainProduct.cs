using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.Core;

namespace Atomia.Store.Services.WebPluginDomainSearch
{
    public class DomainProduct : Product
    {
        public override string Name
        {
            get
            {
                return DomainName;
            }
            set 
            { 
                throw new InvalidOperationException("Cannot set Name for a domain product"); 
            }
        }

        public string DomainName { get; set; }

        public string Status { get; set; }
    }
}
