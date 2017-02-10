/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, utils) {
	'use strict';

    /** Create a Knockout view model for coordinating main and billing contact data. */
    function AccountModel() {
        var self = this;
            
        self.mainContactCustomerType = ko.observable('individual');
        self.billingContactCustomerType = ko.observable('individual');
        self.otherBillingContact = ko.observable(false);
        self.mainContactCountry = ko.observable('');
        self.resellerId = ko.observable('');
        self.customFields = ko.observableArray();
        self.modelFields = {};
        self.existingArticleNumbersForFields = [];

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

        self.mainContactCountry.subscribe(function (newValue) {
            self.updateCustomFields();
        });

        self.updateCustomFields = function () {
            var cartArray = Atomia.VM.cart.cartItems();
            var data = {
                country: self.mainContactCountry(),
                products: [],
                resellerId: self.resellerId(),
                accountName: '',
                keepExistingFields: false
            };

            cartArray.forEach(function (cartItem) {
                data.products.push(cartItem.articleNumber);
            });
            
            if (self.customFields().length > 0) {
                self.customFields().forEach(function(field, index) {
                    data['existingFields[' + index + '].Key'] = field.name;
                    data['existingFields[' + index + '].Value'] = field.value() || '';
                });
            } else {
                data.existingFields = {};
            }

            $.ajax({ url: Atomia.URLS.getApplicableCustomFields, data: data, traditional: true }).done(function (result) {
                var updatedFields = [];
                result.Fields.forEach(function (fieldData) {
                    updatedFields.push({
                        name: fieldData.Name,
                        value: ko.observable(fieldData.Value || self.modelFields[fieldData.Name]),
                        required: fieldData.Required,
                        dropdown: fieldData.Dropdown,
                        dropdownEntries: fieldData.DropdownEntries
                    });
                });

                self.existingArticleNumbersForFields = result.ExistingArticleNumbers;
                self.customFields(updatedFields);
            });
        };

        utils.subscribe('cart:remove', function (removedItem) {
            if (removedItem.isDomainItem()) {
                self.updateCustomFields();
            }
        });
	}


	/* Module exports */
	_.extend(exports, {
	    AccountModel: AccountModel
	});

})(Atomia.ViewModels, _, ko, Atomia.Utils);
