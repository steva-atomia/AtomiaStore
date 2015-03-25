/// <reference path="../../../../Scripts/underscore.js" />
/// <reference path="../../../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="atomia.utils.js" />

/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, utils) {
	'use strict';

	var AccountModelPrototype,
        CreateAccountModel;


	AccountModelPrototype = {
        /** Use other billing contact than main */
		UseOtherBillingContact: function UseSeparateBillingContact() {
			this.OtherBillingContact(true);
		},

        /** Use main as billing contact */
		UseMainAsBillingContact: function UseMainAsBillingContact() {
			this.OtherBillingContact(false);
		}
	};

    /** 
     * Create a Knockout view model for coordinating main and billing contact data. 
     * @param {Objects|Function} extensions - Extensions to the default account view model
     * @returns the created account view model.
     */
    CreateAccountModel = function CreateAccountModel(extensions) {
        var defaults;
            
        defaults = function (self) {
            return {
                MainContactCustomerType: ko.observable('individual'),
                BillingContactCustomerType: ko.observable('individual'),

                OtherBillingContact: ko.observable(false),

                MainContactIsCompany: ko.pureComputed(function () {
                    return self.MainContactCustomerType() === 'company';
                }, self),
                BillingContactIsCompany: ko.pureComputed(function () {
                    return self.BillingContactCustomerType() === 'company';
                }, self)
            };
        };

	    return utils.createViewModel(AccountModelPrototype, defaults, extensions);
	};


	/* Module exports */
	_.extend(exports, {
		CreateAccountModel: CreateAccountModel
	});

})(Atomia.ViewModels, _, ko, Atomia.Utils);
