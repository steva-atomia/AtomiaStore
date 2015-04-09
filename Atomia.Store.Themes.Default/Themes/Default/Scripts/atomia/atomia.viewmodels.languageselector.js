/* jshint -W079 */
var Atomia = Atomia || {};
Atomia.ViewModels = Atomia.ViewModels || {};
/* jshint +W079 */

(function (exports, _, ko, utils) {
    'use strict';

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
     * @param {Object} instance - The object to create language item from
     */
    function LanguageItem(languageData, currentLanguageData) {
        var self = this;

        self.shortName = languageData.ShortName;
        self.name = languageData.Name;
        self.changeLanguageUrl = getNewLanguageURL(currentLanguageData.Tag, languageData.Tag);
    }

    
    /** Create language selector view model. */
    function LanguageSelectorModel() {
        var self = this;

        self.languages = ko.observableArray();
        self.selectedLanguage = ko.observable();


        /** Creates a new LanguageItem object from 'languageData' and 'currentLanguageData' */
        self.createLanguageItem = function(languageData, currentLanguageData) {
            return new LanguageItem(languageData, currentLanguageData);
        };


        /** Load language data generated on server. */
        self.load = function load(response) {
            var tmpLanguages = [];
            var currentLanguageData = response.data.CurrentLanguage;
            var currentLanguage = self.createLanguageItem(currentLanguageData, currentLanguageData);

            self.selectedLanguage(currentLanguage);

            _.each(response.data.Languages, function (languageData) {
                tmpLanguages.push(self.createLanguageItem(languageData, currentLanguageData));
            });

            self.languages(tmpLanguages);
        };
    }


    _.extend(exports, {
        getNewLanguageURL: getNewLanguageURL,
        LanguageItem: LanguageItem,
        LanguageSelectorModel: LanguageSelectorModel
    });

})(Atomia.ViewModels, _, ko, Atomia.Utils);
