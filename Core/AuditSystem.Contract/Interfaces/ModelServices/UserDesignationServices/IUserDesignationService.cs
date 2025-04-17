using AuditSystem.Contract.Models.UserDesignation;

namespace AuditSystem.Contract.Interfaces.ModelServices.UserDesignationServices;

public interface IUserDesignationService
{
    public Task<Guid> CreateUserDesignationAsync(UserDesignationModel userDesignationModel);
    public Task UpdateUserDesignationAsync(UserDesignationModel userDesignationModel);
    public Task DeleteUserDesignationAsync(Guid userDesignationId);
    public Task<UserDesignationModel> GetUserDesignationByIdAsync(Guid Id);
}