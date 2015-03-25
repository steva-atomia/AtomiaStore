/// <reference path="../../../../Scripts/underscore.js" />
/// <reference path="../../../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="atomia.utils.js" />

/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, utils) {
    'use strict';

    var NotificationModelPrototype,
        CreateNotificationModel,
        timeoutId;

    NotificationModelPrototype = {
        /**
         * Open notification.
         * @param {string} title - Notification title
         * @param {string} message - Notification message
         * @param {string} messageType - Notification message type
         */
        Open: function Open(title, message, messageType) {
            if (this.IsOpen()) {
                this.Close();
            }

            this.Title(title);
            this.Message(message);
            this.MessageType(messageType);
            this.IsOpen(true);
        },

        /**
         * Flash notification that closes after a set timeout
         * @param {string} title - Notification title
         * @param {string} message - Notification message
         * @param {string} messageType - Notification message type
         * @param {number} timeout - Number of ms message stays open, defaults to 10000
         */
        Flash: function Flash(title, message, messageType, timeout) {

            if (timeout === undefined) {
                timeout = 10000;
            }

            this.Open(title, message, messageType);

            timeoutId = setTimeout(function () {
                this.Close();
            }.bind(this), timeout);
        },

        /** Close notification */
        Close: function Close() {
            this.IsOpen(false);
            this.Title('');
            this.Message('');
            this.MessageType('');

            clearTimeout(timeoutId);
        },

        /** Register subscription on ajax errors and notify */
        NotifyAjaxErrors: function NotifyAjaxErrors(title, message, messageType) {
            utils.subscribe('request.error', function () {
                this.Open(title, message, messageType);
            }.bind(this));
        }
    };


    /**
     * Create notification model. 
     * @param {Object|Function} - Extensions to default notification model
     */
    CreateNotificationModel = function CreateNotificationModel(extensions) {
        return utils.createViewModel(NotificationModelPrototype, {
            MessageType: ko.observable(''),
            Title: ko.observable(''),
            Message: ko.observable(''),
            IsOpen: ko.observable(false),
        }, extensions);
    };


    _.extend(exports, {
        CreateNotificationModel: CreateNotificationModel
    });

})(Atomia.ViewModels, _, ko, Atomia.Utils);
