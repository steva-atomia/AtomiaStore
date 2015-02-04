/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, utils) {
	'use strict';

	var CreateAccountPrototype, CreateAccount;


	CreateAccountPrototype = {
		UseOtherBillingContact: function UseSeparateBillingContact() {
			this.OtherBillingContact(true);
		},
		UseMainAsBillingContact: function UseMainAsBillingContact() {
			this.OtherBillingContact(false);
		}
	};

    CreateAccount = function CreateAccount(extensions) {
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

	    return utils.createViewModel(CreateAccountPrototype, defaults, extensions);
	};


	/* Module exports */
	_.extend(exports, {
		CreateAccount: CreateAccount
	});

})(Atomia.ViewModels, _, ko, Atomia.Utils);
