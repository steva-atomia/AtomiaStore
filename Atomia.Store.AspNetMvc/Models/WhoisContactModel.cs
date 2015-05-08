using Atomia.Common.Validation;
using Atomia.Web.Plugin.Validation.ValidationAttributes;


namespace Atomia.Store.AspNetMvc.Models
{
    /// <summary>
    /// A <see cref="ContactSubmodel"/> form that collects data only relevant for individual (private person) customers.
    /// </summary>
    /// <remarks>The difference between this and IndividualExtraInfo is that IdentityNumber is required</remarks>
    public class WhoisIndividualExtraInfo : ContactSubmodel
    {
        /// <summary>
        /// Customer provided national identification / identity / registration number for the customer
        /// </summary>
        [AtomiaRequired("Common,ErrorEmptyField")]
        [CustomerValidation(CustomerValidationType.IdentityNumber, "CustomerValidation,IndividualIdentityNumber", CountryField = "Country", ProductField = "CartItems.ArticleNumber", ResellerIdField = "ResellerId")]
        public string IdentityNumber { get; set; }
    }

    /// <summary>
    /// For customer to supply WHOIS contact separate from main contact.
    /// </summary>
    public class WhoisContactModel : ContactModel
    {
        private WhoisIndividualExtraInfo whoisIndividualInfo;

        /// <summary>
        /// Unique <see cref="Atomia.Store.Core.ContactData"/> identifier.
        /// </summary>
        public override string Id
        {
            get { return "WhoisContact"; }
        }

        /// <summary>
        /// <see cref="WhoisIndividualExtraInfo"/> sub-form.
        /// </summary>
        /// <remarks>
        /// We are purposefully hiding the <see cref="IndividualExtraInfo"/> sub-form with 'new' to keep the usage in the view the same, 
        /// and to avoid any confusion to which property should be used.
        /// </remarks>
        public new WhoisIndividualExtraInfo IndividualInfo
        {
            get
            {
                if (whoisIndividualInfo == null)
                {
                    whoisIndividualInfo = new WhoisIndividualExtraInfo();
                }

                if (whoisIndividualInfo.Parent == null)
                {
                    whoisIndividualInfo.Parent = this;
                }

                return whoisIndividualInfo;
            }
            set
            {
                whoisIndividualInfo = value;
            }
        }
    }
}
