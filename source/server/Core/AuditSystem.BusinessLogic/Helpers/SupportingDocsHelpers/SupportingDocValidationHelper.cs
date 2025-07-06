using AuditSystem.Application.Constants.SupportingDocs;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.SupportingDocs;

namespace AuditSystem.BusinessLogic.Helpers.SupportingDocsHelpers;

public class SupportingDocValidationHelper
{
    private readonly IRepository<Guid, SupportingDoc> _repository;

    public SupportingDocValidationHelper(IRepository<Guid, SupportingDoc> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public static void ValidateUserId(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException(SupportingDocsConstants.UserIdEmptyMessage, nameof(userId));
    }

    public static void ValidateSupportingDocId(Guid supportingDocId)
    {
        if (supportingDocId == Guid.Empty)
            throw new ArgumentException(SupportingDocsConstants.SupportingDocIdEmptyMessage, nameof(supportingDocId));
    }

    public async Task<SupportingDoc> ValidateSupportingDocOwnershipAsync(Guid supportingDocId, Guid userId)
    {
        var supportingDoc = await _repository.GetByIdAsync(supportingDocId)
            ?? throw new KeyNotFoundException(string.Format(SupportingDocsConstants.SupportingDocNotFoundMessage, supportingDocId));
        
        if (supportingDoc.UserId != userId)
        {
            throw new UnauthorizedAccessException(
                string.Format(SupportingDocsConstants.UnauthorizedAccessMessage, userId, supportingDocId));
        }
        
        return supportingDoc;
    }
}