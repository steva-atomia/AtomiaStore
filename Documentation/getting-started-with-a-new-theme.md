Getting Started With a Custom Atomia Store Theme
================================================

The quickest way to get started with a new custom theme is to use the theme starter script by following the steps below. Running the example will generate a new empty theme called *MyTheme*.

1. Clone the AtomiaStore repository 

    `git clone https://github.com/atomia/AtomiaStore.git`

2. Open PowerShell and set execution policy 

    `Set-ExecutionPolicy RemoteSigned -Scope CurrentUser`
	
3. Open AtomiaStore\Solution\Store.sln with Visual Studio and rebuild project

4. Run the theme starter script in PowerShell

    `.\AtomiaStore\ThemeStarter\startnewtheme.ps1 MyTheme`

5. Open `MyTheme\MyTheme.sln` in Visual Studio 2013 or above.

6. If you do not have a complete Atomia development environment installed, you can use fake backend services by uncommenting the relevant unity sections in Web.config.

7. Run **DEBUG > Start Debugging** and an order page with the default theme will start.

8. Start customizing!


Next Steps
----------

Continue reading below for some more information about how the custom theme and supporting files are organized, or check the TOC in [README](../README.md) for specific customization task documentation.



Theme Structure
---------------

This is the file structure for a custom theme:

    Adapters/
    App_GlobalResources/
        MyTheme/
            MyThemeCommon.resx
            MyThemeCustomerValidation.resx
            MyThemeValidationErrors.resx
    App_Start/
        BundleConfig.cs
        RouteConfig.cs
        UnityConfig.cs
    Models/
    Themes/
        MyTheme/
            Content/
                css/
                    theme.css
            Scripts/
            Views/
                Web.config
    Transformation Files/
    GlobalEventsHandler.cs
    Global.asax
    Web.config


**Adapters**: New or customized backend components, like implementations of `IDomainsProvider` or `IProductListingProvider`.

**App_GlobalResources**: New or overridden resource strings. Only add the strings you want to override, not all available strings.

**App_Start/BundleConfig.cs**: Wrap any new stylesheets or script files in a separate bundle or override add to default bundles.

**App_Start/RouteConfig.cs**: Override default routes or add new routes, e.g. to a new product listing page.

**App_Start/UnityConfig.cs**: Override or add new adapters or models to the Unity config. Alternatively configure in Web.config

**Models**: Overridden default view models.

**Themes/MyTheme/Content**: Custom stylesheets, fonts and images.

**Themes/MyTheme/Scripts**: Custom scripts, either new or extending default scripts.

**Themes/MyTheme/Views**: Overriding default views or adding new views.

**Transformation Files**: Configuration changes.

**GlobalEventsHandler.cs**: Register any new RouteConfig, BundleConfig or UnityConfig. (Set which handler to use in Web.config)

**Global.asax**: This is only part of the project for technical reasons, to be able to run it standalone in Visual Studio.

**Web.config**: This is only part of the project for technical reasons, to be able to run it standalone in Visual Studio. Any real configuration customizations should be added to `Transformation Files\Web.config`

In addition to the files above there are a number of files added by `startnewtheme.ps1` that are not part of the theme, and not included in the solution:


Supporting Files
----------------

**Lib**: Reference assemblies that are also used for running the development instance of Atomia Store.

**Themes/Default/**: Views, content files and scripts from the Default theme.

**App_GlobalResources/*.resx**: Default resource strings and translations.

**App_Data/**: Configuration files needed to run the development instance.


Adding references
-----------------

By default the `Atomia.Store.Core.dll`, `Atomia.Store.AspNetMvc.dll`, and `Atomia.Store.Themes.Default` assemblies are referenced in the custom theme project. To add other references use **Add reference...** and *Browse* to the `MyTheme\Lib` folder, where all dll:s for Atomia Store are located.



Updating `Lib` and Default Theme Files
------------------------------------

Please note that any files created by `startnewtheme.ps1` that are not part of the theme solution might be replaced if you run the `updatetheme.ps1` script, so any changes you have made to these files will be overwritten.

1. Make sure your **AtomiaStore** repository is up-to-date, e.g. by ruSnning `git pull`.
2. Open PowerShell and set execution policy (see getting started instructions above).
2. Run `AtomiaStore\ThemeStarter\updatetheme.ps1 <ThemePath>`
3. The script will rebuild the AtomiaStore solution and copy and replace all supporting files in the theme directory.



Building and Installing the Custom Theme
----------------------------------------

When you are ready to deploy your custom theme, just build the theme solution with the **Release** configuration, and you will get a `publish` directory added next to your theme project directory. The `publish` directory contains all files you need to add to your installation of Atomia Store, and a file with instructions how to do it.


