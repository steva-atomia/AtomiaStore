/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, utils) {
    'use strict';

    var DomainsModelPrototype,
        CreateDomainsModel;


    /* Domains page prototype and factory */
        DomainsModelPrototype = {
        _DomainRegistrationActive: function _DomainRegistrationActive() {
            return this.QueryType() === 'domainreg';
        },

        _MoveDomainActive: function _MoveDomainActive() {
            return this.QueryType() === 'transfer';
        }
    };

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
