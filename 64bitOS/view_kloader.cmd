@echo off
set ROOT=%~dp0
set BIEW="%DEVDIR%\Tools\biew-610\biew.exe"

%BIEW% %ROOT%\.bin\boot\kloader.bin
