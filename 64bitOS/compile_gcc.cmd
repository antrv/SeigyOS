@echo off

set PATH=%PATH%;%DEVDIR%\Tools\cmake\bin;%DEVDIR%\Tools\mingw64\bin
set CONFIG=%1
IF "%1"=="" (
  set CONFIG=Debug
)

IF NOT EXIST .tmp (
  mkdir .tmp
)

cd .tmp
cmake.exe -G "MinGW Makefiles" -DCMAKE_BUILD_TYPE=%CONFIG% ..
cmake.exe --build .\
