/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, utils) {
    'use strict';

    var CreateDomainMovePrototype,
        CreateDomainMove;
        


    /* Domain move prototype and factory */
    CreateDomainMovePrototype = {
        Submit: function Submit() {
            console.log('Transfer!');
        }
    };

    CreateDomainMove = function CreateDomainMove(extensions) {
        return utils.createViewModel(CreateDomainMovePrototype, {
            Query: ko.observable()
        }, extensions);
    };



    /* Module exports */
    _.extend(exports, {
        CreateDomainMove: CreateDomainMove
    });

})(Atomia.ViewModels, _, ko, Atomia.Utils);
