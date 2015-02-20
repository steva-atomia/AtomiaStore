/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, utils, viewModelsApi) {
	'use strict';

	var CreatePayWithInvoice, CreatePayWithInvoicePrototype;

	CreatePayWithInvoicePrototype = {
		_SelectPaymentMethod: function _SelectPaymentMethod(newValue) {
			var cartItem = this._CreatePostalFeeItem();;

			if (cartItem !== null && newValue !== 'PayWithInvoice') {
				this._Cart.Remove(cartItem);
			}

			this.InvoiceType('email');
		},
		_SelectInvoiceType: function _SelectInvoiceType(newValue) {
			var cartItem = this._CreatePostalFeeItem();

			if (cartItem !== null && newValue === 'email') {
				this._Cart.Remove(cartItem);
			}
			else if (cartItem !== null && newValue === 'post') {
				this._Cart.Add(cartItem);
			}
		},
		_CreatePostalFeeItem: function _CreatePostalFeeItem(item) {
			var cartItem, postalFeeItem = this.PostalFeeItem();

			if (postalFeeItem === null) {
				return null;
			}

			postalFeeItem.CustomAttributes.push({
				Name: 'NotRemovable',
				Value: 'true'
			});

			cartItem = viewModelsApi.AddCartExtensions(this._Cart, postalFeeItem);

			return cartItem;
		},
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

	CreatePayWithInvoice = function CreatePayWithInvoice(cart, paymentSelection, extensions) {
		var defaults, viewModel;

		defaults = {
			_Cart: cart,
			InvoiceType: ko.observable(),
			PostalFeeItem: ko.observable(null)
		};

		viewModel = utils.createViewModel(CreatePayWithInvoicePrototype, defaults, extensions);

		viewModel.InvoiceType.subscribe(viewModel._SelectInvoiceType, viewModel);
		paymentSelection.SelectedPaymentMethod.subscribe(viewModel._SelectPaymentMethod, viewModel);

		return viewModel;
	};


	/* Module exports */
	_.extend(exports, {
		CreatePayWithInvoice: CreatePayWithInvoice
	});

})(Atomia.ViewModels, _, ko, Atomia.Utils, Atomia.ViewModels);
