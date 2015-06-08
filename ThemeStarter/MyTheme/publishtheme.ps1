# This script collects the files you need to deploy this theme to an AtomiaStore installation.
# The files are placed in the 'publish' directory in the theme solution root.

$publishDir = "$($PSScriptRoot)\..\publish"

if ( Test-Path $publishDir ) {
	echo "publish directory already exists. Aborting."
	Exit
}

$themeSolution = "$($PSScriptRoot)\..\MyTheme.sln"
$msbuild = "$($Env:SystemRoot)\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"
Invoke-Expression "$($msbuild) $($themeSolution) /p:Configuration=Debug /t:rebuild"

New-Item $publishDir -Type directory

# Global resources
$resourcesSource = "$($PSScriptRoot)\App_GlobalResources"
$resourcesDest = "$($publishDir)\App_GlobalResources"
robocopy $resourcesSource  $resourcesDest /e /xf Common.resx CustomerValidation.resx

# Bin files
New-Item "$($publishDir)\bin" -Type directory
$themeDllSource = "$($PSScriptRoot)\bin\MyTheme.dll"
$themeDllDest = "$($publishDir)\bin\MyTheme.dll"
Copy-Item $themeDllSource $themeDllDest
$themePdbSource = "$($PSScriptRoot)\bin\MyTheme.pdb"
$themePdbDest = "$($publishDir)\bin\MyTheme.pdb"
Copy-Item $themePdbSource $themePdbDest

# Transformation files
$transformationsSource = "$($PSScriptRoot)\Transformation Files"
$transformationsDest = "$($publishDir)\Transformation Files"
robocopy $transformationsSource $transformationsDest /e

# Themes
$themesSource = "$($PSScriptRoot)\Themes"
$themesDest = "$($publishDir)\Themes"
robocopy $themesSource $themesDest /e /xd Default
