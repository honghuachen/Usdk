@echo off
::set adb="%Android_Home%\platform-tools\adb.exe"
set adb="%~dp0android_sdk\adb.exe"

%adb% logcat -s Unity
