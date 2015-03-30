using System;
using System.Collections.Generic;

namespace Atomia.Store.AspNetMvc.Infrastructure
{
    /// <summary>
    /// A collection of <see cref="OrderFlow">OrderFlows</see> of which one can be set as default.
    /// </summary>
    public sealed class OrderFlowCollection
    {
        private Dictionary<string, OrderFlow> orderFlows = new Dictionary<string, OrderFlow>();
        private OrderFlow defaultOrderFlow = null;

        /// <summary>
        /// Get the default <see cref="OrderFlow"/> in the collection
        /// </summary>
        public OrderFlow Default
        {
            get
            {
                return defaultOrderFlow;
            }
        }

        /// <summary>
        /// Add an <see cref="OrderFlow"/> to the collection
        /// </summary>
        /// <param name="orderFlow">The <see cref="OrderFlow"/> to add</param>
        /// <param name="isDefault">If the added order flow should be default or not.</param>
        /// <exception cref="System.InvalidOperationException">If an <see cref="OrderFlow"/> with the same name already exists in the collection</exception>
        /// <exception cref="System.InvalidOperationException">If trying to set order flow as a default in a collection that already has a default order flow.</exception>
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

        /// <summary>
        /// Add an <see cref="OrderFlow"/> to the collection
        /// </summary>
        /// <param name="orderFlow">The <see cref="OrderFlow"/> to add</param>
        /// <exception cref="System.InvalidOperationException">If an <see cref="OrderFlow"/> with the same name already exists in the collection</exception>
        public void Add(OrderFlow orderFlow)
        {
            this.Add(orderFlow, false);
        }

        /// <summary>
        /// Remove an <see cref="OrderFlow"/> from the collection
        /// </summary>
        /// <param name="orderFlowName">The name of the <see cref="OrderFlow"/> to remove</param>
        /// <returns>If the <see cref="OrderFlow"/> was removed or not.</returns>
        public bool Remove(string orderFlowName)
        {
            if (defaultOrderFlow != null && defaultOrderFlow.Name == orderFlowName)
            {
                defaultOrderFlow = null;
            }

            return orderFlows.Remove(orderFlowName);
        }

        /// <summary>
        /// Remove all <see cref="OrderFlow">OrderFlows</see> from the collection.
        /// </summary>
        public void Clear()
        {
            defaultOrderFlow = null;
            orderFlows.Clear();
        }

        /// <summary>
        /// Get an <see cref="OrderFlow"/> from the collection.
        /// </summary>
        /// <param name="orderFlowName">The name of the <see cref="OrderFlow"/> to get.</param>
        /// <returns>The <see cref="OrderFlow"/></returns>
        public OrderFlow GetOrderFlow(string orderFlowName)
        {
            return orderFlows[orderFlowName];
        }
    }
}
