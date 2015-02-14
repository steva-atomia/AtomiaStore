/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, utils, domainsApi, viewModelsApi) {
    'use strict';

    var DomainRegistrationItemPrototype,
        CreateDomainRegistrationItem,
        DomainRegistrationPrototype,
        CreateDomainRegistration;

    /* Domain registration item protype and factory */
    DomainRegistrationItemPrototype = {
        Equals: function Equals(other) {
            var otherDomainNameAttr = _.find(other.CustomAttributes, function (ca) {
                    return ca.Name === 'DomainName';
                }),
                otherDomainName = otherDomainNameAttr !== undefined ? otherDomainNameAttr.Value : undefined;

            return this.ArticleNumber === other.ArticleNumber && this.DomainName === otherDomainName;
        }
    };

    CreateDomainRegistrationItem = function CreateDomainRegistrationItem(extensions, instance) {
        var domainParts;

        domainParts = instance.DomainName.split('.');

        return utils.createViewModel(DomainRegistrationItemPrototype, {
            UniqueId: _.uniqueId('dmn'),
            DomainNameSld: domainParts[0],
            DomainNameTld: domainParts[1],
            Price: instance.PricingVariants[0].Price,
            RenewalPeriod: instance.PricingVariants[0].RenewalPeriod
        }, instance, extensions);
    };

    /* Domain registration prototype and factory */
    DomainRegistrationPrototype = {
        Submit: function Submit() {
            this.IsLoadingResults(true);

            this.PrimaryResults.removeAll();
            this.SecondaryResults.removeAll();

            domainsApi.FindDomains(this.Query(), function (data) {
                _.each(data, function (result) {
                    var item, primaryAttr;
                    
                    item = viewModelsApi.AddCartExtensions(this._Cart, this.CreateDomainRegistrationItem(result));

                    primaryAttr = _.find(item.CustomAttributes, function (ca) {
                        return ca.Name === 'Premium';
                    });

                    if (primaryAttr !== undefined && primaryAttr.Value === 'true') {
                        this.PrimaryResults.push(item);
                    }
                    else {
                        this.SecondaryResults.push(item);
                    }
                }.bind(this));

                this.IsLoadingResults(false);
            }.bind(this));
        },

        SetShowMoreResults: function SetShowMoreResults() {
            this.ShowMoreResults(true);
        },

        GetTemplateName: function GetTemplateName(item) {
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

        _HasResults: function _HasResults() {
            return this.PrimaryResults().length > 0 || this.SecondaryResults().length > 0;
        }
    };

    CreateDomainRegistration = function CreateDomainRegistration(cart, extensions, itemExtensions) {
        var defaults;

        defaults = function (self) {
            return {
                _Cart: cart,
                CreateDomainRegistrationItem: _.partial(CreateDomainRegistrationItem, itemExtensions || {}),

                Query: ko.observable(),
                IsLoadingResults: ko.observable(false),
                ShowMoreResults: ko.observable(false),
                PrimaryResults: ko.observableArray(),
                SecondaryResults: ko.observableArray(),

                HasResults: ko.pureComputed(self._HasResults, self)
            };
        };

        return utils.createViewModel(DomainRegistrationPrototype, defaults, extensions);
    };



    /* Module exports */
    _.extend(exports, {
        CreateDomainRegistration: CreateDomainRegistration
    });

})(Atomia.ViewModels, _, ko, Atomia.Utils, Atomia.Api.Domains, Atomia.ViewModels);
