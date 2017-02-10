(function ($, ko) {
    'use strict';

    /** Show or hide element with a jQuery slide animation. Binding option 'slideDuration' sets animation time in ms. */
    ko.bindingHandlers.validateCustomField = {
        init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
            var rule = {};
            rule['CustomFields_' + bindingContext.$data.name] = true;

            setTimeout(function () {
                if ($(element)[0].form) {
                    if (Object.keys($(element).rules()).length > 0) {
                        $(element).rules('remove');
                    }

                    $(element).rules('add', rule);
                }

            }, 0);
        }
    };

}(jQuery, ko));