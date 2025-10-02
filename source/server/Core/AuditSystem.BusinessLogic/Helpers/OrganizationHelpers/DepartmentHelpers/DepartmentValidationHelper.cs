using AuditSystem.Application.Constants.Organization.DepartmentConst;
using AuditSystem.Application.Constants.Organization.SublocationConst;
using AuditSystem.BusinessLogic.Utilities;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.DepartmentServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.Organization;

namespace AuditSystem.BusinessLogic.Helpers.OrganizationHelpers.DepartmentHelpers;

public class DepartmentValidationHelper(
    IRepository<Guid, Department> repository)
    : IDepartmentValidation
{
    private readonly IRepository<Guid, Department> _repository = repository
        ?? throw new ArgumentNullException(nameof(repository));

    public static void ValidateUserId(Guid userId)
    {
        if (userId == Guid.Empty) throw new ArgumentException(DepartmentConstants.UserIdEmptyMessage, nameof(userId));
    }

    public async Task<Department> ValidateDepartmentOwnershipAsync(Guid departmentId, Guid userId)
    {
       return await OwnershipValidator.ValidateOwnershipAsync<Department, Guid>(
            _repository.Query(),
            d => d.Id == departmentId,
            DepartmentConstants.DepartmentOwnerIdSelector,
            userId,
            string.Format(DepartmentConstants.DepartmentNotFoundMessage, departmentId),
            string.Format(DepartmentConstants.UnauthorizedDepartmentAccessMessage, userId, departmentId)
        );
    }

    public async Task<SubLocation> ValidateSubLocationOwnershipAsync(Guid subLocationId, Guid userId)
    {
        var subLocationWithDepartment = await OwnershipValidator.ValidateOwnershipAsync<Department, Guid>(
            _repository.Query(),
            sl => sl.SubLocation.Id == subLocationId,
            DepartmentConstants.DepartmentOwnerIdSelector,
            userId,
            string.Format(SubLocationConstants.SubLocationNotFoundMessage, subLocationId),
            string.Format(SubLocationConstants.UnauthorizedAccessMessage, userId, subLocationId)
        );

        return subLocationWithDepartment.SubLocation;
    }
}