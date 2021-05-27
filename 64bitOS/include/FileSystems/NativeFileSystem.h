#pragma once

#include "FileSystems/FileSystem.h"

class NativeFileSystemImpl;

class NativeFileSystem : public IFileSystem
{
  public:
    NativeFileSystem(const std::string &drive);
    ~NativeFileSystem() override;
    std::unique_ptr<IFile> Open(const std::string &path) override;
};