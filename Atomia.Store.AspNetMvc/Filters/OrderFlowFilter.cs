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
    /// Selects order flow to use based on "?flow={flowName}" querystring, application host name, or uses default order flow.
    /// Populates ViewBag with <see cref="Atomia.Store.AspNetMvc.Models.OrderFlowModel"/> and validates current step.
    /// <seealso cref="Atomia.Store.AspNetMvc.Infrastructure.GlobalOrderFlow"/>
    /// </summary>
    public sealed class OrderFlowFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            var routeName = (string)filterContext.RouteData.DataTokens["Name"];
            
            var isQueryStringBased = true;
            var orderFlow = GetOrderFlowFromQueryString(request);
            
            if (orderFlow == null)
            {
                isQueryStringBased = false;
                orderFlow = GetOrderFlowFromHostname(request);
            }
            
            if (orderFlow == null)
            {
                isQueryStringBased = false;
                orderFlow = GlobalOrderFlows.OrderFlows.Default;
            }
            
            if (orderFlow == null)
            {   
                throw new InvalidOperationException("Could not find a matching order flow for 'flow' query string or host name, and no default order flow configured.");
            }

            var currentStep = orderFlow.GetOrderFlowStep(routeName);

            PopulateViewBag(filterContext, orderFlow, currentStep, isQueryStringBased);

            ValidateOrderFlowStep(filterContext, orderFlow, currentStep);
        }

        /// <summary>
        /// Get <see cref="OrderFlow"/> with name from query string.
        /// </summary>
        private OrderFlow GetOrderFlowFromQueryString(System.Web.HttpRequestBase request)
        {
            var orderFlowName = request.QueryString["flow"] as string;
            OrderFlow orderFlow = null;

            if (!String.IsNullOrEmpty(orderFlowName))
            {
                orderFlow = GlobalOrderFlows.OrderFlows.GetOrderFlow(orderFlowName);
            }

            return orderFlow;
        }

        /// <summary>
        /// Get <see cref="OrderFlow"/> with same name as application host name.
        /// </summary>
        private OrderFlow GetOrderFlowFromHostname(System.Web.HttpRequestBase request)
        {
            var hostname = request.Url.Authority;

            var orderFlow = GlobalOrderFlows.OrderFlows.GetOrderFlow(hostname);

            return orderFlow;
        }

        /// <summary>
        /// Adds <see cref="Atomia.Store.AspNetMvc.Models.OrderFlowModel"/> to ViewBag.
        /// </summary>
        private void PopulateViewBag(ActionExecutingContext filterContext, OrderFlow orderFlow, OrderFlowStep currentStep, bool isQueryStringBased)
        {
            var resourceProvider = DependencyResolver.Current.GetService<IResourceProvider>();

            var orderFlowModel = new OrderFlowModel
            {
                IsQueryStringBased = isQueryStringBased,
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
