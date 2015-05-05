Customizing View Models
=======================

AtomiaStore makes data available to views in a few different forms:

* **Resource strings**:  Static (localized) text. See [Resource Strings and Localization](resource-strings-and-localization.md)
* **ASP.NET MVC view models**: E.g. forms. See below.
* **ViewBag**: Mainly data used across multiple views. See [Customizing the Order Flow](customizing-the-orderflow.md) for an example.
* **Knockout.js view models**: JavaScript view models for interactive behavior populated via JSON rendered on page load, e.g. the cart. See below.

ASP.NET MVC View Models
-----------------------

Standard ASP.NET MVC view models are used mostly for handling forms; they let us leverage the built-in functionality for form validation and model binding.

The view model for each page type in the order flow can be extended or replaced:

* `Atomia.Store.AspNetMvc.Models.AccountViewModel`
* `Atomia.Store.AspNetMvc.Models.CheckoutViewModel`
* `Atomia.Store.AspNetMvc.Models.DomainsViewModel`
* `Atomia.Store.AspNetMvc.Models.ProductListingViewModel`

The related `Atomia.Store.AspNetMvc.Models.ProductListingModel` can also be extended.

`AccountViewModel` and the `CheckoutViewModel` are abstract classes with the following default implementations:

* `Atomia.Store.AspNetMvc.Models.DefaultAccountViewModel`
* `Atomia.Store.AspNetMvc.Models.DefaultCheckoutViewModel`


### Defining a New View Model

In the following example we will extend the default `AccountViewModel` with a sub-form for an optional *Professional Survey* of our customers in addition to the default main contact and billing contact forms.

If you used the `startnewtheme.ps1` script to bootstrap your theme, you should already have a `Models` directory. In this directory we add a new file with the classes `SurveyModel` and `MyAccountViewModel`:

    using Atomia.Store.AspNetMvc.Models;
    using Atomia.Store.Core;
    using Atomia.Web.Plugin.Validation.ValidationAttributes; // 1.

    namespace MyTheme.Models
    {
        public class SurveyModel : ContactData // 2.
        {
            public override string Id { get { return "Survey"; } } // 3.

            [AtomiaRequired("Common,ErrorEmptyField")] // 4.
            public string JobTitle { get; set; }

            public string Department { get; set; } // 5.
        }

        public class MyAccountViewModel : DefaultAccountViewModel // 6.
        {
        
            public MyAccountViewModel() : base()
            {
                this.Survey = new SurveyModel();    // 7.
            }

            public SurveyModel Survey { get; set; } // 8.
        }
    }


1. The `Atomia.Web.Plugin.Validation` library has some specific model validation attributes that properly handle error messages as defined in theme resource files. It is not available by default in the bootstrapped theme, so we need to add the reference to the assembly `MyTheme\Lib\Atomia.Web.Plugin.Validation.dll`
2. The subform that we will use to collect the data. It subclasses `Atomia.Store.Core.ContactData` so we can later access it an add it to the order we place in *Atomia Billing*. (See more at [Customizing Order Handling](customizing-order-handling.md) for an extension of this example)
3. A `ContactData` implementation must have an `Id` property. It is convenient to have it be the same as the name of the subform property in the `AccountViewModel`, in this case *Survey*.
4. We do not want to require the customer to fill in survey, but *if* they chose to do so, we want to require that they supply a job title. We also re-use an error message that already exists in the default `resx` files.
5. `Department` is an optional field.
6. Since we just want to add an extra form, and not completely reimplement the `AccountViewModel`, we sub-class the existing `DefaultAccountViewModel`.
7. For the new `SurveyModel` to be available in the view we need to instantiate it when `MyAccoutViewModel` is instantiated.
8. In *3* we mentioned that we do not want to require the user to supply survey data. Here we make that happen by *not* requiring the `SurveyModel` subform. If we POST the form without any of the `SurveyModel` fields, the ASP.NET MVC model binder will ignore the subform and not generate any validation errors for it, but *if* we POST any of the fields from the subform the model validation will be triggered.

We have now defined the new view model, but we don't use it in our Razor views yet.

There are two steps to adding the new model to the view.

* Registering the model with the dependency resolver.
* Setting the model as the `@model` in the view or appropriate partial view.

### Registering the Model With the Dependency Resolver

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

Having prepared the new view model we should now present it to the user by rendering it as a form in the view.

When we have a new view model implementation there are a few things to take into account when deciding how to use it in the views:

* Is the view model a standalone implementation or a sub-class of the default model?
* How extensive are the changes?
* Are there any existing partials that can contain the changes or does the "page" view need to be overridden?

In our case we have sub-classed the `DefaultAccountViewModel` instead of re-implementing our own `AccountViewModel` from scratch. This means we do not need to change any of the views that are using `DefaultAccountViewModel` as their view model since our view model is still of that type. We have also only added to the default implementation, not overridden or hidden any of the existing members, so existing partial views that use the model properties can be left as is.

The `Account/Index.cshtml` view has a couple of pre-defined extension points that can be used to add things without a need to override the whole view to add new partials: `Account/_ExtraForms.cshtml` and `Account/_ExtraScripts.cshtml`.

We want to keep as much of the Default theme as possible to get the benefit of future updates to it, so we decide that we can add our new form to the `Account/_ExtraForms.cshtml` so we add the file `Themes/MyTheme/Views/Account/_ExtraForms.cshtml` with the following markup:

    @model MyTheme.Models.MyAccountViewModel // 1.

    <div class="pro-survey"> // 2.
        <h4>@Html.CommonResource("ProfessionalSurvey")</h4> // 3.

        @Html.FormRowFor(m => m.Survey.JobTitle, Html.CommonResource("JobTitle") + ":", true) // 4.

        @Html.FormRowFor(m => m.Survey.Department, Html.CommonResource("Department") + ":", false) // 5.
    </div>


1. `Index.cshtml` renders the `_ExtraForms.cshml` partial with the whole view model like this:

        Html.RenderPartial("_ExtraForms", Model);

    so we use our newly defined class to strongly type the view, so we can access `Survey` and its members.
2. Wrapping the new markup in a `div` to be able to style it, perhaps by floating it to one side or something else.
3. We add a title for our new sub-form that matches the titles for *Contact Info* and *Billing Address*. We also need to add the resource string `"ProfessionalSurvey"` to `App_GlobalResources/MyTheme/MyThemeCommon.resx` and any localizations we need.
4. To render the input field and label we use an HTML helper from the Default theme. It renders label, input field and validation messages in the standard markup used for form fields in the Default theme. The last boolean argument `true` marks this field as required. We also need to add `"JobTitle"` to `App_GlobalResources/MyTheme/MyThemeCommon.resx` for the label.
5. The same as 4, except this field is not required.

**Note:** The `FormRowFor` HTML helper namespace is added to all views in `Themes/MyTheme/Views/Web.config`, which is added by the theme starter script.

You should now have a basic form added for your survey. It will also have client-side as well as backend validation of the required `JobTitle` field. However, we are not completely fulfilling our initial requirements yet, since currently the survey is not optional and the `JobTitle` field will always be required. In the next section we will see how this can be fixed by working with the knockout.js view model for the page.


Knockout.js View Models
-----------------------

AtomiaStore uses knockout.js view models for more interactive elements of the user interface. As with the backend code, these can also be reused and extended in different ways.

Continuing with our *Professional Survey* example from above, we want to make the entire sub-form optional, similar to how it is optional fill in the Default *Billing Contact* form.

The existing knockout view models on the Account page are all instantiated in the `Themes/Default/Views/Account/_Scripts.cshtml` partial view. We can see there that there is an existing `Atomia.ViewModels.AccountModel` that is instantiated as `Atomia.VM.account`. This is a knockout view model that is used for showing or hiding the *Billing Contact* form and to control if the fields from that form are posted to the server or not, and to customize some fields depending on if the customer type is `"individual"` or `"company"`.

For our new *Professional Survey* form we have the choice to either extend the existing `Atomia.VM.account` model or to add a separate knockout view model. In general, whether we choose to extend an existing knockout view model or create a new one depends on what we want to accomplish.

Below we will do both. We start with a basic knockout view model that is independent of the existing functionality, and then make it dependent on some of the existing account model's functionality and change to an extension model.

In both cases we put our new knockout view model code in `Themes/MyTheme/Scripts/mytheme.viewmodels.survey.js`. The prepared setup on theme creation makes sure that all JavaScript files in the `Themes/MyTheme/Scripts` directory are included in the defult scripts bundle.

### Creating a New Knockout.js View Model

We start by defining a simple knockout view model using a variant of the [module pattern](http://addyosmani.com/resources/essentialjsdesignpatterns/book/#modulepatternjavascript) an placing it under the `MyTheme.ViewModels` namespace:

    var MyTheme = MyTheme || {};
    MyTheme.ViewModels = MyTheme.ViewModels || {}; // 1.

    (function (exports, _, ko) { // 2.
        'use strict';

        function ProfessionalSurveyModel() {
            var self = this; // 3.

            self.wantsToFillOutSurvey = ko.observable(true); // 4.

            self.optOutOfSurvey = function () { // 5.
                self.wantsToFillOutSurvey(false);
            };

            self.optInToSurvey = function () { // 5.
                self.wantsToFillOutSurvey(true);
            };
        }

        _.extend(exports, { // 6.
            ProfessionalSurveyModel: ProfessionalSurveyModel
        });

    })(MyTheme.ViewModels, _, ko); // 7.

1. We define the `MyTheme` and `MyTheme.ViewModels` namespaces, if they don't already exist.
2. The namespace the module `exports` to, and [underscore.js](http://underscorejs.org/) and [knockout.js](http://www.knockoutjs.com) dependencies.
3. We use the common knockout.js pattern of assigning `this` to `self` so we don't need to bind `this` to our functions. (see *Managing 'this'* in knockout's [Computed Observables documentation](http://knockoutjs.com/documentation/computedObservables.html))
4. The knockout observable we will use to keep track if the customer wants to fill out the survey or not. We set it to `true` to begin with, so the customer has to opt out of filling it in.
5. Functions we will bind to clicking a link to let the user opt in or opt out of taking part in the survey.
6. Here we extend the `MyTheme.ViewModels` namespace with our `ProfessionalSurveyModel` constructor.
7. The actual namespace and dependencies that are used in 2.

Next we make an instance of our `ProfessionalSurveyModel` view model in `Account/_ExtraScripts.cshtml`:

    <script>
        Atomia.VM.survey = new MyTheme.ViewModels.ProfessionalSurveyModel();
    </script>

The `ProfessionalSurveyModel` is instantiated as a sub-model of the `Atomia.VM` view model. `Atomia.VM` and all sub-models will be activated on the page via a call to `ko.applyBindings(Atomia.VM)`.

Now we just need to set up the bindings between our `Atomia.VM.survey` model and the markup we added to `Account/_ExtraForms.cshtml` above:

    @model MyTheme.Models.MyAccountViewModel

    <div class="pro-survey" data-bind="with: survey"> // 1.
        <h4>
            @Html.CommonResource("ProfessionalSurvey")

            <span data-bind="visible: !wantsToFillOutSurvey()" style="display:none;"> // 2.
                (<a href="javascript:void(0);" data-bind="click: optInToSurvey">Sure, I'll take the survey</a>)
            </span>

            <span data-bind="visible: wantsToFillOutSurvey"> // 3.
                (<a href="javascript:void(0);" data-bind="click: optOutOfSurvey">No thanks!</a>)
            </span>
        </h4>

        <div data-bind="slideVisible: wantsToFillOutSurvey"> // 4.
            @Html.FormRowFor(m => m.Survey.JobTitle, Html.CommonResource("JobTitle") + ":", true, "if: wantsToFillOutSurvey") // 5.

            @Html.FormRowFor(m => m.Survey.Department, Html.CommonResource("Department") + ":", false, "if: wantsToFillOutSurvey") // 5.
        </div>
    </div>

1. We set the scope of the contained data bindings to `survey` (short for `Atomia.VM.survey`.)
2. We hide the opt-in link to start, and set up the `click` binding to opt in to the survey.
3. We show the the opt-out link, and set up the `click` binding to opt out of the survey.
4. We use the custom `slideVisible` binding, which is the same as the standard `visible`, except it slides down for a more pleasant experience.
5. For both form rows, we set up an `if` binding to keep the `input` bindings in the DOM only if the user wants to fill out the survey. This has the effect of these fields not being posted on form submit if the user has opted out, and subsequently the ASP.NET MVC model binder will not try to bind these fields, and skip validation of `JobTitle` even though it is annotated as required.


### Extending an Existing Knockout.js View Model

We now have a functioning survey. However, we might want to handle it differently depending on if the customer is an individual or company. So let's add a requirement that if the customer is an individual the survey is opt-in, and if the customer is a company the survey is opt-out.

To accomplish this we need to access the `mainContactCustomerType` on the `Atomia.VM.account` model, which is created via the `Atomia.ViewModels.AccountModel` constructor.

We want the start value of the `wantsToFillOutSurey` to depend on the `mainContactCustomerType`. We also want to open the survey if the customer changes customer type to `"company"`.

First we modify the initialization of survey to change `Atomia.VM.account` instead of creating a separate model:

    <script>
        Atomia.VM.account = Atomia.Utils.mix(
            Atomia.ViewModels.AccountModel,
            MyTheme.ViewModels.ProfessionalSurveyModel);
    </script>

Here we use the `Atomia.Utils.mix` method to combine the two constructors `AccountModel` and `ProfessionalSurveyModel` to create a single view model object. When we mix like this the properties declared by `AccountModel` are available to the `ProfessionalSurveyModel` constructor. (Please note that the constructor itself is the argument, not an object created by the constructor, and that only constructors without arguments are supported.)

Next we modify the `ProfessionalSurveyModel` constructor to work with some of the `AccountModel` properties:

    var MyTheme = MyTheme || {};
    MyTheme.ViewModels = MyTheme.ViewModels || {};

    (function (exports, _, ko) {
        'use strict';

        function ProfessionalSurveyModel() {
            var self = this;

            self.wantsToFillOutSurvey = ko.observable(self.mainContactIsCompany()); // 1.

            self.optOutOfSurvey = function () {
                self.wantsToFillOutSurvey(false);
            };

            self.optInToSurvey = function () {
                self.wantsToFillOutSurvey(true);
            };

            self.mainContactCustomerType.subscribe(function(newCustomerType){ // 2.
                if (newCustomerType === 'company') {
                    self.wantsToFillOutSurvey(true);
                }
            });
        }

        _.extend(exports, {
            ProfessionalSurveyModel: ProfessionalSurveyModel
        });

    })(MyTheme.ViewModels, _, ko);

1. We set up the start value of survey to `true` if the customer is `"company"`.
2. We subscribe to changes on the `mainContactCustomerType` property to set `wantsToFillOutSurvey` to `true` if the customer is a `"company"`.

Finally, the markup we defined for the survey should work almost without change. Since setup a binding context with the `with` binding we only need to change that so that the markup binds to the `Atomia.VM.account` model that we are extending, and not the now non-existing `Atomia.VM.survey` model:

    <div class="pro-survey" data-bind="with: account">
        ...
    </div>

In the example above we only *used* some of the properties from the `AccountModel`, but by the nature of JavaScript we might just as easily have redefined some properties as well.