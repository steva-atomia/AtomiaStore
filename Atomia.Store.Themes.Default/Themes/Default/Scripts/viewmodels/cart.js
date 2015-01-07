/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

Atomia.ViewModels.Cart = function (_, ko, request) {
    'use strict';

    var items = ko.observableArray();

    function addItem(item, success) {
        if (!_.has(item, 'ArticleNumber')) {
            throw new Error('Object must have ArticleNumber property to be added to cart.');
        }

        var data = _.pick(item, 'ArticleNumber', 'RenewalPeriod');
        _.defaults(data, { RenewalPeriod: { Period: 1, Unit: 'YEAR' }, Quantity: 1 });

        request({
            resourceId: 'Cart.AddItem',
            data: data,
            success: function (data) {
                items.push(data);

                if (success !== undefined) {
                    success();
                }

                console.log("Success adding!")
                console.log(data);
            },
            error: function (data) {
                console.log("Error adding!")
                console.log(data);
            }
        });
    }

    function removeItem(item, success) {
        console.log("Removed!")
        if (success !== undefined) {
            success();
        }
    }

    return {
        items: items,
        addItem: addItem,
        removeItem: removeItem
    };
};

if (Atomia.RootVM !== undefined) {
    Atomia.RootVM.Cart = Atomia.ViewModels.Cart(_, ko, amplify.request);
}
