/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, utils) {
	'use strict';

    /** Create payment selector */
	function PaymentSelectorModel(cart) {
	    var self = this;

	    self.selectedPaymentMethod = ko.observable();

	    self.selectedPaymentMethod.subscribe(function (paymentMethod) {
	        utils.publish('uiSelectedPaymentMethod', paymentMethod);
	        cart.addUpdateAttr('PaymentMethod', paymentMethod, true);
	    });
	}


	/* Module exports */
	_.extend(exports, {
	    PaymentSelectorModel: PaymentSelectorModel
	});

})(Atomia.ViewModels, _, ko, Atomia.Utils);
