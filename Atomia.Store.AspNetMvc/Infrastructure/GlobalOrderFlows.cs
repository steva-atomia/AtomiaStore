
namespace Atomia.Store.AspNetMvc.Infrastructure
{
    /// <summary>
    /// Singleton for order flow configuration
    /// </summary>
    public static class GlobalOrderFlows
    {
        static GlobalOrderFlows()
        {
            OrderFlows = new OrderFlowCollection();
        }

        public static OrderFlowCollection OrderFlows { get; private set; }
    }
}
