/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, utils) {
	'use strict';

	var PaymentSelectorModelPrototype,
        CreatePaymentSelectorModel;


	PaymentSelectorModelPrototype = {

	};

	CreatePaymentSelectorModel = function CreatePaymentSelectorModel(extensions) {
	    var defaults;

	    defaults = function (self) {
	        return {
	            SelectedPaymentMethod: ko.observable()
	        };
	    };

	    return utils.createViewModel(PaymentSelectorModelPrototype, defaults, extensions);
	};


	/* Module exports */
	_.extend(exports, {
	    CreatePaymentSelectorModel: CreatePaymentSelectorModel
	});

})(Atomia.ViewModels, _, ko, Atomia.Utils);
