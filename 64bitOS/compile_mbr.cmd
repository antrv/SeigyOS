@echo off
set ROOT=%~dp0
set FASM="%DEVDIR%\Tools\fasm\fasm.exe"
set CREATEBOOTIMAGE="%DEVDIR%\Tools\Mosa.Tool.CreateBootImage\Mosa.Tool.CreateBootImage.exe"

mkdir .bin\boot

echo Compiling MBR...
%FASM% %ROOT%\boot\mbr.asm %ROOT%\.bin\boot\mbr.bin
