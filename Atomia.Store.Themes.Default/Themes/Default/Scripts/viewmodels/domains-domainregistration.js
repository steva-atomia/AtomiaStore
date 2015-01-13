/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

Atomia.ViewModels.DomainRegistration = function (_, ko, domainsApi, cart) {
    'use strict';

    var Query = ko.observable(),
        IsLoadingResults = ko.observable(false),
        ShowMoreResults = ko.observable(false),

        PrimaryResults = ko.observableArray(),
        SecondaryResults = ko.observableArray(),

        HasResults = ko.pureComputed(function () {
            return PrimaryResults().length > 0 || SecondaryResults().length > 0;
        });

    function DomainRegistrationItem(itemData, index) {
        var domainName = _.find(itemData.CustomAttributes, function (ca) {
                return ca.Name === 'DomainName';
            }).Value,
            domainParts = domainName.split('.');

        _.extend(this, itemData);

        this.LabelId = 'dmn' + index;
        this.DomainName = domainName;
        this.DomainNameSld = domainParts[0];
        this.DomainNameTld = domainParts[1];
        this.Price = this.PricingVariants[0].Price;
        this.Status = _.find(this.CustomAttributes, function (ca) {
            return ca.Name === 'Status';
        }).Value;

        this.Equals = this._Equals.bind(this);
    }

    DomainRegistrationItem.prototype._Equals = function (other) {
        var otherDomainNameAttr = _.find(other.CustomAttributes, function (ca) { 
            return ca.Name === 'DomainName'; 
        }),
            otherDomainName = otherDomainNameAttr !== undefined ? otherDomainNameAttr.Value : undefined;

        return this.ArticleNumber === other.ArticleNumber && this.DomainName === otherDomainName;
    };

    function Submit() {
        IsLoadingResults(true);

        PrimaryResults.removeAll();
        SecondaryResults.removeAll();

        domainsApi.FindDomains(Query(), function (data) {
            _.each(data, function (result, index) {
                var item = new DomainRegistrationItem(result, index),
                    cartItem = cart.CreateCartItem(item),
                    primaryAttr = _.find(cartItem.CustomAttributes, function (ca) {
                        return ca.Name === 'Premium';
                    });

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
    Atomia.ViewModels.Active.DomainRegistration = Atomia.ViewModels.DomainRegistration(_, ko, Atomia.Api.Domains, Atomia.ViewModels.Active.Cart);
}
