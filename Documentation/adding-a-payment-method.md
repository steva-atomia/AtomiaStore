Adding a Payment Method
=======================

Payment plugins in AtomiaStore are counter-parts to payment plugins in Atomia Billing.

AtomiaStore payment plugins are made up of two parts: 

* `Atomia.Store.AspNetMvc.Ports.PaymentMethodGuiPlugin` implementation that is responsible for presenting the payment method to the user and collecting any needed information.
* `Atomia.Store.PublicBillingApi.Handlers.PaymentDataHandler` implementation that amends the order or transaction (or both) that are sent to Atomia Billing.

Both `PaymentMethodGuiPlugins` and `PaymentDataHandlers` have an `Id` property that must match the `GuiPluginName` property from `Atomia.Billing.Core.Sdk.BusinessObjects.PaymentMethod` in Atomia Billing.

The only payment methods presented to the user are those that are 1) available for the current reseller in Atomia Billing and 2) have an implemented `PaymentMethodGuiPlugin` with the same `Id` property registered with the `DependencyResolver`.

Similarly there must also be a registered `PaymentDataHandler` with matching `Id` to the `PaymentMethodGuiPlugin` that represents the payment method that the user has selected, or an exception will be thrown when the order is placed.

Implementing the `PaymentMethodGuiPlugin`
---------------------------------------

Implement by subclassing the abstract `Atomia.Store.AspNetMvc.Ports.PaymentMethodGuiPlugin`.

* `Id` is a required property and should match the Atomia Billing payment method `GuiPluginName`.
* `Name` is a required property and should return a human-readable localized name of the payment method.
* `Description` is an optional override. It should return a human-readable localized description or instruction for the payment method. Defaults to returning empty string.
* `SupportsPaymentProfile` should return `true` if the corresponding payment plugin in Atomia Billing supports saving credit card information and automatic payments of invoices. Defaults to `false`.
* `Form` is for payment methods that require some additional data from the user (e.g. `CCPayment` and `PayWithInvoice` projects.) See more below.

### Form Details

A form connected to a `PaymentMethodGuiPlugin` via the `Form` property must implement `Atomia.Store.AspNetMvc.Ports.PaymentMethodForm`. Like the `PaymentMethodGuiPlugin` itself it must have an `Id` property with the name of the payment method.

To make the form available in views it should be instantiated in the constructor of the `PaymentMethodGuiPlugin`.

When an available payment method is rendered in the `Checkout/_PaymentMethods.cshtml` view, it will be checked for an instantiated `Form` property, and if that exists try to render a partial for the form. The name of the partial view is by default "_{Id}". This can be changed by overriding the `PartialViewName` property on the `PaymentMethodForm`. E.g. the `CCPayment` method has a form that is rendered by the `Checkout/_CCPayment.cshtml` view.


Implementing the `PaymentDataHandler`
---------------------------------------

The `PaymentDataHandler` is responsible for taking the user's selected payment method and any related `PaymentMethodForm` data and populating the payment details on the order and/or payment transaction that is sent to Atomia Billing.

* `Id` is a required property and should match the Atomia Billing payment method `GuiPluginName`.
* `PaymentMethodType` is a required property and should return either `PaymentMethodEnum.PayByCard` or `PaymentMethodEnum.PayByInvoice`. Most plugins should use `PaymentMethodEnum.PayByCard`.

The actual amendment of payment transactions and/or orders happens in the `AmendPaymentTransaction` and `AmendOrder` methods, each of which can be overridden as needed. Both these methods should return the object (order or payment transaction) the have amended.

`AmendOrder` is currently only used by the `PayWithInvoiceHandler`.

`AmendPaymentTransaction` should be overridden by most `PaymentDataHandlers`. Commonly it will be used to set the `ReturnUrl` property on the `transaction` and add a `CancelUrl` attribute to the `transaction`. The latter can be done by using the `SetCancelUrl` helper method that is available to subclasses of `PaymentDataHandler`. Common URLs, such as `DefaultPaymentUrl` and `CancelUrl` can be retrieved by using the currently registered `Atomia.Store.Core.PaymentUrlProvider`. `DefaultPaymentUrl` matches an action that can handle redirects from `IHttpHandlers` that are implemented by many of the existing Atomia Billing payment plugins to process redirects from payment gateways.

`AmendPaymentTransaction` takes an argument of type `Atomia.Store.Core.PaymentData`. This object contains an instance of any form the payment method uses. The form should be cast to the concrete implementation type before usage, see `CCPaymentHandler` for an example.

The `PaymentData` object also contains `SaveCcInfo` and `AutoPay` properties that contain the choices the user has made regarding saving the payment method info to their acccount and paying invoices automatically. The individual `PaymentDataHandlers` for each payment method do **not** need to handle this data. This is done in common for all payment methods by `Atomia.Store.PublicOrderHandler.PaymentProfileHandler`.


Other Payment Method Functionality
----------------------------------

Some payment plugins might need to have some other sort of functionality apart from the `PaymentMethodGuiPlugin`, `PaymentMethodForm` and `PaymentDataHandler`