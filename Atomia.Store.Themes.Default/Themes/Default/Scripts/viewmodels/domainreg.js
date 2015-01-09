/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

Atomia.ViewModels.DomainReg = function (_, ko, domainsApi, cart) {
    'use strict';

    var Query = ko.observable(),
        IsLoadingResults = ko.observable(false),
        ShowMoreResults = ko.observable(false),

        PrimaryResults = ko.observableArray(),
        SecondaryResults = ko.observableArray(),

        HasResults = ko.pureComputed(function () {
            return PrimaryResults().length > 0 || SecondaryResults().length > 0;
        });

    function Submit() {
        IsLoadingResults(true);

        PrimaryResults.removeAll();
        SecondaryResults.removeAll();

        domainsApi.FindDomains(Query(), function (data) {
            _.each(data, function (result, index) {
                var cartItem,
                    primaryAttr;
                
                cartItem = cart.CreateCartItem({
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
                    equals: function (i1, i2) {
                        var i2DomainNameAttr = _.find(i2.CustomAttributes, function (i) { return i.Name === 'DomainName'; });

                        i2.DomainName = i2DomainNameAttr !== undefined ? i2DomainNameAttr.Value : undefined;

                        return i1.ArticleNumber === i2.ArticleNumber &&
                               i1.DomainName === i2.DomainName;
                    }
                });
                    
                primaryAttr = _.find(cartItem.CustomAttributes, function (i) { return i.Name === 'Premium'; });
                if (primaryAttr !== undefined && primaryAttr.Value === 'true') {
                    PrimaryResults.push(cartItem);
                }
                else {
                    SecondaryResults.push(cartItem);
                }
            });

            IsLoadingResults(false);
        });
    }

    function SetShowMoreResults() {
        ShowMoreResults(true);
    }

    return {
        HasResults: HasResults,
        IsLoadingResults: IsLoadingResults,
        PrimaryResults: PrimaryResults,
        Query: Query,
        SecondaryResults: SecondaryResults,
        SetShowMoreResults: SetShowMoreResults,
        ShowMoreResults: ShowMoreResults,
        Submit: Submit
    };
};


if (Atomia.ViewModels.Active !== undefined) {
    Atomia.ViewModels.Active.DomainReg = Atomia.ViewModels.DomainReg(_, ko, Atomia.Api.Domains, Atomia.ViewModels.Active.Cart);
}
