/// <reference path="../../../../Scripts/underscore.js" />
/// <reference path="../../../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="atomia.utils.js" />

/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, utils) {
	'use strict';

	var CreatePaymentSelectorModel;

    /** Create payment selector
     * @param {Object|function} extensions - Extensions to the default payment selector model
     */
	CreatePaymentSelectorModel = function CreatePaymentSelectorModel(extensions) {
	    return utils.createViewModel({}, {
	        SelectedPaymentMethod: ko.observable()
	    }, extensions);
	};


	/* Module exports */
	_.extend(exports, {
	    CreatePaymentSelectorModel: CreatePaymentSelectorModel
	});

})(Atomia.ViewModels, _, ko, Atomia.Utils);
