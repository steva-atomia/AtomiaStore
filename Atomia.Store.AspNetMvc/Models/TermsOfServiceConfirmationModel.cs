using Atomia.Web.Plugin.Validation.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace Atomia.Store.AspNetMvc.Models
{
    public class TermsOfServiceConfirmationModel
    {
        [AtomiaRequired("ValidationErrors,ErrorEmptyField")]
        public string Id { get; set; }

        public string Name { get; set; }

        // FIXME: Add this back when WebFramePlugin references can be updated to include the new public order api references
        //[AtomiaConfirmation("ValidationErrors,ErrorTermNotChecked")]
        [Range(typeof(bool), "true", "true", ErrorMessage = "You must confirm the Terms of Service.")]
        public bool Confirm { get; set; }
    }
}
