var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};

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

if (Atomia.RootVM !== undefined) {
    Atomia.RootVM.Domains = Atomia.ViewModels.Domains(ko);
}
