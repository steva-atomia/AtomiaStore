using System;
using System.Collections.Generic;

namespace Atomia.Store.AspNetMvc.Infrastructure
{
    public sealed class OrderFlowCollection
    {
        private Dictionary<string, OrderFlow> orderFlows = new Dictionary<string, OrderFlow>();
        private OrderFlow defaultOrderFlow = null;

        public OrderFlow Default
        {
            get
            {
                return defaultOrderFlow;
            }
        }

        public void Add(OrderFlow orderFlow, bool isDefault)
        {
            if (orderFlows.ContainsKey(orderFlow.Name))
            {
                throw new InvalidOperationException(String.Format("Order flows already contains an order flow name {0}", orderFlow.Name));
            }

            orderFlows.Add(orderFlow.Name, orderFlow);

            if (defaultOrderFlow == null && isDefault)
            {
                defaultOrderFlow = orderFlow;
            }
            else if (isDefault)
            {
                throw new InvalidOperationException("Cannot set more than one default order flow.");
            }
        }

        public void Add(OrderFlow orderFlow)
        {
            this.Add(orderFlow, false);
        }

        public bool Remove(string orderFlowName)
        {
            if (defaultOrderFlow != null && defaultOrderFlow.Name == orderFlowName)
            {
                defaultOrderFlow = null;
            }

            return orderFlows.Remove(orderFlowName);
        }

        public void Clear()
        {
            defaultOrderFlow = null;
            orderFlows.Clear();
        }

        public OrderFlow GetOrderFlow(string orderFlowName)
        {
            return orderFlows[orderFlowName];
        }
    }
}
