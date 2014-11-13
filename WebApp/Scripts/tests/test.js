/// <reference path="../viewmodels/domainreg.js" />

QUnit.test("basic", function (assert) {
    ko.applyBindings(Atomia.ViewModel.DomainReg);

    Atomia.ViewModel.DomainReg.results.push("bloop");

    assert.ok(Atomia.ViewModel.DomainReg.hasResults(), "Yay!");
});
