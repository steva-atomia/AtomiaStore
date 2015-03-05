/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, utils) {
    'use strict';

    var DomainMoveModelPrototype,
        CreateDomainMoveModel;
        

    /* Domain move prototype and factory */
    DomainMoveModelPrototype = {
        Submit: function Submit() {
            console.log('Transfer!');
        }
    };

    CreateDomainMoveModel = function CreateDomainMoveModel(extensions) {
        return utils.createViewModel(DomainMoveModelPrototype, {
            Query: ko.observable()
        }, extensions);
    };



    /* Module exports */
    _.extend(exports, {
        CreateDomainMoveModel: CreateDomainMoveModel
    });

})(Atomia.ViewModels, _, ko, Atomia.Utils);
