@echo off
setlocal enabledelayedexpansion
set buildPath=%~dp0
set gradle=%~dp0\tools\gradle-4.4\bin\gradle.bat
set gradletool_path=.\tools\gradle
set gradlebuildTemp=.\buildTemp
set global_properties=global.properties
set publish_properties=publish.properties
set version_properties=version.properties
set gradle_properties=%gradlebuildTemp%\gradle.properties
set local_properties=%gradlebuildTemp%\local.properties
set settings_gradle=%gradlebuildTemp%\settings.gradle
set global_gradle=%gradlebuildTemp%\global.gradle

set buildType=debug
set platform,subPlatform,platformPath=
set versionName,versionCode=
set build,keystore,package,appname,cdn,plugins,icon,splash=

::==================================================================================
:Main
call :ShowAppVersion
call :ShowMenu
call :InitPlatformConfig
call :GenGradleProperties
call :ReadySdkRes
cd %gradlebuildTemp%
::call %gradle% assembleRelease --stacktrace
::apk打包
call gradlew assembleRelease --stacktrace
::aab打包
call gradlew bundleRelease --stacktrace
pause
goto :eof

::==================================================================================
:ShowMenu
echo 由于百度SDK和其他的插件或者渠道SDK都太老，打出来的包会崩溃，就不要打我以前适配的渠道包了，看下示例怎么适配然后自己适配自己的渠道就行。
echo 要想看打包后的运行usdk运行情况，请打none(裸包)渠道
echo.

echo.
echo :Main
echo -----------------------------------
echo 1.Publish game to debug apk file                  [Debug apk]
echo 2.Publish game to release apk file                [Release apk]
echo 3.Modify version information                      [Modify version]
echo -----------------------------------
echo.
set /p selIdx=Select item: 

if %selIdx% equ 1 (
    set buildType=debug
    call :ShowPlatforms
) else (
    if %selIdx% equ 2 (
        set buildType=release
        call :ShowPlatforms
    ) else (
        if %selIdx% equ 3 (
            call :ModifyAppVersion
        )           
        cls
        call :Main
    )
)
goto :eof

::==================================================================================
:ShowPlatforms
set "oc="
for /f " usebackq tokens=1* delims==" %%a in ("%publish_properties%") do (
    if "%%b"=="" (
		for /f "tokens=1,2 delims=[-]" %%i in ("%%a") do (	
			echo !oc!|find "%%i" >nul||set oc=!oc! [%%i]
		)
    )
)
echo Platforms: %oc%
set /p platform=input platform:
echo %oc% | findstr "%platform%">nul && (echo.) || (
    cls
    call :Main
)

set "os="
for /f " usebackq tokens=1* delims==" %%a in ("%publish_properties%") do (
    if "%%b"=="" (
		for /f "tokens=1,2 delims=[-]" %%i in ("%%a") do (	
			if "%platform%"=="%%i" (
				::echo !os!|find "%%i" >nul||set os=!os! [%%j]
				set os=!os! [%%j]
			)	
		)
    )
)
echo SubPlatforms: %os%
set /p subPlatform=input sub platform:
if "%subPlatform%" equ "" (
    set subPlatform=default
)
echo %os% | findstr "%subPlatform%">nul && (echo.) || (
    cls
    call :Main
)
goto :eof

::==================================================================================
:InitPlatformConfig
call :GetPublishProperties build
set build=%result%
call :GetPublishProperties keystore
set keystore=%result%
call :GetPublishProperties package
set package=%result%
call :GetPublishProperties appname
set appname=%result%
call :GetPublishProperties cdn
set cdn=%result%
call :GetPublishProperties plugins
set plugins=%result%
call :GetPublishProperties icon
set icon=%result%
call :GetPublishProperties splash
set splash=%result%
goto :eof

::==================================================================================
:GenGradleProperties
if exist %gradlebuildTemp% (rd /s /q %gradlebuildTemp%)
mkdir %gradlebuildTemp%
xcopy .\tools\gradle\wrapper %gradlebuildTemp% /e/y/q/z

rem gradle.properties
set storePath=./sdk/keystore/%keystore%
call :ReadIni %global_properties% Unity project.dir
set UnityProjectDir=%result%
call :ReadIni %global_properties% Unity export.type
set UnityProjectType=%result%
call :ReadIni %global_properties% Java java.version
set JavaVersion=%result%
call :ReadIni %storePath% keystore keystore
set keystorename=%result%
call :ReadIni %storePath% keystore storepass
set storepass=%result%
call :ReadIni %storePath% keystore alias
set alias=%result%
call :ReadIni %storePath% keystore keypass
set keypass=%result%
 
set keystoreRootPath=%~dp0sdk/keystore
set keystoreRootPath=%keystoreRootPath:\=/%
set	keystorePath=%keystoreRootPath%/%keystorename%
copy %keystoreRootPath:/=\%\%keystorename% %UnityProjectDir:/=\%

echo VersionName=%versionName%>%gradle_properties%
echo VersionCode=%versionCode%>>%gradle_properties%
echo Package=%package%>>%gradle_properties%
echo AppName=%appname%>>%gradle_properties%
echo UnityProjectType=%UnityProjectType%>>%gradle_properties%
echo JavaVersion=%JavaVersion%>>%gradle_properties%
echo AppReleaseDir=./outputs/apk>>%gradle_properties%
echo Keystore=%keystorename%>>%gradle_properties%
echo StorePassword=%storepass%>>%gradle_properties%
echo KeyAlias=%alias%>>%gradle_properties%
echo KeyPassword=%keypass%>>%gradle_properties%

rem echo org.gradle.jvmargs=-Xmx4098m -Xms2048m -XX:MaxPermSize=1024m>>%gradle_properties%
echo android.enableAapt2=false>>%gradle_properties%
echo org.gradle.parallel=true>>%gradle_properties%
echo org.gradle.daemon=true>>%gradle_properties%
echo org.gradle.configureondemand=true>>%gradle_properties%

rem local.properties
rem set android_home=%Android_Home%
rem set android_home=%android_home:\=/%
call :ReadIni %global_properties% AndroidSdk sdk.dir
set SdkDir=%result%
call :ReadIni %global_properties% AndroidSdk ndk.dir
set NdkDir=%result%
echo sdk.dir=%SdkDir%>%local_properties%
rem echo ndk.dir=%NdkDir%>>%local_properties%

rem settings.gradle
echo include ':app'>%settings_gradle%
echo include ':unity'>>%settings_gradle%

set TempUnityProjectDir=%UnityProjectDir:\=/%
echo project^(':unity'^).projectDir=new File^('%TempUnityProjectDir%'^)>>%settings_gradle%

set unityAndroidPath=%UnityProjectDir%
if %UnityProjectType% equ eclipse (
	set appNameXmlPath=%unityAndroidPath%\res\values\strings.xml
)
if %UnityProjectType% equ as (
	set appNameXmlPath=%unityAndroidPath%\src\main\res\values\strings.xml
)
	
for %%i in (%plugins%) do (
    echo include ':%%i'>>%settings_gradle%
    echo project^(':%%i'^).projectDir=new File^('../sdk/plugins/%%i/module'^)>>%settings_gradle%
)
echo include ':%platform%'>>%settings_gradle%
echo project(':%platform%').projectDir=new File('../sdk/platforms/%platform%/module')>>%settings_gradle%
echo include ':usdk'>>%settings_gradle%
echo project(':usdk').projectDir=new File('../sdk/usdk/module')>>%settings_gradle%
set platformSettingGradle=./sdk/platforms/%platform%/module/settings.gradle
if exist %platformSettingGradle% (
    for /f "tokens=*" %%i in (%platformSettingGradle%) do (
        echo %%i>>%settings_gradle%
    )
)

::extend build.gradle
if exist %unityAndroidPath%\build.gradle (del %unityAndroidPath%\build.gradle)
copy %gradletool_path%\templates\mainTemplate.gradle %unityAndroidPath%\build.gradle
echo.>>%unityAndroidPath%\build.gradle
echo dependencies {>>%unityAndroidPath%\build.gradle
echo    compile project(':app')>>%unityAndroidPath%\build.gradle
echo    compile project(':usdk')>>%unityAndroidPath%\build.gradle
echo    compile project(':%platform%')>>%unityAndroidPath%\build.gradle
for %%i in (%plugins%) do (
	echo    compile project^(':%%i'^)>>%unityAndroidPath%\build.gradle
)
echo }>>%unityAndroidPath%\build.gradle
goto :eof

::==================================================================================
:ReadySdkRes
::修改appName
call .\tools\assetconfigtool\ModifyAppName.bat %appNameXmlPath% app_name "%appname%"

::构建临时module用于不同渠道构建差异资源
mkdir %gradlebuildTemp%\app
mkdir %gradlebuildTemp%\app\res
mkdir %gradlebuildTemp%\app\libs
mkdir %gradlebuildTemp%\app\assets
mkdir %gradlebuildTemp%\app\assets\bin
mkdir %gradlebuildTemp%\app\assets\bin\Data
xcopy %icon% %gradlebuildTemp%\app\res /e/y/q/z
xcopy %splash% %gradlebuildTemp%\app\assets\bin\Data /e/y/q/z
copy .\tools\gradle\templates\libTemplate.gradle %gradlebuildTemp%\app\build.gradle
copy .\tools\gradle\templates\EmptyAndroidManifest.xml %gradlebuildTemp%\app\AndroidManifest.xml
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
echo,Option=%~2,Key=%~3,Value=%result%
goto :eof

::==================================================================================
:GetKeystoreProperties
call :ReadIni %bundleIdentifierini% %nowChannel%-%subChannel% keystoreconfig
if defined result (
	set kconfig=%result%
) else (
	call :ReadIni %bundleIdentifierini% %nowChannel%-default keystoreconfig
)
if defined result (
	set kconfig=%result%
)

echo channel keystoreconfig:[%kconfig%]
if defined kconfig (
	call :ReadIni %keystoreconfigini% %kconfig% keystore
	set keystore=%nowDic%\keystore\!result!
	call :ReadIni %keystoreconfigini% %kconfig% keypass
	set keypass=!result!
	call :ReadIni %keystoreconfigini% %kconfig% storepass
	set storepass=!result!
	call :ReadIni %keystoreconfigini% %kconfig% alias
	set alias=!result!
)
echo keystore=%keystore% keypass=%keypass% storepass=%storepass% alias=%alias%
goto :eof

::==================================================================================
:GetPublishProperties [#1=option]
call :ReadIni %publish_properties% %platform%-%subPlatform% %~1
if defined result (
	set result=%result%
) else (
	call :ReadIni %publish_properties% %platform%-default %~1
)
if defined result (
	set result=%result%
)
echo %~1:[%result%]
goto :eof

::==================================================================================
:ShowAppVersion
if not exist "%version_properties%" (
    echo 1.0.0>%version_properties%
    echo 1>>%version_properties%
)

set /a index=0
for /f %%i in (%version_properties%) do (
set /a index+=1
if !index!==1 (set versionName=%%i)
if !index!==2 (set versionCode=%%i)
)

echo.
echo :app version
echo -----------------------------------
echo VersionName: %versionName%
echo VersionCode: %versionCode%
echo -----------------------------------
echo.
goto :eof

::==================================================================================
:ModifyAppVersion
set /p versionName=VersionName:
set /p versionCode=VersionCode:
echo %versionName%>%version_properties%
echo %versionCode%>>%version_properties%
goto :eof