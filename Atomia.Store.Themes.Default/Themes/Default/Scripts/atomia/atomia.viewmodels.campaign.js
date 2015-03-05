/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, utils) {
    'use strict';

    var CampaignModelPrototype,
        CreateCampaignModel;


    CampaignModelPrototype = {
        AddToCart: function AddToCart() {
            var code = this.Code();

            if (_.isString(code) && code !== '') {
                this._Cart.AddCampaignCode(code);
                this.Added(true);
                this.Code('');
            }
        }
    };

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
