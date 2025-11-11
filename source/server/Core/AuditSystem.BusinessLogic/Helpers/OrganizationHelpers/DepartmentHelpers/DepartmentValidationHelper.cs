using AuditSystem.Application.Constants.Organization.DepartmentConst;
using AuditSystem.Application.Constants.Organization.LocationConst;
using AuditSystem.BusinessLogic.Utilities;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.DepartmentServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.Organization;
using Microsoft.EntityFrameworkCore;

namespace AuditSystem.BusinessLogic.Helpers.OrganizationHelpers.DepartmentHelpers;

public class DepartmentValidationHelper(
    IRepository<Guid, Department> repository,
    IRepository<Guid, Location> locationRepository)
    : IDepartmentValidation
{
    private readonly IRepository<Guid, Department> _repository = repository
        ?? throw new ArgumentNullException(nameof(repository));
    private readonly IRepository<Guid, Location> _locationRepository = locationRepository
        ?? throw new ArgumentNullException(nameof(locationRepository));

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

    public async Task<Location> ValidateLocationOwnershipAsync(Guid locationId, Guid userId)
    {
         var location = await _locationRepository.GetByIdAsync(locationId) 
            ?? throw new ArgumentException(LocationConstants.LocationNotFoundMessage, nameof(locationId));
        
        var ownerId = await _locationRepository.Query()
            .AsNoTracking()
            .Where(l => l.Id == locationId)
            .Select(LocationConstants.LocationOwnerIdSelector)
            .FirstOrDefaultAsync();

        if (ownerId == Guid.Empty || ownerId != userId)
            throw new UnauthorizedAccessException(
                string.Format(LocationConstants.UnauthorizedAccessMessage, userId, locationId)
            );
        
        return location;
    }
}