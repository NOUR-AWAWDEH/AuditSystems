using Ardalis.Result;

namespace AuditSystem.Contract.Interfaces.FileStorage;

public interface IURLService
{
    Task<Result<string>> GenerateSignedURLAsync(Guid userId, string fileName, string serviceName);
}