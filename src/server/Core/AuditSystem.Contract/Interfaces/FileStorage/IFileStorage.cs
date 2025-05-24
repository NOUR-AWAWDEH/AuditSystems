using Ardalis.Result;
using AuditSystem.Contract.Models.SupportingDocs;
using Microsoft.AspNetCore.Http;

namespace AuditSystem.Contract.Interfaces.FileStorage;

public interface IFileStorage
{
    Task<Result<FileSaveDetails>> SaveFileAsync(IFormFile file, string userProvidedFileName, Guid userId, string serviceName);
}