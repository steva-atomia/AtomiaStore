Atomia.ViewModel.DomainTransfer = (function (ko) {
    'use strict'

    var query = ko.observable();

    function submit() {
        console.log("Transfer!");
    }

    return {
        query: query,
        submit: submit
    };
} (ko));