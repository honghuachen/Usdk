@echo off
setlocal enabledelayedexpansion
set t0=%time%
set nowDic=%~dp0
set /p source_apk_path=source apk: 
set output_apk_path=%nowDic%apks
set medias=%nowDic%\medias.txt
set mediasini=%nowDic%\medias.ini
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

cd %nowDic%SDK\tools\apktool
call apktool.bat d -f %source_apk_path% -o %output_apk_path%\apktemp
call apktool-modify-yml.bat %output_apk_path%\apktemp\apktool.yml
set assetconfigpath=%output_apk_path%\apktemp\assets\zwwxconfig.xml
	
	::---------------------------
	set "op="
	set "op1="
	for /f " usebackq tokens=1* delims==" %%a in ("%mediasini%") do (
		if "%%b"=="" (
			set "op=%%a"
			if %%a neq [default] (
				echo.
				echo !op! 媒体广告包Building.........
				cd %nowDic%SDK\tools\assetconfigtool			
				for /f " usebackq tokens=1* delims==" %%i in ("%mediasini%") do (
					if "%%j"=="" (
						set "op1=%%i"
					) else (						
						if !op! equ !op1! (
							call assetconfigtool.bat %assetconfigpath% %%i %%j
						)
					)
				)
				cd %nowDic%SDK\tools\apktool
				call apktool.bat b %nowDic%apks\apktemp -o %output_apk_path%\%apkname%_!op!_temp.apk
				call apktool-signer.bat %keystore% %keypass% %storepass% %alias% %output_apk_path%\%apkname%_!op!_temp.apk %output_apk_path%\%apkname%_!op!_compress.apk
				zipalign.exe -f 4 %output_apk_path%\%apkname%_!op!_compress.apk %output_apk_path%\%apkname%_!op!_compress_zipalign.apk
				del %output_apk_path%\%apkname%_!op!_temp.apk
				del %output_apk_path%\%apkname%_!op!_compress.apk
				ren %output_apk_path%\%apkname%_!op!_compress_zipalign.apk %apkname%_!op!.apk
			)
		)	
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