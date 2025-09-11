using AuditSystem.Application.Constants.Organization.DepartmentConst;
using AuditSystem.Application.Constants.Organization.SublocationConst;
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
        var department = await _repository.GetByIdAsync(departmentId, DepartmentConstants.IncludeHierarchy) 
            ?? throw new KeyNotFoundException(string.Format(DepartmentConstants.DepartmentNotFoundMessage, departmentId ));

        if (department.SubLocation?.Location?.UserId != userId)
            throw new UnauthorizedAccessException( string.Format(DepartmentConstants.UnauthorizedDepartmentAccessMessage, userId, departmentId));

        return department;
    }

    public async Task<SubLocation> ValidateSubLocationOwnershipAsync(Guid subLocationId, Guid userId)
    {
        var subLocation = await _repository.GetByIdAsync(subLocationId, SubLocationConstants.IncludeLocation)
                ?? throw new KeyNotFoundException(string.Format(
                    DepartmentConstants.SubLocationNotFoundMessage,
                    subLocationId
                ));

        // Add null check for safety
        if (subLocation.Location?.UserId != userId) 
            throw new UnauthorizedAccessException( string.Format(
                DepartmentConstants.UnauthorizedSubLocationAccessMessage,
                userId, subLocationId
            ));

        return subLocation;
    }
}