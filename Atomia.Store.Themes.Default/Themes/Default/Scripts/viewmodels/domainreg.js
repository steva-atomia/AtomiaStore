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
        showMoreResults = ko.observable(false);

    function submit() {
        isLoadingResults(true);

        results.removeAll();

        domainsApi.findDomains(query(), function (data) {
            _.each(data, function (result, index) {
                var item = new itemsApi.CartItem(result),
                    domainParts;

                item.Id = 'dmn' + index;

                // Make some properties more easily accessible.
                item.DomainName = _.find(item.CustomAttributes, function (i) { return i.Name === 'DomainName'; }).Value;
                item.Status = _.find(item.CustomAttributes, function (i) { return i.Name === 'Status'; }).Value;
                item.Price = item.PricingVariants[0].Price;

                domainParts = item.DomainName.split('.');
                item.DomainNameSld = domainParts[0];
                item.DomainNameTld = domainParts[1];

                results.push(item);
            });

            isLoadingResults(false);
        });
    }

    function setShowMoreResults() {
        showMoreResults(true);
    }

    return {
        query: query,
        results: results,
        hasResults: hasResults,
        isLoadingResults: isLoadingResults,
        submit: submit,
        setShowMoreResults: setShowMoreResults,
        showMoreResults: showMoreResults
    };
};


if (Atomia.RootVM !== undefined) {
    Atomia.RootVM.DomainReg = Atomia.ViewModels.DomainReg(_, ko, Atomia.Domains, Atomia.Items);
}
