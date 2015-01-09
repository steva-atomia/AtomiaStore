/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

Atomia.ViewModels.Domains = function (ko) {
    'use strict';

    var QueryType = ko.observable('domainreg'),
        DomainRegActive = ko.pureComputed(function () {
            return QueryType() === 'domainreg';
        }),
        DomainTransferActive = ko.pureComputed(function () {
            return QueryType() === 'transfer';
        });

    return {
        QueryType: QueryType,
        DomainRegActive: DomainRegActive,
        DomainTransferActive: DomainTransferActive
    };
};

if (Atomia.ViewModels.Active !== undefined) {
    Atomia.ViewModels.Active.Domains = Atomia.ViewModels.Domains(ko);
}
