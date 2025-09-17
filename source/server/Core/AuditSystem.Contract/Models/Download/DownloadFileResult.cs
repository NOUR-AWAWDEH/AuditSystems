namespace AuditSystem.Contract.Models.Download;
public class DownloadFileResult
{
    public Stream FileStream { get; set; }
    public string ContentType { get; set; }
    public string FileName { get; set; }

    public DownloadFileResult(Stream fileStream, string contentType, string fileName)
    {
        FileStream = fileStream;
        ContentType = contentType;
        FileName = fileName;
    }
}

