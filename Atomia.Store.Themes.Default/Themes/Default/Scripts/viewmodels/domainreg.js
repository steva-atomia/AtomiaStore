/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

Atomia.ViewModels.DomainReg = function (_, ko, domainsApi, cart) {
    'use strict';

    var query = ko.observable(),
        primaryResults = ko.observableArray(),
        secondaryResults = ko.observableArray(),
        hasResults = ko.pureComputed(function () {
            return primaryResults().length > 0 || secondaryResults().length > 0;
        }),
        isLoadingResults = ko.observable(false),
        showMoreResults = ko.observable(false);

    function Item(item) {
        var theItem = this;
        theItem.IsInCart = ko.observable(false);
        theItem.IsInCart.subscribe(function (newValue) {
            newValue ? theItem.addToCart() : theItem.removeFromCart();
        });

        _.extend(this, item);
    }

    Item.prototype.addToCart = function () {
        var theItem = this;
        cart.addItem(this, function () {
            theItem.IsInCart(true);
        });
    };

    Item.prototype.removeFromCart = function () {
        var theItem = this;
        cart.removeItem(this, function () {
            theItem.IsInCart(false);
        });
    };

    function submit() {
        isLoadingResults(true);

        primaryResults.removeAll();
        secondaryResults.removeAll();

        domainsApi.findDomains(query(), function (data) {
            _.each(data, function (result, index) {
                var item = new Item(result),
                    domainParts,
                    primaryAttr = _.find(item.CustomAttributes, function (i) { return i.Name === 'Premium'; });

                item.Id = 'dmn' + index;

                // Make some properties more easily accessible.
                item.DomainName = _.find(item.CustomAttributes, function (i) { return i.Name === 'DomainName'; }).Value;
                item.Status = _.find(item.CustomAttributes, function (i) { return i.Name === 'Status'; }).Value;
                item.Price = item.PricingVariants[0].Price;

                domainParts = item.DomainName.split('.');
                item.DomainNameSld = domainParts[0];
                item.DomainNameTld = domainParts[1];

                if (primaryAttr !== undefined && primaryAttr.Value === 'true') {
                    primaryResults.push(item);
                }
                else {
                    secondaryResults.push(item);
                }
            });

            isLoadingResults(false);
        });
    }

    function setShowMoreResults() {
        showMoreResults(true);
    }

    return {
        query: query,
        primaryResults: primaryResults,
        secondaryResults: secondaryResults,
        hasResults: hasResults,
        isLoadingResults: isLoadingResults,
        submit: submit,
        setShowMoreResults: setShowMoreResults,
        showMoreResults: showMoreResults
    };
};


if (Atomia.RootVM !== undefined) {
    Atomia.RootVM.DomainReg = Atomia.ViewModels.DomainReg(_, ko, Atomia.Domains, Atomia.RootVM.Cart);
}
