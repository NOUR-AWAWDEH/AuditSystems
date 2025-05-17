using Ardalis.Result;
using AuditSystem.Contract.Interfaces.FileStorage;
using AuditSystem.Contract.Interfaces.ModelServices.SupportingDocsServices;
using AuditSystem.Contract.Models.SupportingDocs;
using AutoMapper;
using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.SupportingDocs.Uplode;

public class UploadSupportingDocCommandHandler : IRequestHandler<UploadSupportingDocCommand, Result<Guid>>
{
    private readonly IFileStorage _fileStorage;
    private readonly ISupportingDocService _supportingDocService;
    private readonly IMapper _mapper;
    private readonly ILogger<UploadSupportingDocCommandHandler> _logger;

    public UploadSupportingDocCommandHandler(
        IFileStorage fileStorage,
        ISupportingDocService supportingDocService,
        IMapper mapper,
        ILogger<UploadSupportingDocCommandHandler> logger)
    {
        _fileStorage = fileStorage ?? throw new ArgumentNullException(nameof(fileStorage));
        _supportingDocService = supportingDocService ?? throw new ArgumentNullException(nameof(supportingDocService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Result<Guid>> Handle(UploadSupportingDocCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate request
            if (request.File == null || request.File.Length == 0)
            {
                _logger.LogWarning("Empty file received for user {UserId}", request.UserId);
                return Result.Error("File cannot be empty");
            }

            // Save the file
            var fileSaveResult = await _fileStorage.SaveFileAsync(request.File, request.FileName);
            if (!fileSaveResult.IsSuccess)
            {
                return Result.Error((ErrorList)fileSaveResult.Errors);
            }

            // Create document with proper IDs
            var supportingDocModel = new SupportingDocModel
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                FileName = fileSaveResult.Value.SavedFileName,
                FileSize = fileSaveResult.Value.FileSize,
                URL = fileSaveResult.Value.FullPath,
                DriveFileId = Guid.NewGuid(), // Changed from Guid.Empty
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Save to database
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