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
    IFileStorage fileStorage,
    ISupportingDocService supportingDocService,
    ILogger<UploadSupportingDocCommandHandler> logger)
    : IRequestHandler<UploadSupportingDocCommand, Result<Guid>>
{
    private readonly IFileStorage _fileStorage = fileStorage ?? throw new ArgumentNullException(nameof(fileStorage));
    private readonly ISupportingDocService _supportingDocService = supportingDocService ?? throw new ArgumentNullException(nameof(supportingDocService));
    private readonly ILogger<UploadSupportingDocCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<Result<Guid>> Handle(UploadSupportingDocCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.File == null || request.File.Length == 0)
            {
                _logger.LogWarning("Empty file received for user {UserId}", request.UserId);
                return Result.Error("File cannot be empty");
            }

            var fileSaveResult = await _fileStorage.SaveFileAsync(request.File, request.FileName, request.UserId, "SupportingDocs");
            if (!fileSaveResult.IsSuccess)
            {
                return Result.Error(new ErrorList(fileSaveResult.Errors));
            }

            var supportingDocModel = new SupportingDocModel
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                FileName = fileSaveResult.Value.SavedFileName,
                FileSize = fileSaveResult.Value.FileSize,
                DriveFileId = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var docId = await _supportingDocService.CreateSupportingDocAsync(supportingDocModel);
            return Result.Success(docId, "File uploaded successfully");
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
