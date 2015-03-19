Resource Strings and Localization
=================================

By convention, AtomiaStore uses the `App_GlobalResources\Common.resx` file and derived theme files and translations for all changable resource strings and translations except validation messages, which are located in `CustomerValidation.resx` and `ValidationErrors.resx`.

File Structure
--------------

An example file structure with themes and translations:

    App_GlobalResources/
        MyTheme/
            MyThemeCommon.resx
            MyThemeCommon.de-DE.resx
            MyThemeCustomerValidation.resx
            MyThemeCustomerValidation.de-DE.resx
            MyThemeValidationErrors.resx
            MyThemeValidationErrors.de-DE.resx
        Common.resx
        Common.de-DE.resx
        CustomerValidation.resx
        CustomerValidation.de-DE.resx
        ValidationErrors.resx
        ValidationErrors.de-DE.resx
        

The example above has the default English resource strings in the `resx` files without language code, and default German localization in the `*.de-DE.resx` files.

The theme resource files must be located in a directory with the same name as the theme, and must also be prefixed with that name, `MyTheme` in this example.

**Note**: Only the theme specific files should be changed when implementing a custom theme.


Adding or Overriding *Default* Resource Strings
-----------------------------------------------

When you start a new theme, empty resource files are added to the theme for all default (English) and localized `resx` files that are available in the *Default* theme. You can use these files to add your own resource strings, or override resource strings from the *Default* theme.

**Note:** You should only ever add resource strings to your theme `resx` files that are *not available* in the *Default* theme files, or that you want to *override*. Just adding the same content as in the *Default* theme `resx` files means you will not get the benefits of any spelling corrections or other improvements to the *Default* files.


Special Resource String
-----------------------

Some resource strings in `Common.resx` and derived files have a specific *Name* or *Value* format. These are annotated with comments in the `Common.resx` file in the *Default* theme.

E.g. language names are located via *Name* keys on the form `<LanguageTag>_name` and `<LanguageTag>_shortname`: `EN_name` - *English*, `EN_shortname` - *EN*, `FR_name` - *French*, `FR_shortname` - *FR* etc.