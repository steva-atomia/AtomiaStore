/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */


(function (module, _, ko, amplify) {
    'use strict';

    var HostingPackagesItem, HostingPackages,
        NO_DOMAIN = '---';


    /* HostingPackagesItem and prototype */
    HostingPackagesItem = function HostingPackagesItem(productData) {
        var selectedPricingVariantInitialized = false;

        _.extend(this, productData);

        this.UniqueId = _.uniqueId('productitem-');

        this.SelectedPricingVariant = ko.observable();
        this.SelectedPricingVariant.subscribe(function () {
            if (selectedPricingVariantInitialized && this.IsInCart()) {
                this.RemoveFromCart();
            }

            selectedPricingVariantInitialized = true;
        }, this);

        this.Price = ko.pureComputed(function () {
            if (this.HasVariants()) {
                return this.SelectedPricingVariant().Price;
            }

            return this.PricingVariants[0].Price;
        }, this);

        this.RenewalPeriod = ko.pureComputed(function () {
            if (this.HasVariants()) {
                return this.SelectedPricingVariant().RenewalPeriod;
            }

            return this.PricingVariants[0].RenewalPeriod;
        }, this);

        this.HasVariants = ko.pureComputed(function () {
            return this.PricingVariants.length > 1;
        }, this);

        this._InitPricingVariant = function () {
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
        }.bind(this);

        this._SetDomainNameAttribute = function (domainName) {
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
        }.bind(this);

        this._RemoveDomainNameAttribute = function () {
            var domainAttr = _.findWhere(this.CustomAttributes, { Name: 'DomainName' });

            if (domainAttr !== undefined) {
                this.CustomAttributes = _.without(this.CustomAttributes, domainAttr);
            }
        }.bind(this);
    };



    /* HostingPackages and prototype */
    HostingPackages = function HostingPackages() {
        this._Options = {
            MainDomainIsRequired: false,
            PackageIsRequired: true
        };

        this.MainDomainIsRequired = ko.pureComputed(function () {
            return this._Options.MainDomainIsRequired;
        }, this);

        this.PackageIsRequired = ko.pureComputed(function () {
            return this._Options.PackageIsRequired;
        }, this);

        this.HostingPackagesItem = HostingPackagesItem;
        this.Products = ko.observableArray();

        // Main domain options
        this.MainDomainOptions = ko.observableArray();
        this.MainDomainOptionsCount = ko.observable();

        // Selected package
        this.SelectedPackage = ko.observable();
        this.PackageIsSelected = ko.pureComputed(function () {
            return this.SelectedPackage() !== undefined;
        },this)

        // Selected main domain
        this.SelectedMainDomain = ko.observable();
        this.SelectedMainDomain.subscribe(function (newValue) {
            var selectedPackage;

            if (newValue === undefined) {
                return;
            }
            else if (newValue === NO_DOMAIN) {
                _.invoke(this.Products(), '_RemoveDomainNameAttribute');
            }
            else {
                _.invoke(this.Products(), '_SetDomainNameAttribute', newValue);
            }

            selectedPackage = this.SelectedPackage();
            if (selectedPackage !== undefined) {
                this.SelectPackage(selectedPackage);
            }
        }, this);
        this.MainDomainIsSelected = ko.pureComputed(function () {
            return this.SelectedMainDomain() !== undefined && this.SelectedMainDomain() !== NO_DOMAIN;
        }, this);

        this.AllowContinue = ko.pureComputed(function () {
            var conditions = [];

            if (this.PackageIsRequired()) {
                conditions.push(this.PackageIsSelected());
            }

            if (this.MainDomainIsRequired()) {
                conditions.push(this.MainDomainIsSelected());
            }

            return _.every(conditions);

        }, this);
        
        _.bindAll(this, 'Init', 'Load', 'SelectPackage', '_UpdateProducts', '_UpdateMainDomainOptions');
    };

    HostingPackages.prototype = {
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

        SelectPackage: function (item) {
            this.SelectedPackage(item);

            _.each(this.Products(), function (product) {
                if (this._Cart.Contains(product)) {
                    this._Cart.Remove(product);
                }
            }.bind(this));

            this._Cart.Add(item);
        },

        _UpdateMainDomainOptions: function () {
            this.MainDomainOptions.removeAll();

            this.MainDomainOptions.push(NO_DOMAIN);

            _.each(this._Cart.DomainItems(), function (item) {
                var domainNameAttr = _.findWhere(item.CustomAttributes, { Name: 'DomainName' });

                this.MainDomainOptions.push(domainNameAttr.Value);
            }, this);

            this.MainDomainOptionsCount(this.MainDomainOptions().length)
        },

        _UpdateProducts: function (products) {
            _.each(products, function (product) {
                var domainAttr,
                    itemInCart,
                    item = new this.HostingPackagesItem(product);

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

                    this.SelectPackage(item);
                }

                this.Products.push(item);
            }, this);
        }
    };
    


    /* Export models */
    module.HostingPackagesItem = HostingPackagesItem;
    module.HostingPackages = HostingPackages;

})(Atomia.ViewModels, _, ko, amplify);
