using AuditSystem.Contract.Models.SupportingDocs;

namespace AuditSystem.Application.Features.SupportingDocs.Get.GetURLByFileId;

public record class GetSupportingDocURLQueryByIdResponse
{
    public string FileName { get; init; } = string.Empty;
    public FileHttpResult FileHttpResult { get; init; } = null!;
}
