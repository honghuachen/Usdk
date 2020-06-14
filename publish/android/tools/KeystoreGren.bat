@echo off
set keytool=%~dp0jdk1.8.0_102\bin\keytool.exe

%keytool% -genkey -alias cdh -keyalg RSA -validity 20000 -keystore KeystoreGren.keystore
