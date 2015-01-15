/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */


(function (module, _, ko, domainsApi) {
    'use strict';

    var DomainRegistrationItem, DomainRegistration;

    /* DomainRegistrationItem and prototype */
    DomainRegistrationItem = function DomainRegistrationItem(itemData) {
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
        this.RenewalPeriod = this.PricingVariants[0].RenewalPeriod;
        this.Status = _.find(this.CustomAttributes, function (ca) {
            return ca.Name === 'Status';
        }).Value;

        _.bindAll(this, 'Equals');
    };

    DomainRegistrationItem.prototype = {
        Equals: function (other) {
            var otherDomainNameAttr = _.find(other.CustomAttributes, function (ca) {
                return ca.Name === 'DomainName';
            }),
                otherDomainName = otherDomainNameAttr !== undefined ? otherDomainNameAttr.Value : undefined;

            return this.ArticleNumber === other.ArticleNumber && this.DomainName === otherDomainName;
        }
    };



    /* DomainRegistration and prototype */
    DomainRegistration = function DomainRegistration() {
        this.DomainRegistrationItem = DomainRegistrationItem;

        this.Query = ko.observable();
        this.IsLoadingResults = ko.observable(false);
        this.ShowMoreResults = ko.observable(false);
        this.PrimaryResults = ko.observableArray();
        this.SecondaryResults = ko.observableArray();

        this.HasResults = ko.pureComputed(this.HasResults, this);
        _.bindAll(this, 'Init', 'Submit', 'SetShowMoreResults', 'GetTemplateName');
    };
    
    DomainRegistration.prototype = {
        HasResults: function () {
            return this.PrimaryResults().length > 0 || this.SecondaryResults().length > 0;
        },

        Init: function (cart) {
            this._ExtendWithCartProperties = cart.ExtendWithCartProperties;
        },

        Submit: function () {
            var self = this;

            this.IsLoadingResults(true);

            this.PrimaryResults.removeAll();
            this.SecondaryResults.removeAll();

            domainsApi.FindDomains(this.Query(), function (data) {
                _.each(data, function (result) {
                    var item, primaryAttr;
                    
                    item = self._ExtendWithCartProperties(new self.DomainRegistrationItem(result));
                    primaryAttr = _.find(item.CustomAttributes, function (ca) {
                        return ca.Name === 'Premium';
                    });

                    if (primaryAttr !== undefined && primaryAttr.Value === 'true') {
                        self.PrimaryResults.push(item);
                    }
                    else {
                        self.SecondaryResults.push(item);
                    }
                });

                self.IsLoadingResults(false);
            });
        },

        SetShowMoreResults: function() {
            this.ShowMoreResults(true);
        },

        GetTemplateName: function (item) {
            var primaryAttr = _.find(item.CustomAttributes, function (ca) {
                return ca.Name === 'Premium';
            }),
                primary = primaryAttr !== undefined && primaryAttr.Value === 'true',
                displayType = 'domainregistration-';

            displayType += primary ? 'primary-' : 'secondary-';

            if (primary) {
                displayType += item.Status === 'available' ? 'available' : 'taken';
            }
            else {
                displayType += item.Status;
            }

            return displayType;
        },
    };



    /* Export models */
    module.DomainRegistrationItem = DomainRegistrationItem;
    module.DomainRegistration = DomainRegistration;

})(Atomia.ViewModels, _, ko, Atomia.Api.Domains);
