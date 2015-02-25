REM Build Atomia Store
%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe %~dp0Store.sln /p:Configuration=Release
IF %ERRORLEVEL% NEQ 0 GOTO lexit

REM Build the MSI
%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe %~dp0AtomiaStoreInstaller.sln /p:Configuration=Release
IF %ERRORLEVEL% NEQ 0 GOTO lexit 

REM Exit test with previously recorded error status
:lexit
exit %ERRORLEVEL%
