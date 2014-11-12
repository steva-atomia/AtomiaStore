Atomia.ViewModel.Domains = (function ($, ko, domainsApi) {
    'use strict'

    var queryType = ko.observable('find'),

        find = (function () {
            var query = ko.observable(),
                results = ko.observableArray(),
                active = ko.pureComputed(function () { return queryType() === 'find'; }),
                hasResults = ko.pureComputed(function () { return results().length > 0; });

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
                hasResults: hasResults,
                results: results,
                active: active,
                submit: submit
            };
        } ()),

        transfer = (function () {
            var query = ko.observable(),
                active = ko.pureComputed(function () { return queryType() === 'transfer'; });

            function submit() {
                console.log("Transfer!");
            };

            return {
                query: query,
                active: active,
                submit: submit
            };
        } ());

    function init(options) {

    }

    return {
        init: init,
        queryType: queryType,
        find: find,
        transfer: transfer
    };
} (jQuery, ko, Atomia.Domains));
