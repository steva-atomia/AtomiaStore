/// <reference path="../../../../Scripts/underscore.js" />
/// <reference path="../../../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="atomia.utils.js" />

/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, utils) {
    'use strict';

    var CampaignModelPrototype,
        CreateCampaignModel;


    CampaignModelPrototype = {
        /** Add campaign code to cart. */
        AddToCart: function AddToCart() {
            var code = this.Code();
            if (_.isString(code) && code !== '') {
                this._Cart.AddCampaignCode(code);
                this.Added(true);
                this.Code('');
            }
        }
    };

    /** 
     * Knockout view model for adding a campaign code to cart. 
     * @param {Object} cart - Instance of cart wher campaign code can be added.
     * @param {Object|Function} extensions - Extensions to the default campaign view model.
     * @returns the created campaign view model
     */
    CreateCampaignModel = function CreateCampaignModel(cart, extensions) {
        return utils.createViewModel(CampaignModelPrototype, {
            _Cart: cart,
            Code: ko.observable(''),
            Added: ko.observable(false)
        }, extensions);
    };


    /* Module exports */
    _.extend(exports, {
        CreateCampaignModel: CreateCampaignModel
    });

})(Atomia.ViewModels, _, ko, Atomia.Utils);
