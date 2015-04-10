
(function ($, ko) {
    'use strict';

    /** Wraps default 'submit' binding with a validation check using jQuery Validation */
    ko.bindingHandlers.submitValid = {
        init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
            var validatingValueAccessor = function () {
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
