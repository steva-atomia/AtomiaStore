/// <reference path="../../../Themes/Default/Themes/Default/Scripts/viewmodels/domainreg.js" />

var FakeDomainsApi = {
    findDomainsWasCalled: false,
    findDomains: function () {
        'use strict';

        FakeDomainsApi.findDomainsWasCalled = true;
    }
};

QUnit.test('basic', function (assert) {
    'use strict';

    var domainReg = Atomia.ViewModels.DomainReg(_, ko, FakeDomainsApi);

    domainReg.results.push('bloop');

    assert.ok(domainReg.hasResults(), 'Yay!');
});

QUnit.test('test2', function (assert) {
    'use strict';

    var domainReg = Atomia.ViewModels.DomainReg(_, ko, FakeDomainsApi);

    domainReg.submit();

    assert.ok(FakeDomainsApi.findDomainsWasCalled, 'Yay!');
});
