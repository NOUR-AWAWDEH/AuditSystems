// AuditSystem.Contract/Models/SupportingDocs/FileSaveDetails.cs
namespace AuditSystem.Contract.Models.SupportingDocs;

public class FileSaveDetails
{
    public string SavedFileName { get; set; } = string.Empty;
    public string FullPath { get; set; } = string.Empty;
    public string PublicUrl { get; set; } = string.Empty;
    public long FileSize { get; set; }
}