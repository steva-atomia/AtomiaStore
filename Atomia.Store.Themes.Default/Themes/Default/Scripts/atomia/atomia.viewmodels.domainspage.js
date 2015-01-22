/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, utils) {
    'use strict';

    var CreateDomainsPage,
        CreateDomainsPagePrototype;



    /* Domains page prototype and factory */
    CreateDomainsPagePrototype = {
        _DomainRegActive: function _DomainRegActive() {
            return this.QueryType() === 'domainreg';
        },

        _DomainTransferActive: function _DomainTransferActive() {
            return this.QueryType() === 'transfer';
        }
    };

    CreateDomainsPage = function CreateDomainsPage(extensions) {
        var defaults;

        defaults = function (self) {
            return {
                QueryType: ko.observable('domainreg'),

                DomainRegActive: ko.pureComputed(self._DomainRegActive, self),
                DomainTransferActive: ko.pureComputed(self._DomainTransferActive, self)
            };
        };

        return utils.createViewModel(CreateDomainsPagePrototype, defaults, extensions);
    };



    /* Module exports */
    _.extend(exports, {
        CreateDomainsPage: CreateDomainsPage
    });

})(Atomia.ViewModels, _, ko, Atomia.Utils);
