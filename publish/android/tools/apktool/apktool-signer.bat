@echo off

::set jarsigner="%Java_Home%/bin/jarsigner.exe"
set jarsigner=%~dp0..\Java\jdk1.8.0_102\bin\jarsigner.exe
::%jarsigner% -digestalg SHA1 -sigalg MD5withRSA -tsa https://timestamp.geotrust.com/tsa -storepass f3f9be6df8a2327bf891b5235ec6101d -keypass b8aa15f0ddac2b14a753b48c6367ec4f -keystore %keystore% -signedjar %channel%_signed.apk %keystoreApk% 15515.keystore

set keystore=%1%
set keypass=%2%
set storepass=%3%
set alias=%4%
set sourceapk=%5%
set output=%6%
%jarsigner% -digestalg SHA1 -sigalg MD5withRSA -storepass %storepass% -keypass %keypass% -keystore %keystore% -signedjar %output% %sourceapk% %alias%