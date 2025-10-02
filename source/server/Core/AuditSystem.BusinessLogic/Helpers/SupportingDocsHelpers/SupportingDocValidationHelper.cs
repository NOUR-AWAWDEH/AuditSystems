using AuditSystem.Application.Constants.SupportingDocs;
using AuditSystem.BusinessLogic.Utilities;
using AuditSystem.Contract.Interfaces.ModelServices.SupportingDocsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.SupportingDocs;

namespace AuditSystem.BusinessLogic.Helpers.SupportingDocsHelpers;

public class SupportingDocValidationHelper(IRepository<Guid, SupportingDoc> repository) : ISupportingDocValidation
{
    private readonly IRepository<Guid, SupportingDoc> _repository = repository ?? throw new ArgumentNullException(nameof(repository));

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
        return await OwnershipValidator.ValidateOwnershipAsync<SupportingDoc, Guid>(
            _repository.Query(),
            sd => sd.Id == supportingDocId,
            SupportingDocsConstants.SupportingDocOwnerIdSelector,
            userId,
            string.Format(SupportingDocsConstants.SupportingDocNotFoundMessage, supportingDocId),
            string.Format(SupportingDocsConstants.UnauthorizedAccessMessage, userId, supportingDocId)
        );
    }
}