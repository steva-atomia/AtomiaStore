using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atomia.Store.Payment.PayPal
{
    public class PayPalViewModel
    {
        public string PayAmount { get; set; }

        public string ReferenceNumber { get; set; }

        public string PayerId { get; set; }

        public string CurrencyFormat { get; set; }

        public string NumberFormat { get; set; }

        public string Currency { get; set; }

        public string CancelUrl { get; set; }

        public string Action { get; set; }
    }
}
