
namespace Atomia.Store.AspNetMvc.Infrastructure
{
    /// <summary>
    /// A single step in an <see cref="OrderFlow"/>.
    /// </summary>
    public sealed class OrderFlowStep
    {
        /// <summary>
        /// The original un-aliased route name of the step.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The original un-aliased route name of the previous step or empty string if the current step is the first one.
        /// </summary>
        public string Previous { get; set; }

        /// <summary>
        /// The original un-aliased route name of the next step or empty string if the current step is the last one.
        /// </summary>
        public string Next { get; set; }

        /// <summary>
        /// The 1-based index of the step in the order flow.
        /// </summary>
        public int StepNumber { get; set; }
    }
}
