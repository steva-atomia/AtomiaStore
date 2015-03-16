
namespace Atomia.Store.AspNetMvc.Infrastructure
{
    public static class GlobalOrderFlows
    {
        static GlobalOrderFlows()
        {
            OrderFlows = new OrderFlowCollection();
        }

        public static OrderFlowCollection OrderFlows { get; private set; }
    }
}
