using System.Collections.Generic;

namespace Atomia.Store.Core
{
    /// <summary>
    /// Get default and available payment methods. <see cref="PaymentMethod"/>
    /// </summary>
    public interface IPaymentMethodsProvider
    {
        /// <summary>
        /// Get all available payment methods. <see cref="PaymentMethod"/>
        /// </summary>
        IEnumerable<PaymentMethod> GetPaymentMethods();

        /// <summary>
        /// Get the default <see cref="PaymentMethod"/>
        /// </summary>
        PaymentMethod GetDefaultPaymentMethod();
    }
}
