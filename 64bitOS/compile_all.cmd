@echo off

set PATH=%PATH%;%DEVDIR%\Tools\cmake\bin;%DEVDIR%\Tools\mingw64\bin
set CONFIG=%1
IF "%1"=="" (
  set CONFIG=Debug
)

if not exist .tmp (
  mkdir .tmp
)

set ROOT=%~dp0
set FASM="%DEVDIR%\Tools\fasm\fasm.exe"
set CREATEBOOTIMAGE="%DEVDIR%\Tools\Mosa.Tool.CreateBootImage\Mosa.Tool.CreateBootImage.exe"

mkdir .bin\boot

echo Compiling MBR...
%FASM% %ROOT%\boot\mbr.asm %ROOT%\.bin\boot\mbr.bin
IF ERRORLEVEL 1 GOTO :eof

echo Compiling BootSector...
%FASM% %ROOT%\boot\bootsector.asm %ROOT%\.bin\boot\bootsector.bin
IF ERRORLEVEL 1 GOTO :eof

echo Compiling Stage 2 Loader...
%FASM% %ROOT%\boot\kloader.asm %ROOT%\.tmp\kloader.asm.obj
rem %FASM% %ROOT%\boot\kloader.asm %ROOT%\.bin\boot\kloader.bin
IF ERRORLEVEL 1 GOTO :eof

cd .tmp
cmake.exe -G "MinGW Makefiles" -DCMAKE_BUILD_TYPE=%CONFIG% ..
IF ERRORLEVEL 1 GOTO :eof
cmake.exe --build .\
IF ERRORLEVEL 1 GOTO :eof

objcopy -O binary -j .text %ROOT%\.bin\Debug\kloader.dll %ROOT%\.bin\boot\kloader.bin
IF ERRORLEVEL 1 GOTO :eof

cd ..
echo Create VHD...
%CREATEBOOTIMAGE% --out %ROOT%\.bin\disk.vhd --format vhd --filesystem fat32 --blocks 120000 --volume-label SYSTEM --mbr %ROOT%.bin\boot\mbr.bin --boot .bin\boot\bootsector.bin %ROOT%.bin\boot\kloader.bin
IF ERRORLEVEL 1 GOTO :eof
