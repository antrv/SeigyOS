cmake_minimum_required (VERSION 3.5)

project (App1)

#set(CMAKE_VERBOSE_MAKEFILE ON)
#set(CMAKE_BUILD_TYPE Release)

set(CMAKE_CXX_STANDARD 17)
set(CMAKE_GENERATOR_PLATFORM x64)
include_directories("${PROJECT_SOURCE_DIR}")
include_directories("${PROJECT_SOURCE_DIR}/include")

#add_definitions(-DCMAKE_EXPORT_COMPILE_COMMANDS=1)

set(BINARY_SUBDIR "${CMAKE_BUILD_TYPE}")
if (${CMAKE_BUILD_TYPE} STREQUAL Release)

  if ("${CMAKE_CXX_COMPILER_ID}" STREQUAL "Clang")
    # Clang
  elseif ("${CMAKE_CXX_COMPILER_ID}" STREQUAL "GNU")
    # GCC
    set(CMAKE_C_FLAGS "${CMAKE_C_FLAGS} -O3 -s")
    set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -O2 -s") # -fdata-sections -ffunction-sections -fwhole-program -Wl,--gc-sections
  elseif ("${CMAKE_CXX_COMPILER_ID}" STREQUAL "Intel")
    # Intel C++
  elseif ("${CMAKE_CXX_COMPILER_ID}" STREQUAL "MSVC")
    # Visual Studio C++
    set(CMAKE_C_FLAGS "${CMAKE_C_FLAGS} /Ox")
    set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} /Ox")
  endif()

endif()

if ("${CMAKE_CXX_COMPILER_ID}" STREQUAL "Clang")
  # Clang
  set(BINARY_DIR "${CMAKE_SOURCE_DIR}/.bin/${BINARY_SUBDIR}")
elseif ("${CMAKE_CXX_COMPILER_ID}" STREQUAL "GNU")
  # GCC
  set(CMAKE_C_FLAGS "${CMAKE_C_FLAGS} -Wall -m64 -masm=intel -msse4.2 -mfpmath=sse") # -Wa,-adhln -fverbose-asm
  set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -Wall -m64 -masm=intel -msse4.2 -mfpmath=sse") # -Wa,-adhln -fverbose-asm
  set(BINARY_DIR "${CMAKE_SOURCE_DIR}/.bin/${BINARY_SUBDIR}")
elseif ("${CMAKE_CXX_COMPILER_ID}" STREQUAL "Intel")
  # Intel C++
  set(BINARY_DIR "${CMAKE_SOURCE_DIR}/.bin/${BINARY_SUBDIR}")
elseif ("${CMAKE_CXX_COMPILER_ID}" STREQUAL "MSVC")
  # Visual Studio C++
  set(CMAKE_C_FLAGS "${CMAKE_C_FLAGS} /Gv /W4 /FC /FAsu /Gw")
  set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} /Gv /W4 /FC /FAsu /Gw") # /GL /LTCG /Gw
  set(BINARY_DIR "${CMAKE_SOURCE_DIR}/.bin")
endif()

set(CMAKE_SHARED_LIBRARY_PREFIX "")

set(CMAKE_RUNTIME_OUTPUT_DIRECTORY "${BINARY_DIR}")
set(CMAKE_ARCHIVE_OUTPUT_DIRECTORY "${BINARY_DIR}")
set(CMAKE_LIBRARY_OUTPUT_DIRECTORY "${BINARY_DIR}")


#file(GLOB_RECURSE FSLIB src/*.cpp)
#add_library(fslib SHARED ${FSLIB})
#add_executable(app ${FSLIB})

#add_executable(app src/main.cpp)
#set_target_properties(app PROPERTIES OUTPUT_NAME "app")
#target_link_libraries (app stdlib)

add_library(kloader SHARED src/kloader.cpp ../.tmp/kloader.asm.obj)
#target_compile_options(kloader PUBLIC -Wall -m64 -masm=intel -msse4.2 -mfpmath=sse -mabi=ms -mcmodel=small -Os -nostdinc -nostdinc++ -nostdlib -fno-builtin -fno-rtti -fno-exceptions -fno-stack-protector -fomit-frame-pointer -fno-ident -ffreestanding -nostartfiles -nodefaultlibs -s -fwhole-program -e kloader_entry)
set_target_properties(kloader PROPERTIES CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -Wall -m64 -masm=intel -msse4.2 -mfpmath=sse -mabi=ms -mcmodel=small -Os -nostdinc -nostdinc++ -nostdlib -fno-builtin -fno-rtti -fno-exceptions -fno-stack-protector -fomit-frame-pointer -fno-ident -ffreestanding -nostartfiles -nodefaultlibs -s -fwhole-program -e kloader_entry")
set_target_properties(kloader PROPERTIES LINK_FLAGS "-T ${CMAKE_SOURCE_DIR}/boot/kloader.ld -Wl,--gc-sections -Wl,--build-id=none -Wl,--strip-discarded -Wl,--strip-all -Wl,-O -Wl,--discard-all -Wl,-nostdlib -Wl,--no-seh -Wl,--high-entropy-va")
