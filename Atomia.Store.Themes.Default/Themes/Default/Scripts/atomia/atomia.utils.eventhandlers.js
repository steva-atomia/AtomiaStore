/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.Utils = Atomia.Utils || {};
/* jshint +W079 */

(function (exports, $, utils) {
    'use strict';

    /** Capture ajax error message and publish a notification message. */
    function warnAjaxErrors(title, message) {

        utils.subscribe('request.error', function () {
            utils.publish('uiSetNotification', {
                title: title,
                message: message,
                messageType: 'warning'
            });
        });
    }

    /** Atomia customer validation plugin expects an event on body */
    function relayCartUpdateToValidation() {
        utils.subscribe('cart:update', function () {
            $('body').trigger('cart:update');
        });
    }

    _.extend(exports, {
        warnAjaxErrors: warnAjaxErrors,
        relayCartUpdateToValidation: relayCartUpdateToValidation
    });

})(Atomia.Utils, jQuery, Atomia.Utils);
