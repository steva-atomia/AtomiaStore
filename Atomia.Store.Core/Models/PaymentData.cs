
namespace Atomia.Store.Core
{
    /// <summary>
    /// Payment data collected from customer choice.
    /// </summary>
    public class PaymentData
    {
        /// <summary>
        /// <see cref="PaymentMethod"/> identifier
        /// </summary>
        public string Id { get; set; }

        public object PaymentForm { get; set; }

        public bool SaveCcInfo { get; set; }

        public bool AutoPay { get; set; }
    }
}
