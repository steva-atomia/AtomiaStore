/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko) {
	'use strict';

	function ProductMixin(productData, cart) {
	    var self = this;

		self._selectedPricingVariantInitialized = false;

		self.selectedPricingVariant = ko.observable();

		self.articleNumber = productData.ArticleNumber;
		self.name = productData.Name;
		self.description = productData.Description;
		self.pricingVariants = [];
		self.attrs = {};
		self._origAttrNames = {};

		_.each(productData.PricingVariants, function (variantData) {
			var pricingVariant = {
				price: variantData.Price,
				toString: function () {
					return variantData.Display;
				},
				renewalPeriod: null
			};

			if (variantData.RenewalPeriod) {
				pricingVariant.renewalPeriod = {
					period: variantData.RenewalPeriod.Period,
					unit: variantData.RenewalPeriod.Unit,
					toString: function () {
						return variantData.RenewalPeriod.Display;
					}
				};
			}

			self.pricingVariants.push(pricingVariant);
		});

		_.each(productData.CustomAttributes, function (attributeData) {
			var name = attributeData.Name[0].toLowerCase() + attributeData.Name.slice(1);

			self.attrs[name] = attributeData.Value;

		    // Save the attribute name with original casing, to be able to send it back to api that way.
			self._origAttrNames[name] = attributeData.Name;
		});

	    /** 
         * Checks if item is equal to other item based on article number.
         * @param {Object} other - The item to compare to
         * @returns {boolean} whether the items are equal or not.
         */
		self.equals = function equals(other) {
		    return self.articleNumber === other.articleNumber;
		};

		/** Shortcut to price of pricing variant. */
		self.price = ko.pureComputed(function () {
			if (self.hasVariants()) {
				return self.selectedPricingVariant().price;
			}

			return self.pricingVariants[0].price;
		});

		/** Shortcut to renewal period of pricing variant */
		self.renewalPeriod = ko.pureComputed(function () {
			if (self.hasVariants()) {
				return self.selectedPricingVariant().renewalPeriod;
			}

			return self.pricingVariants[0].renewalPeriod;
		});

		/** Check if there is more than one pricing variant for the product. */
		self.hasVariants = ko.pureComputed(function () {
			return self.pricingVariants.length > 1;
		});

		/** Pre-select pricing variant to match the one added to cart. */
		self.initPricingVariant = function initPricingVariant() {
			var itemInCart = cart.getExisting(self);
			var selectedPricingVariant = _.find(self.pricingVariants, function (pv) {
				if (pv.renewalPeriod === null || itemInCart.renewalPeriod === null) {
					return false;
				}

				return pv.renewalPeriod.unit === itemInCart.renewalPeriod.unit &&
						pv.renewalPeriod.period === itemInCart.renewalPeriod.period;
			});

			if (selectedPricingVariant !== undefined) {
				self.selectedPricingVariant(selectedPricingVariant);
			}
		};

		/** Select pricing variant for product and sync with cart. */
		self.selectedPricingVariant.subscribe(function selectPricingVariant() {
			if (self._selectedPricingVariantInitialized && cart.contains(self)) {
				cart.remove(self);
			}

			self._selectedPricingVariantInitialized = true;
		});
	}

	_.extend(exports, {
		ProductMixin: ProductMixin
	});

})(Atomia.ViewModels, _, ko);
