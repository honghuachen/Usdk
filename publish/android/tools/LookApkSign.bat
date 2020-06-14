@echo off
set keytool="C:\Program Files\Java\jre1.8.0_202\bin\keytool.exe"

%keytool% -list -printcert -jarfile E:\Download\ACTX_YUXIANG_PUB_V1.1.12.4_20191203_2031.apk
::%keytool% -list -keystore E:\ACT\branches\os2\project\platform\Keystore\actx.keystore
pause