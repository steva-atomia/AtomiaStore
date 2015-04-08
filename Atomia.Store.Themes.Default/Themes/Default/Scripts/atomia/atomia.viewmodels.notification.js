/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, utils) {
    'use strict';

    /** Create notification model. */
    function NotificationModel() {
        var self = this;
     
        self.messageType = ko.observable('');
        self.title = ko.observable('');
        self.message = ko.observable('');
        self.isOpen = ko.observable(false);

        /**
         * Open notification.
         * @param {string} title - Notification title
         * @param {string} message - Notification message
         * @param {string} messageType - Notification message type
         */
        self.open = function open(title, message, messageType) {
            if (self.isOpen()) {
                self.close();
            }

            self.title(title);
            self.message(message);
            self.messageType(messageType);
            self.isOpen(true);
        };

        /** Close notification */
        self.close = function close() {
            self.isOpen(false);
            self.title('');
            self.message('');
            self.messageType('');
        };

        /** Register subscription on ajax errors and notify */
        self.notifyAjaxErrors = function notifyAjaxErrors(title, message, messageType) {
            utils.subscribe('request.error', function () {
                self.open(title, message, messageType);
            });
        };
    }


    _.extend(exports, {
        NotificationModel: NotificationModel
    });

})(Atomia.ViewModels, _, ko, Atomia.Utils);
