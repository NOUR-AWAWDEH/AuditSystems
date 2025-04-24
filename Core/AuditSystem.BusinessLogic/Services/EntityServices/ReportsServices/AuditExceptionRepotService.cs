using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Reports;
using AuditSystem.Domain.Entities.Reports;
using AutoMapper;
using Microsoft.Extensions.Logging;
using AuditSystem.BusinessLogic.Services.CacheServices;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ReportsServices;

internal sealed class AuditExceptionRepotService(
    IRepository<Guid, AuditExceptionReport> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<AuditExceptionRepotService> logger) : 
    UnifiedCacheService<Guid, AuditExceptionReport, AuditExceptionReportModel>(
        repository,
        mapper,
        cacheService,
        logger,
        CacheKeys.AuditExceptionReport,
        AuditExceptionReportTags,
        ListTags
    ),
    IAuditExceptionReportService
{
    private static readonly string[] AuditExceptionReportTags = ["audit-exception-reports", "audit-exception-report-list"];
    private static readonly string[] ListTags = ["audit-exception-report-list"];

    public async Task<Guid> CreateAuditExceptionReportAsync(AuditExceptionReportModel auditExceptionReportModel)
    {
        ArgumentNullException.ThrowIfNull(auditExceptionReportModel, nameof(auditExceptionReportModel));

        var entity = mapper.Map<AuditExceptionReport>(auditExceptionReportModel);
        var createdEntity = await repository.CreateAsync(entity);

        var cacheKey = GetCacheKey(createdEntity.Id);
        await SetCacheAsync(cacheKey, createdEntity);

        return createdEntity.Id;
    }
    
    public async Task DeleteAuditExceptionReportAsync(Guid auditExceptionReportId)
    {
        var cacheKey = GetCacheKey(auditExceptionReportId);
        
        var entity = await repository.GetByIdAsync(auditExceptionReportId)
            ?? throw new KeyNotFoundException($"AuditExceptionReport with ID {auditExceptionReportId} not found.");

        await repository.DeleteAsync(auditExceptionReportId);
        await ClearCacheAsync(cacheKey);
    }
    
    public async Task<AuditExceptionReportModel> GetAuditExceptionReportByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        var cachedEntity = await cacheService.GetAsync<AuditExceptionReport>(cacheKey);
        if (cachedEntity != null)
            return mapper.Map<AuditExceptionReportModel>(cachedEntity);
        

        var entity = await repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"AuditExceptionReport with ID '{id}' not found.");

        await SetCacheAsync(cacheKey, entity);
        
        return mapper.Map<AuditExceptionReportModel>(entity);
    }

    public Task<PaginationResult<AuditExceptionReportModel>> SearchAsync(string? name, Page page)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAuditExceptionReportAsync(AuditExceptionReportModel auditExceptionReportModel)
    {
        ArgumentNullException.ThrowIfNull(auditExceptionReportModel, nameof(auditExceptionReportModel));
        
        var entity = mapper.Map<AuditExceptionReport>(auditExceptionReportModel);
        await repository.UpdateAsync(entity);
        
        var cacheKey = GetCacheKey(entity.Id);
        await SetCacheAsync(cacheKey, entity);
    }
}
