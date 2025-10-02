using AuditSystem.Application.Constants.Organization.DepartmentConst;
using AuditSystem.Application.Constants.Organization.SublocationConst;
using AuditSystem.BusinessLogic.Utilities;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.DepartmentServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.Organization;
using Microsoft.EntityFrameworkCore;

namespace AuditSystem.BusinessLogic.Helpers.OrganizationHelpers.DepartmentHelpers;

public class DepartmentValidationHelper(
    IRepository<Guid, Department> repository,
    IRepository<Guid, SubLocation> subLocationRepository)
    : IDepartmentValidation
{
    private readonly IRepository<Guid, Department> _repository = repository
        ?? throw new ArgumentNullException(nameof(repository));
    private readonly IRepository<Guid, SubLocation> _subLocationRepository = subLocationRepository
        ?? throw new ArgumentNullException(nameof(subLocationRepository));

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
         var subLocation = await _subLocationRepository.GetByIdAsync(subLocationId) 
            ?? throw new ArgumentException(SubLocationConstants.SubLocationNotFoundMessage, nameof(subLocationId));
        
        var ownerId = await _subLocationRepository.Query()
            .AsNoTracking()
            .Where(sl => sl.Id == subLocationId)
            .Select(SubLocationConstants.SubLocationOwnerIdSelector)
            .FirstOrDefaultAsync();

        if (ownerId == Guid.Empty || ownerId != userId)
            throw new UnauthorizedAccessException(
                string.Format(SubLocationConstants.UnauthorizedAccessMessage, userId, subLocationId)
            );
        
        return subLocation;
    }
}