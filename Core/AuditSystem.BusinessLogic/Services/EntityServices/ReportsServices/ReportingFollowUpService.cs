using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Reports;
using AuditSystem.Domain.Entities.Reports;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ReportsServices;

internal sealed class ReportingFollowUpService(
    IRepository<Guid, ReportingFollowUp> repository,
    IMapper mapper,
    ICacheService cacheService) 
    : IReportingFollowUpService
{
    public Task<Guid> CreateReportingFollowUpAsync(ReportingFollowUpModel reportingFollowUpModel)
    {
        throw new NotImplementedException();
    }
}