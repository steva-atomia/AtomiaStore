/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, utils) {
    'use strict';

    function DomainMoveModel() {
        var self = this;

        self.query = ko.observable();

        self.submit = function submit() {
            //console.log('Transfer!');
        };
    }

    /* Module exports */
    _.extend(exports, {
        DomainMoveModel: DomainMoveModel
    });

})(Atomia.ViewModels, _, ko, Atomia.Utils);
