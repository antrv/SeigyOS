@echo off
set ROOT=%~dp0
set QEMUDIR=%DEVDIR%\Tools\qemu
set QEMU=%QEMUDIR%\qemu-system-x86_64.exe
set QEMUIMG=%QEMUDIR%\qemu-img.exe
set DRIVE=%ROOT%.bin\disk.vhd
set DRIVERAW=%ROOT%.bin\disk.raw

rem %QEMU% -machine q35 -cpu max -smp 2 -m 512 -net nic,model=virtio -net user -drive file=%DRIVE%,format=vpc,if=virtio
rem %QEMUIMG% convert -f vpc -O raw %DRIVE% %DRIVERAW%
%QEMU% -machine q35 -cpu max -smp 2 -m 512 -net nic,model=virtio -net user -drive file=%DRIVE%,index=0,media=disk,format=vpc
