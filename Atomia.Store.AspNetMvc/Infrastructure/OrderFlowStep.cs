
namespace Atomia.Store.AspNetMvc.Infrastructure
{
    public sealed class OrderFlowStep
    {
        public string Name { get; set; }

        public string Previous { get; set; }

        public string Next { get; set; }

        public int StepNumber { get; set; }
    }
}
