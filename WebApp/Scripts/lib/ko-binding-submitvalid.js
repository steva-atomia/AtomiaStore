/* jshint unused: false */
(function ($, ko) {
    'use strict';

    ko.bindingHandlers.submitValid = {
        init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
            var validatingValueAccessor = function (data, event) {
                return function (data, event) {
                    if ($(element).valid()) {
                        valueAccessor().call(viewModel, data, event);
                    }
                };
            };

            ko.bindingHandlers.submit.init(element, validatingValueAccessor, allBindings, viewModel, bindingContext);
        }
    };

} (jQuery, ko));
