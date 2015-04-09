/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, utils, viewModels) {
    'use strict';

    /** Create notification model. */
    function NotificationModel() {
        var self = _.extend(this, new viewModels.ModalMixin());
     
        self.title = ko.observable('');
        self.message = ko.observable('');
        self.messageType = ko.observable('');

        utils.subscribe('uiSetNotification', function (notification) {
            self.title(notification.title || 'Notification');
            self.message(notification.message);
            self.messageType(notification.messageType || 'info');

            self.openModal();
        });
    }

    _.extend(exports, {
        NotificationModel: NotificationModel
    });

})(Atomia.ViewModels, _, ko, Atomia.Utils, Atomia.ViewModels);
