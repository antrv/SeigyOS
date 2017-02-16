## SeigyOS is Operating System in C# and .NET

Seigyo-Sei (制御性) means Managed in Japanese.

The idea is to write OS in C# that will run in fully Managed .NET runtime:
1) First, the kernel is prepared with minimum native code.
2) The bootloader loads kernel in memory and runs it
3) The kernel initializes memory manager, builds structures in memory for .NET runtime, associates native methods in kernel with managed methods
4) The kernel initializes JIT-compiler, after that point the native code contains stubs to JIT-compiler
5) The kernel initializes GC, loads drivers, etc
6) The simple command line runs (graphical desktop in the future)
7) You can run console .NET applications (or graphical in the future)

This is not so hard to do how it looks. We have now with opened sources:
1) .NET Core runtime and BCL
2) Mono
3) MOSA
4) Cosmos
5) System.Reflection.Metadata, Mono.Cecil libraries
6) Roslyn

The 1st iteration will include:
1) The minimum required BCLs
2) JIT, runtime (GC?)
3) Stubs for memory managing, file system functions, and drivers (console and keyboard). The invokation will be redirected to the real OS (windows or linux).
4) Minimal kernel image
5) The bootloader emulator

The 2nd iteration will also include:
1) Real bootloader
2) Real memory manager
2) Real console and keyboard drivers
3) File system support (FAT probably)

From the 3rd iteration I would like to invite enthusiasts to help with BCL and drivers implemetation, JIT optimizations, etc.
