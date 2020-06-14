@echo off
::set adb="%Android_Home%\platform-tools\adb.exe"
set adb="%~dp0android_sdk\adb.exe"
set /p apk_path=Install Apk(°ÑAPKÍÏ½øÀ´)£º

%adb% install %apk_path%
pause