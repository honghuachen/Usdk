@echo off
set adb="%~dp0android_sdk\adb.exe"
%adb% forward tcp:54999 localabstract:Unity-com.linyou.ssss
pause