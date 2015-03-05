/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, utils) {
    'use strict';

    var DomainConnectionModelPrototype,
        CreateDomainConnectionModel,
        CreateDomainStatusModel;


    /* DomainConnection prototype and factory */
    DomainConnectionModelPrototype = {
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

        _HandleAddedCartItem: function _HandleAddedCartItem(addedItem) {
            var selectedItem = this.SelectedItem();

            if (addedItem.IsDomainItem()) {
                this._UpdateDomainNameOptions();
            }
            else if (selectedItem.Equals(addedItem)) {
                this._HandleSelectedDomainName(this.SelectedDomainName());
            }
        },

        _HandleRemovedCartItem: function _HandleRemovedCartItem(removedItem) {
            if (removedItem.IsDomainItem()) {
                this._UpdateDomainNameOptions();
            }
        }
    };
    
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



    /* DomainStatus factory */
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
