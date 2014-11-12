Atomia.ViewModel.Domains = (function (ko) {
    'use strict'

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
} (ko));
