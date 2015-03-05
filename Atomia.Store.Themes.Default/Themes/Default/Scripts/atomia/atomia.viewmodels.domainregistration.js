/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, utils, domainsApi, viewModelsApi) {
    'use strict';

    var DomainRegistrationItemPrototype,
        CreateDomainRegistrationItem,
        DomainRegistrationModelPrototype,
        CreateDomainRegistrationModel;

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
    DomainRegistrationModelPrototype = {
        Submit: function Submit() {
            this.IsLoadingResults(true);
            this.PrimaryResults.removeAll();
            this.SecondaryResults.removeAll();
            this.ShowMoreResults(false);
            this.SubmittedQuery(this.Query());
            this.NoResults(false);

            domainsApi.FindDomains(this.Query(), function (data) {
                var domainSearchId = data.DomainSearchId;

                if (data.Results.length === 0 && data.FinishSearch) {
                    this.NoResults(true);
                    this.IsLoadingResults(false);
                }
                else if (data.FinishSearch) {
                    this.UpdateResults(data.Result);
                }
                else {
                    domainsApi.CheckStatus(domainSearchId,
                        function (data) {
                            if (data.Results.length === 0 && data.FinishSearch) {
                                this.NoResults(true);
                                this.IsLoadingResults(false);
                            }
                            else {
                                this.UpdateResults(data.Results);
                            }
                        }.bind(this));
                }
            }.bind(this));
        },

        PrimaryResultsAreFinished: function () {
            if (this.PrimaryResults().length === 0) {
                return false;
            }

            return !_.any(this.PrimaryResults(), function(r) { 
                return r.Status === 'loading'; 
            });
        },

        UpdateResults: function UpdateResults(results) {
            var primaryResults = [],
                secondaryResults = [];

            _.each(results, function (result) {
                var item, primaryAttr;

                item = viewModelsApi.AddCartItemExtensions(this._Cart, this.CreateDomainRegistrationItem(result));

                primaryAttr = _.find(item.CustomAttributes, function (ca) {
                    return ca.Name === 'Premium';
                });

                if (primaryAttr !== undefined && primaryAttr.Value === 'true') {
                    primaryResults.push(item);
                }
                else {
                    secondaryResults.push(item);
                }
            }.bind(this));

            // Set all at once to avoid triggering bindings on each push.
            this.PrimaryResults(primaryResults);
            this.SecondaryResults(secondaryResults);

            if (this.PrimaryResultsAreFinished()) {
                this.IsLoadingResults(false);
            }
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

    CreateDomainRegistrationModel = function CreateDomainRegistrationModel(cart, extensions, itemExtensions) {
        var defaults;

        defaults = function (self) {
            return {
                _Cart: cart,
                CreateDomainRegistrationItem: _.partial(CreateDomainRegistrationItem, itemExtensions || {}),

                SubmittedQuery: ko.observable(),
                Query: ko.observable(),
                IsLoadingResults: ko.observable(false),
                ShowMoreResults: ko.observable(false),
                PrimaryResults: ko.observableArray(),
                SecondaryResults: ko.observableArray(),
                NoResults: ko.observable(false),

                HasResults: ko.pureComputed(self._HasResults, self)
            };
        };

        return utils.createViewModel(DomainRegistrationModelPrototype, defaults, extensions);
    };



    /* Module exports */
    _.extend(exports, {
        CreateDomainRegistrationModel: CreateDomainRegistrationModel
    });

})(Atomia.ViewModels, _, ko, Atomia.Utils, Atomia.Api.Domains, Atomia.ViewModels);
