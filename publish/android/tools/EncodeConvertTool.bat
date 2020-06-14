@echo off

set txtpath=%1%
set aCode=%2%
set bCode=%3%

(echo aCode = "UTF-8"
echo bCode = "GB2312"
echo Set objArgs = WScript.Arguments
echo.
echo FileUrlSrc = objArgs^(0^)
echo FileUrlDst = objArgs^(1^)
echo Call WriteToFile^(FileUrlDst, ReadFile^(FileUrlSrc, aCode^), bCode^)
echo.
echo Function ReadFile^(FileUrlSrc, CharSet^)
echo     Dim Str
echo     Set stm = CreateObject^("Adodb.Stream"^)
echo     stm.Type = 2
echo     stm.mode = 3
echo     stm.charset = CharSet
echo     stm.Open
echo     stm.loadfromfile FileUrlSrc
echo     Str = stm.readtext
echo     stm.Close
echo     Set stm = Nothing
echo     ReadFile = Str
echo End Function
echo.
echo Function WriteToFile ^(FileUrlDst, Str, CharSet^)
echo     Set stm = CreateObject^("Adodb.Stream"^)
echo     stm.Type = 2
echo     stm.mode = 3
echo     stm.charset = CharSet
echo     stm.Open
echo     stm.WriteText Str
echo     stm.SaveToFile FileUrlDst, 2
echo     stm.flush
echo     stm.Close
echo     Set stm = Nothing
echo End Function)>U82ANI.vbs

for /r %txtpath% %%a in (*.java) do (
   U82ANI.vbs "%%~a" "%%~a.ansi"
   move /y "%%~a.ansi" "%%~a">nul
)
del U82ANI.vbs