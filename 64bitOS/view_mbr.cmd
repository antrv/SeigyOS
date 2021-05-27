@echo off
set ROOT=%~dp0
set BIEW="%DEVDIR%\Tools\biewx64\biew.exe"

%BIEW% %ROOT%\.bin\boot\mbr.bin
