using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

using Atomia.Common.Validation;
using Atomia.Web.Plugin.Validation.ValidationAttributes;
using Atomia.Store.Core;
using Atomia.Store.AspNetMvc.Ports;

namespace Atomia.Store.AspNetMvc.Models
{
    public abstract class ContactModel : ContactDataForm
    {
        private readonly ICustomerTypeProvider customerTypeProvider = DependencyResolver.Current.GetService<ICustomerTypeProvider>();
        private readonly ICountryProvider countryProvider = DependencyResolver.Current.GetService<ICountryProvider>();

        private IndividualExtraInfo individualInfo;
        private CompanyExtraInfo companyInfo;

        public ContactModel()
        {
            var resellerProvider = DependencyResolver.Current.GetService<IResellerProvider>();
            var cartProvider = DependencyResolver.Current.GetService<ICartProvider>();

            var resellerId = resellerProvider.GetReseller().Id;
            var cart = cartProvider.GetCart();

            this.ResellerId = resellerId;
            this.CartItems = cart.CartItems.ToList();
            this.CompanyInfo = new CompanyExtraInfo();
            this.IndividualInfo = new IndividualExtraInfo();
        }

        public virtual IEnumerable<SelectListItem> CustomerTypeOptions { 
            get
            {
                var customerTypes = customerTypeProvider.GetCustomerTypes();

                return customerTypes.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id
                });
            }
        }

        public virtual IEnumerable<SelectListItem> CountryOptions
        {
            get
            {
                var countries = countryProvider.GetCountries();
                var defaultCountry = countryProvider.GetDefaultCountry();

                return countries.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Code,
                    Selected = (x.Code == defaultCountry.Code)
                });
            }
        }

        [AtomiaRequired("ValidationErrors,ErrorEmptyField")]
        public virtual string CustomerType { get; set; }

        [AtomiaRequired("ValidationErrors,ErrorEmptyField")]
        [CustomerValidation(CustomerValidationType.Email, "CustomerValidation,Email")]
        public virtual string Email { get; set; }

        [AtomiaRequired("ValidationErrors,ErrorEmptyField")]
        [CustomerValidation(CustomerValidationType.FirstName, "CustomerValidation,FirstName")]
        public virtual string FirstName { get; set; }

        [AtomiaRequired("ValidationErrors,ErrorEmptyField")]
        [CustomerValidation(CustomerValidationType.LastName, "CustomerValidation,LastName")]
        public virtual string LastName { get; set; }

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

        public virtual IndividualExtraInfo IndividualInfo
        {
            get
            {
                return individualInfo;
            }

            set
            {
                if (value.Parent == null)
                {
                    value.Parent = this;

                }

                individualInfo = value;
            }
        }

        public virtual CompanyExtraInfo CompanyInfo
        {
            get
            {
                return companyInfo;
            }

            set
            {
                if (value.Parent == null)
                {
                    value.Parent = this;
                }

                companyInfo = value;
            }
        }

        public virtual Guid ResellerId { get; set; }

        public virtual List<CartItem> CartItems { get; set; }
    }

    public class ContactSubmodel
    {
        internal ContactModel Parent { get; set; }

        public string Country
        {
            get
            {
                return Parent.Country;
            }
        }

        public List<CartItem> CartItems
        {
            get
            {
                return Parent.CartItems;
            }
        }

        public Guid ResellerId
        {
            get
            {
                return Parent.ResellerId;
            }
        }
    }


    public class CompanyExtraInfo : ContactSubmodel
    {
        [AtomiaRequired("ValidationErrors,ErrorEmptyField")]
        [CustomerValidation(CustomerValidationType.CompanyName, "CustomerValidation,CompanyName")]
        public virtual string CompanyName { get; set; }

        [AtomiaRequired("ValidationErrors,ErrorEmptyField")]
        [CustomerValidation(CustomerValidationType.IdentityNumber, "CustomerValidation,CompanyIdentityNumber", CountryField = "Country", ProductField = "CartItems.ArticleNumber", ResellerIdField = "ResellerId")]
        public virtual string IdentityNumber { get; set; }

        [CustomerValidation(CustomerValidationType.VatNumber, "CustomerValidation,VatNumber", CountryField = "Country", ProductField = "CartItems.ArticleNumber", ResellerIdField = "ResellerId")]
        public virtual string VatNumber { get; set; }

    }

    public class IndividualExtraInfo : ContactSubmodel
    {
        [CustomerValidation(CustomerValidationType.IdentityNumber, "CustomerValidation,IndividualIdentityNumber", CountryField = "Country", ProductField = "CartItems.ArticleNumber", ResellerIdField = "ResellerId")]
        public virtual string IdentityNumber { get; set; }
    }
}
