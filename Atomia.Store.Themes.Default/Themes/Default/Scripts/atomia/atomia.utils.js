/// <reference path="../../../../Scripts/amplify-vsdoc.js" />
/// <reference path="../../../../Scripts/underscore.js" />

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

    /**
     * Create a view model from prototype object with optional extensions.
     * @param {Object}             proto       - Prototype for the object to create
     * @param {Object|Function}    arguments   - Remaining arguments extend the created view model, see extendViewModel
     * @returns {Object} The created view model
     */
    function createViewModel(proto) {
        var viewModel, extensions, protoFuncs;
        
        if (!_.isObject(proto)) {
            return proto;
        }

        // Create the view model instance.
        viewModel = Object.create(proto);

        // Bind all functions on the prototype to the instance (sets "this" inside the functions.)
        protoFuncs = _.functions(proto);
        if (protoFuncs.length > 0) {
            _.bindAll.apply(_, [viewModel].concat(protoFuncs));
        }

        // Any other arguments are treated as extensions.
        extensions = [].slice.call(arguments, 1);
        _.each(extensions, function(extension) {
            viewModel = extendViewModel(viewModel, extension);
        });

        return viewModel;
    }

    /** 
     * Extend the 'viewModel' with and object or function 'extension'.
     * @param {Object} viewModel - The view model object to extend.
     * @param {Object|Function} extension - If object properties get copied, if function it is called with the 'viewModel' as argument.
     */
    function extendViewModel(viewModel, extension) {
        if (!_.isObject(viewModel)) {
            return viewModel;
        }

        if (_.isFunction(extension)) {
            // extension is a function so call it with the viewModel
            return _.extend(viewModel, extension(viewModel));
        }
        else if (_.isObject(extension)) {
            return _.extend(viewModel, extension);
        }

        return viewModel;
    }

    _.extend(exports, {
        request: request,
        publish: publish,
        subscribe: subscribe,
        createViewModel: createViewModel,
        extendViewModel: extendViewModel
    });

})(Atomia.Utils, _, amplify);
