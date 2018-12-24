java -jar "%~dp0\apktool.jar" d source.apk -o target
java -jar bin.jar
rd /s /q target
pause