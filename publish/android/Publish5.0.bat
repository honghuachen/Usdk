@echo off
setlocal enabledelayedexpansion
echo                             I---------------------------------I
echo                             I-------PublishTool 5.0-----------I
echo                             I---------------------------------I

set nowDic=%~dp0
set projectPath=%nowDic%..\..
set ver_file=%nowDic%\Version.txt
set gradle_properties_file=%nowDic%\SDK\apktemp\gradle.properties
set gradle_local_properties_file=%nowDic%\SDK\apktemp\local.properties

set bundleIdentifierini==%nowDic%\publishconfig.ini
set channelconfigini==%nowDic%\channelconfig.ini
set resourceconfigini==%nowDic%\ReplaceResource\replaceresouceconfig.ini
set keystoreconfigini==%nowDic%\keystore\keystoreconfig.ini

set ant=%~dp0\SDK\tools\apache-ant-1.9.11\bin\ant.bat
set gradle=%~dp0\SDK\tools\gradle-4.4\bin\gradle.bat

set company_name=zwwx
set build=unity
set identifier=com.game.jsws.default
set product_name=三生三世
set cdnurl=http://192.168.1.211/ssss/
set cconfig=
set rconfig=
set kconfig=

set res_path=%projectPath%\..\res\android
set build_UIRes_path=%projectPath%/../client_android
set ui_output=%build_UIRes_path%/Assets/StreamingAssets/res/ui
set ui_resource_path=%build_UIRes_path%/Assets/Resources

set keystore=%nowDic%SDK\common\keystore\SSSS.keystore
set keypass=XscHFAeSGNHIw75I
set storepass=FSq0FgfbKSi3kNYP
set alias=ssss

set cloudserver=\\192.168.1.10\ssss\越南分支20181025

::==================================================================================
:Main
call :Show_Version
echo.
echo :Main
echo -----------------------------------
echo 1.Publish game to debug apk file                  [Debug apk]
echo 2.Publish game to release apk file                [Release apk]
echo 3.Ready game resource                             [Ready resource]
echo 4.Modify version information                      [Modify version]
::echo 5.Rebuild ui resource and ready game resource     [Rebuild ui resource]
::echo.
::echo a.Key pack debug apk file                         [Key pack debug]
::echo b.Key pack release apk file                       [Key pack release]
echo -----------------------------------
echo.
set /p selIdx=Select item: 

if %selIdx% equ a (
"%SVN%" /command:revert /path:%projectPath%\Assets*%ui_resource_path%*%res_path% /closeonend:2
"%SVN%" /command:update /path:%ui_resource_path%*%res_path% /closeonend:2
set publishType=debug
call :Build_Channel
)

if %selIdx% equ b (
::"%SVN%" /command:revert /path:"%projectPath%\Assets\" /closeonend:2
"%SVN%" /command:revert /path:%ui_resource_path%*%res_path%*%projectPath%\Assets /closeonend:2
"%SVN%" /command:update /path:"%ui_resource_path%"*"%res_path%" /closeonend:2
set publishType=release
call :Build_Channel
)

if %selIdx% equ 1 (
"%SVN%" /command:revert /path:"%projectPath%\Assets\" /closeonend:2
set publishType=debug
call :Build_Channel
)

if %selIdx% equ 2 (
"%SVN%" /command:revert /path:"%projectPath%\Assets\" /closeonend:2
set publishType=release
call :Build_Channel
)

if %selIdx% equ 3 (
::update res
"%SVN%" /command:revert /path:"%projectPath%\Assets\*%projectPath%\NGUI\*%projectPath%\GameSettings\*%res_path%" /closeonend:2
"%SVN%" /command:update /path:"%projectPath%\Assets\*%projectPath%\NGUI\*%projectPath%\GameSettings\*%res_path%" /closeonend:2
call :Ready_GameRes
call :Main
)

if %selIdx% equ 4 (
call :Update_Version
call :Main
)

if %selIdx% equ 5 (
"%SVN%" /command:revert /path:"%ui_resource_path%*%res_path%*%projectPath%\Assets\*%projectPath%\NGUI\*%projectPath%\GameSettings\" /closeonend:2
"%SVN%" /command:update /path:"%ui_resource_path%*%res_path%*%projectPath%\Assets\*%projectPath%\NGUI\*%projectPath%\GameSettings\" /closeonend:2
call :Build_UIRes
call :Ready_GameRes
call :Main
)

pause
goto :eof

::==================================================================================
:Build_Channel
setlocal enabledelayedexpansion
echo -----------------------------------
call :GetChannelList %bundleIdentifierini%
echo -----------------------------------
set /p nowChannel=Channel:
echo -----------------------------------
call :GetSubChannelList %bundleIdentifierini% %nowChannel%
echo -----------------------------------
set /p subChannel=SubChannel:
::set /p adidChannel=AdIdChannel:
if "%SubChannel%" equ "" (
set subChannel=default
)

set t0=%time%
set /a index=0
for /f %%i in (%ver_file%) do (
set /a index+=1
if !index!==1 (set version=%%i)
if !index!==2 (set generation=%%i)
)
set timenow=%date:~0,4%%date:~5,2%%date:~8,2%%time:~0,2%%time:~3,2%
set timenow=%timenow: =0%
set outPut_ApkName=%nowChannel%_%timenow%_%version%_%generation%_%publishType%

::get config data
call :GetChannel_Build
call :GetChannel_BundleIdentifier
call :GetChannel_Name
call :GetChannel_cdnurl
call :CreateCdnUrl
call :GetChannel_keystoreconfig
call :GetChannel_resourceconfig
call :GetChannel_channelconfig
::call :ReplaceResourceConfig

if %selIdx% equ a (
call :Build_UIRes
call :Ready_GameRes
call :Build_Common
)
if %selIdx% equ b (
call :Build_UIRes
call :Ready_GameRes
call :Build_Common
)
if %selIdx% equ 1 (
call :Build_Common
)
if %selIdx% equ 2 (
call :Build_Common
)
goto :eof


::==================================================================================
:Build_Common
call :Ready_SDKRes
call :ReplaceResourceConfig
if not exist %nowDic%\Logs (md %nowDic%\Logs)
if not exist %nowDic%\Apks (md %nowDic%\Apks)
if %build% equ unity (
	call :Build_Unity
	if %nowChannel% equ xigua (
		cd %nowDic%\SDK\tools\xgsdk-cmd-1.2\
		call xiguaBuild.bat %subChannel% %nowDic%Apks\%outPut_ApkName%.apk %nowDic%Apks
		call :CopyApkToServer %nowDic%Apks\%outPut_ApkName%_%subChannel%.apk
	) 
	if %nowChannel% neq xigua (
		call :ApkTool_Build
	)
)

::修改zwwxconfig.xml中的debug配置
set debug=false
if %publishType% equ debug (
	set debug=true
)
if %publishType% equ release (
	set debug=false
)

if %build% equ ant (
	rem ready game res
	"%SVN%" /command:update /path:%res_path% /closeonend:2
	cd %res_path%
	call compress.bat

	rem ready sdk
	rem xcopy %nowDic%SDK\channel\%nowChannel%\Plugin\assets %nowDic%SDK\apktemp\assets\ /e/y/q/z
	
	copy %res_path%\0.zip %nowDic%SDK\apktemp\assets\0.zip /z
	del %res_path%\0.zip
	xcopy %res_path% %nowDic%SDK\apktemp\assets\res\ /e/y/q/z
	
	del %res_path%\Game.dll
	del %nowDic%SDK\apktemp\assets\res\Game.dll.mdb
	del %nowDic%SDK\apktemp\assets\res\reference.db
	rd /s /q %nowDic%SDK\apktemp\assets\res\resource
	rd /s /q %nowDic%SDK\apktemp\assets\res\proto
	rd /s /q %nowDic%SDK\apktemp\assets\res\Lua
	rd /s /q %nowDic%SDK\apktemp\assets\res\txt
	rd /s /q %nowDic%SDK\apktemp\assets\res\language
	
	cd %nowDic%SDK\tools\assetconfigtool
	call assetconfigtool.bat %nowDic%SDK\apktemp\assets\zwwxconfig.xml debug %debug%
	call assetconfigtool.bat %nowDic%SDK\apktemp\assets\zwwxconfig.xml url %cdnurl%
	call assetconfigtool.bat %nowDic%SDK\apktemp\assets\zwwxconfig.xml version %version%
	call ModifyAppName.bat %nowDic%SDK\apktemp\res\values\strings.xml app_name %product_name%
	cd %nowDic%SDK\apktemp
						
	echo -----------------"%cconfig%"----------------------
	if "%cconfig%" neq "" (
		for %%c in (%cconfig%) do (
			echo %%c 媒体广告包Building.........
			cd %nowDic%SDK\tools\assetconfigtool		
			set "op="
			for /f " usebackq tokens=1* delims==" %%i in (%channelconfigini%) do (
				if "%%j"=="" (
					set "op=%%i"
				) else (						
					if [%%c] equ !op! (
						call assetconfigtool.bat %nowDic%SDK\apktemp\assets\zwwxconfig.xml %%i %%j
						echo modify [%%i] to [%%j]
					)
				)
			)
			cd %nowDic%SDK\apktemp
			call %ant%
			copy dest\%nowChannel%_release.apk %nowDic%Apks\%outPut_ApkName%_%subChannel%_[%%c]_compress.apk /z
			call :CopyApkToServer %nowDic%Apks\%outPut_ApkName%_%subChannel%_[%%c]_compress.apk
		)
	) else (
		cd %nowDic%SDK\apktemp
		call %ant%
		
		copy dest\%nowChannel%_release.apk %nowDic%Apks\%outPut_ApkName%_%subChannel%_compress.apk /z
		call :CopyApkToServer %nowDic%Apks\%outPut_ApkName%_%subChannel%_compress.apk
	)

	"%SVN%" /command:update /path:%res_path% /closeonend:2
	if exist %nowDic%SDK\apktemp (rd /s /q %nowDic%SDK\apktemp)
)

set android_home=%Android_Home%
set android_home=%android_home:\=/%
if %build% equ gradle (
	call :Gen_Gradle_Properties
	echo sdk.dir=%android_home%>>%gradle_local_properties_file%

	rem ready game res
	"%SVN%" /command:update /path:%res_path% /closeonend:2
	cd %res_path%
	call compress.bat
	
	rem ready sdk
	rem xcopy %nowDic%SDK\channel\%nowChannel%\Plugin\assets %nowDic%SDK\apktemp\assets\ /e/y/q/z
	rem xcopy %nowDic%SDK\channel\%nowChannel%\Plugin\res %nowDic%SDK\apktemp\res\ /e/y/q/z
	rem xcopy %nowDic%SDK\channel\%nowChannel%\src %nowDic%SDK\apktemp\src\ /e/y/q/z
	rem copy %nowDic%SDK\channel\%nowChannel%\gradle\build.gradle %nowDic%SDK\channel\app\build.gradle /z
	rem copy %nowDic%SDK\channel\%nowChannel%\gradle\settings.gradle %nowDic%SDK\apktemp\settings.gradle /z
	rem del %nowDic%SDK\apktemp\libs\android-support-v4.jar
	
	copy %res_path%\0.zip %nowDic%SDK\apktemp\assets\0.zip /z
	del %res_path%\0.zip
	xcopy %res_path% %nowDic%SDK\apktemp\assets\res\ /e/y/q/z
	del %nowDic%SDK\apktemp\assets\res\Game.dll
	del %nowDic%SDK\apktemp\assets\res\Game.dll.mdb
	del %nowDic%SDK\apktemp\assets\res\reference.db
	rd /s /q %nowDic%SDK\apktemp\assets\res\resource
	rd /s /q %nowDic%SDK\apktemp\assets\res\proto
	rd /s /q %nowDic%SDK\apktemp\assets\res\Lua
	rd /s /q %nowDic%SDK\apktemp\assets\res\txt
	rd /s /q %nowDic%SDK\apktemp\assets\res\language
	
	cd %nowDic%SDK\tools\assetconfigtool
	call assetconfigtool.bat %nowDic%SDK\apktemp\assets\zwwxconfig.xml debug %debug%
	call assetconfigtool.bat %nowDic%SDK\apktemp\assets\zwwxconfig.xml url %cdnurl%
	call assetconfigtool.bat %nowDic%SDK\apktemp\assets\zwwxconfig.xml version %version%
	rem 越南appname乱码 从SDK\channel\yuenangradle\Plugin\res\values\string.xml配置修改
	rem call ModifyAppName.bat %nowDic%SDK\apktemp\res\values\strings.xml app_name %product_name%
	cd %nowDic%SDK\apktemp
						
	echo -----------------"%cconfig%"----------------------
	if "%cconfig%" neq "" (
		for %%c in (%cconfig%) do (
			echo %%c 媒体广告包Building.........
			cd %nowDic%SDK\tools\assetconfigtool		
			set "op="
			for /f " usebackq tokens=1* delims==" %%i in (%channelconfigini%) do (
				if "%%j"=="" (
					set "op=%%i"
				) else (						
					if [%%c] equ !op! (
						call assetconfigtool.bat %nowDic%SDK\apktemp\assets\zwwxconfig.xml %%i %%j
						echo modify [%%i] to [%%j]
					)
				)
			)
			cd %nowDic%SDK\apktemp
			call %gradle% assembleRelease
			copy %nowDic%SDK\channel\app\build\outputs\apk\release\app-release.apk %nowDic%Apks\%outPut_ApkName%_%subChannel%_[%%c]_compress.apk /z
			call :CopyApkToServer %nowDic%Apks\%outPut_ApkName%_%subChannel%_[%%c]_compress.apk
		)
	) else (
		cd %nowDic%SDK\apktemp
		call %gradle% assembleRelease
		
		copy %nowDic%SDK\channel\app\build\outputs\apk\release\app-release.apk %nowDic%Apks\%outPut_ApkName%_%subChannel%_compress.apk /z
		call :CopyApkToServer %nowDic%Apks\%outPut_ApkName%_%subChannel%_compress.apk
	)

	"%SVN%" /command:update /path:%res_path% /closeonend:2
	if exist %nowDic%SDK\apktemp (rd /s /q %nowDic%SDK\apktemp)
)

call start %nowDic%\Apks
call :costTime
goto :eof

::==================================================================================
:Build_Unity
echo.
echo.
echo --------------------------------------------
echo :unity build begin,please wait.
echo --------------------------------------------
echo.

tasklist|find /i "Unity.exe"&&(taskkill /f /im Unity.exe)
if not exist %nowDic%\Logs (md %nowDic%\Logs)
if not exist %nowDic%\Apks (md %nowDic%\Apks)

copy %nowDic%\config.xml %projectPath%\Assets\Resources
copy %nowDic%\url.txt %projectPath%\Assets\Resources
::set storeName=%nowDic%\SDK\common\keystore\xgsdk.keystore
set keystoreArgs=keystoreName=%keystore% keystorePass=%storepass% keyaliasName=%alias% keyaliasPass=%keypass%
set parameter=channel=%nowChannel% version=%version% generation=%generation% publish=%publishType% output_path=%outPut_ApkName%.apk
set identifierArgs=identifier_prefix=%identifier% identifier_postfix=
set appArgs=companyName=%company_name% productName=%product_name%

set UNITY_PATH="%UNITY_PATH_4%\Unity.exe"
set args=-projectPath %projectPath% -executeMethod AutoPackageApk.BuildForAndroid %parameter% %appArgs% %identifierArgs% %keystoreArgs%
set logfile=%nowDic%\Logs\%timenow%.log
%UNITY_PATH% -quit -batchmode -logFile %logfile% %args%
::%UNITY_PATH% -logFile %logfile% %args%

echo Unity build completed: %outPut_ApkName%.apk
goto :eof

::==================================================================================
:Ready_GameRes
@echo off
echo.
echo.
echo --------------------------------------------
echo :ready game resource begin,please wait.
echo --------------------------------------------
echo.

set build_path=%projectPath%\Assets\StreamingAssets
set build_dll_path=%projectPath%/../AutoBuildDll
set commonassets=%nowDic%SDK\channel\app\assets

::编译Game.dll代码（暂时屏蔽）
::build Game.dll
::cd %build_dll_path%
::call build.bat

::增量打包ab资源，编译Game.dll代码
call BuildUI.bat

::copy unity res
@echo ready game resource,please wait…………
if exist %build_path%\res (rd /s /q %build_path%\res)
xcopy %res_path% %build_path%\res\ /e/y/q/z
rd /s /q %build_path%\res\resource
rd /s /q %build_path%\res\txt
del %build_path%\res\Game.dll
del %build_path%\res\reference.db

::copy ant res(暂时先屏蔽ant的资源准备)
::if exist %commonassets%\res (rd /s /q %commonassets%\res)
::mkdir %commonassets%\res
::xcopy %res_path% %commonassets%\res\ /e/y/q/z
::rd /s /q %commonassets%\res\resource
::rd /s /q %commonassets%\res\txt
::del %commonassets%\res\Game.dll
::del %commonassets%\res\reference.db

::合并db文件[db文件不再需要，换成lua配置]
::%nowDic%/SDK/tools/combinedb/CombineDB.exe %version% %res_path%\resource %res_path%

::compress 0.zip and copy
cd %res_path%
call compress.bat
copy %res_path%\0.zip %build_path%
::copy %res_path%\0.zip %commonassets%
del %res_path%\0.zip
cd %nowDic%
goto :eof

::==================================================================================
:Ready_SDKRes
@echo off
echo.
echo.
echo --------------------------------------------
echo :ready sdk resource begin,please wait.
echo --------------------------------------------
echo.

if %build% equ unity (
	cd %nowDic%
	if exist %projectPath%\Assets\Plugins\Android (rd /s /q %projectPath%\Assets\Plugins\Android)
	mkdir %projectPath%\Assets\Plugins\Android
	xcopy "%projectPath%\NGUI\android\NGUI.dll" "%projectPath%\Assets\Plugins\Reference\NGUI.dll"/y /s /e
	@rem ::xcopy "%projectPath%\GameSettings\Andriod" "%projectPath%\ProjectSettings\" /y /s /e
	
	@rem ::Convert Encoding of .java
	@rem ::call %nowDic%\SDK\tools\EncodeConvertTool.bat %nowDic%\SDK\channel UTF-8 GB232
	@rem ::call %nowDic%\SDK\tools\EncodeConvertTool.bat %nowDic%\SDK\common UTF-8 GB232
	@rem ::call %nowDic%\SDK\tools\EncodeConvertTool.bat %nowDic%\SDK\plugins UTF-8 GB232
	@rem ::copy unity setting
)
if %build% equ ant (
	if exist SDK\apktemp (rd /s /q SDK\apktemp)
	mkdir SDK\apktemp
	mkdir SDK\apktemp\assets
	mkdir SDK\apktemp\assets\res
	mkdir SDK\apktemp\assets\bin
	mkdir SDK\apktemp\assets\bin\data
	mkdir SDK\apktemp\libs
	mkdir SDK\apktemp\res
	mkdir SDK\apktemp\src
)
if %build% equ gradle (
	if exist SDK\apktemp (rd /s /q SDK\apktemp)
	mkdir SDK\apktemp
	mkdir SDK\apktemp\assets
	mkdir SDK\apktemp\assets\res
	mkdir SDK\apktemp\assets\bin
	mkdir SDK\apktemp\assets\bin\data
	mkdir SDK\apktemp\libs
	mkdir SDK\apktemp\res
	mkdir SDK\apktemp\src
	mkdir SDK\apktemp\gradle
)

::copy android SDK channel res
cd %nowDic%\SDK\channel
call genJAR2.0.bat %nowChannel% %subChannel% %build%
cd %nowDic%\SDK\plugins
call genJAR2.0.bat %nowChannel% %subChannel% %build%
goto :eof

::==================================================================================
:Build_UIRes
echo.
echo.
echo --------------------------------------------
echo :Build ui resource begin,please wait.
echo --------------------------------------------
echo.

tasklist|find /i "Unity.exe"&&(taskkill /f /im Unity.exe)
if not exist %nowDic%\Logs (md %nowDic%\Logs)
if exist "%ui_output%" (rd /s /q "%ui_output%")

set UNITY_PATH="%UNITY_PATH_4%\Unity.exe"
set args=-projectPath %build_UIRes_path% -executeMethod JSWSUIToolsEditor.BuildAllUIAssets
set logfile=%nowDic%\Logs\%timenow%.log
%UNITY_PATH% -quit -batchmode -logFile %logfile% %args%
::%UNITY_PATH% -logFile %logfile% %args%

if exist %res_path%\ui\ (rd /s /q %res_path%\ui\)
xcopy "%ui_output%" "%res_path%\ui\" /e/y/q/z
goto :eof

::==================================================================================
:ApkTool_Build
echo.
echo.
echo --------------------------------------------
echo :apktool build begin,please wait.
echo --------------------------------------------
echo.

if exist %nowDic%Apks\apktemp (rd /s /q %nowDic%Apks\apktemp)
setlocal enabledelayedexpansion

cd %nowDic%SDK\tools\apktool
call apktool.bat d -f %nowDic%Apks\%outPut_ApkName%.apk -o %nowDic%Apks\apktemp
call apktool-modify-yml.bat %nowDic%Apks\apktemp\apktool.yml

::build default apk
call apktool.bat b %nowDic%Apks\apktemp -o %nowDic%Apks\%outPut_ApkName%_temp.apk
call apktool-signer.bat %keystore% %keypass% %storepass% %alias% %nowDic%Apks\%outPut_ApkName%_temp.apk %nowDic%Apks\%outPut_ApkName%_compress.apk
zipalign.exe -f 4 %nowDic%Apks\%outPut_ApkName%_compress.apk %nowDic%Apks\%outPut_ApkName%_compress_zipalign.apk
del %nowDic%Apks\%outPut_ApkName%_temp.apk
del %nowDic%Apks\%outPut_ApkName%_compress.apk
ren %nowDic%Apks\%outPut_ApkName%_compress_zipalign.apk %outPut_ApkName%_%subChannel%_compress.apk

echo -----------------"%cconfig%"----------------------
set assetconfigpath=%nowDic%Apks\apktemp\assets\zwwxconfig.xml
if "%cconfig%" neq "" (
	for %%c in (%cconfig%) do (
		echo %%c 媒体广告包Building.........
		cd %nowDic%SDK\tools\assetconfigtool		
		set "op="
		for /f " usebackq tokens=1* delims==" %%i in (%channelconfigini%) do (
			if "%%j"=="" (
				set "op=%%i"
			) else (						
				if [%%c] equ !op! (
					call assetconfigtool.bat %assetconfigpath% %%i %%j
					echo modify [%%i] to [%%j]
				)
			)
		)
		cd %nowDic%SDK\tools\apktool
		call apktool.bat b %nowDic%Apks\apktemp -o %nowDic%Apks\%outPut_ApkName%_[%%c]_temp.apk
		call apktool-signer.bat %keystore% %keypass% %storepass% %alias% %nowDic%Apks\%outPut_ApkName%_[%%c]_temp.apk %nowDic%Apks\%outPut_ApkName%_[%%c]_compress.apk
		zipalign.exe -f 4 %nowDic%Apks\%outPut_ApkName%_[%%c]_compress.apk %nowDic%Apks\%outPut_ApkName%_[%%c]_compress_zipalign.apk
		del %nowDic%Apks\%outPut_ApkName%_[%%c]_temp.apk
		del %nowDic%Apks\%outPut_ApkName%_[%%c]_compress.apk
		ren %nowDic%Apks\%outPut_ApkName%_[%%c]_compress_zipalign.apk %outPut_ApkName%_%subChannel%_[%%c]_compress.apk
		call :CopyApkToServer %nowDic%Apks\%outPut_ApkName%_%subChannel%_[%%c]_compress.apk
	)
) else (
	call :CopyApkToServer %nowDic%Apks\%outPut_ApkName%_%subChannel%_compress.apk
)

if exist %nowDic%Apks\apktemp (rd /s /q %nowDic%Apks\apktemp)
goto :eof

::==================================================================================
:GetChannel_Build
call :ReadIni %bundleIdentifierini% %nowChannel%-%subChannel% build
if defined result (
	set build=%result%
) else (
	call :ReadIni %bundleIdentifierini% %nowChannel%-default build
)
if defined result (
	set build=%result%
)
echo build:[%build%]
goto :eof

::==================================================================================
:GetChannel_BundleIdentifier
call :ReadIni %bundleIdentifierini% %nowChannel%-%subChannel% package
if defined result (
	set identifier=%result%
) else (
	call :ReadIni %bundleIdentifierini% %nowChannel%-default package
)
if defined result (
	set identifier=%result%
)
echo identifier:[%identifier%]
goto :eof

::==================================================================================
:GetChannel_Name
call :ReadIni %bundleIdentifierini% %nowChannel%-%subChannel% name
if defined result (
	set product_name=%result%
) else (
	call :ReadIni %bundleIdentifierini% %nowChannel%-default name
)
if defined result (
	set product_name=%result%
)
echo channel name:[%product_name%]
pause
goto :eof

::==================================================================================
:GetChannel_cdnurl
call :ReadIni %bundleIdentifierini% %nowChannel%-%subChannel% url
if defined result (
	set cdnurl=%result%
) else (
	call :ReadIni %bundleIdentifierini% %nowChannel%-default url
)
if defined result (
	set cdnurl=%result%
)
echo channel url:[%cdnurl%]
goto :eof

::==================================================================================
:GetChannel_channelconfig
call :ReadIni %bundleIdentifierini% %nowChannel%-%subChannel% channelconfig
if defined result (
	set cconfig=%result%
) else (
	call :ReadIni %bundleIdentifierini% %nowChannel%-default channelconfig
)
if defined result (
	set cconfig=%result%
)
echo channel channelconfig:[%cconfig%]
goto :eof

::==================================================================================
:GetChannel_resourceconfig
call :ReadIni %bundleIdentifierini% %nowChannel%-%subChannel% resouceconfig
if defined result (
	set rconfig=%result%
) else (
	call :ReadIni %bundleIdentifierini% %nowChannel%-default resouceconfig
)
if defined result (
	set rconfig=%result%
)
echo channel resouceconfig:[%rconfig%]
goto :eof

::==================================================================================
:GetChannel_keystoreconfig
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
:ReplaceResourceConfig
if "%rconfig%" neq "" (
	for %%c in (%rconfig%) do (
		echo %%c ReplaceResourceConfig.........
		set "op="
		for /f " usebackq tokens=1* delims==" %%i in (%resourceconfigini%) do (
			if "%%j"=="" (
				set "op=%%i"
			) else (						
				if [%%c] equ !op! (				
					copy %nowDic%\ReplaceResource\%%i %projectPath%\%%j
				)
			)
		)
	)
)
goto :eof

::==================================================================================
:CreateCdnUrl
set cdnurlpath=url.txt
echo %cdnurl%>%cdnurlpath%
goto :eof

::==================================================================================
:Show_Version
if not exist "%ver_file%" (
    echo 1.0.0>%ver_file%
    echo 1>>%ver_file%
)

set /a index=0
for /f %%i in (%ver_file%) do (
set /a index+=1
if !index!==1 (set version=%%i)
if !index!==2 (set generation=%%i)
)

::update unity version.text
set unityVersion_file=%projectPath%\Assets\Resources\version.txt
echo %version%>%unityVersion_file%

echo.
echo :App version information
echo -----------------------------------
echo VersionName: %version%
echo VersionCode: %generation%
echo -----------------------------------
echo.
goto :eof

::==================================================================================
:Update_Version
set /p new_version=VersionName:
set /p new_generation=VersionCode:
echo %new_version%>%ver_file%
echo %new_generation%>>%ver_file%
goto :eof

::==================================================================================
:Gen_Gradle_Properties
echo VERSION_NAME=%version%>%gradle_properties_file%
echo VERSION_CODE=%generation%>>%gradle_properties_file%
echo PACKAGE=%identifier%>>%gradle_properties_file%
echo App_Name=%product_name%>>%gradle_properties_file%
goto :eof

::==================================================================================
:costTime
set t1=%time%
set/a "mm=1%t1:~3,2%-1%t0:~3,2%,ss=1%t1:~6,2%-1%t0:~6,2%"
if %ss% lss 0 set/a ss+=60,mm-=1
(if %mm% lss 0 set/a mm+=60)

echo.
echo -----------------------------------
echo Publish complated.Cost time:%mm% m %ss% s
echo -----------------------------------
echo.
goto :eof

::==================================================================================
:CopyApkToServer [#1=sourceapk]
set day=%date:~0,4%%date:~5,2%%date:~8,2%
set server_path=%cloudserver%\%day%
echo.
echo.
echo --------------------------------------------
echo :copy apk to %server_path%  begin,please wait.
echo --------------------------------------------
echo.
if not exist %server_path% (md %server_path%)
::if %nowChannel% neq xigua (
::copy %nowDic%Apks\%outPut_ApkName%_%subChannel%_compress.apk %server_path%
::)
::if %nowChannel% equ xigua (
::copy %nowDic%Apks\%outPut_ApkName%_%subChannel%.apk %server_path%
::)
copy %~1 %server_path% /z
call start %server_path%
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
::echo,Option=%~2,Key=%~3,Value=%result%
goto :eof


::==================================================================================
:GetChannelList [#1=ini]
set "oc="
for /f " usebackq tokens=1* delims==" %%a in ("%~1") do (
    if "%%b"=="" (
		for /f "tokens=1,2 delims=[-]" %%i in ("%%a") do (	
			echo !oc!|find "%%i" >nul||set oc=!oc! [%%i]
		)
    )
)
echo channel lis:%oc%
goto :eof

::==================================================================================
:GetSubChannelList [#1=ini] [#2=channel]
set "os="
for /f " usebackq tokens=1* delims==" %%a in ("%~1") do (
    if "%%b"=="" (
		for /f "tokens=1,2 delims=[-]" %%i in ("%%a") do (	
			if "%~2"=="%%i" (
				::echo !os!|find "%%i" >nul||set os=!os! [%%j]
				set os=!os! [%%j]
			)	
		)
    )
)
echo subChannel list:%os%
goto :eof
