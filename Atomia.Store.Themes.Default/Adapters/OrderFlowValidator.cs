using Atomia.Store.AspNetMvc.Ports;
using Atomia.Store.Core;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Atomia.Store.Themes.Default.Adapters
{
    public class OrderFlowValidator : IOrderFlowValidator
    {
        private readonly ICartProvider cartProvider = DependencyResolver.Current.GetService<ICartProvider>();
        private readonly IContactDataProvider contactDataProvider = DependencyResolver.Current.GetService<IContactDataProvider>();
        private readonly IResourceProvider resourceProvider = DependencyResolver.Current.GetService<IResourceProvider>();
        private List<string> validationErrors = new List<string>();

        public IEnumerable<string> ValidateOrderFlowStep(AspNetMvc.Infrastructure.OrderFlow orderFlow, AspNetMvc.Infrastructure.OrderFlowStep currentOrderFlowStep)
        {
            if (currentOrderFlowStep.Name == "Account")
            {
                ValidateCart();
            }
            else if (currentOrderFlowStep.Name == "Checkout")
            {
                ValidateCart();
                ValidateContactData();
            }

            return this.validationErrors;
        }

        private void ValidateCart()
        {
            var cart = cartProvider.GetCart();

            if (cart.IsEmpty())
            {
                this.validationErrors.Add(resourceProvider.GetResource("OrderFlowValidation_CartIsEmpty"));
            }
        }

        private void ValidateContactData()
        {
            var contactDataCollection = contactDataProvider.GetContactData();
            var contactData = contactDataCollection != null 
                ? contactDataCollection.GetContactData()
                : new List<ContactData>();

            if (contactData.Count() == 0)
            {
                this.validationErrors.Add(resourceProvider.GetResource("OrderFlowValidation_ContactDataIsEmpty"));
            }
        }
    }
}
