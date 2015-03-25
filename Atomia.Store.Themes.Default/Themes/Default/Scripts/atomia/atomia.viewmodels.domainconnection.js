/// <reference path="../../../../Scripts/underscore.js" />
/// <reference path="../../../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="atomia.utils.js" />

/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, utils) {
    'use strict';

    var DomainConnectionModelPrototype,
        CreateDomainConnectionModel,
        CreateDomainStatusModel;


    DomainConnectionModelPrototype = {

        /** Set domain name if 'SelectedItem' already has domain name associated. */
        SetInitialDomainName: function () {
            var selectedItem = this.SelectedItem(),
                itemInCart,
                domainName;

            if (selectedItem !== undefined) {
                itemInCart = selectedItem.GetItemInCart();
            }

            if (itemInCart !== undefined) {
                domainName = itemInCart.GetDomainName();
            }

            if (domainName !== undefined) {
                this.SelectedDomainName(domainName);
            }
        },

        /** Handler for updating domain name options when cart is updated. */
        _UpdateDomainNameOptions: function _UpdateDomainNameOptions() {
            var domainItems = this._Cart.DomainItems(),
                domainNames = [];

            if (domainItems !== undefined) {
                _.each(domainItems, function (item) {
                    var domainAttr = _.findWhere(item.CustomAttributes, { Name: 'DomainName' });
                    domainNames.push(domainAttr.Value);
                });
            }
            
            this.DomainNameOptions(domainNames);

            if (this.StatusNotifier !== undefined) {
                this.StatusNotifier.DomainNameOptionsCount(domainNames.length);
            }
        },

        /** Add or remove association between 'selectedDomainName' and 'SelectedItem' in cart. */
        _HandleSelectedDomainName: function _HandleSelectedDomainName(selectedDomainName) {
            var selectedItem = this.SelectedItem();

            if (this.StatusNotifier !== undefined) {
                this.StatusNotifier.DomainNameHasBeenSelected(selectedDomainName !== undefined);
            }

            if (selectedItem === undefined || !selectedItem.IsInCart()) {
                return;
            }

            if (selectedDomainName !== undefined) {
                this._Cart.AddDomainName(selectedItem, selectedDomainName);
            }
            else {
                this._Cart.RemoveDomainName(selectedItem);
            }
        },

        /** Update domain connection when an item ('addedItem') is added to cart. */
        _HandleAddedCartItem: function _HandleAddedCartItem(addedItem) {
            var selectedItem = this.SelectedItem();

            if (addedItem.IsDomainItem()) {
                this._UpdateDomainNameOptions();
            }
            else if (selectedItem.Equals(addedItem)) {
                this._HandleSelectedDomainName(this.SelectedDomainName());
            }
        },

        /** Update domain connection when an item ('removedItem') is removed from cart. */
        _HandleRemovedCartItem: function _HandleRemovedCartItem(removedItem) {
            if (removedItem.IsDomainItem()) {
                this._UpdateDomainNameOptions();
            }
        }
    };
    
    /** Create domain connection view model. 
     * @param {Object} cart - Instance of cart to operate on.
     * @param {Object} selectedItem - Item to connect domain to. Can be an observable.
     * @param {Object|Function} extensions - Extensions to the default view model.
     */
    CreateDomainConnectionModel = function CreateDomainConnectionModel(cart, selectedItem, extensions) {
        var item;

        item = utils.createViewModel(DomainConnectionModelPrototype, {
            _Cart: cart,
            SelectedItem: _.isFunction(selectedItem) ? selectedItem : function () { return selectedItem; },
            UniqueId: _.uniqueId('domain-connection-'),
            DomainNameOptions: ko.observableArray(),
            SelectedDomainName: ko.observable()
        }, extensions);

        item.SelectedDomainName.subscribe(item._HandleSelectedDomainName);
        
        utils.subscribe('cart:add', item._HandleAddedCartItem);
        utils.subscribe('cart:remove', item._HandleRemovedCartItem);

        item._UpdateDomainNameOptions();

        return item;
    };



    /** Create and object to relay status of domain connection.
     * @param {Object|Function} extensions - Extensions to the default domain status view model.
      */
    CreateDomainStatusModel = function CreateDomainStatusModel(extensions) {
        var statusItem = Object.create({}),
            defaults = {
                DomainNameHasBeenSelected: ko.observable(false),
                DomainNameOptionsCount: ko.observable(0)
            };

        return _.extend(statusItem, defaults, extensions);
    };



    /* Module exports */
    _.extend(exports, {
        CreateDomainStatusModel: CreateDomainStatusModel,
        CreateDomainConnectionModel: CreateDomainConnectionModel
    });

})(Atomia.ViewModels, _, ko, Atomia.Utils);
