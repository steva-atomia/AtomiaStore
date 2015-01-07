/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

Atomia.ViewModels.DomainReg = function (_, ko, domainsApi, cartApi, CartItem) {
    'use strict';

    var query = ko.observable(),
        isLoadingResults = ko.observable(false),
        showMoreResults = ko.observable(false),

        primaryResults = ko.observableArray(),
        secondaryResults = ko.observableArray(),

        hasResults = ko.pureComputed(function () {
            return primaryResults().length > 0 || secondaryResults().length > 0;
        });

    function submit() {
        isLoadingResults(true);

        primaryResults.removeAll();
        secondaryResults.removeAll();

        domainsApi.findDomains(query(), function (data) {
            _.each(data, function (result, index) {
                var item = new CartItem(cartApi),
                    domainParts,
                    primaryAttr;

                _.extend(item, result);

                item.LabelId = 'dmn' + index;
                item.DomainName = _.find(item.CustomAttributes, function (i) { return i.Name === 'DomainName'; }).Value;
                item.Status = _.find(item.CustomAttributes, function (i) { return i.Name === 'Status'; }).Value;
                item.Price = item.PricingVariants[0].Price;

                domainParts = item.DomainName.split('.');
                item.DomainNameSld = domainParts[0];
                item.DomainNameTld = domainParts[1];

                primaryAttr = _.find(item.CustomAttributes, function (i) { return i.Name === 'Premium'; })
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
        hasResults: hasResults,
        isLoadingResults: isLoadingResults,
        primaryResults: primaryResults,
        query: query,
        secondaryResults: secondaryResults,
        setShowMoreResults: setShowMoreResults,
        showMoreResults: showMoreResults,
        submit: submit
    };
};


if (Atomia.RootVM !== undefined) {
    Atomia.RootVM.DomainReg = Atomia.ViewModels.DomainReg(_, ko, Atomia.Domains, Atomia.Cart, Atomia.ViewModels.CartItem);
}
