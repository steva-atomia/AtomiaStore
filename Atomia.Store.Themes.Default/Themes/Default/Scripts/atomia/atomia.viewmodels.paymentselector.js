/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, utils) {
	'use strict';

	var CreatePaymentSelectorPrototype, CreatePaymentSelector;


	CreatePaymentSelectorPrototype = {

	};

	CreatePaymentSelector = function CreatePaymentSelector(extensions) {
	    var defaults;

	    defaults = function (self) {
	        return {
	            SelectedPaymentMethod: ko.observable()
	        };
	    };

	    return utils.createViewModel(CreatePaymentSelectorPrototype, defaults, extensions);
	};


	/* Module exports */
	_.extend(exports, {
	    CreatePaymentSelector: CreatePaymentSelector
	});

})(Atomia.ViewModels, _, ko, Atomia.Utils);
