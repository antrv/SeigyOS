@echo off

set PATH=%PATH%;%DEVDIR%\Tools\cmake\bin
set CONFIG=%1
IF "%1"=="" (
  set CONFIG=Debug
)

IF NOT EXIST .tmp (
  mkdir .tmp
)

cd .tmp
cmake.exe -DCMAKE_BUILD_TYPE=%CONFIG% -DCMAKE_GENERATOR_PLATFORM=x64 ..
cmake.exe --build .\ --config %CONFIG%
