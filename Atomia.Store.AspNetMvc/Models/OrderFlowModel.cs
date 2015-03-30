using System.Collections.Generic;

namespace Atomia.Store.AspNetMvc.Models
{
    /// <summary>
    /// View representation of <see cref="Atomia.Store.AspNetMvc.Infrastructure.OrderFlow"/>
    /// </summary>
    public class OrderFlowModel
    {
        /// <summary>
        /// The ordered steps of the order flow
        /// </summary>
        public IEnumerable<OrderFlowStepModel> Steps { get; set; }

        /// <summary>
        /// The name of the order flow
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// If the order flow is the default one or not.
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// The current order flow step
        /// </summary>
        public OrderFlowStepModel CurrentStep { get; set; }
    }
}
