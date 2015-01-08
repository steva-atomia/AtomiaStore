/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

Atomia.ViewModels.DomainReg = function (_, ko, domainsApi, cartApi) {
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
                var cartItem,
                    primaryAttr;
                
                cartItem = cartApi.createCartItemViewModel({
                    init: function (item) {
                        var domainParts;

                        _.extend(item, result);

                        item.LabelId = 'dmn' + index;
                        item.DomainName = _.find(item.CustomAttributes, function (i) { return i.Name === 'DomainName'; }).Value;
                        item.Status = _.find(item.CustomAttributes, function (i) { return i.Name === 'Status'; }).Value;
                        item.Price = item.PricingVariants[0].Price;

                        domainParts = item.DomainName.split('.');
                        item.DomainNameSld = domainParts[0];
                        item.DomainNameTld = domainParts[1];
                    },
                    description: function (i) {
                        return i.DomainName;
                    },
                    equals: function (i1, i2) {
                        var i2DomainNameAttr = _.find(i2.CustomAttributes, function (i) { return i.Name === 'DomainName'; });

                        i2.DomainName = i2DomainNameAttr !== undefined ? i2DomainNameAttr.Value : undefined;

                        return i1.ArticleNumber === i2.ArticleNumber &&
                               i1.DomainName === i2.DomainName;
                    }
                });
                    
                primaryAttr = _.find(cartItem.CustomAttributes, function (i) { return i.Name === 'Premium'; });
                if (primaryAttr !== undefined && primaryAttr.Value === 'true') {
                    primaryResults.push(cartItem);
                }
                else {
                    secondaryResults.push(cartItem);
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
    Atomia.RootVM.DomainReg = Atomia.ViewModels.DomainReg(_, ko, Atomia.Domains, Atomia.Cart);
}
