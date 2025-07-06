namespace AuditSystem.Contract.Interfaces.FileStorage;

public interface ISignedUrlService
{
    string GenerateSignedUrl(string filePath, TimeSpan validFor);
    bool ValidateSignedUrl(string filePath, long expires, string signature);
}