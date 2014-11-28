using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomia.Store.Core.Cart
{
    public class DomainCartItem : CartItem
    {
        public override int Quantity
        {
            get
            {
                return 1;
            }
            set
            {

            }
        }

        public virtual string DomainName { get; set; }
    }
}
