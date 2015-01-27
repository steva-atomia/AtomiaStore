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
		return utils.createViewModel(CreateAccountPrototype, {
			OtherBillingContact: ko.observable(false)
		}, extensions);
	};


	/* Module exports */
	_.extend(exports, {
		CreateAccount: CreateAccount
	});

})(Atomia.ViewModels, _, ko, Atomia.Utils);
