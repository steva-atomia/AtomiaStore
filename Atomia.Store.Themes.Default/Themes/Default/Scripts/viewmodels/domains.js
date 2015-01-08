/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

Atomia.ViewModels.Domains = function (ko) {
    'use strict';

    var queryType = ko.observable('domainreg'),
        domainRegActive = ko.pureComputed(function () {
            return queryType() === 'domainreg';
        }),
        domainTransferActive = ko.pureComputed(function () {
            return queryType() === 'transfer';
        });

    return {
        queryType: queryType,
        domainRegActive: domainRegActive,
        domainTransferActive: domainTransferActive
    };
};

if (Atomia.ViewModels.Active !== undefined) {
    Atomia.ViewModels.Active.Domains = Atomia.ViewModels.Domains(ko);
}
