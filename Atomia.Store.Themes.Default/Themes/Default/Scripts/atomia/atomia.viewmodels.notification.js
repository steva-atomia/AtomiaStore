/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, utils) {
    'use strict';

    var CreateNotification, CreateNotificationPrototype, timeoutId;

    CreateNotificationPrototype = {
        Open: function Open(title, message, messageType) {
            if (this.IsOpen()) {
                this.Close();
            }

            this.Title(title);
            this.Message(message);
            this.MessageType(messageType);
            this.IsOpen(true);
        },

        Flash: function Flash(title, message, messageType, timeout) {

            if (timeout === undefined) {
                timeout = 10000;
            }

            this.Open(title, message, messageType);

            timeoutId = setTimeout(function () {
                this.Close();
            }.bind(this), timeout);
        },

        Close: function Close() {
            this.IsOpen(false);
            this.Title('');
            this.Message('');
            this.MessageType('');

            clearTimeout(timeoutId);
        },

        NotifyAjaxErrors: function NotifyAjaxErrors(title, message, messageType) {
            utils.subscribe('request.error', function () {
                this.Open(title, message, messageType);
            }.bind(this));
        }
    };


    CreateNotification = function CreateNotification(extensions) {
        return utils.createViewModel(CreateNotificationPrototype, {
            MessageType: ko.observable(''),
            Title: ko.observable(''),
            Message: ko.observable(''),
            IsOpen: ko.observable(false),
        }, extensions);
    };


    /* Module exports */
    _.extend(exports, {
        CreateNotification: CreateNotification
    });

})(Atomia.ViewModels, _, ko, Atomia.Utils);
