@echo off
rem if "%PATH_BASE%" == "" set PATH_BASE=%PATH%
rem set PATH=%CD%;%PATH_BASE%;
::%JAVA_HOME/%java -jar -Duser.language=en "%~dp0\AssetConfigTool.jar" %1 %2 %3 %4

set java_t=%~dp0..\Java\jdk1.8.0_102\bin\java.exe
%java_t% -jar -Duser.language=en "%~dp0\AssetConfigTool.jar" %1 %2 %3 %4