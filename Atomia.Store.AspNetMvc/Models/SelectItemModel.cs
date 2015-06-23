using Atomia.Store.Core;
using Atomia.Web.Plugin.Validation.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atomia.Store.AspNetMvc.Models
{
    public class SelectItemModel
    {
        public SelectItemModel()
        {
            this.CustomAttributes = new List<CustomAttribute>();
        }

        [AtomiaRequired("Common,ErrorEmptyField")]
        public string ArticleNumber { get; set; }

        [AtomiaRegularExpression("^$|[0-9]-MONTH|[0-9]-YEAR", "ErrorRenewalPeriod", false)]
        public string RenewalPeriod { get; set; }

        public int Quantity { get; set; }

        public IEnumerable<CustomAttribute> CustomAttributes { get; set; }

        public CartItem CartItem {
            get
            {
                
                var cartItem = new CartItem
                {
                    ArticleNumber = this.ArticleNumber,
                    RenewalPeriod = this.ParseRenewalPeriod(),
                    Quantity = this.Quantity <= 0 ? 1 : this.Quantity,
                    CustomAttributes = this.CustomAttributes
                        .Where(ca => !String.IsNullOrEmpty(ca.Name) && !string.IsNullOrEmpty(ca.Value))
                        .ToList()
                };

                return cartItem;
            }
        }

        private RenewalPeriod ParseRenewalPeriod()
        {
            RenewalPeriod renewalPeriod = null;

            if (!string.IsNullOrEmpty(this.RenewalPeriod))
            {
                var period = Int32.Parse(RenewalPeriod.Substring(0, 1));
                var unit = RenewalPeriod.Substring(2);
                renewalPeriod = new RenewalPeriod(period, unit);
            }

            return renewalPeriod;
        }
    }
}
