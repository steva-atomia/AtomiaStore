using Atomia.Store.Core;
using System;
using System.Web.Mvc;

namespace Atomia.Store.AspNetMvc.Models
{
    public class TaxModel
    {
        private Tax tax;
        private readonly ICurrencyFormatter currencyFormatter = DependencyResolver.Current.GetService<ICurrencyFormatter>();
        private readonly IResourceProvider resourceProvider = DependencyResolver.Current.GetService<IResourceProvider>();

        public TaxModel(Tax tax)
        {
            if (tax == null)
            {
                throw new ArgumentNullException("tax");
            }

            this.tax = tax;
        }

        public string Name
        {
            get
            {
                var name = this.tax.Name;
                var nameFormatString = resourceProvider.GetResource("TaxNameDisplay");

                if (!String.IsNullOrEmpty(nameFormatString))
                {
                    name = string.Format(nameFormatString, this.Percentage);
                }

                return name;
            }
        }

        public string Amount
        {
            get
            {
                return currencyFormatter.FormatAmount(this.tax.Amount);
            }
        }

        public string Percentage
        {
            get
            {
                return currencyFormatter.FormatPercentageRate(this.tax.Percentage);
            }
        }
    }
}
