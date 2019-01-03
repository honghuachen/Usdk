BIN=./bin/XcodeSetting.exe
MCS="D:\Program Files\Unity2017.4.15f1\Editor\Data\MonoBleedingEdge\lib\mono\4.5\mcs.exe"

"$MCS" *.cs PBX/*.cs XcodeSetting/*.cs -out:$BIN -debug -unsafe -r:./bin/System.Xml.Linq.dll
read -p "按回车键继续"