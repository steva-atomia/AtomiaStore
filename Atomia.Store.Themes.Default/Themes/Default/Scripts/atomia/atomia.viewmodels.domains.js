/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, utils) {
    'use strict';

    var CreateDomains,
        CreateDomainsPrototype;



    /* Domains page prototype and factory */
    CreateDomainsPrototype = {
        _DomainRegistrationActive: function _DomainRegistrationActive() {
            return this.QueryType() === 'domainreg';
        },

        _MoveDomainActive: function _MoveDomainActive() {
            return this.QueryType() === 'transfer';
        }
    };

    CreateDomains = function CreateDomains(extensions) {
        var defaults;

        defaults = function (self) {
            return {
                QueryType: ko.observable('domainreg'),

                DomainRegistrationActive: ko.pureComputed(self._DomainRegistrationActive, self),
                MoveDomainActive: ko.pureComputed(self._MoveDomainActive, self)
            };
        };

        return utils.createViewModel(CreateDomainsPrototype, defaults, extensions);
    };



    /* Module exports */
    _.extend(exports, {
        CreateDomains: CreateDomains
    });

})(Atomia.ViewModels, _, ko, Atomia.Utils);
