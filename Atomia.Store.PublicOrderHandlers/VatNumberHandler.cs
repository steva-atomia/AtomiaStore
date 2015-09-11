using Atomia.Store.Core;
using Atomia.Store.PublicBillingApi.Handlers;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System;

namespace Atomia.Store.PublicOrderHandlers
{
    public sealed class VatValidationHandler : OrderDataHandler
    {
        private readonly IVatNumberValidator vatNumberValidator;

        public VatValidationHandler(IVatNumberValidator vatNumberValidator)
        {
            if (vatNumberValidator == null)
            {
                throw new ArgumentNullException("vatNumberValidator");
            }

            this.vatNumberValidator = vatNumberValidator;
        }

        /// <summary>
        /// Remove invalid VAT number from order.
        /// </summary>
        public override PublicOrder AmendOrder(PublicOrder order, PublicOrderContext orderContext)
        {
            if (!string.IsNullOrEmpty(order.LegalNumber))
            {
                var validationResult = vatNumberValidator.ValidateCustomerVatNumber();

                if (!validationResult.Valid)
                {
                    order.LegalNumber = String.Empty;
                }
            }

            return order;
        }
    }
}
