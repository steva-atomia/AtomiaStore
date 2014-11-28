using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomia.Store.Core.Cart
{
    public class TransferDomainCartItem : DomainCartItem
    {
        public virtual string AuthCode { get; set; }
    }
}
