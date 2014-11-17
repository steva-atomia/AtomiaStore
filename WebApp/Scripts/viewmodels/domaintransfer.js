var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};

Atomia.ViewModels.DomainTransfer = function (ko) {
    'use strict';

    var query = ko.observable();

    function submit() {
        console.log('Transfer!');
    }

    return {
        query: query,
        submit: submit
    };
};

if (Atomia.RootVM !== undefined) {
    Atomia.RootVM.DomainTransfer = Atomia.ViewModels.DomainTransfer(ko);
}
