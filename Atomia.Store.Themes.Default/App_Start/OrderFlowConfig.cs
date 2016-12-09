using Atomia.Store.AspNetMvc.Infrastructure;

namespace Atomia.Store.Themes.Default
{
    public static class OrderFlowConfig
    {
        public static void RegisterOrderFlows(OrderFlowCollection orderFlows)
        {
            var defaultOrderFlow = new OrderFlow("DefaultFlow", new[] { "Domains", "HostingPackage", "Account", "Checkout" });
            var existingCustomerOrderFlow = new OrderFlow("ExistingCustomerFlow", new[] { "Domains", "HostingPackage", "ExistingCustomer", "Checkout" });

            // Alias Default to Domains, to match Default route setup. 
            defaultOrderFlow.AddRouteNameAlias("OrderFlowStart", "Domains");

            orderFlows.Add(defaultOrderFlow, true);
            orderFlows.Add(existingCustomerOrderFlow, false);
        }
    }
}
