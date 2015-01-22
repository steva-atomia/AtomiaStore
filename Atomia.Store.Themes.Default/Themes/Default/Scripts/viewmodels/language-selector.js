/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, utils) {
    'use strict';

    var LanguageSelectorPrototype,
        CreateLanguageSelector,
        CreateLanguage;

    function getNewLanguageURL(currentLanguageCode, newLanguageCode) {
        var currentURL = window.location.href,
            newURL,
            currentQueryString = '?lang=' + currentLanguageCode,
            newQueryString = '?lang=' + newLanguageCode;

        if (currentURL.indexOf(currentQueryString) !== -1) {
            newURL = currentURL.replace(currentQueryString, newQueryString);
        }
        else {
            newURL = currentURL + newQueryString;
        }

        return newURL;
    }

    CreateLanguage = function CreateLanguage(extensions, instance) {
        var defaults;
        
        defaults = {
            ChangeLanguageUrl: getNewLanguageURL(instance._CurrentLanguage.Code, instance.Code)
        };
        
        return utils.createViewModel({}, defaults, instance, extensions);
    };

    LanguageSelectorPrototype = {
        ToggleDropdown: function ToggleDropdown() {
            this.IsOpen() ? this.IsOpen(false) : this.IsOpen(true);
        },

        Load: function Load(getLanguagesResponse) {
            this._UpdateLanguages(getLanguagesResponse.data);
        },

        _UpdateLanguages: function _UpdateLanguages(data) {
            var tmpLanguages = [],
                currentLanguage = data.CurrentLanguage;

            this.SelectedLanguage(currentLanguage);

            _.each(data.Languages, function (language) {
                language._CurrentLanguage = currentLanguage;

                tmpLanguages.push(this.CreateLanguage(language));
            }, this);

            this.Languages(tmpLanguages);
        }
    };

    CreateLanguageSelector = function CreateLanguageSelector(extensions, itemExtensions) {
        var defaults;

        defaults = {
            CreateLanguage: _.partial(CreateLanguage, itemExtensions || {}),
            IsOpen: ko.observable(false),
            Languages: ko.observableArray(),
            SelectedLanguage: ko.observable()
        };

        return utils.createViewModel(LanguageSelectorPrototype, defaults, extensions);
    };


    /* Module exports */
    _.extend(exports, {
        CreateLanguageSelector: CreateLanguageSelector
    });

})(Atomia.ViewModels, _, ko, Atomia.Utils);
