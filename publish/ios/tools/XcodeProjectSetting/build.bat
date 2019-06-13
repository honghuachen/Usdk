@echo off
set BIN=../XcodeSetting.exe
set CC="D:\Program Files\Unity\Editor\Data\MonoBleedingEdge\lib\mono\4.5\mcs.exe"

%CC% XcodeSetting/*.cs XcodeSetting/PBX/*.cs XcodeSetting/XcodeSetting/*.cs -out:%BIN% -r:System.Xml.Linq.dll
pause