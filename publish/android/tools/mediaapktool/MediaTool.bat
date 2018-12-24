@echo off
setlocal enabledelayedexpansion
set t0=%time%
set nowDic=%~dp0
set /p source_apk_path=source apk: 
set output_apk_path=%nowDic%apks
set medias=%nowDic%\medias.txt
call :GetSourceApkName %source_apk_path%
set apkname=%filename%

echo. %apkname%
echo.
echo --------------------------------------------
echo :apk tool build begin,please wait.
echo --------------------------------------------
echo.

if exist %output_apk_path%\apktemp (rd /s /q %output_apk_path%\apktemp)
set keystore=%nowDic%SDK\common\keystore\xgsdk.keystore
set keypass=b8aa15f0ddac2b14a753b48c6367ec4f
set storepass=f3f9be6df8a2327bf891b5235ec6101d
set alias=15515.keystore

::cd %nowDic%SDK\tools\apktool
::call apktool-signer.bat %keystore% %keypass% %storepass% %alias% %source_apk_path% 1003_signer.apk
::pause

cd %nowDic%SDK\tools\apktool
call apktool.bat d -f %source_apk_path% -o %output_apk_path%\apktemp
call apktool-modify-yml.bat %output_apk_path%\apktemp\apktool.yml
set assetconfigpath=%output_apk_path%\apktemp\assets\config.xml

	::build adid apk
	for /f %%i in (%medias%) do (
		echo.
		echo %%i 媒体广告包Building.........
		cd %nowDic%SDK\tools\assetconfigtool
		call assetconfigtool.bat %assetconfigpath% AdId %%i
		echo %output_apk_path%\%apkname%_%%i

		cd %nowDic%SDK\tools\apktool
		call apktool.bat b %nowDic%apks\apktemp -o %output_apk_path%\%apkname%_%%i_temp.apk
		call apktool-signer.bat %keystore% %keypass% %storepass% %alias% %output_apk_path%\%apkname%_%%i_temp.apk %output_apk_path%\%apkname%_%%i_compress.apk
		zipalign.exe -f 4 %output_apk_path%\%apkname%_%%i_compress.apk %output_apk_path%\%apkname%_%%i_compress_zipalign.apk
		del %output_apk_path%\%apkname%_%%i_temp.apk
		del %output_apk_path%\%apkname%_%%i_compress.apk
		ren %output_apk_path%\%apkname%_%%i_compress_zipalign.apk %apkname%_%%i.apk
	)
	
if exist %nowDic%apks\apktemp (rd /s /q %nowDic%apks\apktemp)
call :costTime
pause
goto :eof

::==================================================================================
:GetSourceApkName
set filename=%~n1
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