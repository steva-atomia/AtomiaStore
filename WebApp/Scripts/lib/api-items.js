/* jshint -W079 */
var Atomia = Atomia || {};
Atomia._unbound = Atomia._unbound || {};
/* jshint +W079 */

Atomia._unbound.Items = function () {
    'use strict';

    function Item() {

    }

    Item.prototype.addToCart = function() {

    };

    return {
        Item: Item
    };
};

Atomia.Items = Atomia._unbound.Items();
