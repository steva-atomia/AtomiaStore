/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, utils) {
    'use strict';

    var CreateProgress;


    CreateProgress = function CreateProgress(extensions) {
        return utils.createViewModel({}, {
            Step: ko.observable(0)
        }, extensions);
    };


    /* Module exports */
    _.extend(exports, {
        CreateProgress: CreateProgress
    });

})(Atomia.ViewModels, _, ko, Atomia.Utils);
