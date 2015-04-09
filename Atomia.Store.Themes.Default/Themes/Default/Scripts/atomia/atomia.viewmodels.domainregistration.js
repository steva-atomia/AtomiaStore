/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, utils, domainsApi, viewModelsApi) {
    'use strict';

    /**
     * Create domain registration item view model
     * @param {Object} instance - the object to create a domain registration item from.
     */
    function DomainRegistrationItem(instance) {
        var self = this;
        var domainParts = instance.DomainName.split('.');
        var primaryAttr = _.find(instance.CustomAttributes, function (ca) {
            return ca.Name === 'Premium';
        });

        self.isPrimary = primaryAttr !== undefined && primaryAttr.Value === 'true';
        self.uniqueId = _.uniqueId('dmn');
        self.domainNameSld = domainParts[0];
        self.domainNameTld = domainParts[1];
        self.price = instance.PricingVariants[0].Price;
        self.renewalPeriod = instance.PricingVariants[0].RenewalPeriod

        // TODO: Remove this when change to using all lower case is done.
        self.Price = self.price;

        /** 
         * Checks if domain registration item is equal to other item based on article number and domain name.
         * @param {Object} other - The item to compare to
         * @returns {boolean} whether the items are equal or not.
         */
        self.equals = function equals(other) {
            var otherDomainNameAttr = _.find(other.CustomAttributes, function (ca) {
                return ca.Name === 'DomainName';
            });
            var otherDomainName = otherDomainNameAttr !== undefined ? otherDomainNameAttr.Value : undefined;

            return self.ArticleNumber === other.ArticleNumber && self.DomainName === otherDomainName;
        };

        _.extend(self, instance);
    }


    /** 
     * Create domain registration view model.
     * @param {Object} cart - instance of cart to add or remove items to.
     */
    function DomainRegistrationModel(cart) {
        var self = this;

        self._cart = cart;

        self.submittedQuery = ko.observable();
        self.query = ko.observable();
        self.isLoadingResults = ko.observable(false);
        self.showMoreResults = ko.observable(false);
        self.primaryResults = ko.observableArray();
        self.secondaryResults = ko.observableArray();
        self.noResults = ko.observable(false);

        self.hasResults = ko.pureComputed(function () {
            return self.primaryResults().length > 0 || self.secondaryResults().length > 0;
        });

        self.createDomainRegistrationItem = function(instance) {
            return new DomainRegistrationItem(instance);
        }

        /** Submit a domain search query. */
        self.submit = function submit() {
            self.isLoadingResults(true);
            self.primaryResults.removeAll();
            self.secondaryResults.removeAll();
            self.showMoreResults(false);
            self.submittedQuery(self.query());
            self.noResults(false);
            
            domainsApi.findDomains(self.query(), function (data) {
                var domainSearchId = data.DomainSearchId;

                if (data.Results.length === 0 && data.FinishSearch) {
                    self.noResults(true);
                    self.isLoadingResults(false);
                }
                else if (data.FinishSearch) {
                    self.updateResults(data.Result);
                }
                else {
                    domainsApi.checkStatus(domainSearchId,
                        function (data) {
                            if (data.Results.length === 0 && data.FinishSearch) {
                                self.noResults(true);
                                self.isLoadingResults(false);
                            }
                            else {
                                self.updateResults(data.Results);
                            }
                        });
                }
            });
        };
        
        /** Primary TLD search results have finished loading. */
        self.primaryResultsAreFinished = function primaryResultsAreFinished() {
            if (self.primaryResults().length === 0) {
                return false;
            }

            return !_.any(self.primaryResults(), function (r) {
                return r.Status === 'loading';
            });
        };

        /** 
         * Create domain registration items from primary and secondary TLD results and update view model.
         * @param {Array} results - The domain search results.
         */
        self.updateResults = function updateResults(results) {
            var primaryResults = [],
                secondaryResults = [];

            _.each(results, function (result) {
                var item = viewModelsApi.addCartItemExtensions(self._cart, self.createDomainRegistrationItem(result));

                if (item.isPrimary) {
                    primaryResults.push(item);
                }
                else {
                    secondaryResults.push(item);
                }
            });

            // Set all at once to avoid triggering bindings on each push.
            self.primaryResults(primaryResults);
            self.secondaryResults(secondaryResults);

            if (self.primaryResultsAreFinished()) {
                self.isLoadingResults(false);
            }
        };

        /** Show more results */
        self.setShowMoreResults = function setShowMoreResults() {
            self.showMoreResults(true);
        };

        /**
         * Get template name to use for rendering 'item': 'domainregistration-{primary|secondary}-{available|taken|<status>}
         * @param {Object} item - The item to select template name for
         * @returns the template name.
         */
        self.getTemplateName = function getTemplateName(item) {
            
            if (item.isPrimary && item.Status === 'available') {
                return 'domainregistration-primary-available';
            }
            else if (item.isPrimary) {
                return 'domainregistration-primary-taken';
            }
            else {
                return 'domainregistration-secondary-' + item.Status;
            }
        };
    };



    /* Module exports */
    _.extend(exports, {
        DomainRegistrationModel: DomainRegistrationModel
    });

})(Atomia.ViewModels, _, ko, Atomia.Utils, Atomia.Api.Domains, Atomia.ViewModels);
