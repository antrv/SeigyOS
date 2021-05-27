@echo off
set ROOT=%~dp0
set FASM="%DEVDIR%\Tools\fasm\fasm.exe"
set CREATEBOOTIMAGE="%DEVDIR%\Tools\Mosa.Tool.CreateBootImage\Mosa.Tool.CreateBootImage.exe"

mkdir .bin\boot

echo Compiling MBR...
%FASM% %ROOT%\boot\mbr.asm %ROOT%\.bin\boot\mbr.bin
echo Compiling BootSector...
%FASM% %ROOT%\boot\bootsector.asm %ROOT%\.bin\boot\bootsector.bin
echo Compiling Stage 2 Loader...
%FASM% %ROOT%\boot\kloader.asm %ROOT%\.bin\boot\kloader.bin
echo Create VHD...
%CREATEBOOTIMAGE% %ROOT%\boot.config %ROOT%\.bin\disk.vhd
