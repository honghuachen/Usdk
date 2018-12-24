@echo off
call manifest-merger.bat -v merge -o android.xml --libs AndroidManifest.xml --main AndroidManifest1.xml
pause