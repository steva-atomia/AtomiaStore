Rem Company: Atomia AB
Rem Copyright (C) 2015 Atomia AB. All rights reserved

@ECHO off

Rem -------------------
Rem Site root
Rem -------------------

copy /y "%~dp0..\Atomia.Store.Themes.Default\Global.asax" "%~dp0publish\Global.asax"
copy /y "%~dp0..\Atomia.Store.Themes.Default\Web.config" "%~dp0publish\Web.config"
md "%~dp0publish\Original Files"
md "%~dp0publish\Transformation Files"
copy  "%~dp0..\Atomia.Store.Themes.Default\Original Files\*.config" "%~dp0publish\Original Files\"


Rem --------
Rem App_Data
Rem --------

xcopy /e /y /I "%~dp0..\Atomia.Store.Themes.Default\App_Data" "%~dp0publish\App_Data"
md "%~dp0publish\App_Data\Original Files"
md "%~dp0publish\App_Data\Transformation Files"
copy /y "%~dp0..\Atomia.Store.Themes.Default\Original Files\App_Data\*.config" "%~dp0publish\App_Data\Original Files\"


Rem -------------------
Rem App_GlobalResources
Rem -------------------

xcopy /e /y /I "%~dp0..\Atomia.Store.Themes.Default\App_GlobalResources" "%~dp0publish\App_GlobalResources"
del "%~dp0publish\App_GlobalResources\*.designer.cs"


Rem -------------------
Rem bin
Rem Assumes that other project bins have been copied to Atomia.Store.Themes.Default\bin on build
Rem -------------------

xcopy /e /y /I "%~dp0..\Atomia.Store.ActionTrail\bin\%1" "%~dp0publish\bin"
xcopy /e /y /I "%~dp0..\Atomia.Store.AspNetMvc\bin\%1" "%~dp0publish\bin"
xcopy /e /y /I "%~dp0..\Atomia.Store.Core\bin\%1" "%~dp0publish\bin"
xcopy /e /y /I "%~dp0..\Atomia.Store.OrderHandlers\bin\%1" "%~dp0publish\bin"
xcopy /e /y /I "%~dp0..\Atomia.Store.Payment.AdyenHpp\bin\%1" "%~dp0publish\bin"
xcopy /e /y /I "%~dp0..\Atomia.Store.Payment.DibsFlexwin\bin\%1" "%~dp0publish\bin"
xcopy /e /y /I "%~dp0..\Atomia.Store.Payment.Invoice\bin\%1" "%~dp0publish\bin"
xcopy /e /y /I "%~dp0..\Atomia.Store.Payment.PayExRedirect\bin\%1" "%~dp0publish\bin"
xcopy /e /y /I "%~dp0..\Atomia.Store.Payment.PayPal\bin\%1" "%~dp0publish\bin"
xcopy /e /y /I "%~dp0..\Atomia.Store.Payment.WorldPay\bin\%1" "%~dp0publish\bin"
xcopy /e /y /I "%~dp0..\Atomia.Store.Payment.WorldPayXml\bin\%1" "%~dp0publish\bin"
xcopy /e /y /I "%~dp0..\Atomia.Store.PublicBillingApi\bin\%1" "%~dp0publish\bin"
xcopy /e /y /I "%~dp0..\Atomia.Store.Themes.Default\bin" "%~dp0publish\bin"
xcopy /e /y /I "%~dp0..\Atomia.Store.WebBase\bin\%1" "%~dp0publish\bin"
xcopy /e /y /I "%~dp0..\Atomia.Store.WebPluginDomainSearch\bin\%1" "%~dp0publish\bin"
del "%~dp0publish\bin\Atomia.Store.Fakes.*"
md "%~dp0publish\bin\Original Files"
md "%~dp0publish\bin\Transformation Files"
copy /y "%~dp0..\Atomia.Store.Themes.Default\Original Files\bin\*.config" "%~dp0publish\bin\Original Files\"


Rem -------------
Rem Default theme
Rem -------------

xcopy /e /y /I "%~dp0..\Atomia.Store.Themes.Default\Themes\Default" "%~dp0publish\Themes\Default" 
