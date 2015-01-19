/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (module, _, ko, amplify) {
    'use strict';

    var DomainConnection;


    /* ItemConnections and prototype */
    DomainConnection = function DomainConnection() {
        this.SelectedItem = undefined; // Set in Init

        this.DomainNameOptions = ko.observableArray();
        this.SelectedDomainName = ko.observable();
        this.SelectedDomainName.subscribe(this._DomainNameSelected, this);

        this.DomainHasBeenSelected = ko.pureComputed(function () {
            return this.SelectedDomainName() !== undefined;
        }, this);
        this.DomainNameOptionsCount = ko.pureComputed(function () {
            return this.DomainNameOptions().length;
        }, this);

        _.bindAll(this, 'Init', 'SetInitialDomainName', '_UpdateDomainNameOptions', '_DomainNameSelected');
    };

    DomainConnection.prototype = {
        Init: function (cart, itemObservable) {
            this._Cart = cart;

            if (!ko.isObservable(itemObservable)) {
                throw new Error('itemObservable must be a knockout observable.');
            }
            this.SelectedItem = itemObservable;

            this._UpdateDomainNameOptions();

            amplify.subscribe('cart:add', this, function (addedItem) {
                var selectedItem = this.SelectedItem();

                if (addedItem.Category === 'Domain') {
                    this._UpdateDomainNameOptions();
                }
                else if (selectedItem.Equals(addedItem)) {
                    this._DomainNameSelected(this.SelectedDomainName());
                }
            });

            amplify.subscribe('cart:remove', this, function (removedItem) {
                if (removedItem.Category === 'Domain') {
                    this._UpdateDomainNameOptions();
                }
            });
        },

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

        _UpdateDomainNameOptions: function () {
            var domainItems = this._Cart.DomainItems(),
                domainNames = [];

            if (domainItems !== undefined) {
                _.each(domainItems, function (item) {
                    var domainAttr = _.findWhere(item.CustomAttributes, { Name: 'DomainName' });
                    domainNames.push(domainAttr.Value);
                }.bind(this));
            }

            this.DomainNameOptions(domainNames);
        },

        _DomainNameSelected: function (selectedDomainName) {
            var selectedItem = this.SelectedItem();

            if (selectedItem === undefined) {
                return;
            }

            if (selectedDomainName !== undefined) {
                this._Cart.AddDomainName(selectedItem, selectedDomainName);
            }
            else {
                this._Cart.RemoveDomainName(selectedItem);
            }
        }
    };

    /* Export models */
    module.DomainConnection = DomainConnection;

})(Atomia.ViewModels, _, ko, amplify);
