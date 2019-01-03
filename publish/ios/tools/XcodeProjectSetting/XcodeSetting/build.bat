@echo off
set BIN=./bin/XcodeSetting.exe
set CC="D:\Program Files\Unity2017.4.15f1\Editor\Data\MonoBleedingEdge\lib\mono\4.5\mcs.exe"

%CC% *.cs PBX/*.cs XcodeSetting/*.cs -out:%BIN% -r:./bin/System.Xml.Linq.dll
pause