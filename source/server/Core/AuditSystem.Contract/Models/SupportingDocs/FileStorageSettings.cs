namespace AuditSystem.Contract.Models.SupportingDocs;

public class FileStorageSettings
{
    public string UploadPath { get; set; } = default!;
    public string PublicBaseUrl { get; set; } = default!;
    public string SignedUrlSecret {get; set;} = default!;
}