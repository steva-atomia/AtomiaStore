var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};

(function (exports, _, ko) {
    'use strict';

    /**
     * Utility function to convert current URL to a URL with ?cc= query string for switching currency
     * @param {string} currentCurrencyCode - Code of currently used currency
     * @param {string} newCurrencyCode - Code of currency to switch to
     */
    function getNewCurrencyURL(currentCurrencyCode, newCurrencyCode) {
        var currentURL = window.location.href,
            newURL,
            currentQueryString = '?ccy=' + currentCurrencyCode,
            newQueryString = '?ccy=' + newCurrencyCode;

        if (currentURL.indexOf(currentQueryString) !== -1) {
            newURL = currentURL.replace(currentQueryString, newQueryString);
        }
        else {
            newURL = currentURL + newQueryString;
        }

        return newURL;
    }

    /**
     * Create a currency item.
     * @param {Object} currencyData - The object to create currency item from
     * @param {Object} currentCurrencyData - Instance of current selected currency
     */
    function CurrencyItem(currencyData, currentCurrencyData) {
        var self = this;

        self.code = currencyData.Code;
        self.name = currencyData.Name;
        self.changeCurrencyUrl = getNewCurrencyURL(currentCurrencyData.Code, currencyData.Code);
    }

    /** Create currency selector view model. */
    function CurrencySelectorModel() {
        var self = this;

        self.currencies = ko.observableArray();
        self.selectedCurrency = ko.observable();

        /** Creates a new CurrencyItem object from 'currencyData' and 'currentCurrencyData' */
        self.createCurrencyItem = function (currencyData, currentCurrencyData) {
            return new CurrencyItem(currencyData, currentCurrencyData);
        };

        /** Load currency data generated on server. */
        self.load = function load(response) {
            var tmpCurrencies = [];
            var currentCurrencyData = response.data.CurrentCurrency;
            var currentCurrency = self.createCurrencyItem(currentCurrencyData, currentCurrencyData);

            self.selectedCurrency(currentCurrency);

            _.each(response.data.Currencies, function (currencyData) {
                tmpCurrencies.push(self.createCurrencyItem(currencyData, currentCurrencyData));
            });

            self.currencies(tmpCurrencies);
        };
    }

    _.extend(exports, {
        getNewCurrencyURL: getNewCurrencyURL,
        CurrencyItem: CurrencyItem,
        CurrencySelectorModel: CurrencySelectorModel
    });

})(Atomia.ViewModels, _, ko);
