using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomia.Store.Core
{
    public abstract class PaymentUrlProvider
    {
        public abstract string SuccessUrl { get; }

        public abstract string FailureUrl { get; }

        public abstract string CancelUrl { get; }

        public abstract string PaymentRedirectUrl { get; }

        public abstract string QualifiedUrl(string path);
    }
}
