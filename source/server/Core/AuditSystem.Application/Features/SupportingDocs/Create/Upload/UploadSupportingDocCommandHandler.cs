using Ardalis.Result;
using AuditSystem.Contract.Interfaces.FileStorage;
using AuditSystem.Contract.Interfaces.ModelServices.SupportingDocsServices;
using AuditSystem.Contract.Models.SupportingDocs;
using AutoMapper;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.SupportingDocs.Create.Upload;

public class UploadSupportingDocCommandHandler(
    ISupportingDocService supportingDocService,
    ILogger<UploadSupportingDocCommandHandler> logger)
    : IRequestHandler<UploadSupportingDocCommand, Result<UploadSupportingDocCommandResponse>>
{
    private readonly ISupportingDocService _supportingDocService = supportingDocService ?? throw new ArgumentNullException(nameof(supportingDocService));
    private readonly ILogger<UploadSupportingDocCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<Result<UploadSupportingDocCommandResponse>> Handle(UploadSupportingDocCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.File == null || request.File.Length == 0)
            {
                _logger.LogWarning("Empty file received for user {UserId}", request.UserId);
                return Result.Error("File cannot be empty");
            }

            var docId = await _supportingDocService.CreateSupportingDocAsync(request.File, request.FileName, request.UserId);
            if (!docId.IsSuccess)
            {
                return Result.Error(new ErrorList(docId.Errors));
            }

            var response = new UploadSupportingDocCommandResponse(docId.Value);
            return Result.Success(response, "File uploaded successfully");
        }
        catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2601)
        {
            _logger.LogError(ex, "Duplicate DriveFileId violation");
            return Result.Error("Document storage conflict. Please try again.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading file");
            return Result.Error("Upload failed");
        }
    }
}
