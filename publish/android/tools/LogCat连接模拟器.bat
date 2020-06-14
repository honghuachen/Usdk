@echo off
d:
cd D:\Android\sdk\platform-tools
::天天模拟器
::adb connect 127.0.0.1:6555

::海马玩模拟器
::adb connect 127.0.0.1:26944

::逍遥游模拟器
::adb connect 127.0.0.1:51358

::夜神模拟器
::adb connect 127.0.0.1:62001

echo ----------------
echo 1.tian tian
echo 2.hai ma wan
echo 3.xiao yao you
echo 4.ye shen
echo ----------------

set /p selIdx=Select item:

if %selIdx% equ 1 (
adb connect 127.0.0.1:6555
)

if %selIdx% equ 2 (
adb connect 127.0.0.1:26944
)

if %selIdx% equ 3 (
adb connect 127.0.0.1:51358
)

if %selIdx% equ 4 (
adb connect 127.0.0.1:62001
)

pause