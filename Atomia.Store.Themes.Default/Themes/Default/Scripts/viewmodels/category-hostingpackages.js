/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */


(function (module, _, ko, amplify) {
    'use strict';

    var SingleSelectionDomainProductsItem,
        SingleSelectionDomainProducts,
        NO_DOMAIN = '---';


    /* SingleSelectionDomainProductsItem and prototype */
    SingleSelectionDomainProductsItem = function SingleSelectionDomainProductsItem(productData) {
        this._selectedPricingVariantInitialized = false;

        _.extend(this, productData);

        this.UniqueId = _.uniqueId('productitem-');

        this.SelectedPricingVariant = ko.observable();
        this.SelectedPricingVariant.subscribe(this._SelectPricingVariant, this);

        this.Price = ko.pureComputed(this._Price, this);
        this.RenewalPeriod = ko.pureComputed(this._RenewalPeriod, this);
        this.HasVariants = ko.pureComputed(this._HasVariants, this);

        _.bindAll(this, '_InitPricingVariant', '_SetDomainNameAttribute', '_RemoveDomainNameAttribute');
    };

    SingleSelectionDomainProductsItem.prototype = {
        _SelectPricingVariant: function () {
            if (this._selectedPricingVariantInitialized && this.IsInCart()) {
                this.RemoveFromCart();
            }

            this._selectedPricingVariantInitialized = true;
        },

        _Price: function () {
            if (this.HasVariants()) {
                return this.SelectedPricingVariant().Price;
            }

            return this.PricingVariants[0].Price;
        },

        _RenewalPeriod: function () {
            if (this.HasVariants()) {
                return this.SelectedPricingVariant().RenewalPeriod;
            }

            return this.PricingVariants[0].RenewalPeriod;
        },

        _HasVariants: function () {
            return this.PricingVariants.length > 1;
        },

        _InitPricingVariant: function () {
            var itemInCart = this.GetItemInCart(),
                selectedPricingVariant = _.find(this.PricingVariants, function (pv) {
                    if (pv.RenewalPeriod === null || itemInCart.RenewalPeriod === null) {
                        return false;
                    }

                    return pv.RenewalPeriod.Unit === itemInCart.RenewalPeriod.Unit &&
                           pv.RenewalPeriod.Period === itemInCart.RenewalPeriod.Period;
                });

            if (selectedPricingVariant !== undefined) {
                this.SelectedPricingVariant(selectedPricingVariant);
            }
        },

        _SetDomainNameAttribute: function (domainName) {
            var domainAttr = _.findWhere(this.CustomAttributes, { Name: 'DomainName' });

            if (domainAttr !== undefined) {
                domainAttr.Value = domainName;
            }
            else {
                this.CustomAttributes.push({
                    Name: 'DomainName',
                    Value: domainName
                });
            }
        },

        _RemoveDomainNameAttribute: function () {
            var domainAttr = _.findWhere(this.CustomAttributes, { Name: 'DomainName' });

            if (domainAttr !== undefined) {
                this.CustomAttributes = _.without(this.CustomAttributes, domainAttr);
            }
        }
    };



    /* SingleSelectionDomainProducts and prototype */
    SingleSelectionDomainProducts = function SingleSelectionDomainProducts() {

        this.Item = SingleSelectionDomainProductsItem;
        this._Options = {
            MainDomainIsRequired: false,
            ProductIsRequired: true
        };

        this.Products = ko.observableArray();
        this.SelectedProduct = ko.observable();
        this.ProductIsSelected = ko.pureComputed(this._ProductIsSelected, this);
        this.ProductIsRequired = ko.pureComputed(this._ProductIsRequired, this);

        this.MainDomainOptions = ko.observableArray();
        this.MainDomainOptionsCount = ko.observable();
        this.SelectedMainDomain = ko.observable();
        this.SelectedMainDomain.subscribe(this.SelectMainDomain, this);
        this.MainDomainIsSelected = ko.pureComputed(this._MainDomainIsSelected, this);
        this.MainDomainIsRequired = ko.pureComputed(this._MainDomainIsRequired, this);
        
        this.AllowContinue = ko.pureComputed(this._AllowContinue, this);
        
        _.bindAll(this, 'Init', 'Load', 'SelectProduct', '_UpdateProducts', '_UpdateMainDomainOptions');
    };

    SingleSelectionDomainProducts.prototype = {
        Init: function (cart, options) {
            this._Cart = cart;

            options = options || {};
            _.extend(this._Options, options);

            this._UpdateMainDomainOptions();

            amplify.subscribe('cart:add', this, function (item) {
                if (item.Category === 'Domain') {
                    this._UpdateMainDomainOptions();
                }
            });

            amplify.subscribe('cart:remove', this, function (item) {
                if (item.Category === 'Domain') {
                    this._UpdateMainDomainOptions();
                }
            });
        },

        Load: function (listProductsDataResponse) {
            this._UpdateProducts(listProductsDataResponse.data.CategoryData.Products);
        },

        SelectProduct: function (item) {
            this.SelectedProduct(item);

            _.each(this.Products(), function (product) {
                if (this._Cart.Contains(product)) {
                    this._Cart.Remove(product);
                }
            }.bind(this));

            this._Cart.Add(item);
        },

        SelectMainDomain: function (newDomain) {
            var selectedProduct;

            if (newDomain === undefined) {
                return;
            }
            else if (newDomain === NO_DOMAIN) {
                _.invoke(this.Products(), '_RemoveDomainNameAttribute');
            }
            else {
                _.invoke(this.Products(), '_SetDomainNameAttribute', newDomain);
            }

            selectedProduct = this.SelectedProduct();
            if (selectedProduct !== undefined) {
                this.SelectProduct(selectedProduct);
            }
        },

        _MainDomainIsRequired: function () {
            return this._Options.MainDomainIsRequired;
        },

        _ProductIsRequired: function () {
            return this._Options.ProductIsRequired;
        },

        _MainDomainIsSelected: function () {
            return this.SelectedMainDomain() !== undefined && this.SelectedMainDomain() !== NO_DOMAIN;
        },

        _ProductIsSelected: function () {
            return this.SelectedProduct() !== undefined;
        },

        _UpdateMainDomainOptions: function () {
            this.MainDomainOptions.removeAll();

            this.MainDomainOptions.push(NO_DOMAIN);

            _.each(this._Cart.DomainItems(), function (item) {
                var domainNameAttr = _.findWhere(item.CustomAttributes, { Name: 'DomainName' });

                this.MainDomainOptions.push(domainNameAttr.Value);
            }, this);

            this.MainDomainOptionsCount(this.MainDomainOptions().length);
        },

        _UpdateProducts: function (products) {
            _.each(products, function (product) {
                var domainAttr,
                    itemInCart,
                    item = new this.Item(product);

                item = this._Cart.ExtendWithCartProperties(item);

                if (this._Cart.Contains(item)) {
                    item._InitPricingVariant();
                    
                    itemInCart = item.GetItemInCart();
                    domainAttr = _.findWhere(itemInCart.CustomAttributes, { Name: 'DomainName' });

                    if (domainAttr !== undefined) {
                        this.SelectedMainDomain(domainAttr.Value);
                    }
                    else {
                        this.SelectedMainDomain(NO_DOMAIN);
                    }

                    this.SelectProduct(item);
                }

                this.Products.push(item);
            }, this);
        },

        _AllowContinue: function () {
            var conditions = [];

            if (this.ProductIsRequired()) {
                conditions.push(this.ProductIsSelected());
            }

            if (this.MainDomainIsRequired()) {
                conditions.push(this.MainDomainIsSelected());
            }

            return _.every(conditions);

        }
    };
    


    /* Export models */
    module.SingleSelectionDomainProductsItem = SingleSelectionDomainProductsItem;
    module.SingleSelectionDomainProducts = SingleSelectionDomainProducts;

})(Atomia.ViewModels, _, ko, amplify);
