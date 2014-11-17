/// <reference path="../viewmodels/domainreg.js" />

var FakeDomainsApi = {
    findDomainsWasCalled: false,
    findDomains: function(searchTerm, callback) {
        FakeDomainsApi.findDomainsWasCalled = true;      
    }
};

QUnit.test("basic", function (assert) {
    var domainReg = Atomia.ViewModels.DomainReg(_, ko, FakeDomainsApi);

    domainReg.results.push("bloop");

    assert.ok(domainReg.hasResults(), "Yay!");
});

QUnit.test("test2", function (assert) {
    var domainReg = Atomia.ViewModels.DomainReg(_, ko, FakeDomainsApi);

    domainReg.submit();

    assert.ok(FakeDomainsApi.findDomainsWasCalled, "Yay!");
});
