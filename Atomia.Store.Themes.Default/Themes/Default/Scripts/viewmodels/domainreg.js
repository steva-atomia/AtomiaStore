/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

Atomia.ViewModels.DomainReg = function (_, ko, domainsApi, itemsApi) {
    'use strict';

    var query = ko.observable(),
        results = ko.observableArray(),
        hasResults = ko.pureComputed(function () {
            return results().length > 0;
        }),
        isLoadingResults = ko.observable(false),
        extendItem = function (item) {
            return item;
        };

    function submit() {
        isLoadingResults(true);

        results.removeAll();

        domainsApi.findDomains(query(), function (data) {
            _.each(data, function (result) {
                var baseItem = new itemsApi.CartItem(result),
                    item;

                // Make some properties more easily accessible.
                baseItem.DomainName = _.find(baseItem.CustomAttributes, function (i) { return i.Name === 'DomainName'; }).Value;
                baseItem.Status = _.find(baseItem.CustomAttributes, function (i) { return i.Name === 'Status'; }).Value;

                item = extendItem(baseItem);

                if (item === undefined) {
                    throw Error('extendItems function must return an item');
                }

                results.push(item);
            });

            isLoadingResults(false);
        });
    }

    // Allows for setting a function that can extend or transform an item before being pushed to results array.
    function extendItems(itemExtenderFn) {
        extendItem = itemExtenderFn;
    }

    return {
        query: query,
        results: results,
        hasResults: hasResults,
        isLoadingResults: isLoadingResults,
        submit: submit,
        extendItems: extendItems
    };
};


if (Atomia.RootVM !== undefined) {
    Atomia.RootVM.DomainReg = Atomia.ViewModels.DomainReg(_, ko, Atomia.Domains, Atomia.Items);
}
