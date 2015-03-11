using Atomia.Web.Plugin.Validation.ValidationAttributes;

namespace Atomia.Store.AspNetMvc.Models
{
    public class TermsOfServiceConfirmationModel
    {
        [AtomiaRequired("ValidationErrors,ErrorEmptyField")]
        public string Id { get; set; }

        public string Name { get; set; }

        [AtomiaConfirmation("ValidationErrors,ErrorTermNotChecked")]
        public bool Confirm { get; set; }
    }
}
