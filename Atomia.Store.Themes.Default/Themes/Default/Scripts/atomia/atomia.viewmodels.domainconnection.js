/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, utils) {
    'use strict';
    
    /** Create domain connection view model. 
     * @param {Object} cart - Instance of cart to operate on.
     * @param {Object} selectedItem - Item to connect domain to. Can be an observable.
     */
    function DomainConnectionModel(cart, selectedItem) {
        var self = this;

        self._cart = cart;

        self.selectedItem = _.isFunction(selectedItem) ? selectedItem : function () { return selectedItem; };
        self.uniqueId = _.uniqueId('domain-connection-');
        self.domainNameOptions = ko.observableArray();
        self.selectedDomainName = ko.observable();

        /** Set domain name if 'SelectedItem' already has domain name associated. */
        self.setInitialDomainName = function setInitialDomainName() {
            var selectedItem = self.selectedItem(),
                itemInCart,
                domainName;

            if (selectedItem !== undefined) {
                itemInCart = selectedItem.GetItemInCart();
            }

            if (itemInCart !== undefined) {
                domainName = itemInCart.GetDomainName();
            }

            if (domainName !== undefined) {
                self.selectedDomainName(domainName);
            }
        };

        /** Handler for updating domain name options when cart is updated. */
        self.updateDomainNameOptions = function updateDomainNameOptions() {
            var domainItems = self._cart.DomainItems(),
                domainNames = [];

            if (domainItems !== undefined) {
                _.each(domainItems, function (item) {
                    var domainAttr = _.findWhere(item.CustomAttributes, { Name: 'DomainName' });
                    domainNames.push(domainAttr.Value);
                });
            }
            
            self.domainNameOptions(domainNames);

            if (self.statusNotifier !== undefined) {
                self.statusNotifier.domainNameOptionsCount(domainNames.length);
            }
        };

        /** Add or remove association between 'selectedDomainName' and 'SelectedItem' in cart. */
        self._handleSelectedDomainName = function _handleSelectedDomainName(selectedDomainName) {
            var selectedItem = self.selectedItem();

            if (self.statusNotifier !== undefined) {
                self.statusNotifier.domainNameHasBeenSelected(selectedDomainName !== undefined);
            }

            if (selectedItem === undefined || !selectedItem.IsInCart()) {
                return;
            }

            if (selectedDomainName !== undefined) {
                self._cart.AddDomainName(selectedItem, selectedDomainName);
            }
            else {
                self._cart.RemoveDomainName(selectedItem);
            }
        };

        /** Update domain connection when an item ('addedItem') is added to cart. */
        utils.subscribe('cart:add', function(addedItem) {
            var selectedItem = self.selectedItem();

            if (addedItem.IsDomainItem()) {
                self.updateDomainNameOptions();
            }
            else if (selectedItem.Equals(addedItem)) {
                self._handleSelectedDomainName(self.selectedDomainName());
            }
        });

        /** Update domain connection when an item ('removedItem') is removed from cart. */
        utils.subscribe('cart:remove', function (removedItem) {
            if (removedItem.IsDomainItem()) {
                self.updateDomainNameOptions();
            }
        });

        self.selectedDomainName.subscribe(self._handleSelectedDomainName);
    }



    /** Create and object to relay status of domain connection. */
    function DomainStatusModel() {
        var self = this;

        self.domainNameHasBeenSelected = ko.observable(false);
        self.domainNameOptionsCount = ko.observable(0);
    }



    /* Module exports */
    _.extend(exports, {
        DomainStatusModel: DomainStatusModel,
        DomainConnectionModel: DomainConnectionModel
    });

})(Atomia.ViewModels, _, ko, Atomia.Utils);
