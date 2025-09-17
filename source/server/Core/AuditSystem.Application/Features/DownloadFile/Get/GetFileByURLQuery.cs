using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.DownloadFile.Get;

public sealed record class GetFileByURLQuery(string URL) : ICommand<Result<GetFileByURLQueryResponse>>;