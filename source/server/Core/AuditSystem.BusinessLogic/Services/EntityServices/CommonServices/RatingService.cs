using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.CommonServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Common;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Common;
using AutoMapper;
using Microsoft.Extensions.Logging;
using AuditSystem.BusinessLogic.Services.CacheServices;
using System;

namespace AuditSystem.BusinessLogic.Services.EntityServices.CommonServices;

internal sealed class RateService : UnifiedCacheService<Guid, Rate, RateModel>, IRateService
{
    private static readonly string[] RatingTags = ["ratings", "rating-list"];
    private static readonly string[] ListTags = ["rating-list"];

    private readonly IRepository<Guid, Rate> _repository;
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;
    private readonly ILogger<RateService> _logger;

    public RateService(
        IRepository<Guid, Rate> repository,
        IMapper mapper,
        ICacheService cacheService,
        ILogger<RateService> logger
    ) : base(repository, mapper, cacheService, logger, CacheKeys.Rate, RatingTags, ListTags)
    {
        _repository = repository;
        _mapper = mapper;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<Guid> CreateRateAsync(RateModel rateModel)
    {
        ArgumentNullException.ThrowIfNull(rateModel, nameof(rateModel));
        _logger.LogInformation("Creating new rate with level {Level}", rateModel.Level);

        // Validate enum
        if (!Enum.IsDefined<RateLevel>(rateModel.Level))
        {
            _logger.LogWarning("Invalid rate level {Level}", rateModel.Level);
            throw new InvalidOperationException($"Invalid rate level: {rateModel.Level}");
        }

        // Check for duplicates
        var exists = await _repository.CountAsync(r => r.Level == rateModel.Level);
        if (exists > 0)
        {
            _logger.LogWarning("Rate with level {Level} already exists.", rateModel.Level);
            throw new InvalidOperationException($"Rate with level '{rateModel.Level}' already exists.");
        }

        var entity = _mapper.Map<Rate>(rateModel);
        var createdEntity = await _repository.CreateAsync(entity);

        var cacheKey = GetCacheKey(createdEntity.Id);
        await SetCacheAsync(cacheKey, entity);

        _logger.LogInformation("Rate created successfully with ID {Id}", createdEntity.Id);
        return createdEntity.Id;
    }
    
    public async Task UpdateRateAsync(RateModel rateModel)
    {
        ArgumentNullException.ThrowIfNull(rateModel, nameof(rateModel));
        _logger.LogInformation("Updating rate with ID {Id}", rateModel.Id);

        // Validate enum
        if (!Enum.IsDefined<RateLevel>(rateModel.Level))
        {
            _logger.LogWarning("Invalid rate level {Level}", rateModel.Level);
            throw new InvalidOperationException($"Invalid rate level: {rateModel.Level}");
        }

        // Check for duplicate level in other rows
        var exists = await _repository.CountAsync(r => r.Level == rateModel.Level && r.Id != rateModel.Id);
        if (exists > 0)
        {
            _logger.LogWarning("Rate level {Level} already exists.", rateModel.Level);
            throw new InvalidOperationException($"Rate with level '{rateModel.Level}' already exists.");
        }

        var entity = _mapper.Map<Rate>(rateModel);
        await _repository.UpdateAsync(entity);

        var cacheKey = GetCacheKey(entity.Id);
        await SetCacheAsync(cacheKey, entity);

        _logger.LogInformation("Rate with ID {Id} updated successfully", entity.Id);
    }

    public async Task DeleteRateAsync(Guid rateId)
    {
        _logger.LogInformation("Deleting rate with ID {Id}", rateId);

        var cacheKey = GetCacheKey(rateId);
        var entity = await _repository.GetByIdAsync(rateId);

        if (entity is null)
        {
            _logger.LogWarning("Rate with ID {Id} not found for deletion", rateId);
            throw new KeyNotFoundException($"Rate with ID {rateId} not found.");
        }

        await _repository.DeleteAsync(rateId);
        await ClearCacheAsync(cacheKey);

        _logger.LogInformation("Rate with ID {Id} deleted successfully", rateId);
    }

    public async Task<PaginationResult<RateModel>> SearchAllAsync(Page page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        _logger.LogInformation("Retrieving all rate levels - Page {PageNumber}, Size {PageSize}", page.Number, page.Size);

        var (cacheKey, countCacheKey) = GetSearchCacheKeys(null, page);

        // Try getting from cache first
        var cachedResult = await GetFromCacheAsync<RateModel>(cacheKey, countCacheKey, page);
        if (cachedResult is not null)
        {
            _logger.LogInformation("Returning {Count} cached rate levels", cachedResult.Items.Count);
            return cachedResult;
        }

        // Fetch all records from repository
        var result = await GetAllFromRepositoryAsync<RateModel>(cacheKey, countCacheKey, page);

        _logger.LogInformation("Found {Count} rate levels from repository", result.Items.Count);

        return result;
    }


    public async Task<RateModel> GetRateByIdAsync(Guid id)
    {
        _logger.LogInformation("Fetching rate by ID {Id}", id);

        var cacheKey = GetCacheKey(id);
        var cachedEntity = await _cacheService.GetAsync<Rate>(cacheKey);

        if (cachedEntity is not null)
        {
            _logger.LogInformation("Returning cached rate with ID {Id}", id);
            return _mapper.Map<RateModel>(cachedEntity);
        }

        var entity = await _repository.GetByIdAsync(id);
        if (entity is null)
        {
            _logger.LogWarning("Rate with ID {Id} not found", id);
            throw new KeyNotFoundException($"Rate with ID '{id}' not found.");
        }

        await SetCacheAsync(cacheKey, entity);
        _logger.LogInformation("Rate with ID {Id} retrieved from repository", id);

        return _mapper.Map<RateModel>(entity);
    }
}