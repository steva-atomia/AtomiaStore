/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (module, _, ko, amplify) {
    'use strict';

    var DomainConnection, DomainStatus, DomainConnectionFactory;

    DomainStatus = function DomainStatus() {
        this.DomainNameHasBeenSelected = ko.observable(false);
        this.DomainNameOptionsCount = ko.observable(0);
    };



    /* DomainConnection and prototype */
    DomainConnection = function DomainConnection() {
        
        this.SelectedItem = undefined; // Set in Init

        this.UniqueId = _.uniqueId('domain-connection-');

        this.DomainNameOptions = ko.observableArray();
        this.SelectedDomainName = ko.observable();
        this.SelectedDomainName.subscribe(this._DomainNameSelected, this);

        _.bindAll(this, 'Init', 'SetInitialDomainName', '_UpdateDomainNameOptions', '_DomainNameSelected');
    };

    DomainConnection.prototype = {
        Init: function (cart, selectedItem, statusNotifier) {
            
            this._Cart = cart;
            this._StatusNotifier = statusNotifier;
            
            this._UpdateDomainNameOptions();

            if (_.isFunction(selectedItem)) {
                this.SelectedItem = selectedItem;
            }
            else {
                this.SelectedItem = function () { return selectedItem; };
            }

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

            if (this._StatusNotifier !== undefined) {
                this._StatusNotifier.DomainNameOptionsCount(domainNames.length);
            }
        },

        _DomainNameSelected: function (selectedDomainName) {
            var selectedItem = this.SelectedItem();

            if (this._StatusNotifier !== undefined) {
                this._StatusNotifier.DomainNameHasBeenSelected(selectedDomainName !== undefined);
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
        }
    };

    

    DomainConnectionFactory = function DomainConnectionFactory(params) {
        var viewModel = new DomainConnection();

        viewModel.Init(params.cart, params.selectedItem, params.statusNotifier);

        return viewModel;
    };


    /* Export models */
    module.DomainStatus = DomainStatus;
    module.DomainConnection = DomainConnection;
    module.DomainConnectionFactory = DomainConnectionFactory;

})(Atomia.ViewModels, _, ko, amplify);
