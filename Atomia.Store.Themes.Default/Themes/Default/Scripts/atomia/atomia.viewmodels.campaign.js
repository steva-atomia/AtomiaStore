/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, utils) {
    'use strict';

    /** 
     * Knockout view model for adding a campaign code to cart. 
     * @param {Object} cart - Instance of cart where campaign code can be added.
     * @returns the created campaign view model
     */
    function CampaignCodeModel(cart) {
        var self = this;

        self.code = ko.observable('');
        self.added = ko.observable(false);

        /** Add campaign code to cart. */
        self.addToCart = function addToCart() {
            var code = self.code();

            if (_.isString(code) && code !== '') {
                cart.addCampaignCode(code);
                self.added(true);
                self.code('');
            }
        };
    };


    /* Module exports */
    _.extend(exports, {
        CampaignCodeModel: CampaignCodeModel
    });

})(Atomia.ViewModels, _, ko, Atomia.Utils);
