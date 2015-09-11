
namespace Atomia.Store.Core
{
    public enum VatValidationDetail 
    {
        Valid,
        NoVatNumber,
        Invalid,
        ServiceError
    }

    public sealed class VatValidationResult
    {
        public string VatNumber { get; set; }

        public bool Valid { get; set; }

        public VatValidationDetail ValidationDetail { get; set; }

        public string ValidationMessage { get; set; }
    }
}
