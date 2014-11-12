Atomia.ViewModel.DomainReg = (function ($, ko, domainsApi) {
    'use strict'

    var query = ko.observable(),
        results = ko.observableArray(),
        hasResults = ko.pureComputed(function () {
            return results().length > 0;
        });

    function submit() {
        domainsApi.findDomains(query().split('\n'), function (data) {
            results.removeAll();
            $.each(data, function (i, result) {
                results.push(result);
            });
        });
    }

    return {
        query: query,
        results: results,
        hasResults: hasResults,
        submit: submit
    };
} (jQuery, ko, Atomia.Domains));
