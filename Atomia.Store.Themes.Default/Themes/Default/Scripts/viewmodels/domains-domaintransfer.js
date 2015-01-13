/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (module, ko) {
    'use strict';

    var DomainTransfer = function DomainTransfer() {
        this.Query = ko.observable();

        _.bindAll(this, 'Submit');
    };

    DomainTransfer.prototype = {
        Submit: function() {
            console.log('Transfer!');
        }
    };

    module.DomainTransfer = DomainTransfer;

})(Atomia.ViewModels, ko);
