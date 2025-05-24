namespace AuditSystem.Contract.Models.SupportingDocs;

public class FileHttpResult(string url)
{
    public string Url { get; init; } = url;
}
