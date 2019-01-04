BIN=../bin/XcodeSetting.exe
MCS="D:\Program Files\Unity2017.4.15f1\Editor\Data\MonoBleedingEdge\lib\mono\4.5\mcs.exe"

"$MCS" XcodeSetting/*.cs XcodeSetting/PBX/*.cs XcodeSetting/XcodeSetting/*.cs -out:$BIN -debug -unsafe -r:./XcodeSetting/bin/System.Xml.Linq.dll
read -p "按回车键继续"