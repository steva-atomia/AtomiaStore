/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, checkoutApi) {
    'use strict';

    /** 
     * Knockout view model for validating VAT number
     */
    function VatValidationModel(cart, checkVat) {
        var self = this;
        self.vatNumber = ko.observable('');
        self.valid = ko.observable(!checkVat);
        self.initialCheckDone = ko.observable(!checkVat);
        self.revalidationEnabled = ko.observable(false);
        self.validating = ko.observable(false);

        self.validateVatNumber = function validateVatNumber() {
            self.validating(true);

            checkoutApi.validateVatNumber(self.vatNumber(), function (result) {
                self.valid(result.Valid);
                self.vatNumber(result.VatNumber);

                self.validating(false);

                if (result.Valid) {
                    cart.recalculate();
                }
                
                if (!self.revalidationEnabled()) {
                    self.revalidationEnabled(!result.Valid);
                }

                self.initialCheckDone(true);
            });
        };

        self.enableRevalidation = function enableRevalidation() {
            self.revalidationEnabled(true);
        };
    }

    /* Module exports */
    _.extend(exports, {
        VatValidationModel: VatValidationModel
    });

})(Atomia.ViewModels, _, ko, Atomia.Api.Checkout);
