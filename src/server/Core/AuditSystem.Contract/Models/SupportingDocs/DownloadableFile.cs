namespace AuditSystem.Contract.Models.SupportingDocs;

public class DownloadableFile
{
    public string FilePath { get; set; } = default!;
    public string FileName { get; set; } = default!;
    public string ContentType { get; set; } = "application/octet-stream";
}
