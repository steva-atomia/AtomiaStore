/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, utils, viewModelsApi) {
	'use strict';

    /**
     * Creates postal fee item
     * @param {Object} item - The item data to create postal fee item from.
     * @param {Object} cart - Instance of cart.
     */
	function PostalFeeItem(itemData, cart) {
	    var self = this;

	    _.extend(self, new viewModelsApi.ProductMixin(itemData, cart));
	    viewModelsApi.addCartItemExtensions(cart, self);

	    self.attrs.notRemovable = 'true';
    }

    /** 
     * Create pay with invoice model.
     * @param {Object} cart - A cart view model instance.
     */
	function PayWithInvoiceModel(cart) {
	    var self = this;

		self.invoiceType = ko.observable();
		self.postalFeeItem = null;

	    /** Create postal fee item.*/
	    self.createPostalFeeItem = function createPostalFeeItem(itemData) {
	        return new PostalFeeItem(itemData, cart);
	    };

	    /** Load postal fee item data generated on server. */
	    self.loadPostalFeeItem = function loadPostalFeeItem(response) {
	        if (response.status === 'success') {
	            self.postalFeeItem = self.createPostalFeeItem(response.data.Item);
	        }
	    }

	    /** Callback handler for when invoice type is selected.*/
	    self.invoiceType.subscribe(function (newInvoiceType) {
	        if (self.postalFeeItem != null && newInvoiceType === 'email') {
	            cart.remove(self.postalFeeItem);
	        }
	        else if (self.postalFeeItem != null && newInvoiceType === 'post') {
	            cart.add(self.postalFeeItem);
	        }
	    });
		
	    /** Callback handler for when payment method is selected. */
	    utils.subscribe('uiSelectedPaymentMethod', function(paymentMethod){
	        if (self.postalFeeItem != null && paymentMethod !== 'PayWithInvoice') {
	            cart.remove(self.postalFeeItem);
	        }

	        self.invoiceType('email');
	    });
	};

	_.extend(exports, {
	    PayWithInvoiceModel: PayWithInvoiceModel
	});

})(Atomia.ViewModels, _, ko, Atomia.Utils, Atomia.ViewModels);
