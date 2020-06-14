@echo off
set /p sourceapk=apk:

::set set keystore=%~dp0..\common\%channel%\code_android\yulesign.keystore
set keystore=E:\Work\client_build_android\Publish\Android\SDK\common\keystore\xgsdk.keystore
set jarsigner="%Java_Home%/bin/jarsigner.exe"

%jarsigner% -digestalg SHA1 -sigalg MD5withRSA -storepass f3f9be6df8a2327bf891b5235ec6101d -keypass b8aa15f0ddac2b14a753b48c6367ec4f -keystore %keystore% -signedjar C:\Users\Administrator\Desktop\signer.apk %sourceapk% 15515.keystore
pause