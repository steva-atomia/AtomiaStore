Param(
    [string]$themeDir
)

$themeName = Split-Path $themeDir -Leaf
$createDir = Join-Path (Join-Path (pwd) (Split-Path $themeDir -Parent)) $ThemeName
$themeDir = Join-Path $createDir $themeName
$themeLibDir = "$($createDir)\Lib"

$defaultThemeDir = "$($PSScriptRoot)\..\Atomia.Store.Themes.Default"
$storeSolution = "$($PSScriptRoot)\..\Solution\Store.sln"

if (-Not (Test-Path $themeDir))
{
	echo "Theme directory $($createDir) does not exist. Aborting."
	Exit
}

# Rebuild AtomiaStore solution 
$msbuild = "C:\Progra~2\MSBuild\14.0\bin\msbuild.exe"
Invoke-Expression "$($msbuild) ""$($storeSolution)"" /p:Configuration=Debug /t:rebuild"

# Create Lib and copy bin files needed to run project on dev server, and for adding references
if (-Not (Test-Path $themeLibDir)) {
	New-Item $themeLibDir -Type directory
}

Copy-Item "$($defaultThemeDir)\bin\*.*" $themeLibDir

# Copy default theme files to have fallback when running dev server
Copy-Item "$($defaultThemeDir)\Themes\Default" "$($themeDir)\Themes" -Recurse -Force
Copy-Item "$($defaultThemeDir)\App_GlobalResources\*.resx" "$($themeDir)\App_GlobalResources" -Recurse -Force
Copy-Item "$($defaultThemeDir)\App_Data" "$($themeDir)" -Recurse -Force
Copy-Item "$($defaultThemeDir)\Global.asax" "$($themeDir)\Global.asax" -Force
Copy-Item "$($defaultThemeDir)\Global.asax.cs" "$($themeDir)\Global.asax.cs" -Force
