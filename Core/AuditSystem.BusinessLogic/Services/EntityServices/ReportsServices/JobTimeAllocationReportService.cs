using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Reports;
using AuditSystem.Domain.Entities.Reports;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ReportsServices;

internal sealed class JobTimeAllocationReportService(
    IRepository<Guid, JobTimeAllocationReport> repository,
    IMapper mapper,
    ICacheService cacheService)
    : IJobTimeAllocationReportService
{
    public Task<Guid> CreateJobTimeAllocationReportAsync(JobTimeAllocationReportModel jobTimeAllocationReportModel)
    {
        throw new NotImplementedException();
    }
}
