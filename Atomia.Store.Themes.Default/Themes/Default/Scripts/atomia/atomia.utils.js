/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.Utils = Atomia.Utils || {};
/* jshint +W079 */

(function (exports, _, amplify) {
    'use strict';

    function request(options) {
        amplify.request(options);
    }

    function publish(topic, item) {
        amplify.publish(topic, item);
    }

    function subscribe(topic, handler, context) {
        if (context !== undefined) {
            amplify.subscribe(topic, context, handler);
        }
        else {
            amplify.subscribe(topic, handler);
        }
    }

    function createViewModel(proto) {
        var viewModel, extensions, protoFuncs;
        
        if (!_.isObject(proto)) {
            return proto;
        }

        viewModel = Object.create(proto);
        protoFuncs = _.functions(proto);

        if (protoFuncs.length > 0) {
            _.bindAll.apply(_, [viewModel].concat(protoFuncs));
        }

        extensions = [].slice.call(arguments, 1);

        _.each(extensions, function(extension) {
            viewModel = extendViewModel(viewModel, extension);
        });

        return viewModel;
    }

    function extendViewModel(viewModel, extension) {
        if (!_.isObject(viewModel)) {
            return viewModel;
        }

        if (_.isFunction(extension)) {
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
