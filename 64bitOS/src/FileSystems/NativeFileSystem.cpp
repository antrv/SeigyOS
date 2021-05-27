#include <cstdio>
#include "FileSystems/NativeFileSystem.h"

class NativeFile : public IFile
{
    std::unique_ptr<std::string> name;

  public:
    NativeFile(const std::string &path) : name{new std::string{path}} {}
    ~NativeFile() override {}
    FileOpenMode GetMode() override { return FileOpenMode::Read; }
    uint64_t GetLength() override { return 0; }
    const std::string &GetName() override { return *name; }
};

NativeFileSystem::NativeFileSystem(const std::string &drive)
{
}

NativeFileSystem::~NativeFileSystem()
{
}

std::unique_ptr<IFile> NativeFileSystem::Open(const std::string &path)
{
    IFile *file = new NativeFile{path};
    return std::unique_ptr<IFile>(file);
}