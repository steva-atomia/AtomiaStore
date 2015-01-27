using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Atomia.Common.Validation;
using Atomia.Web.Plugin.Validation.ValidationAttributes;
using Atomia.Store.Core;

namespace Atomia.Store.AspNetMvc.Models
{
    public abstract class ContactModel
    {
        [AtomiaRequired("ValidationErrors,ErrorEmptyField")]
        [CustomerValidation(CustomerValidationType.Email, "CustomerValidation,Email")]
        public virtual string Email { get; set; }

        [AtomiaRequired("ValidationErrors,ErrorEmptyField")]
        [CustomerValidation(CustomerValidationType.FirstName, "CustomerValidation,FirstName")]
        public virtual string FirstName { get; set; }

        [AtomiaRequired("ValidationErrors,ErrorEmptyField")]
        [CustomerValidation(CustomerValidationType.LastName, "CustomerValidation,LastName")]
        public virtual string LastName { get; set; }

        [CustomerValidation(CustomerValidationType.CompanyName, "CustomerValidation,CompanyName")]
        public virtual string CompanyName { get; set; }

        [CustomerValidation(CustomerValidationType.IdentityNumber, "CustomerValidation,IdentityNumber", CountryField = "Country", ProductField = "CartItems.ArticleNumber", ResellerIdField = "ResellerId")]
        public virtual string IdentityNumber { get; set; }

        [AtomiaRequired("ValidationErrors,ErrorEmptyField")]
        [CustomerValidation(CustomerValidationType.Address, "CustomerValidation,Address")]
        public virtual string Address { get; set; }

        [CustomerValidation(CustomerValidationType.Address, "CustomerValidation,address")]
        public virtual string Address2 { get; set; }

        [AtomiaRequired("ValidationErrors,ErrorEmptyField")]
        [CustomerValidation(CustomerValidationType.City, "CustomerValidation,City")]
        public virtual string City { get; set; }

        [AtomiaRequired("ValidationErrors,ErrorEmptyField")]
        [CustomerValidation(CustomerValidationType.Zip, "CustomerValidation,Zip", CountryField = "Country")]
        public virtual string Zip { get; set; }

        //[Required]
        //[CustomerValidation(CustomerValidationType.Country, "CustomerValidation,Country")]
        public virtual string Country { get; set; }

        public virtual string State { get; set; }

        [AtomiaRequired("ValidationErrors,ErrorEmptyField")]
        [CustomerValidation(CustomerValidationType.Phone, "CustomerValidation,Phone", CountryField = "Country")]
        public virtual string Phone { get; set; }

        [CustomerValidation(CustomerValidationType.Fax, "CustomerValidation,Fax", CountryField = "Country")]
        public virtual string Fax { get; set; }

        public virtual Guid ResellerId { get; set; }

        public virtual List<CartItem> CartItems { get; set; }
    }


    public class MainContactModel : ContactModel
    {
        [AtomiaRequired("ValidationErrors,ErrorEmptyField")]
        [CustomerValidation(CustomerValidationType.Email, "CustomerValidation,Email")]
        [AtomiaUsername("ValidationErrors,ErrorUsernameNotAvailable")]
        public override string Email { get; set; }
    }


    public class BillingContactModel : ContactModel
    {

    }
}
