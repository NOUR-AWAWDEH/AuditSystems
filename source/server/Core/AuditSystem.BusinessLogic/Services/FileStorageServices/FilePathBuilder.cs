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

    public string GetFullPath(Guid userId, string serviceName, string fileName)
    {
        var relativePath = GetRelativePath(userId, serviceName, fileName);
        var fullPath = Path.Combine(_rootPath, relativePath);
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);
        return Path.GetFullPath(fullPath);
    }

    public string GetFullPath(string relativePath)
    {
        var fullPath = Path.Combine(_rootPath, relativePath);
        return Path.GetFullPath(fullPath);
    } 

    private static string GetRelativePath(Guid userId,  string serviceName, string fileName)
    {
        return Path.Combine(userId.ToString(), serviceName, fileName);
    }

    public string CombineWithUploadPath(string relativePath)
    {
        var fullPath = Path.Combine(_rootPath, relativePath);
        return Path.GetFullPath(fullPath);
    }
}
