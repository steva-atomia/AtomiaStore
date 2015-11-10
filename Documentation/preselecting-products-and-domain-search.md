Pre-selecting Products, Custom Attributes and Domain Search
===========================================================

Atomia Store supports pre-selecting products, campaign codes and custom attributes for a customer, i.e. adding them to the cart as part of linking or redirecting the customer to Atomia Store. You can also link directly to a domain search.

This can be done e.g. from a marketing site or an email newsletter.

Adding one or more products with optional campaign code to the cart, or adding custom attributes can be done by sending a form with a POST request. Adding a campaign code can be done by using a POST request or a special link.

**CAUTION!** Even though many advanced options like multiple products and custom attributes are available, it is recommended to limit the usage of the pre-select feature to simple use cases (see more in Validation below). Pre-selection is **not** meant as an integration point for something like a completely separate order page. 


Limited Validation
------------------

There is limited direct validation in using these forms, and when validation fails it will do so silently by ignoring the invalid field and not adding anything to the cart.

This is done to keep the use of these forms simple and to at least let an end user continue with adding products manually in case anything goes wrong.

This means that you need to be careful to add the correct values, e.g. article numbers for products must exist and renewal periods must be available for a particular article number.

Be extra careful when adding custom attributes since they are not validated at all; E.g. a `DomainName` attribute on a domain registration product will be checked neither for correctness nor availability.


Adding a Single Product
-----------------------

Adding a single product is done by a POST request with the following form fields:

* **ArticleNumber** &ndash; required
* **RenewalPeriod** &ndash; required depending on product. Format is [1-9]-(MONTH|YEAR), e.g. 1-MONTH
* **Quantity** &ndash; optional, defaults to 1
* **Campaign** &ndash; optional, campaign code
* **Next** &ndash; optional URL for where redirect should go after product has been added, e.g. /Account. Default is start of order flow.
* **CustomAttributes** &ndash; optional one or more, needs to be indexed see example below
* **CartCustomAttributes** &ndash; optional one or more, needs to be indexed see example below

An example of a hidden form adding a product with two custom attributes, a campaign code and next redirect URL:

    <form action="https://store.example.com/Select" method="post">

        <input type="hidden" name="ArticleNumber" value="HST-GLDY" />
        <input type="hidden" name="RenewalPeriod" value="1-YEAR" />    
        <input type="hidden" name="CustomAttributes[0].Name" value="Foo" />
        <input type="hidden" name="CustomAttributes[0].Value" value="Bar">
        <input type="hidden" name="CustomAttributes[1].Name" value="Fizz" />
        <input type="hidden" name="CustomAttributes[1].Value" value="Buzz">

        <input type="hidden" name="CartCustomAttributes[0].Name" value="ReferralCode" />
        <input type="hidden" name="CartCustomAttributes[0].Value" value="FooBar">
        <input type="hidden" name="Campaign" value="SUMMER-2015" />
        <input type="hidden" name="Next" value="/Account" />

        <button type="submit">Gold Package</button>

    </form>


Adding Multiple Products
------------------------

Adding multiple products is done similarly to how a single product is added, with the addition of indexing for each product to add. This works the same as for custom attributes, as can be seen in the following example. Note that the action is `/Select/Multi` in this case.

    <form action="https://store.example.com/Select/Multi" method="post">

        <input type="hidden" name="Items[0].ArticleNumber" value="HST-GLDY" />
        <input type="hidden" name="Items[0].RenewalPeriod" value="1-YEAR" />    
        <input type="hidden" name="Items[0].CustomAttributes[0].Name" value="Foo" />
        <input type="hidden" name="Items[0].CustomAttributes[0].Value" value="Bar">
        
        <input type="hidden" name="Items[1].ArticleNumber" value="XSV-MYADDON" />
        <input type="hidden" name="Items[1].RenewalPeriod" value="1-YEAR" />    
        <input type="hidden" name="Items[1].CustomAttributes[0].Name" value="Fizz" />
        <input type="hidden" name="Items[1].CustomAttributes[0].Value" value="Buzz">

        <input type="hidden" name="CartCustomAttributes[0].Name" value="ReferralCode" />
        <input type="hidden" name="CartCustomAttributes[0].Value" value="FooBar">
        <input type="hidden" name="Campaign" value="SUMMER-2015" />
        <input type="hidden" name="Next" value="/Account" />

        <button type="submit">Gold Package with MyAddon</button>

    </form>


Adding Only a Campaign Code
---------------------------

If you want to add just a campaign code you can do this either by POST request like the product examples (note the `/Select/Campaign` action):

    <form action="https://store.example.com/Select/Campaign" method="post">

        <input type="hidden" name="Campaign" value="SUMMER-2015" />
        <input type="hidden" name="Next" value="/Account" />

        <button type="submit">Summer campaign</button>

    </form>

You can optionally use a nicely formatted regular link on the form `/Campaign/{campaign}`, e.g. `<a href="https://store.example.com/Campaign/SUMMER-2015">Summer campaign</a>`

Here you can also use the optional `Next`: `<a href="https://store.example.com/Campaign/SUMMER-2015?Next=/Domains">Summer campaign</a>`


Adding Only Cart Custom Attributes
----------------------------------

If you want to add cart custom attributes only (and optional campaign code) you can do this by a POST request without any products (note the `/Select/Attrs` action), e.g. for setting a referral code:

    <form action="https://store.example.com/Select/Attrs" method="post">

        <input type="hidden" name="CartCustomAttributes[0].Name" value="ReferralCode" />
        <input type="hidden" name="CartCustomAttributes[0].Value" value="FooBar">
        <input type="hidden" name="Campaign" value="WINTER-2015" />
        <input type="hidden" name="Next" value="/Account" />

        <button type="submit">Order now!</button>

    </form>


Linking to a Domain Search
--------------------------

Since domain name registration requires a lot of validation checks, it is not recommended to directly add such products to the customers cart. Instead you can use a link or form that will start a search for the customer for a particular query.

Use a link like this: `<a href="https://store.example.com/Domains?query=foobar">Find foobar.com</a>`

Or a form like this:

    <form action="https://store.example.com/Domains" method="get">
        <input type="text" name="Query" value="" />
        <button type="submit">Search</button>
    </form>

This will start a domain search query on the `/Domains` page.
