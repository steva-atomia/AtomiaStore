Atomia.ViewModel.DomainReg = (function (_, ko, domainsApi) {
    'use strict'

    var query = ko.observable(),
        results = ko.observableArray(),
        hasResults = ko.pureComputed(function () {
            return results().length > 0;
        });

    function submit() {
        domainsApi.findDomains(query().split('\n'), function (data) {
            results.removeAll();
            _.each(data, function (result) {
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
} (_, ko, Atomia.Domains));
