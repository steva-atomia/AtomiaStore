This is a starter project template for creating a custom theme for Atomia Store.

* Web.config is only included because it is needed to run the project in Visual Studio. Any permanent changes should be added to "Transformation Files/Web.MyTheme.Config".
* There is an included .gitignore file that ignores everything but the actual theme files. So if you have just checked out this theme from somewhere, you might want to use the `updatetheme.ps1` script from a checked out AtomiaStore to get a theme that compiles and runs.
* If you get complaints from ASP.NET about not finding the codebehind assembly when parsing Global.asax you want to cleanout the ASP.NET temp files typically located at `C:\Users\username\AppData\Local\Temp\Temporary ASP.NET Files` and restart Visual Studio. This is more likely to happen if you work with multiple themes, or when you run the `updatetheme.ps1` script.
* To get the latest default assemblies and Default theme files, run the `updatetheme.ps1` script from an up-to-date checkout out of the AtomiaStore repository.
