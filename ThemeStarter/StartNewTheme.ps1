Param(
    [string]$themeDir
)

# Set $PSScriptRoot on older versions of PowerShell
$PSVersion = ((Get-Host).Version).Major

if ($PSVersion -le 2) {
	$PSScriptRoot = Split-Path $MyInvocation.MyCommand.Path -Parent
}

$themeName = Split-Path $themeDir -Leaf
$createDir = Join-Path (Join-Path (pwd) (Split-Path $themeDir -Parent)) $ThemeName
$themeDir = Join-Path $createDir $themeName
$themeSolution = "$($createDir)\$($themeName).sln"
$themeLibDir = "$($createDir)\Lib"
$themeGitIgnore = "$($createDir)\.gitignore"

$templateDir = "$($PSScriptRoot)\MyTheme\"
$templateSolution = "$($PSScriptRoot)\MyTheme.sln"
$templateGitIgnore = "$($PSScriptRoot)\gitignore"

$defaultThemeDir = "$($PSScriptRoot)\..\Atomia.Store.Themes.Default\"
$storeSolution = "$($PSScriptRoot)\..\Solution\Store.sln"

if (Test-Path $themeDir)
{
	echo "$($createDir) already exists. Aborting."
	Exit
}

# Copy theme project, solution template and gitignore
echo "$templateDir"
robocopy "$templateDir " "$themeDir " /e /xf "MyTheme.csproj.user" "packages.config" /xd bin obj
Copy-Item "$templateSolution" "$themeSolution"
Copy-Item "$templateGitIgnore" "$themeGitIgnore"


# Compile Atomia Store solution if not done yet
if (-Not (Test-Path "$($defaultThemeDir)bin\Atomia.Store.Core.dll")) {
	$msbuild = "C:\Progra~2\MSBuild\14.0\bin\msbuild.exe"
	Invoke-Expression "$($msbuild) $($storeSolution) /p:Configuration=Debug"
}


# Create Lib and copy bin files needed to run project on dev server, and for adding references
New-Item $themeLibDir -Type directory
Copy-Item "$($defaultThemeDir)bin\*.*" $themeLibDir


# Copy default theme files to have fallback when running dev server
Copy-Item "$($defaultThemeDir)\Themes\Default" "$($themeDir)\Themes\Default" -Recurse
Copy-Item "$($defaultThemeDir)\App_GlobalResources\*.resx" "$($themeDir)\App_GlobalResources" -Recurse


# Change project and solution names
if ("$themeName" -ne "MyTheme")
{
	Rename-Item "$($themeDir)\MyTheme.csproj" "$($themeName).csproj"
	Rename-Item "$($themeDir)\Themes\MyTheme" $themeName
	Rename-Item "$($themeDir)\App_GlobalResources\MyTheme" $themeName
	Rename-Item "$($themeDir)\Transformation Files\Web.MyTheme.config" "Web.$($themeName).config"
}

Get-ChildItem "$($themeDir)\App_GlobalResources\$($themeName)"|Rename-Item -NewName {$_.name -replace 'MyTheme', $themeName }

if ($PSVersion -le 2) {
	(gc "$($themeDir)\$($themeName).csproj") -replace 'MyTheme', $themeName | sc "$($themeDir)\$($themeName).csproj"
	(gc "$($createDir)\$($themeName).sln") -replace 'MyTheme', $themeName | sc "$($createDir)\$($themeName).sln"
	(gc "$($themeDir)\Web.config") -replace 'MyTheme', $themeName | sc "$($themeDir)\Web.config"
	(gc "$($themeDir)\Transformation Files\Web.$($themeName).config") -replace 'MyTheme', $themeName | sc "$($themeDir)\Transformation Files\Web.$($themeName).config"
	(gc "$($themeDir)\publishtheme.ps1") -replace 'MyTheme', $themeName | sc "$($themeDir)\publishtheme.ps1"
	(gc "$($createDir)\.gitignore") -replace 'MyTheme', $themeName | sc "$($createDir)\.gitignore"

	(gc "$($themeDir)\App_Start\BundleConfig.cs") -replace '\$MyTheme\$', $themeName | sc "$($themeDir)\App_Start\BundleConfig.cs"
	(gc "$($themeDir)\App_Start\FilterConfig.cs") -replace '\$MyTheme\$', $themeName | sc "$($themeDir)\App_Start\FilterConfig.cs"
	(gc "$($themeDir)\App_Start\RouteConfig.cs") -replace '\$MyTheme\$', $themeName | sc "$($themeDir)\App_Start\RouteConfig.cs"
	(gc "$($themeDir)\App_Start\UnityConfig.cs") -replace '\$MyTheme\$', $themeName | sc "$($themeDir)\App_Start\UnityConfig.cs"
	(gc "$($themeDir)\App_Start\OrderFlowConfig.cs") -replace '\$MyTheme\$', $themeName | sc "$($themeDir)\App_Start\OrderFlowConfig.cs"
	(gc "$($themeDir)\GlobalEventsHandler.cs") -replace '\$MyTheme\$', $themeName | sc "$($themeDir)\GlobalEventsHandler.cs"
} else {
	(gc "$($themeDir)\$($themeName).csproj").replace('MyTheme', $themeName)|sc "$($themeDir)\$($themeName).csproj"
	(gc "$($createDir)\$($themeName).sln").replace('MyTheme', $themeName)|sc "$($createDir)\$($themeName).sln"
	(gc "$($themeDir)\Web.config").replace('MyTheme', $themeName)|sc "$($themeDir)\Web.config"
	(gc "$($themeDir)\Transformation Files\Web.$($themeName).config").replace('MyTheme', $themeName)|sc "$($themeDir)\Transformation Files\Web.$($themeName).config"
	(gc "$($themeDir)\publishtheme.ps1").replace('MyTheme', $themeName)|sc "$($themeDir)\publishtheme.ps1"
	(gc "$($createDir)\.gitignore").replace('MyTheme', $themeName)|sc "$($createDir)\.gitignore"

	(gc "$($themeDir)\App_Start\BundleConfig.cs").replace('$MyTheme$', $themeName)|sc "$($themeDir)\App_Start\BundleConfig.cs"
	(gc "$($themeDir)\App_Start\FilterConfig.cs").replace('$MyTheme$', $themeName)|sc "$($themeDir)\App_Start\FilterConfig.cs"
	(gc "$($themeDir)\App_Start\RouteConfig.cs").replace('$MyTheme$', $themeName)|sc "$($themeDir)\App_Start\RouteConfig.cs"
	(gc "$($themeDir)\App_Start\UnityConfig.cs").replace('$MyTheme$', $themeName)|sc "$($themeDir)\App_Start\UnityConfig.cs"
	(gc "$($themeDir)\App_Start\OrderFlowConfig.cs").replace('$MyTheme$', $themeName)|sc "$($themeDir)\App_Start\OrderFlowConfig.cs"
	(gc "$($themeDir)\GlobalEventsHandler.cs").replace('$MyTheme$', $themeName)|sc "$($themeDir)\GlobalEventsHandler.cs"
}
