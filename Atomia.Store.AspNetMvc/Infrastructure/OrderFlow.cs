using System;
using System.Collections.Generic;
using System.Linq;

namespace Atomia.Store.AspNetMvc.Infrastructure
{
    public sealed class OrderFlow
    {
        private readonly string name;
        private readonly List<OrderFlowStep> orderFlowSteps = new List<OrderFlowStep>();
        private readonly Dictionary<string, OrderFlowStep> stepIndex = new Dictionary<string, OrderFlowStep>();

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

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public IEnumerable<OrderFlowStep> Steps
        {
            get
            {
                return orderFlowSteps;
            }
        }

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
