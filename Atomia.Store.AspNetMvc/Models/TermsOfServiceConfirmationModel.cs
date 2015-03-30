using Atomia.Web.Plugin.Validation.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace Atomia.Store.AspNetMvc.Models
{
    /// <summary>
    /// View model for collecting customer confirmation of terms of service.
    /// </summary>
    public class TermsOfServiceConfirmationModel
    {
        /// <summary>
        /// The unique id of the terms of service.
        /// </summary>
        [AtomiaRequired("ValidationErrors,ErrorEmptyField")]
        public string Id { get; set; }

        /// <summary>
        /// Localized name of the terms of service
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Customer provided confirmation for the terms of service.
        /// </summary>
        // FIXME: Add this back when WebFramePlugin references can be updated to include the new public order api references
        //[AtomiaConfirmation("ValidationErrors,ErrorTermNotChecked")]
        [Range(typeof(bool), "true", "true", ErrorMessage = "You must confirm the Terms of Service.")]
        public bool Confirm { get; set; }
    }
}
