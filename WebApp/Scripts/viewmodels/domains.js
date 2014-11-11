Atomia.ViewModel.Domains = (function (ko, domainsApi) {
    var queryType = ko.observable('find'),

        find = {
            query: ko.observable(),
            results: ko.observableArray(),
            active: ko.pureComputed(function () { return queryType() === 'find'; }),
            submit: function () {
                console.log("Bloop from find!");
            }
        },

        transfer = {
            query: ko.observable(),
            active: ko.pureComputed(function () { return queryType() === 'transfer'; }),
            submit: function () {
                console.log("Bloop from transfer!");
            }
        };

    function init(options) {

    }

    return {
        init: init,
        queryType: queryType,
        find: find,
        transfer: transfer
    };
} (ko, Atomia.DomainsApi));
