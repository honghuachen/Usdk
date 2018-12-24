@echo off

set channelName=%1%
set subChannelName=%2%
set build=%3%

set channel=%channelName%\Plugin
set gencodeengineDir=%~dp0..\tools\gencodeengine
set gencodeenginetool=%gencodeengineDir%\GenCodeEngine.bat
set bundleIdentifierini=%~dp0..\..\publishconfig.ini

set android.jar=%~dp0../tools/android_build_tools/android-26/android.jar
set aapt=%~dp0../tools/android_sdk/build-tools/23.0.2/aapt
set javac=%~dp0../tools/Java/jdk1.8.0_102/bin/javac.exe
set jar=%~dp0../tools/Java/jdk1.8.0_102/bin/jar.exe

set currentDic=%cd%
if exist temp (rd /s /q temp)

::call :GetChannel_ExPlugins
::	dir /b /ad>list.txt
::	set plginlist=list.txt
::	set os=0
::	for /f %%i in (%plginlist%) do (
::		for %%j in (%expluginlist%) do (	
::			if "%%i"=="%%j" (
::				set os=1
::			)	
::		)
::		if "!os!"=="0" (
::			call :GenPluginJar %%i
::		)
::		set os=0
::	) 

call :GetChannel_Plugins
for %%i in (%pluginlist%) do (
	call :GenPluginJar %%i
)
goto :eof

::==================================================================================
:GenPluginJar [#1=plugin]
echo.
echo.
echo --------------------------------------------
echo :gen %~1Plugin.jar,please wait.
echo --------------------------------------------
echo.

set plugin=%currentDic%\%~1\plugin
set src=%currentDic%/%~1/src
set libs=%plugin%\libs

if exist temp (rd /s /q temp)
mkdir temp\bin
mkdir temp\libs
set bin=./temp/bin
set templibs=./temp/libs

copy %currentDic%\..\tools\classes.jar %currentDic%\temp\libs\
xcopy %libs% %currentDic%\temp\libs /e/y/q
if %build% equ unity (
	xcopy %currentDic%\..\channel\%channel%\libs %currentDic%\temp\libs /e/y/q
)
if %build% equ ant (
	copy %currentDic%\..\apktemp\libs\ZWWXSDK.jar %currentDic%\temp\libs\
)
if %build% equ gradle (
	copy %currentDic%\..\apktemp\libs\ZWWXSDK.jar %currentDic%\temp\libs\
)

%javac% -source 1.7 -target 1.7 -bootclasspath %android.jar% -Djava.ext.dirs=%templibs% -d %bin% %src%/*.java
cd ./temp/bin
%jar% -cvf %~1Plugin.jar com/

cd %currentDic%
copy temp\bin\*.jar %libs%
rd /s /q temp

if %build% equ unity (
	xcopy %plugin% %currentDic%\..\..\..\..\Assets\Plugins\Android /e/y/q
)
if %build% equ ant (
	xcopy %plugin% %currentDic%\..\apktemp /e/y/q
)
if %build% equ gradle (
	xcopy %plugin% %currentDic%\..\apktemp /e/y/q
)
goto :eof

::==================================================================================
:GetChannel_ExPlugins
call :ReadIni %bundleIdentifierini% %channelName%-%subChannelName% explugin
if defined result (
	set expluginlist=%result%
) else (
	call :ReadIni %bundleIdentifierini% %channelName%-default explugin
)
if defined result (
	set expluginlist=%result%
)
echo expluginlist:[%expluginlist%]
goto :eof

::==================================================================================
:GetChannel_Plugins
call :ReadIni %bundleIdentifierini% %channelName%-%subChannelName% plugin
if defined result (
	set pluginlist=%result%
) else (
	call :ReadIni %bundleIdentifierini% %channelName%-default plugin
)
if defined result (
	set pluginlist=%result%
)
echo pluginlist:[%pluginlist%]
goto :eof

::==================================================================================
:ReadIni [#1=ini] [#2=section] [#3=key]
set "op="
for /f " usebackq tokens=1* delims==" %%a in ("%~1") do (
    if "%%b"=="" (
        set "op=%%a"
    ) else (
        set "##!op!#%%a=%%b"
    )
)

set result=!##[%~2]#%~3!
::echo,Option=%~2,Key=%~3,Value=!##[%~2]#%~3!
goto :eof