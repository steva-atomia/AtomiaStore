using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Web.Plugin.Validation.ValidationAttributes;

namespace Atomia.Store.AspNetMvc.Models
{
    public class NoridDeclarationModel
    {
        [AtomiaRequired("Common,ErrorEmptyField")]
        public string SignedName { get; set; }

        [AtomiaConfirmation("Common,ErrorTermNotChecked")]
        public bool AcceptedDeclaration { get; set; }
    }
}
