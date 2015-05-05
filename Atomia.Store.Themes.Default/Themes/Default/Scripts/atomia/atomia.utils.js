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

    /** Creates a single object by mixing constructors. */
    function mix(/* constructors */) {
        var i, obj = {};

        for (i = 0; i < arguments.length; i += 1) {
            arguments[i].call(obj);
        }

        return obj;
    }

    _.extend(exports, {
        request: request,
        publish: publish,
        subscribe: subscribe,
        mix: mix
    });

})(Atomia.Utils, _, amplify);
