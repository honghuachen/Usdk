@echo off
rem if "%PATH_BASE%" == "" set PATH_BASE=%PATH%
rem set PATH=%CD%;%PATH_BASE%;
::%JAVA_HOME/%java -jar -Duser.language=en "%~dp0\GenCodeEngine.jar" %1 %2 %3 %4 %5 %6 %7 %8 %9

set java_t=%~dp0..\Java\jdk1.8.0_102\bin\java.exe
%java_t% -jar -Duser.language=en "%~dp0\GenCodeEngine.jar" %1 %2 %3 %4 %5 %6 %7 %8 %9