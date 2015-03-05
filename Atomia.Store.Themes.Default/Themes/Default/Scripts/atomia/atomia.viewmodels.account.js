/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, utils) {
	'use strict';

	var AccountModelPrototype,
        CreateAccountModel;


	AccountModelPrototype = {
		UseOtherBillingContact: function UseSeparateBillingContact() {
			this.OtherBillingContact(true);
		},
		UseMainAsBillingContact: function UseMainAsBillingContact() {
			this.OtherBillingContact(false);
		}
	};

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
