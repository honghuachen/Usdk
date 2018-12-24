@echo off
setlocal enabledelayedexpansion
set channelName=%1%
set channel=%channelName%\Plugin
set subChannelName=%2%
set build=%3%

set android.jar=%~dp0../tools/android_build_tools/android-25/android.jar
set aapt=%~dp0../tools/android_sdk/build-tools/23.0.2/aapt
set javac=%~dp0../tools/Java/jdk1.8.0_102/bin/javac.exe
set jar=%~dp0../tools/Java/jdk1.8.0_102/bin/jar.exe

set currentDic=%cd%
set projectPath2=%currentDic%\..\..\..\..\Assets\Plugins
set channelPath=%currentDic%\%channelName%
set appiconpath=%channelPath%\Icon
set appsplashpath=%channelPath%\Splash

set gencodeengineDir=%~dp0..\tools\gencodeengine
set gencodeenginetool=%gencodeengineDir%\GenCodeEngine.bat
set bundleIdentifierini=%~dp0..\..\publishconfig.ini

call :GetAppIconPath
call :GetAppSplashPath
call :GetChannel_BundleIdentifier
call :GetChannel_ExPlugins
call :GetChannel_Plugins

call :GenUsdkJar
if %build% equ unity (
	call :GenPlatformJar
	xcopy %channelPath%\Plugin %projectPath2%\Android\ /e/y/q
	xcopy %appiconpath% %projectPath2%\Android\res\ /e/y/q
	xcopy %appsplashpath% %projectPath2%\Android\assets\bin\Data\ /e/y/q
	xcopy %currentDic%\..\usdk\Plugin %projectPath2%\Android\ /e/y/q
)
if %build% equ ant (
	xcopy %currentDic%\..\usdk\Plugin %currentDic%\..\apktemp\ /e/y/q

	xcopy %channelPath%\Plugin\assets %currentDic%\..\apktemp\assets\ /e/y/q/z
	xcopy %channelPath%\Plugin\res %currentDic%\..\apktemp\res\ /e/y/q/z

	xcopy %appiconpath% %currentDic%\..\apktemp\res\ /e/y/q
	xcopy %appsplashpath% %currentDic%\..\apktemp\assets\bin\Data\ /e/y/q	
)
if %build% equ gradle (
	rem ready sdk
	xcopy %currentDic%\..\usdk\Plugin %currentDic%\..\apktemp\ /e/y/q
	xcopy %channelPath%\Plugin\assets %currentDic%\..\apktemp\assets\ /e/y/q/z
	xcopy %channelPath%\Plugin\res %currentDic%\..\apktemp\res\ /e/y/q/z
	xcopy %channelPath%\src %currentDic%\..\apktemp\src\ /e/y/q/z
	
	xcopy %appiconpath% %currentDic%\..\apktemp\res\ /e/y/q
	xcopy %appsplashpath% %currentDic%\..\apktemp\assets\bin\Data\ /e/y/q
	
	copy %channelPath%\gradle\build.gradle %currentDic%\app\build.gradle /z
	copy %channelPath%\gradle\settings.gradle %currentDic%\..\apktemp\settings.gradle /z
	del %currentDic%\..\apktemp\libs\android-support-v4.jar
)
goto :eof


:GenPlatformJar
echo.
echo.
echo --------------------------------------------
echo :gen %channelName%Platform.jar,please wait.
echo --------------------------------------------
echo.

if exist temp (rd /s /q temp)
mkdir temp\bin
set bin=./temp/bin
%javac% -source 1.7 -target 1.7 -bootclasspath %android.jar% -Djava.ext.dirs=%channel%\libs -d %bin% %channel%/../src/*.java
cd ./temp/bin
%jar% -cvf %channelName%Platform.jar com/
cd %currentDic%
copy temp\bin\*.jar %channel%\libs
rd /s /q temp
goto :eof

:GenUsdkJar
echo.
echo.
echo --------------------------------------------
echo :gen Usdk.jar,please wait.
echo --------------------------------------------
echo.

if exist temp (rd /s /q temp)
mkdir temp\gen
mkdir temp\src
mkdir temp\res

::merge mainfest
call %currentDic%\..\tools\gencodeengine\GenCodeEngine.bat %packagename% AndroidManifest %channelPath%\AndroidManifest.ftl %channelPath%\Plugin .xml
call :MergeManifest

if %build% equ unity (
	::copy temp res
	xcopy %channel%\res temp\res\ /e/y/q
	xcopy %appiconpath% temp\res\ /e/y/q
	xcopy %currentDic%\..\usdk\Plugin\res temp\res\ /e/y/q
	::genR
	%aapt% package -f -m -J ./temp/gen -S temp\res -I %android.jar% -M %channel%\AndroidManifest.xml
	call %currentDic%\..\tools\EncodeConvertTool.bat temp UTF-8 GB232
	if %channelName% equ tiantuo (
		%aapt% package -f -m -J ./temp/gen -S temp\res -I %android.jar% -M %channelName%\com.tiantuo.sdk.user\AndroidManifest.xml
	)
)

::genClass
mkdir temp\bin
mkdir temp\libs
set bin=./temp/bin
set libs=./temp/libs

set rpath=%packagename%
set rpath=%rpath:.=\%

copy %currentDic%\..\tools\classes.jar temp\libs\
xcopy %currentDic%\..\usdk\Plugin\libs temp\libs\ /y /s /e
if %build% equ unity (
	xcopy %channel%\libs temp\libs\ /y /s /e
)
if %channelName% equ tiantuo (
	copy temp\gen\com\tiantuo\sdk\user\R.java temp\src
	%javac% -source 1.7 -target 1.7 -bootclasspath %android.jar% -Djava.ext.dirs=%libs% -d %bin% ../usdk/src/*.java ./temp/gen/%rpath%/*.java ./temp/src/*.java 
)
if %channelName% neq tiantuo (
	if %build% equ unity (
		%javac% -source 1.7 -target 1.7 -bootclasspath %android.jar% -Djava.ext.dirs=%libs% -d %bin% ../usdk/src/*.java ./temp/gen/%rpath%/*.java
	)
	if %build% equ ant (
		%javac% -source 1.7 -target 1.7 -bootclasspath %android.jar% -Djava.ext.dirs=%libs% -d %bin% ../usdk/src/*.java
	)
	if %build% equ gradle (
		%javac% -source 1.7 -target 1.7 -bootclasspath %android.jar% -Djava.ext.dirs=%libs% -d %bin% ../usdk/src/*.java
	)
)

::genJar
cd ./temp/bin
%jar% -cvf Usdk.jar com/

cd %currentDic%
if %build% equ unity (
	copy temp\bin\*.jar %channel%\libs
)
if %build% equ ant (
	copy temp\bin\*.jar %currentDic%\..\apktemp\libs
	copy %channel%\AndroidManifest.xml %currentDic%\..\apktemp
	copy %channel%\..\build.xml %currentDic%\..\apktemp
)
if %build% equ gradle (
	copy temp\bin\*.jar %currentDic%\..\apktemp\libs
	copy %channel%\AndroidManifest.xml %currentDic%\..\apktemp
	xcopy %currentDic%\app\gradle %currentDic%\..\apktemp\ /y /s /e
)
rd /s /q temp
goto :eof

::==================================================================================
:MergeManifest
set output=%channel%\AndroidManifest.xml
set mainfest=%channel%\AndroidManifest.xml
set commanifest=%currentDic%\..\usdk\AndroidManifest.xml
call %currentDic%\..\tools\manifest-merger\manifest-merger.bat -v merge --out %output% --libs %commanifest% --main %mainfest%

if exist %currentDic%\..\plugins\temp (rd /s /q %currentDic%\..\plugins\temp)

for %%i in (%pluginlist%) do (
	echo merge [%%i] mainfest..........
	::call %currentDic%\..\tools\gencodeengine\GenCodeEngine.bat %packagename% AndroidManifest %currentDic%\..\plugins\%%i\AndroidManifest.ftl %currentDic%\..\plugins\%%i .xml
	call %currentDic%\..\tools\manifest-merger\manifest-merger.bat -v merge --out %output% --libs %currentDic%\..\plugins\%%i\AndroidManifest.xml --main %mainfest%
	xcopy %currentDic%\..\plugins\%%i\Plugin\res temp\res\ /e/y/q
)
goto :eof

::==================================================================================
:GetChannel_BundleIdentifier
call :ReadIni %bundleIdentifierini% %channelName%-%subChannelName% package
if defined result (
	set packagename=%result%
) else (
	call :ReadIni %bundleIdentifierini% %channelName%-default package
)
if defined result (
	set packagename=%result%
)
echo packagename:[%packagename%]
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
:GetAppIconPath
call :ReadIni %bundleIdentifierini% %channelName%-%subChannelName% icon
if defined result (
	set appiconpath=%~dp0..\..\%result%
) else (
	call :ReadIni %bundleIdentifierini% %channelName%-default icon
)
if defined result (
	set appiconpath=%~dp0..\..\%result%
)
echo app icon path:[%appiconpath%]
goto :eof

::==================================================================================
:GetAppSplashPath
call :ReadIni %bundleIdentifierini% %channelName%-%subChannelName% splash
if defined result (
	set appsplashpath=%~dp0..\..\%result%
) else (
	call :ReadIni %bundleIdentifierini% %channelName%-default splash
)
if defined result (
	set appsplashpath=%~dp0..\..\%result%
)
echo app splash path:[%appsplashpath%]
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