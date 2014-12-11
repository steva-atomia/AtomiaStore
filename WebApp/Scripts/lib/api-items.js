/* jshint -W079 */
var Atomia = Atomia || {};
Atomia._unbound = Atomia._unbound || {};
/* jshint +W079 */

Atomia._unbound.Items = function (_, request) {
    'use strict';

    function CartItem(item) {
        _.extend(this, item);
    }

    CartItem.prototype.addToCart = function () {
        var data = _.pick(this, 'ArticleNumber', 'RenewalPeriod');
        _.defaults(data, { RenewalPeriod: { Period: 1 } });

        request({
            resourceId: 'Cart.AddItem',
            data: data,
            success: function (data) {
                console.log(data);
            },
            error: function (data) {
                console.log(data);
            }
        });
    };

    return {
        CartItem: CartItem
    };
};

Atomia.Items = Atomia._unbound.Items(_, amplify.request);
