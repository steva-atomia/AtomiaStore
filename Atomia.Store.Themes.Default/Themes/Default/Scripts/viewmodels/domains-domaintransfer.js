/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

Atomia.ViewModels.DomainTransfer = function (ko) {
    'use strict';

    var Query = ko.observable();

    function Submit() {
        console.log('Transfer!');
    }

    return {
        Query: Query,
        Submit: Submit
    };
};

if (Atomia.ViewModels.Active !== undefined) {
    Atomia.ViewModels.Active.DomainTransfer = Atomia.ViewModels.DomainTransfer(ko);
}
