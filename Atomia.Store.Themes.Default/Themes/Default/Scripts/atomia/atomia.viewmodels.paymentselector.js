/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, utils) {
	'use strict';

    /** Create payment selector */
	function PaymentSelectorModel() {
	    var self = this;

	    self.selectedPaymentMethod = ko.observable();

	    self.selectedPaymentMethod.subscribe(function (paymentMethod) {
	        utils.publish('uiSelectedPaymentMethod', paymentMethod);
	    });
	}


	/* Module exports */
	_.extend(exports, {
	    PaymentSelectorModel: PaymentSelectorModel
	});

})(Atomia.ViewModels, _, ko, Atomia.Utils);
