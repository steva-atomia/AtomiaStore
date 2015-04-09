/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, utils, viewModelsApi) {
	'use strict';

    /** 
     * Create pay with invoice model.
     * @param {Object} cart - A cart view model instance.
     */
	function PayWithInvoiceModel(cart) {
		var self = this;

		self._cart = cart;
		self.invoiceType = ko.observable();
		self.postalFeeItem = ko.observable(null);

	    /** Make cartable item out of postal fee item.*/
	    self.createPostalFeeItem = function createPostalFeeItem() {
	        var cartItem, postalFeeItem = self.postalFeeItem();

	        if (postalFeeItem === null) {
	            return null;
	        }

	        postalFeeItem.CustomAttributes.push({
	            Name: 'NotRemovable',
	            Value: 'true'
	        });

	        cartItem = viewModelsApi.AddCartItemExtensions(self._cart, postalFeeItem);

	        return cartItem;
	    };

	    /** Load postal fee item data generated on server. */
	    self.loadPostalFeeItem = function loadPostalFeeItem(response) {
	        var item;

	        if (response.status === 'success') {
	            item = response.data.Item;

	            // Extend with some fake values that are needed in cart summary before recalc.	
	            _.extend(item, {
	                Price: item.PricingVariants[0].Price,
	                RenewalPeriod: null,
	                Category: '',
	                Discount: 0,
	                Total: item.PricingVariants[0].Price
	            });

	            self.postalFeeItem(item);
	        }
	        else {
	            self.postalFeeItem(null);
	        }
	    }

	    /** Callback handler for when invoice type is selected. Adds or removes postal fee from cart. */
	    self.invoiceType.subscribe(function (newInvoiceType) {
	        var cartItem = self.createPostalFeeItem();

	        if (cartItem !== null && newInvoiceType === 'email') {
	            self._cart.Remove(cartItem);
	        }
	        else if (cartItem !== null && newInvoiceType === 'post') {
	            self._cart.Add(cartItem);
	        }
	    });
		
	    /** 
         * Callback handler for when payment method is selected. 
	     * Removes postal fee from cart if payment method is not invoice.
	     */
	    utils.subscribe('uiSelectedPaymentMethod', function(paymentMethod){
	        var cartItem = self.createPostalFeeItem();

	        if (cartItem !== null && paymentMethod !== 'PayWithInvoice') {
	            self._cart.Remove(cartItem);
	        }

	        self.invoiceType('email');
	    });
	};


	_.extend(exports, {
	    PayWithInvoiceModel: PayWithInvoiceModel
	});

})(Atomia.ViewModels, _, ko, Atomia.Utils, Atomia.ViewModels);
