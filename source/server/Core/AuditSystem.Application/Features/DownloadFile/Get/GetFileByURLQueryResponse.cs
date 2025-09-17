namespace AuditSystem.Application.Features.DownloadFile.Get;

public sealed class GetFileByURLQueryResponse
{
    public Stream FileStream { get; }
    public string ContentType { get; }
    public string FileName { get; }

    public GetFileByURLQueryResponse(
        Stream fileStream,
        string contentType,
        string fileName)
    {
        FileStream = fileStream;
        ContentType = contentType;
        FileName = fileName;
    }
}