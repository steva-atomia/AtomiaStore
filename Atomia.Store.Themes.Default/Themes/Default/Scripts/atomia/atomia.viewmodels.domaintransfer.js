/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, utils) {
    'use strict';

    var DomainTransferPrototype,
        CreateDomainTransfer;
        


    /* Domain transfer prototype and factory */
    DomainTransferPrototype = {
        Submit: function Submit() {
            console.log('Transfer!');
        }
    };

    CreateDomainTransfer = function CreateDomainTransfer(extensions) {
        return utils.createViewModel(DomainTransferPrototype, {
            Query: ko.observable()
        }, extensions);
    };



    /* Module exports */
    _.extend(exports, {
        CreateDomainTransfer: CreateDomainTransfer
    });

})(Atomia.ViewModels, _, ko, Atomia.Utils);
