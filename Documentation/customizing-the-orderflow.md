Customizing the Order Flow
==========================

AtomiaStore makes it easy to create custom order flows. The order flow relies on mapping named routes in the route configuration and then putting them together into one or more order flows.

Configuration of the order flow happens in `MyTheme\App_Start\OrderFlowConfig.cs` and route configuration in `MyTheme\App_Start\RouteConfig.cs`.

The following controller/action pairs are currently possible to use as part of an order flow:

* **Domains/Index**
* **ProductListing/Index**
* **Account/Index**
* **Checkout/Index**

**Note**: Currently the *Account* and *Checkout* steps are required to be the second to last and last steps in an order flow. They are included in the order flow to hook in to order flow validation and to get access to order flow presentation data like previous and next actions.

All routes that should be part of an order flow must be mapped with the `RouteCollection` extension method `MapOrderFlowRoute`.

You can specify one order flow as the default order flow. All order flows except the default one must be identified in the URL by the query string `?flow=<OrderFlowName>`. The default order flow can be used with or without the query string.


Re-ordering Existing Order Flow Steps
-------------------------------------

The Default theme has a single default order flow named *DefaultFlow* that contains the following steps: *Domains => HostingPackage => Account => Checkout*.

As mentioned above, the *Account* and *Checkout* steps should always be located in last two steps of the order flow, so for re-ordering the existing *DefaultFlow* order flow, the only realistic option left is to switch places between *Domains* and *HostingPackage*.

To start we configure the new  order flow in `MyTheme\App_Start\OrderFlowConfig.cs`. The final configuration will look something like this (with each step explained below):

    public static void RegisterOrderFlows(OrderFlowCollection orderFlows)
    {
        // 1.
        orderFlows.Clear();

        // 2.
        var orderFlow = new OrderFlow("MyOrderFlow", new [] {"HostingPackage", "Domains", "Account", "Checkout"});

        // 3.
        orderFlow.AddRouteNameAlias("OrderFlowStart", "HostingPackage");
    
        // 4.
        orderFlows.Add(orderFlow, true);
    }

1. Since we want to re-order the one order flow we have, we completely remove it from the existing order flows.
2. The new order flow *MyOrderFlow* is defined with *HostingPackage* placed before *Domains*.
3. Since we want to have the first step in the order flow be accessible at the root path of the application an alias from the *OrderFlowStart* route to *HostingPackage* is added (see more below).
4. Finally we add *MyOrderFlow* to the order flows used by the application.

In step 3 above, we added an alias from the *OrderFlowStart* route to *HostingPackage*. However, in the default route config the *OrderFlowStart* route is set to resolve to *Domains*, so we must also add a change to `MyTheme\App_Start\RouteConfig.cs`:

    public static void RegisterRoutes(RouteCollection routes)
    {
        // 1.
        routes.Remove(routes["OrderFlowStart"]);

        // 2.
        routes.MapOrderFlowRoute(
            name: "OrderFlowStart",
            url: "",
            defaults: new
            {
                controller = "ProductListing",
                action = "Index",
                query = "HostingPackage",
                viewName = "HostingPackage"
            }
        );
    }

1. We first remove the original *OrderFlowStart* route.
2. The new *OrderFlowStart* route is defined with the same defaults as the *HostingPackage* route, but with a new `url` value to match the root URL of the application.


Adding a New Order Flow Step
----------------------------

Continuing with the example from *Re-ordering Existing Order Flow Steps* we might want to add a page offering extra services as the final step before *Account* and *Checkout*. This will adding a new route to a page listing the extra services, and adding that route to the order flow.

This time we start by adding the following route to the route configuration:

    routes.MapOrderFlowRoute(
        name: "ExtraService",
        url: "Addons",
        defaults: new
        {
            controller = "ProductListing",
            action = "Index",
            query = "ExtraService",
            viewName = "ExtraService"
        }
    );

Some notes about the route configuration options:

* `name`: The route is named `ExtraService`, which is also the name we must use in the order flow configuration.
* `url`: How the route is represented to the end user is not dependent on the route name so we chose `/Addons` instead.
* `controller` and `action`: This is a page for listing products.
* `query`: By default the `ProductListing.Index` action lists products with the category specified in `query`.
* `viewName`: We want to implement a different view for this page, so we must later add the `Themes\MyTheme\Views\ProductListing\ExtraService.cshtml` since it is not part of the `Default` theme. We should also add new resource translations for the step title and description (see *Changing the Order Flow Presentation*)

We can now add the new page to the order flow:

    var orderFlow = new OrderFlow("MyOrderFlow", new [] {"HostingPackage", "Domains", "ExtraService", "Account", "Checkout"});


Creating Multiple Order Flows
-----------------------------

It is possible to create multiple order flows in AtomiaStore. E.g. you might want to have two different order flows for selling *shared hosting* and *VPS*. Assuming Atomia Billing had been set up for this and that the routes had been added you could configure the order flows something like this:

    public static void RegisterOrderFlows(OrderFlowCollection orderFlows)
    {
        orderFlows.Clear();

        var sharedHostingFlow = new OrderFlow("Hosting", new[] { "Domains", "HostingPackage", "Account", "Checkout" });
        sharedHostingFlow.AddRouteNameAlias("OrderFlowStart", "Domains");

        var vpsFlow = new OrderFlow("VPS", new[] { "Domains", "VPS", "Account", "Checkout" });
        vpsFlow.AddRouteNameAlias("OrderFlowStart", "Domains");

        orderFlows.Add(sharedHostingFlow, true);
        orderFlows.Add(vpsFlow);
    }

Things to note:

* If you want to have a single entry point for the two order flows, e.g. the *Domains* page, you must your self manually set up the links to the next step in respective order flow (see *Changing the Order Flow Presentation* below.)
* The *shared hosting* flow is set as default so will be used without adding a query string and with the added query string `?flow=Hosting`.
* The *vps* flow will only be used with the added query string `?flow=VPS`


Changing the Order Flow Presentation
------------------------------------

Views that are part of the order flow have access to order flow data of the type `Atomia.Store.AspNetMvc.Models.OrderFlowModel` via `ViewBag.OrderFlow`.

The *Default* theme uses this data in the step specific `_Actions.cshtml` partial views to render buttons to move between steps, and the shared partial view `_Progress.cshtml` to present the order flow progress.

Each order flow step has a `Title`, `Description` and `StepNumber`. `Title` and `Description` are populated by resource strings from `App_GlobalResources\Common.*.resx` with names on the form *StepTitle{Name}* and *StepDescription{Name}*. `StepNumber` is the 1-based index of the step in the order flow.

The steps also have the `Previous` and `Next` properties, which are the names of the previous and next steps in the order flow. If the value of either property is an empty string it means that there is no step in that direction, e.g. *Checkout* always has an empty `Next` property.

The `Previous` and `Next` properties match named routes so can be used e.g. with the `RouteLink` HTML-helper to render links to another step.

    @Html.RouteLink(Html.CommonResource("Back"), orderFlow.CurrentStep.Previous, new { flow = orderFlow.Name }, new { id = "back_step" })

The above example renders a link to the previous step, and also sets the `flow` query string to the value from `orderFlow.Name`.


Validating the Order Flow Steps
-------------------------------

The steps in an order flow can be validated by implementing the `Atomia.Store.AspNetMvc.Ports.IOrderFlowValidator` interface. 

The `ValidateOrderFlowStep` method gets called for the current step, and if the step is not valid there is an automatic redirect to the previous step.

The purpose of this is to prevent users from going directly to an order flow step that does not make sense.

There is a default implementation in `Atomia.Store.Themes.Default.Adapters.OrderFlowValidator` that checks that the cart is not empty in the *Account* step, and that the cart is not empty and that contact data is not empty in the *Checkout* step.

**Note**: The order flow validation is currently limited to the backend. Each view is responsible for checking if the customer should be allowed to proceed to the next step. The views are also responsible for handling changes to the cart, e.g. if the customer removes all items while on the last *Checkout* step.