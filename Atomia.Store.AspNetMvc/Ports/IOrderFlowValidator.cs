using System.Collections.Generic;
using Atomia.Store.AspNetMvc.Infrastructure;

namespace Atomia.Store.AspNetMvc.Ports
{
    public interface IOrderFlowValidator
    {
        /// <summary>
        /// Validate the current order flow step.
        /// </summary>
        /// <param name="orderFlow">The complete order flow</param>
        /// <param name="currentOrderFlowStep">The current order flow step</param>
        /// <returns>An IEnumerable of error messages. If no error messages are returned, the step is assumed to be valid.</returns>
        IEnumerable<string> ValidateOrderFlowStep(OrderFlow orderFlow, OrderFlowStep currentOrderFlowStep);
    }
}
