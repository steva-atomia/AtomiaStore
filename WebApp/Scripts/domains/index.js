PageViewModel.DomainsViewModel = (function (ko) {
    var domainQueryType = ko.observable('register');

    var isRegisterQuery = ko.pureComputed(function() {
        return domainQueryType() === 'register';
    });

    var isTransferQuery = ko.pureComputed(function() {
        return domainQueryType() === 'transfer';
    });

    return {
        domainQueryType: domainQueryType,
        isRegisterQuery: isRegisterQuery,
        isTransferQuery: isTransferQuery
    }
}(ko));
