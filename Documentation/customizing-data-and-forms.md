Customizing Data and Forms
==========================

The AtomiaStore makes data available to the views in a few different forms. This the division between different data types used in views:

* **Static text**: Resource strings. See [Resource Strings and Localization](resource-strings-and-localization.md)
* **Forms and server-rendered data**: Asp.NET MVC view models. See below.
* **Cross-cutting data**: Added to `ViewBag`. See [Customizing the Order Flow](customizing-the-orderflow.md) for an example.
* **Client-side data**: Knockout.js view models populated via JSON rendered on page load, e.g. the cart. See below.

The above division should not be viewed as a set of definite rules, but more as a guide for where to start looking when you want to customize something.

Forms and Server-rendered Data
------------------------------

Standard ASP.NET MVC view models are used mostly for handling forms; they let us leverage the built-in functionality for form validation and model binding.

The view models of each order flow step type can be extended or replaced:

* `Atomia.Store.AspNetMvc.Models.AccountViewModel`
* `Atomia.Store.AspNetMvc.Models.CheckoutViewModel`
* `Atomia.Store.AspNetMvc.Models.DomainsViewModel`
* `Atomia.Store.AspNetMvc.Models.ProductListingViewModel`

Additionally the `Atomia.Store.AspNetMvc.Models.ProductListingModel` can also be extended. It makes product data available as JSON.

The `AccountViewModel` and the `CheckoutViewModel` are abstract classes with the following default implementations:

* `Atomia.Store.AspNetMvc.Models.DefaultAccountViewModel`
* `Atomia.Store.AspNetMvc.Models.DefaultCheckoutViewModel`


### Defining a New View Model

In the following example we will extend the default `AccountViewModel` with a sub-form that let's the customer optionally provide their occupation in addition to the default *main contact* and *billing contact* forms..

If you used the `startnewtheme.ps1` script to bootstrap your theme, you should already have a `Models` directory. In this directory we add a new file with the classes `OccupationModel` and `MyAccountViewModel`:

    using Atomia.Store.AspNetMvc.Models;
    using Atomia.Store.AspNetMvc.Ports;
    using Atomia.Web.Plugin.Validation.ValidationAttributes; // 1.

    namespace MyTheme.Models
    {
        public class OccupationModel : ContactDataForm // 2.
        {
            public override string Id { get { return "Occupation"; } } // 3.

            [AtomiaRequired("Common,ErrorEmptyField")] // 4.
            public string JobTitle { get; set; }

            [AtomiaRange(0, 100, "Common,ErrorYearsExperience")] // 5.
            public int YearsExperience { get; set; }
        }

        public class MyAccountViewModel : DefaultAccountViewModel // 6.
        {
            public OccupationModel Occupation { get; set; } // 7.
        }
    }


1. The `Atomia.Web.Plugin.Validation` library has some specific model validation attributes that properly handle error messages as defined in theme resource files. It is not available by default in the bootstrapped theme, so we need to add the reference to the assembly `MyTheme\Lib\Atomia.Web.Plugin.Validation.dll`
2. The subform that we will use to collect the data. It subclasses `Atomia.Store.AspNetMvc.Ports.ContactDataForm` so we can later access it an add it to the order we place in *Atomia Billing*. (See more at FIXME)
3. A `ContactDataForm` implementation must have an `Id` property. It is convenient to have it be the same as the name of the subform property in the `AccountViewModel`, in this case *Occupation*.
4. We do not want to require the customer to fill in occupation, but *if* they chose to do so, we want to require that they supply a job title. We also re-use an error message that already exists in the default `resx` files.
5. `YearsExperience` is an optional field. The error message is custom, so we have to add it to our `MyThemeValidationErrors.resx` file, and any localized variants.
6. Since we just want to add an extra form, and not completely reimplement the `AccountViewModel`, we sub-class the existing `DefaultAccountViewModel`.
7. In *3* we mentioned that we do not want to require the user to supply occupation data. Here we make that happen by *not* requiring the `OccupationModel` subform. If we POST the form without any of the `OccupationModel` fields, the ASP.NET MVC model binder will ignore the subform and not generate any validation errors for it, but *if* we POST fields from the subform the model validation will be triggered on the fields.

We have now defined the new view model, but the `Account\Index.cshtml` view still uses the `DefaultAccountViewModel`.

There are two steps to adding the new model to the view.

* Registering the model with the dependency resolver.
* Setting the model as the `@model` in the view.

### Registering the Model

AtomiaStore leverages **Unity** and the ASP.NET MVC `DependencyResolver` to make many parts of the application replacable and extendable via dependency injection or service location.

By default the `DefaultAccountViewModel` is registered to be provided when an instance of `AccountViewModel` is needed.

We can override this registration either programmatically in `App_Start\UnityConfig.cs` or with configuration in the `unity` section of `Web.config`.

**How to do it in** `UnityConfig`:

    public class UnityConfig
    {
        public static void RegisterComponents(UnityContainer container)
        {
            container.RegisterType<AccountViewModel, MyTheme.Models.MyAccountViewModel>();
        }
    }

**How to do it in** `Web.config`:

    <unity xmlns="http://schemas.microsoft.com/practices/2010/unity">
    ...
        <alias alias="AccountViewModel" type="Atomia.Store.AspNetMvc.Models.AccountViewModel, Atomia.Store.AspNetMvc" />
        <alias alias="MyAccountViewModel" type="MyTheme.Models.MyAccountViewModel, MyTheme" />
        ...
        <container>
            ...
            <register type="AccountViewModel" mapTo="MyAccountViewModel" />
            ...
        </container>
    </unity>



### Using the Model in the View

* Overriding `_ExtraForms.cshtml`


Client-side Data
----------------

* Overriding `_ExtraScripts.cshtml`
* Extending view model
* Changing view model


@Html.JsonAction