using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Atomia.Store.Core;

namespace Atomia.Store.AspNetMvc.Ports
{
    public abstract class PaymentForm
    {
        public abstract MvcHtmlString Render();
    }

    public abstract class PaymentMethodGuiPlugin : PaymentMethodData
    {
        public abstract string Name { get; }

        public virtual string Description
        {
            get
            {
                return "";
            }
        }

        public virtual PaymentForm PaymentForm
        {
            get
            {
                return null;
            }
        }
    }
}
