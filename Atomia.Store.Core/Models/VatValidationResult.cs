
namespace Atomia.Store.Core
{
    public sealed class VatValidationResult
    {
        public string VatNumber { get; set; }

        public bool Valid { get; set; }

        public string ValidationMessage { get; set; }
    }
}
