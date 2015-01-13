/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (module, ko) {
    'use strict';

    var Domains = function Domains() {
        this.QueryType = ko.observable('domainreg');

        this.DomainRegActive = ko.pureComputed(this.DomainRegActive, this);
        this.DomainTransferActive = ko.pureComputed(this.DomainTransferActive, this);
    };

    Domains.prototype = {
        DomainRegActive: function () {
            return this.QueryType() === 'domainreg';
        },
        DomainTransferActive: function () {
            return this.QueryType() === 'transfer';
        }
    };

    module.Domains = Domains;

})(Atomia.ViewModels, ko);
