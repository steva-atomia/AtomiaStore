using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomia.Store.AspNetMvc.Models
{
    /// <summary>
    /// View representation of an <see cref="Atomia.Store.AspNetMvc.Infrastructure.OrderFlowStep"/>
    /// </summary>
    public class OrderFlowStepModel
    {
        /// <summary>
        /// The 1-based index of this step in the order flow.
        /// </summary>
        public int StepNumber { get; set; }

        /// <summary>
        /// The route name of this step
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The localized title of this step, e.g. "Checkout"
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The localized description of this step.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The route name of the previous step, or empty string if current step is the first one.
        /// </summary>
        public string Previous { get; set; }

        /// <summary>
        /// The route name of the next step, or empty string if current step is the last one.
        /// </summary>
        public string Next { get; set; }
    }
}
