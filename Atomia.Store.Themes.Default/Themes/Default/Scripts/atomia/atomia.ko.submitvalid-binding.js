/// <reference path="../../../../Scripts/jquery-2.1.3.intellisense.js" />
/// <reference path="../../../../Scripts/knockout-3.2.0.debug.js" />

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
