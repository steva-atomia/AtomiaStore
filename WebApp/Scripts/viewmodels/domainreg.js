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
        });

    function submit() {
        results.removeAll();
        domainsApi.findDomains(query(), function (data) {
            _.each(data, function (result) {
                var item = new itemsApi.CartItem(result);
                results.push(item);
            });
        });
    }

    return {
        query: query,
        results: results,
        hasResults: hasResults,
        submit: submit
    };
};


if (Atomia.RootVM !== undefined) {
    Atomia.RootVM.DomainReg = Atomia.ViewModels.DomainReg(_, ko, Atomia.Domains, Atomia.Items);
}
