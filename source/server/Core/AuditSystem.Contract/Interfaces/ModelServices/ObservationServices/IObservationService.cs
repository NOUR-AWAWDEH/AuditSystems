using AuditSystem.Contract.Models.Observation;
using AuditSystem.Contract.Models.Pagination;

namespace AuditSystem.Contract.Interfaces.ModelServices.ObservationServices;

public interface IObservationService
{
    Task<Guid> CreateObservationAsync(ObservationModel observationModel, Guid userId);
    Task UpdateObservationAsync(ObservationModel observationModel, Guid userId);
    Task DeleteObservationAsync(Guid observationId, Guid userId);
    Task<ObservationModel> GetObservationByIdAsync(Guid id, Guid userId);
    Task<PaginationResult<ObservationModel>> SearchByUserIdAsync(Guid userId, string? searchTerm, Guid auditProgramId, Page? page);
    Task<IReadOnlyList<ObservationModel>> GetObservationsByUserIdAsync(Guid userId);

    Task<PaginationResult<ObservationModel>> SearchAsync(string? searchTerm, Page page);
}