using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Ports
{
    public abstract class PaymentUrlProvider
    {
        public abstract string Host { get; }

        public abstract string SuccessUrl { get; }

        public abstract string FailureUrl { get; }

        public abstract string PaymentUrl { get; }

        public abstract string CancelUrl { get; }
    }
}
