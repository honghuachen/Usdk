
@echo off
set /p apk_path=Look Apk Info

call ./apktool/apktool.bat d -f %apk_path% -o "C:\Users\chendehuai\Desktop\apktemp"
pause