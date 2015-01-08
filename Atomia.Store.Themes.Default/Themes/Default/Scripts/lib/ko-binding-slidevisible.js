(function ($, ko) {
    'use strict';

    ko.bindingHandlers.slideVisible = {
        init: function (element, valueAccessor) {
            var val = ko.unwrap(valueAccessor());
            $(element).toggle(val);
        },
        update: function (element, valueAccessor, allBindings) {
            var val = ko.unwrap(valueAccessor()),
                duration = allBindings.get('slideDuration') || 500;

            if (val === true) {
                $(element).slideDown(duration);
            }
            else {
                $(element).slideUp(duration);
            }
        }
    };

}(jQuery, ko));