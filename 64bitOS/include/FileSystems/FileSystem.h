#pragma once

#include <cstdint>
#include <memory>
#include <string>

struct DiskGeometry
{
    uint16_t Cylinders;
    uint8_t Heads; // tracks per cylinder
    uint8_t SectorsPerTrack;
    uint16_t BytesPerSector;
};

enum class FileOpenMode : uint8_t
{
    Read = 0,
    ReadWrite = 1,
};

class IFileSystemEntry
{
  public:
    virtual ~IFileSystemEntry() {}
    virtual const std::string &GetName() = 0;
};

class IFile : public IFileSystemEntry
{
  public:
    ~IFile() override {}
    virtual FileOpenMode GetMode() = 0;
    virtual uint64_t GetLength() = 0;
};

class IDirectory : public IFileSystemEntry
{
  public:
    virtual ~IDirectory() {}
};

class IFileSystem
{
  public:
    virtual ~IFileSystem() {}
    virtual std::unique_ptr<IFile> Open(const std::string &path) = 0;
};

class IDiskDrive
{
  public:
    virtual ~IDiskDrive() {}
    virtual const DiskGeometry &GetGeometry() = 0;
};