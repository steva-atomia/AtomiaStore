Param(
    [string]$themeDir
)

$themeName = Split-Path $themeDir -Leaf
$createDir = Join-Path (Join-Path (pwd) (Split-Path $themeDir -Parent)) $ThemeName
$themeDir = Join-Path $createDir $themeName
$themeSolution = "$($createDir)\$($themeName).sln"
$themeLibDir = "$($createDir)\Lib"

$templateDir = "$($PSScriptRoot)\MyTheme\"
$templateSolution = "$($PSScriptRoot)\MyTheme.sln"

$defaultThemeDir = "$($PSScriptRoot)\..\Atomia.Store.Themes.Default\"
$storeSolution = "$($PSScriptRoot)\..\Solution\Store.sln"

if (Test-Path $themeDir)
{
	echo "$($createDir) already exists. Aborting."
	Exit
}

# Copy theme project and solution templates
robocopy $templateDir $themeDir /e /xf MyTheme.csproj.user packages.config /xd bin obj
Copy-Item $templateSolution $themeSolution


# Compile Atomia Store solution if not done yet
if (-Not (Test-Path "$($defaultThemeDir)bin\Atomia.Store.Core.dll")) {
	$msbuild = "$($Env:SystemRoot)\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"
	Invoke-Expression "$($msbuild) $($storeSolution) /p:Configuration=Debug"
}


# Create Lib and copy bin files needed to run project on dev server, and for adding references
New-Item $themeLibDir -Type directory
Copy-Item "$($defaultThemeDir)bin\*.*" $themeLibDir


# Copy default theme files to have fallback when running dev server
Copy-Item "$($defaultThemeDir)\Themes\Default" "$($themeDir)\Themes\Default" -Recurse
Copy-Item "$($defaultThemeDir)\App_GlobalResources\*.resx" "$($themeDir)\App_GlobalResources" -Recurse


# Change project and solution names
Rename-Item "$($themeDir)\MyTheme.csproj" "$($themeName).csproj"
Rename-Item "$($themeDir)\Themes\MyTheme" $themeName
Rename-Item "$($themeDir)\App_GlobalResources\MyTheme" $themeName
Get-ChildItem "$($themeDir)\App_GlobalResources\$($themeName)"|Rename-Item -NewName {$_.name -replace 'MyTheme', $themeName }

(gc "$($themeDir)\$($themeName).csproj").replace('MyTheme', $themeName)|sc "$($themeDir)\$($themeName).csproj"
(gc "$($createDir)\$($themeName).sln").replace('MyTheme', $themeName)|sc "$($createDir)\$($themeName).sln"
(gc "$($themeDir)\Web.config").replace('MyTheme', $themeName)|sc "$($themeDir)\Web.config"

(gc "$($themeDir)\App_Start\BundleConfig.cs").replace('$MyTheme$', $themeName)|sc "$($themeDir)\App_Start\BundleConfig.cs"
(gc "$($themeDir)\App_Start\FilterConfig.cs").replace('$MyTheme$', $themeName)|sc "$($themeDir)\App_Start\FilterConfig.cs"
(gc "$($themeDir)\App_Start\RouteConfig.cs").replace('$MyTheme$', $themeName)|sc "$($themeDir)\App_Start\RouteConfig.cs"
(gc "$($themeDir)\App_Start\UnityConfig.cs").replace('$MyTheme$', $themeName)|sc "$($themeDir)\App_Start\UnityConfig.cs"
(gc "$($themeDir)\App_Start\OrderFlowConfig.cs").replace('$MyTheme$', $themeName)|sc "$($themeDir)\App_Start\OrderFlowConfig.cs"
(gc "$($themeDir)\GlobalEventsHandler.cs").replace('$MyTheme$', $themeName)|sc "$($themeDir)\GlobalEventsHandler.cs"
