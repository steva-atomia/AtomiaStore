/// <reference path="../../../../Scripts/underscore.js" />
/// <reference path="../../../../Scripts/knockout-3.2.0.debug.js" />
/// <reference path="atomia.utils.js" />

/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, utils) {
    'use strict';

    var LanguageSelectorModelPrototype,
        CreateLanguageSelectorModel,
        CreateLanguageItem;

    /**
     * Utility function to convert current URL to a URL with ?lang= query string for switching language
     * @param {string} currentLanguageCode - Code of currently used language
     * @param {string} newLanguageCode - Code of language to switch to
     */
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

    /**
     * Create a language item.
     * @param {Object|Function} extensions - Extensions to the default language item view model.
     * @param {Object} instance - The object to create language item from
     */
    CreateLanguageItem = function CreateLanguageItem(extensions, instance) {
        var defaults;
        
        defaults = {
            ChangeLanguageUrl: getNewLanguageURL(instance._CurrentLanguage.Tag, instance.Tag)
        };
        
        return utils.createViewModel({}, defaults, instance, extensions);
    };

    LanguageSelectorModelPrototype = {
        /** Open or close language selector based on current state. */
        ToggleDropdown: function ToggleDropdown() {
            if (this.IsOpen()) {
                this.IsOpen(false);
            }
            else {
                utils.publish('dropdown:open');
                this.IsOpen(true);
            }
        },

        /** Close language selector. */
        CloseDropdown: function CloseDropdown() {
            this.IsOpen(false);
        },

        /** Load language data generated on server. */
        Load: function Load(getLanguagesResponse) {
            this._UpdateLanguages(getLanguagesResponse.data);
        },

        _UpdateLanguages: function _UpdateLanguages(data) {
            var tmpLanguages = [],
                currentLanguage = data.CurrentLanguage;

            this.SelectedLanguage(currentLanguage);

            _.each(data.Languages, function (language) {
                language._CurrentLanguage = currentLanguage;

                tmpLanguages.push(this.CreateLanguageItem(language));
            }, this);

            this.Languages(tmpLanguages);
        }
    };


    /**
     * Create language selector view model.
     * @param {Object|Function} extensions - Extensions to the default language selector view model.
     * @param {Object|Function} itemExtensions - Extensions to the default language item selector
     */
    CreateLanguageSelectorModel = function CreateLanguageSelectorModel(extensions, itemExtensions) {
        var defaults, viewModel;

        defaults = {
            CreateLanguageItem: _.partial(CreateLanguageItem, itemExtensions || {}),
            IsOpen: ko.observable(false),
            Languages: ko.observableArray(),
            SelectedLanguage: ko.observable()
        };

        viewModel = utils.createViewModel(LanguageSelectorModelPrototype, defaults, extensions);

        utils.subscribe('dropdown:open', function () {
            viewModel.IsOpen(false);
        });

        return viewModel;
    };


    _.extend(exports, {
        CreateLanguageSelectorModel: CreateLanguageSelectorModel
    });

})(Atomia.ViewModels, _, ko, Atomia.Utils);
