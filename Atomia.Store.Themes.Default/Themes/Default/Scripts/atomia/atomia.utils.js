/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.Utils = Atomia.Utils || {};
/* jshint +W079 */

(function (exports, _, amplify) {
    'use strict';

    /**
     * Make an AJAX request.
     * @param {Object}      options
     * @param {string}      options.resourceId   - What resource to call, see atomia.api.definitions.js
     * @param {Object}      options.data         - The request data.
     * @param {Function}    options.success      - Callback handler on success
     * @param {Function}    options.error        - Callback handler on error
     */
    function request(options) {
        return amplify.request(options);
    }

    /** Capture ajax error message and publish a notification message. */
    function warnAjaxErrors(title, message) {

        subscribe('request.error', function () {
            publish('uiSetNotification', {
                title: title,
                message: message,
                messageType: 'warning'
            });
        });
    }

    /**
     * Publish 'item' to 'topic' for broadcast to subscribers.
     * @param {string} topic - The topic to broadcast on.
     * @param {Object} item  - Optional data payload for subscribers.
     */
    function publish(topic, item) {
        amplify.publish(topic, item);
    }

    /**
     * Subscribe to a topic and invoke a handler on received message.
     * @param {string}      topic   - The topic to subscribe to.
     * @param {Function}    handler - Callback handler on received message.
     * @param {Object}      context - Optional object to bind 'this' to in the handler.
     */
    function subscribe(topic, handler, context) {
        if (context !== undefined) {
            amplify.subscribe(topic, context, handler);
        }
        else {
            amplify.subscribe(topic, handler);
        }
    }

    _.extend(exports, {
        request: request,
        warnAjaxErrors: warnAjaxErrors,
        publish: publish,
        subscribe: subscribe
    });

})(Atomia.Utils, _, amplify);
