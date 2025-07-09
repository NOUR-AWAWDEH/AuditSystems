using AuditSystem.Application.Constants.Organization.DepartmentConst;
using AuditSystem.Application.Constants.Organization.SublocationConst;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.Organization;

namespace AuditSystem.BusinessLogic.Helpers.OrganizationHelpers.DepartmentHelpers;

public class DepartmentValidationHelper
{
    private readonly IRepository<Guid, Department> _repository;

    public DepartmentValidationHelper(IRepository<Guid, Department> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public static void ValidateUserId(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException(DepartmentConstants.UserIdEmptyMessage, nameof(userId));
    }
    public async Task<Department> ValidateDepartmentOwnerShipAsync(Guid departmentId, Guid userId)
    {
        var department = await _repository.GetByIdAsync(departmentId, DepartmentConstants.IncludeSubLocation)
            ?? throw new KeyNotFoundException(string.Format(DepartmentConstants.DepartmentNotFoundMessage, departmentId));

        if (department.SubLocation.Location.UserId != userId)
        {
            throw new UnauthorizedAccessException(
                string.Format(DepartmentConstants.UnauthorizedDepartmentAccessMessage, userId, departmentId));
        }

        return department;
    }
    
    public async Task<SubLocation> ValidateSubLocationOwnerShipAsync(Guid subLocationId, Guid userId)
    {
        var subLocation = await _repository.GetByIdAsync<SubLocation>(subLocationId)
            ?? throw new KeyNotFoundException(string.Format(DepartmentConstants.SubLocationNotFoundMessage, subLocationId));

        if (subLocation.Location.UserId != userId)
        {
            throw new UnauthorizedAccessException(
                string.Format(DepartmentConstants.UnauthorizedSubLocationAccessMessage, userId, subLocationId));
        }

        return subLocation;
    }
}

