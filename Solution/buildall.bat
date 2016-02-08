SET MSBUILD="%ProgramFiles(x86)%\MSBuild\14.0\bin\msbuild.exe"

REM Build Atomia Store
%MSBUILD% %~dp0Store.sln /p:Configuration=Release
IF %ERRORLEVEL% NEQ 0 GOTO lexit

REM Build the MSI
%MSBUILD% %~dp0AtomiaStoreInstaller.sln /p:Configuration=Release
IF %ERRORLEVEL% NEQ 0 GOTO lexit 

REM Exit test with previously recorded error status
:lexit
exit %ERRORLEVEL%
