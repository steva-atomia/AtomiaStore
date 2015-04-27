/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko) {
    'use strict';

    /** Create model for CCPayment payment method. */
    function CCPaymentModel() {
        var self = this;

        self.saveccinfo = ko.observable(false);
        self.autopay = ko.observable(false);

        self.saveccinfo.subscribe(function (newValue) {
            if (newValue === false) {
                self.autopay(false);
            }
        });

        self.autopay.subscribe(function (newValue) {
            if (newValue === true) {
                self.saveccinfo(true);
            }
        });
    }

    _.extend(exports, {
        CCPaymentModel: CCPaymentModel
    });

})(Atomia.ViewModels, _, ko);
