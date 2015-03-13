Customizing Views
=================

AtomiaStore views are written in **Razor**, make heavy use of partial views and are enhanced with [Knockout.js](http://www.knockoutjs.com).

The recommended way of customizing the markup of pages in AtomiaStore is to override just the layouts, views or partial views that need to be changed.

Data is accessible in the views via strongly typed ASP.NET MVC view models, and Knockout.js view models.

Organization
------------

**Layouts** are located in the root of the `Themes\MyTheme\Views` folder and are named `_<LayoutName>.cshtml`.

**Views** mapping to an action are located in their respective folders, e.g. `Themes\MyTheme\Views\Account` and are named just `<ViewName>.cshtml`.

**Partial views** are named `_<PartialViewName>.cshtml` and are located either with their parent page view, or if they are useful in more than one page, in the `Themes\MyTheme\Shared` folder.

There is also a static markup page in `Themes\Content\Error.html`, used in last resort error handling.

As part of the Default theme the following layouts are available:

* **_NoCartLayout.cshtml**: For views where the cart should not be shown, e.g. after an order has been placed.
* **_NoScriptsLayout.cshtml**: For views that use no JavaScript. Currently the Error views.
* **_OrderFlowLayout.cshtml**: For views that are part of the order flow steps.


View Structure and Naming
-------------------------

The views are structured in a standard ASP.NET MVC layout with *Layout => View => Partial View*.

    Layout
	    Shared Partial
	    ...
	    Page
		    Page Partial
		    Page Partial
		    ...

All views use strongly typed view models, and regular use of ViewBag and ViewData is discouraged.

The naming of page views are as customary mapped to action names in ASP.NET MVC. One exception to this rule is *ProductListing* pages, where the view name can be customized depending on the type of products to show.

Another naming convention is that view names are named the same as Knockout.js view models. This goes for both page views and partial views.

If you need to also change the view model of a page, see Customizing View Models and Forms.


Knockout.js View Models Overview
--------------------------------

Since most views are in some way making use of the JavaScript Model-View-ViewModel library [Knockout.js](http://www.knockoutjs.com) (**KO**) it is useful to know how the KO view models are put together.

As mentioned earlier each KO view model corresponds to a view or view partial with the same name. When the page loads each KO view model in use is created and added to the root view model `Atomia.VM` which then has bindings applied.

As an example this is the structure of the HostingPackages page KO view model:

    Atomia.VM = {
	    LanguageSelector: ...
        Cart: ...,
        Progress: ...,	
        Notification: ...,
	    ConnectedDomainStatus: ...,
        HostingPackages: ...
    }

Some KO view models also have an array of child items. For example a `ProductListing` view model, which `HostingPackages` is an instance of, contains an array of `ProductListingItem`.

Data bindings in the views are then used with a prefix of the KO view model that is being used, e.g.:

    data-bind="visible: Notification.IsOpen, css: Notification.MessageType"

The default KO view model are defined in `Themes\Default\Scripts\atomia\atomia.viewmodels.*.js`. Each view model file is a JavaScript module with a factory function for making instances of a view model.

There are also some custom KO bindings defined in  `Themes\Default\Scripts\atomia\atomia.ko.*-binding.js`:

* **slideVisible**: wraps the default *visible* binding in a slide animation.
* **submitValid**: triggers jQuery validation on forms before submitting, and should be used when submitting AJAX POSTs.

For more on working with the Knockout.js view models see 'Working with Knockout.js View Models'