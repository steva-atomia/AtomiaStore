Atomia.ViewModel.Domains = (function (ko) {
    var domainQueryType = ko.observable('register'),
        registerDomainQuery = ko.observable(),
        transferDomainQuery = ko.observable(),
        isRegisterQuery = ko.pureComputed(function () {
            return domainQueryType() === 'register';
        }),
        isTransferQuery = ko.pureComputed(function () {
            return domainQueryType() === 'transfer';
        });

    function submit() {

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
} (ko));
