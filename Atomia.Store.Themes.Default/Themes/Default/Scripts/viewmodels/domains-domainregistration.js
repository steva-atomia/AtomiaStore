/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (module, _) {
    'use strict';

    var DomainRegistrationItem = function DomainRegistrationItem(itemData) {
        var domainName = _.find(itemData.CustomAttributes, function (ca) {
            return ca.Name === 'DomainName';
        }).Value,
            domainParts = domainName.split('.');

        _.extend(this, itemData);

        this.UniqueId = _.uniqueId('dmn');
        this.DomainName = domainName;
        this.DomainNameSld = domainParts[0];
        this.DomainNameTld = domainParts[1];
        this.Price = this.PricingVariants[0].Price;
        this.Status = _.find(this.CustomAttributes, function (ca) {
            return ca.Name === 'Status';
        }).Value;

        this.Equals = this._Equals.bind(this);
    };

    DomainRegistrationItem.prototype = {
        _Equals: function (other) {
            var otherDomainNameAttr = _.find(other.CustomAttributes, function (ca) {
                    return ca.Name === 'DomainName';
                }),
                otherDomainName = otherDomainNameAttr !== undefined ? otherDomainNameAttr.Value : undefined;

            return this.ArticleNumber === other.ArticleNumber && this.DomainName === otherDomainName;
        }
    };

    module.DomainRegistrationItem = DomainRegistrationItem;

})(Atomia.ViewModels, _);


(function (module, _, ko, domainsApi) {
    'use strict';

    var DomainRegistration = function DomainRegistration(MakeCartItem, DomainRegistrationItem) {
        this._MakeCartItem = MakeCartItem;
        this._DomainRegistrationItem = DomainRegistrationItem;

        this.Query = ko.observable();
        this.IsLoadingResults = ko.observable(false);
        this.ShowMoreResults = ko.observable(false);
        this.PrimaryResults = ko.observableArray();
        this.SecondaryResults = ko.observableArray();

        this.HasResults = ko.pureComputed(this._HasResults, this);
        this.Submit = this._Submit.bind(this);
        this.SetShowMoreResults = this._SetShowMoreResults.bind(this);
    };
    
    DomainRegistration.prototype = {
        _HasResults: function () {
            return this.PrimaryResults().length > 0 || this.SecondaryResults().length > 0;
        },

        _Submit: function () {
            var self = this;

            this.IsLoadingResults(true);

            this.PrimaryResults.removeAll();
            this.SecondaryResults.removeAll();

            domainsApi.FindDomains(this.Query(), function (data) {
                _.each(data, function (result) {
                    var item = new self._DomainRegistrationItem(result),
                        cartItem = self._MakeCartItem(item),
                        primaryAttr = _.find(cartItem.CustomAttributes, function (ca) {
                            return ca.Name === 'Premium';
                        });

                    if (primaryAttr !== undefined && primaryAttr.Value === 'true') {
                        self.PrimaryResults.push(cartItem);
                    }
                    else {
                        self.SecondaryResults.push(cartItem);
                    }
                });

                self.IsLoadingResults(false);
            });
        },

        _SetShowMoreResults: function() {
            this.ShowMoreResults(true);
        }
    };

    module.DomainRegistration = DomainRegistration;

})(Atomia.ViewModels, _, ko, Atomia.Api.Domains);
