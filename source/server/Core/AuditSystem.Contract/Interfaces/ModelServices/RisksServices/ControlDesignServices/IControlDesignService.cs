using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Risks;

namespace AuditSystem.Contract.Interfaces.ModelServices.RisksServices.ControlDesignServices;

public interface IControlDesignService
{
    Task<Guid> CreateControlDesignAsync(ControlDesignModel controlDesignModel, Guid userId);
    Task UpdateControlDesignAsync(ControlDesignModel controlDesignModel, Guid userId);
    Task DeleteControlDesignAsync(Guid id, Guid userId);
    Task<ControlDesignModel> GetControlDesignByIdAsync(Guid id, Guid userId);
    Task<IReadOnlyList<ControlDesignModel>> GetControlDesignsByUserIdAsync(Guid userId);
    Task<PaginationResult<ControlDesignModel>> SearchByUserIdAsync( Guid userId, string? searchTerm, Guid exitControlId, Page? page);
    Task<PaginationResult<ControlDesignModel>> SearchAsync(string? searchTerm, Page? page);
}
