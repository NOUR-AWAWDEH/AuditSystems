using AuditSystem.Contract.Interfaces.FileStorage;
using Microsoft.Extensions.Configuration;

namespace AuditSystem.BusinessLogic.Services.FileStorageServices;

public class FilePathBuilder : IFilePathBuilder
{
    private readonly string _rootPath;

    public FilePathBuilder(IConfiguration config)
    {
        _rootPath = Environment.GetEnvironmentVariable("FILESTORAGE_UPLOADPATH")
            ?? throw new InvalidOperationException("Missing FILESTORAGE_UPLOADPATH");
    }

    public string GetFullPath(Guid userId, string fileName, string serviceName)
    {
        var relativePath = GetRelativePath(userId, fileName, serviceName);
        var fullPath = Path.Combine(_rootPath, relativePath);
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);
        return Path.GetFullPath(fullPath);
    }

    public string GetRelativePath(Guid userId, string fileName, string serviceName)
    {
        return Path.Combine(userId.ToString(), serviceName, fileName);
    }

    public string CombineWithUploadPath(string relativePath)
    {
        var fullPath = Path.Combine(_rootPath, relativePath);
        return Path.GetFullPath(fullPath);
    }
}
