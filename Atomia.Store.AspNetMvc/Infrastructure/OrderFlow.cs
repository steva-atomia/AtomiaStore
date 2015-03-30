using System;
using System.Collections.Generic;
using System.Linq;

namespace Atomia.Store.AspNetMvc.Infrastructure
{
    /// <summary>
    /// A named list of rotues that define an order flow.
    /// </summary>
    public sealed class OrderFlow
    {
        private readonly string name;
        private readonly List<OrderFlowStep> orderFlowSteps = new List<OrderFlowStep>();
        private readonly Dictionary<string, OrderFlowStep> stepIndex = new Dictionary<string, OrderFlowStep>();

        /// <summary>
        /// Construct the <see cref="OrderFlow"/>
        /// </summary>
        /// <param name="name">The name of the order flow</param>
        /// <param name="routeNames">The names of the routes that should make up the order flow.</param>
        public OrderFlow(string name, string[] routeNames)
        {
            this.name = name;

            for (var i = 0; i < routeNames.Length; i += 1)
            {
                var routeName = routeNames[i];

                if (orderFlowSteps.Any(s => s.Name == name))
                {
                    throw new ArgumentException("routeNames", "Duplicate route names.");
                }

                var orderFlowStep = new OrderFlowStep
                {
                    Previous = "",
                    Name = routeName,
                    Next = "",
                    StepNumber = i + 1
                };

                if (i > 0) 
                {
                    orderFlowStep.Previous = routeNames[i - 1];
                }

                if (i < routeNames.Length - 1)
                {
                    orderFlowStep.Next = routeNames[i + 1];
                }

                this.orderFlowSteps.Add(orderFlowStep);
                this.stepIndex[routeName] = orderFlowStep;
            }
        }

        /// <summary>
        /// The order flow name
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
        }

        /// <summary>
        /// The contained <see cref="OrderFlowStep">OrderFlowSteps</see>.
        /// </summary>
        public IEnumerable<OrderFlowStep> Steps
        {
            get
            {
                return orderFlowSteps;
            }
        }

        /// <summary>
        /// Add an alternative route name for a step to map several routes to a single order flow step.
        /// </summary>
        /// <param name="alias">The additional route name for a step</param>
        /// <param name="routeName">The route name for the step as defined in the constructor</param>
        /// <remarks>
        /// An order flow step might need to reachable by several different routes, e.g. if the domain search route is the first order step 
        /// you might want to have both "/" and "/Domains" mapping to the same order flow step.
        /// </remarks>
        public void AddRouteNameAlias(string alias, string routeName)
        {
            if (this.stepIndex.ContainsKey(alias))
            {
                throw new ArgumentException("alias", String.Format("Order flow already contains step named or aliased to {0}", alias));
            }

            if (!this.stepIndex.ContainsKey(routeName))
            {
                throw new ArgumentException("routeName", String.Format("Order flow does not contain any step named {0}", routeName));
            }

            this.stepIndex[alias] = this.stepIndex[routeName];
        }

        /// <summary>
        /// Get the <see cref="OrderFlowStep"/> that is mapped to a route.
        /// </summary>
        /// <param name="routeName">The name of the route to get the step for.</param>
        /// <returns>The <see cref="OrderFlowStep"/></returns>
        /// <exception cref="System.ArgumentException">If no step with the specified route exists in the order flow.</exception>
        public OrderFlowStep GetOrderFlowStep(string routeName)
        {
            if (!this.stepIndex.ContainsKey(routeName))
            {
                throw new ArgumentException("routeName", String.Format("No order flow step named {0}", routeName));
            }

            return this.stepIndex[routeName];
        }
    }
}
