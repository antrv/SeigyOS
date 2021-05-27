@echo off

set PATH=%PATH%;%DEVDIR%\Tools\mingw64\bin

set ROOT=%~dp0

ld.exe -T %ROOT%boot\kloader.ld --gc-sections --build-id=none --strip-discarded ^
       --strip-all -O --discard-all -nostdlib --no-seh --high-entropy-va ^
       --output %ROOT%.bin\boot\kloader.bin ^
       %ROOT%.tmp\kloader.asm.obj %ROOT%.tmp\CMakeFiles\kloader.dir\src\kloader.cpp.obj
