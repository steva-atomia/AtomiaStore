/// <reference path="../../../../Scripts/underscore.js" />
/// <reference path="../../../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="atomia.utils.js" />

/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, utils) {
    'use strict';

    var DomainsModelPrototype,
        CreateDomainsModel;


    DomainsModelPrototype = {
        /** Checks if 'domainreg' is the current 'QueryType'. */
        _DomainRegistrationActive: function _DomainRegistrationActive() {
            return this.QueryType() === 'domainreg';
        },

        /** Checks if 'transfer' is the current 'QueryType'. */
        _MoveDomainActive: function _MoveDomainActive() {
            return this.QueryType() === 'transfer';
        }
    };

    /**
     * Create domains view model.
     * @param {Object|Function} extensions - Extensions to the default domain view model.
     * @returns the created domains view model.
     */
    CreateDomainsModel = function CreateDomainsModel(extensions) {
        var defaults;

        defaults = function (self) {
            return {
                QueryType: ko.observable('domainreg'),

                DomainRegistrationActive: ko.pureComputed(self._DomainRegistrationActive, self),
                MoveDomainActive: ko.pureComputed(self._MoveDomainActive, self)
            };
        };

        return utils.createViewModel(DomainsModelPrototype, defaults, extensions);
    };



    /* Module exports */
    _.extend(exports, {
        CreateDomainsModel: CreateDomainsModel
    });

})(Atomia.ViewModels, _, ko, Atomia.Utils);
