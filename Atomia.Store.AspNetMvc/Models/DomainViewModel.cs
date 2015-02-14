using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atomia.Store.Core;
using Atomia.Web.Plugin.Validation.ValidationAttributes;

namespace Atomia.Store.AspNetMvc.Models
{
    public class DomainViewModel
    {

        [AtomiaRequired("ValidationErrors,ErrorEmptyField")]
        [AtomiaRegularExpression("DomainSearch", "ValidationErrors,Invalid domain name", true)]
        public string SearchQuery { get; set; }
    }
}
