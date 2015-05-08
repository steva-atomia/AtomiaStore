/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko) {
    'use strict';

    /** Create a Knockout view model for using separate WHOIS contact. */
    function WhoisModel() {
        var self = this;

        self.whoisContactCustomerType = ko.observable('individual');
        self.otherWhoisContact = ko.observable(false);

        self.whoisContactIsCompany = ko.pureComputed(function () {
            return self.whoisContactCustomerType() === 'company';
        });

        /** Use other WHOIS contact than main */
        self.useOtherWhoisContact = function useOtherWhoisContact() {
            self.otherWhoisContact(true);
        };

        /** Use main as WHOIS contact */
        self.useMainAsWhoisContact = function useMainAsWhoisContact() {
            self.otherWhoisContact(false);
        };
    }


    /* Module exports */
    _.extend(exports, {
        WhoisModel: WhoisModel
    });

})(Atomia.ViewModels, _, ko);
