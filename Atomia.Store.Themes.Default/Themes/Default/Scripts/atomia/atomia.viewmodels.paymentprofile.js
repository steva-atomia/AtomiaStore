/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, utils) {
    'use strict';

    /** Create model for PaymentProfileModel payment method. */
    function PaymentProfileModel(defaultPaymentMethod, paymentMethodsWithSupport) {
        var self = this;
        var defaultMethodHasSupport = _.contains(paymentMethodsWithSupport, defaultPaymentMethod);

        self.paymentMethodsWithSupport = paymentMethodsWithSupport;
        self.saveccinfo = ko.observable(false);
        self.autopay = ko.observable(false);
        self.isSupportedByPaymentMethod = ko.observable(defaultMethodHasSupport);

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

        utils.subscribe('uiSelectedPaymentMethod', function (paymentMethod) {
            var supported = _.contains(self.paymentMethodsWithSupport, paymentMethod);

            // Reset selection
            self.saveccinfo(false);
            self.autopay(false);

            self.isSupportedByPaymentMethod(supported);
        });
    }

    _.extend(exports, {
        PaymentProfileModel: PaymentProfileModel
    });

})(Atomia.ViewModels, _, ko, Atomia.Utils);
