namespace AuditSystem.Contract.Interfaces.FileStorage;

public interface IFilePathBuilder
{
    string GetRelativePath(Guid userId, string fileName, string serviceName);
    string GetFullPath(Guid userId, string fileName, string serviceName);
    string CombineWithUploadPath(string relativePath);
}
