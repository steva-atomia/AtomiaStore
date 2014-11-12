Atomia.ViewModels.DomainReg = (function ($, ko, domainsApi) {
    var query = ko.observable(),
        results = ko.observableArray(),
        hasResults = ko.pureComputed(function () { return results().length > 0; });

    function submit() {
        domainsApi.findDomains(query().split('\n'), function (data) {
            results.removeAll();
            $.each(data, function (i, result) {
                results.push(result);
            });
        });
    }

    function init(parent) {
        parent.DomainReg = this;
    }

    return {
        query: query,
        hasResults: hasResults,
        results: results,
        submit: submit,
        init: init
    };
} (jQuery, ko, Atomia.Domains));


Atomia.ViewModels.DomainTransfer = (function (ko) {
    var query = ko.observable();

    function submit() {
        console.log("Transfer!");
    }

    function init(parent) {
        parent.DomainTransfer = this;
    }

    return {
        query: query,
        submit: submit,
        init: init
    };
} (ko));


Atomia.ViewModels.Domains = (function (ko) {
    'use strict'

    var queryType = ko.observable('domainreg'),
        domainRegActive = ko.pureComputed(function () { return queryType() === 'domainreg'; }),
        domainTransferActive = ko.pureComputed(function () { return queryType() === 'transfer'; });

    function init(parent) {
        parent.Domains = this;
    }

    return {
        queryType: queryType,
        domainRegActive: domainRegActive,
        domainTransferActive: domainTransferActive,
        init: init
    };
} (ko));
