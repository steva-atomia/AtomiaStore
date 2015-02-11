using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Web.Plugin.Validation.ValidationAttributes;
using Atomia.Store.Core;

namespace Atomia.Store.AspNetMvc.Models
{
    public class TermsOfServiceModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Terms { get; set; }

//        [AtomiaRequiredConfirmation("ValidationErrors, TermsConfirmation")]
        public bool Confirmed { get; set; }
    }
}
