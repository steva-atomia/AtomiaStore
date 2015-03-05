
namespace Atomia.Store.Core
{
    public abstract class PaymentUrlProvider
    {
        public abstract string SuccessUrl { get; }

        public abstract string FailureUrl { get; }

        public abstract string CancelUrl { get; }

        public abstract string DefaultPaymentUrl { get; }

        public abstract string QualifiedUrl(string path);
    }
}
