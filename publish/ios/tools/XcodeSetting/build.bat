@echo off
set BIN=../XcodeSetting.exe
set CC="D:\Program Files\Unity\Editor\Data\MonoBleedingEdge\lib\mono\4.5\mcs.exe"

%CC% *.cs PBXProject2018/*.cs PBXProject2018/Extensions/*.cs PBXProject2018/PBX/*.cs XcodeSetting/*.cs Cocoapods/*.cs -out:%BIN% -r:System.Xml.Linq.dll
pause