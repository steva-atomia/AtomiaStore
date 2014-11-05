Atomia.ViewModel.Domains = (function (ko, domainsApi) {
    var domainQueryType = ko.observable('register'),

        registerDomainQuery = ko.observable(),
        isRegisterQuery = ko.pureComputed(function () {
            return domainQueryType() === 'register';
        }),
        domainSearchResults = ko.observableArray(),
        searchingForDomains = ko.observable(false),

        transferDomainQuery = ko.observable(),
        isTransferQuery = ko.pureComputed(function () {
            return domainQueryType() === 'transfer';
        });

    function submit() {
        var data = parseDomainQuery(registerDomainQuery());

        domainsApi.findDomains(data, function (result) {
            // TODO: set results as registerDomainList
            // TODO: call domainsApi.checkStatus for each domain in list.
        });

        searchingForDomains = true;
    }

    function init(options) {
        console.log(options.allowedNumberOfDomains);
    }

    return {
        init: init,
        submit: submit,
        domainQueryType: domainQueryType,
        registerDomainQuery: registerDomainQuery,
        transferDomainQuery: transferDomainQuery,
        isRegisterQuery: isRegisterQuery,
        isTransferQuery: isTransferQuery
    }
} (ko, Atomia.DomainsApi));
