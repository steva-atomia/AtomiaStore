/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, utils) {
    'use strict';

    /** Create domains view model. */
    function DomainsModel(extensions) {
        var self = this;

        self.queryType = ko.observable('domainreg');
        
        /** Checks if 'domainreg' is the current 'queryType'. */
        self.domainRegistrationActive = ko.pureComputed(function() {
            return self.queryType() === 'domainreg';
        });

        /** Checks if 'transfer' is the current 'queryType'. */
        self.moveDomainActive = ko.pureComputed(function () {
            return self.queryType() === 'transfer';
        });
    }


    /* Module exports */
    _.extend(exports, {
        DomainsModel: DomainsModel
    });

})(Atomia.ViewModels, _, ko, Atomia.Utils);
