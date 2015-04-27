/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko) {
	'use strict';

    /** Create a Knockout view model for coordinating main and billing contact data. */
    function AccountModel() {
        var self = this;
            
        self.mainContactCustomerType = ko.observable('individual');
        self.billingContactCustomerType = ko.observable('individual');
        self.otherBillingContact = ko.observable(false);

        self.mainContactIsCompany = ko.pureComputed(function () {
            return self.mainContactCustomerType() === 'company';
        });
                
        self.billingContactIsCompany = ko.pureComputed(function () {
            return self.billingContactCustomerType() === 'company';
        });

        /** Use other billing contact than main */
        self.useOtherBillingContact = function useOtherBillingContact() {
            self.otherBillingContact(true);
        };

        /** Use main as billing contact */
        self.useMainAsBillingContact = function useMainAsBillingContact() {
            self.otherBillingContact(false);
        };
	}


	/* Module exports */
	_.extend(exports, {
	    AccountModel: AccountModel
	});

})(Atomia.ViewModels, _, ko);
