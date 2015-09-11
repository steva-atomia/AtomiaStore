using Atomia.Store.Core;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System;

namespace Atomia.Store.PublicBillingApi.Adapters
{
    public sealed class VatNumberValidator : PublicBillingApiClient, IVatNumberValidator
    {
        private readonly IVatDataProvider vatDataProvider;
        private readonly IResourceProvider resourceProvider;
        
        public VatNumberValidator(IVatDataProvider vatDataProvider, IResourceProvider resourceProvider, PublicBillingApiProxy billingApi)
            : base(billingApi)
        {
            if (vatDataProvider == null)
            {
                throw new ArgumentNullException("vatDataProvider");
            }

            if (resourceProvider == null)
            {
                throw new ArgumentNullException("resourceProvider");
            }

            this.vatDataProvider = vatDataProvider;
            this.resourceProvider = resourceProvider;
        }

        public VatValidationResult ValidateCustomerVatNumber()
        {
            var vatNumber = this.vatDataProvider.VatNumber;
            var countryCode = this.vatDataProvider.CountryCode;
            var result = new VatValidationResult { VatNumber = vatNumber };

            if (!String.IsNullOrEmpty(vatNumber))
            {
                // Billing API method does not handle country prefix numbers, remove if exists
                if (vatNumber.ToLowerInvariant().StartsWith(countryCode.ToLowerInvariant()))
                {
                    vatNumber = vatNumber.Substring(countryCode.Length);
                }

                var validationResult = BillingApi.ValidateVatNumber(countryCode, vatNumber);
                
                switch (validationResult)
                {
                    case VatNumberValidationResultType.Valid:
                        result.Valid = true;
                        result.ValidationDetail = VatValidationDetail.Valid;
                        result.ValidationMessage = String.Empty;
                        break;
                    case VatNumberValidationResultType.Invalid:
                        result.Valid = false;
                        result.ValidationDetail = VatValidationDetail.Invalid;
                        result.ValidationMessage = resourceProvider.GetResource("InvalidVatNumber");
                        break;
                    case VatNumberValidationResultType.ValidationError:
                    default:
                        result.Valid = false;
                        result.ValidationDetail = VatValidationDetail.ServiceError;
                        result.ValidationMessage = resourceProvider.GetResource("CouldNotValidateVatNumber");
                        break;
                }
            }
            else
            {
                result.Valid = false;
                result.ValidationDetail = VatValidationDetail.NoVatNumber;
                result.ValidationMessage = resourceProvider.GetResource("NoVatNumber");
            }
            
            return result;
        }
    }
}
