using Atomia.Store.Core;
using Atomia.Web.Plugin.OrderServiceReferences.AtomiaBillingPublicService;
using System;
using System.Web;


namespace Atomia.Store.PublicBillingApi.Adapters
{
    public sealed class CachedVatNumberValidator : IVatNumberValidator
    {
        private readonly IVatNumberValidator vatNumberValidator;
        private readonly IVatDataProvider vatDataProvider;

        public CachedVatNumberValidator(IVatNumberValidator vatNumberValidator, IVatDataProvider vatDataProvider)
        {
            if (vatNumberValidator == null)
            {
                throw new ArgumentNullException("vatNumberValidator");
            }

            if (vatDataProvider == null)
            {
                throw new ArgumentNullException("vatDataProvider");
            }

            this.vatNumberValidator = vatNumberValidator;
            this.vatDataProvider = vatDataProvider;
        }

        public VatValidationResult ValidateCustomerVatNumber()
        {
            var countryCode = this.vatDataProvider.CountryCode;
            var vatNumber = this.vatDataProvider.VatNumber;
            var cacheKey = "VAT" + countryCode + vatNumber;

            var validationResult = HttpContext.Current.Cache[cacheKey] as VatValidationResult;

            if (validationResult == null || validationResult.ValidationDetail == VatValidationDetail.ServiceError)
            {
                validationResult = this.vatNumberValidator.ValidateCustomerVatNumber();
                HttpContext.Current.Cache[cacheKey] = validationResult;
            }

            return validationResult;
        }
    }
}
