using Microsoft.Identity.Client;

namespace AuditSystem.Contract.Interfaces.FileStorage;

public interface IFilePathBuilder
{
    string GetFullPath(Guid userId, string fileName, string serviceName);
    string CombineWithUploadPath(string relativePath);
}
