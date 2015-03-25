/// <reference path="../../../../Scripts/underscore.js" />
/// <reference path="../../../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="atomia.utils.js" />
/// <reference path="atomia.viewmodels.cart.js" />

/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, utils, viewModelsApi) {
	'use strict';

	var PayWithInvoiceModelPrototype,
        CreatePayWithInvoiceModel;
    

	PayWithInvoiceModelPrototype = {
	    /** 
         * Callback handler for when payment method is selected. 
	     * Removes postal fee from cart if payment method is not invoice.
	     */
		_SelectPaymentMethod: function _SelectPaymentMethod(newValue) {
			var cartItem = this._CreatePostalFeeItem();

			if (cartItem !== null && newValue !== 'PayWithInvoice') {
				this._Cart.Remove(cartItem);
			}

			this.InvoiceType('email');
		},

        /** Callback handler for when invoice type is selected. Adds or removes postal fee from cart. */
		_SelectInvoiceType: function _SelectInvoiceType(newValue) {
			var cartItem = this._CreatePostalFeeItem();

			if (cartItem !== null && newValue === 'email') {
				this._Cart.Remove(cartItem);
			}
			else if (cartItem !== null && newValue === 'post') {
				this._Cart.Add(cartItem);
			}
		},

        /** Make cartable item out of postal fee item.*/
		_CreatePostalFeeItem: function _CreatePostalFeeItem() {
			var cartItem, postalFeeItem = this.PostalFeeItem();

			if (postalFeeItem === null) {
				return null;
			}

			postalFeeItem.CustomAttributes.push({
				Name: 'NotRemovable',
				Value: 'true'
			});

			cartItem = viewModelsApi.AddCartItemExtensions(this._Cart, postalFeeItem);

			return cartItem;
		},

        /** Load postal fee item data generated on server. */
		LoadPostalFeeItem: function LoadPostalFeeItem(getItemResponse) {
			var item;

			if (getItemResponse.status === 'success') {
				item = getItemResponse.data.Item;

				// Extend with some fake values that are needed in cart summary before recalc.	
				_.extend(item, {
					Price: item.PricingVariants[0].Price,
					RenewalPeriod: null,
					Category: '',
					Discount: 0,
					Total: item.PricingVariants[0].Price
				});

				this.PostalFeeItem(item);
			}
			else {
				this.PostalFeeItem(null);
			}
		}
	};


    /** 
     * Create pay with invoice model.
     * @param {Object} cart - A cart view model instance.
     * @param {Object} paymentSelection - A payment selection view model instance.
     * @param {Object|Function} extensions - Extensions to the default pay with invoice view model.
     */
	CreatePayWithInvoiceModel = function CreatePayWithInvoiceModel(cart, paymentSelection, extensions) {
		var defaults, viewModel;

		defaults = {
			_Cart: cart,
			InvoiceType: ko.observable(),
			PostalFeeItem: ko.observable(null)
		};

		viewModel = utils.createViewModel(PayWithInvoiceModelPrototype, defaults, extensions);

		viewModel.InvoiceType.subscribe(viewModel._SelectInvoiceType, viewModel);
		paymentSelection.SelectedPaymentMethod.subscribe(viewModel._SelectPaymentMethod, viewModel);

		return viewModel;
	};


	_.extend(exports, {
	    CreatePayWithInvoiceModel: CreatePayWithInvoiceModel
	});

})(Atomia.ViewModels, _, ko, Atomia.Utils, Atomia.ViewModels);
