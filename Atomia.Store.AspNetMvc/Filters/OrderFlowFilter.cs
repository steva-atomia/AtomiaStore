using Atomia.Store.AspNetMvc.Infrastructure;
using Atomia.Store.AspNetMvc.Models;
using Atomia.Store.AspNetMvc.Ports;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Collections.Generic;
using Atomia.Store.Core;

namespace Atomia.Store.AspNetMvc.Filters
{
    /// <summary>
    /// Filter for getting data for and validating order flow steps.
    /// Selects order flow to use based on "?flow={flowName}" querystring, or uses default order flow.
    /// Populates ViewBag with <see cref="Atomia.Store.AspNetMvc.Models.OrderFlowModel"/> and validates current step.
    /// <seealso cref="Atomia.Store.AspNetMvc.Infrastructure.GlobalOrderFlow"/>
    /// </summary>
    public sealed class OrderFlowFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            var routeName = (string)filterContext.RouteData.DataTokens["Name"];
            var orderFlowName = request.QueryString["flow"] as string;
            var useDefaultOrderFlow = String.IsNullOrEmpty(orderFlowName);

            var orderFlow = useDefaultOrderFlow
                ? GlobalOrderFlows.OrderFlows.Default
                : GlobalOrderFlows.OrderFlows.GetOrderFlow(orderFlowName);

            if (orderFlow == null && !String.IsNullOrEmpty(orderFlowName))
            {
                throw new InvalidOperationException(String.Format("No order flow named {0} and no default order flow configured.", orderFlowName));
            }
            else if (orderFlow == null)
            {   
                throw new InvalidOperationException("Missing order flow configuration");
            }

            var currentStep = orderFlow.GetOrderFlowStep(routeName);

            PopulateViewBag(filterContext, orderFlow, currentStep, useDefaultOrderFlow);

            ValidateOrderFlowStep(filterContext, orderFlow, currentStep);
        }

        /// <summary>
        /// Adds <see cref="Atomia.Store.AspNetMvc.Models.OrderFlowModel"/> to ViewBag.
        /// </summary>
        private void PopulateViewBag(ActionExecutingContext filterContext, OrderFlow orderFlow, OrderFlowStep currentStep, bool isDefaultOrderFlow)
        {
            var resourceProvider = DependencyResolver.Current.GetService<IResourceProvider>();

            var orderFlowModel = new OrderFlowModel
            {
                IsDefault = isDefaultOrderFlow,
                Name = orderFlow.Name,
                Steps = orderFlow.Steps.Select(s => new OrderFlowStepModel
                {
                    Name = s.Name,
                    Previous = s.Previous,
                    Next = s.Next,
                    StepNumber = s.StepNumber,
                    Title = resourceProvider.GetResource("StepTitle" + s.Name),
                    Description = resourceProvider.GetResource("StepDescription" + s.Name)
                }),
                CurrentStep = new OrderFlowStepModel
                {
                    Name = currentStep.Name,
                    Previous = currentStep.Previous,
                    Next = currentStep.Next,
                    StepNumber = currentStep.StepNumber,
                    Title = resourceProvider.GetResource("StepTitle" + currentStep.Name),
                    Description = resourceProvider.GetResource("StepDescription" + currentStep.Name)
                }
            };

            filterContext.Controller.ViewBag.OrderFlow = orderFlowModel;
        }

        /// <summary>
        /// Validates <see cref="Atomia.Store.AspNetMvc.Models.OrderFlowModel"/> for current step.
        /// </summary>
        private void ValidateOrderFlowStep(ActionExecutingContext filterContext, OrderFlow orderFlow, OrderFlowStep currentOrderFlowStep)
        {
            var orderFlowValidator = DependencyResolver.Current.GetService<IOrderFlowValidator>();

            var validationMessages = orderFlowValidator.ValidateOrderFlowStep(orderFlow, currentOrderFlowStep);

            if (validationMessages.Count() > 0)
            {
                // TODO: Make messages available to next controller via flash message provider
                filterContext.Result = new RedirectToRouteResult(currentOrderFlowStep.Previous, new RouteValueDictionary());
            }
        }
    }
}
