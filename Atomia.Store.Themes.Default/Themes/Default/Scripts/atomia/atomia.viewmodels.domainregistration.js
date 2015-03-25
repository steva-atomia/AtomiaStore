/// <reference path="../../../../Scripts/underscore.js" />
/// <reference path="../../../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="atomia.utils.js" />
/// <reference path="atomia.api.domains.js" />
/// <reference path="atomia.viewmodels.cart.js" />

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
    
    DomainRegistrationItemPrototype = {
        /** 
         * Checks if domain registration item is equal to other item based on article number and domain name.
         * @param {Object} other - The item to compare to
         * @returns {boolean} whether the items are equal or not.
         */
        Equals: function Equals(other) {
            var otherDomainNameAttr = _.find(other.CustomAttributes, function (ca) {
                    return ca.Name === 'DomainName';
                }),
                otherDomainName = otherDomainNameAttr !== undefined ? otherDomainNameAttr.Value : undefined;

            return this.ArticleNumber === other.ArticleNumber && this.DomainName === otherDomainName;
        }
    };

    /**
     * Create domain registration item view model
     * @param {Object|Function} extensions - extensions to the default domain registration item view model
     * @param {Object} instance - the object to create a domain registration item from.
     * @returns the created domain registration item.
     */
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
    
    
    DomainRegistrationModelPrototype = {
        
        /**
         * Submit a domain search query.
         */
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
        
        /** Primary TLD search results have finished loading. */
        PrimaryResultsAreFinished: function () {
            if (this.PrimaryResults().length === 0) {
                return false;
            }

            return !_.any(this.PrimaryResults(), function(r) { 
                return r.Status === 'loading'; 
            });
        },

        /** 
         * Create domain registration items from primary and secondary TLD results and update view model.
         * @param {Array} results - The domain search results.
         */
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

        /** Show more results */
        SetShowMoreResults: function SetShowMoreResults() {
            this.ShowMoreResults(true);
        },

        /**
         * Get template name to use for rendering 'item': 'domainregistration-{primary|secondary}-{available|taken|<status>}
         * @param {Object} item - The item to select template name for
         * @returns the template name.
         */
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

        /** Check if there are domain search results on the view model. */
        _HasResults: function _HasResults() {
            return this.PrimaryResults().length > 0 || this.SecondaryResults().length > 0;
        }
    };

    /** 
     * Create domain registration view model.
     * @param {Object} cart - instance of cart to add or remove items to.
     * @param {Object|Function} extensions - Extensions to the default domain registration view model.
     * @param {Object|Function} itemExtensions - Extensions to the default domain registration item view model.
     */
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
