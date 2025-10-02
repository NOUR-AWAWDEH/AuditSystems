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
using Microsoft.EntityFrameworkCore;

namespace AuditSystem.BusinessLogic.Services.EntityServices.CommonServices;

internal sealed class RateService(
    IRepository<Guid, Rate> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<RateService> logger
    ) : UnifiedCacheService<Guid, Rate, RateModel>(repository, mapper, cacheService, logger, CacheKeys.Rate, RatingTags, ListTags), IRateService
{
    private static readonly string[] RatingTags = ["ratings", "rating-list"];
    private static readonly string[] ListTags = ["rating-list"];

    private readonly IRepository<Guid, Rate> _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<RateService> _logger = logger;

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

        // Check for duplicates using EF Core directly
        var exists = await _repository.GetAll()
            .AnyAsync(r => r.Level == rateModel.Level);
        if (exists)
        {
            _logger.LogWarning("Rate with level {Level} already exists.", rateModel.Level);
            throw new InvalidOperationException($"Rate with level '{rateModel.Level}' already exists.");
        }

        var entity = _mapper.Map<Rate>(rateModel);
        var createdEntity = await _repository.CreateAsync(entity);

        await CacheEntityAsync(entity);
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

        // Check for duplicate level in other rows using EF Core directly
        var exists = await _repository.GetAll()
            .AnyAsync(r => r.Level == rateModel.Level && r.Id != rateModel.Id);
        if (exists)
        {
            _logger.LogWarning("Rate level {Level} already exists.", rateModel.Level);
            throw new InvalidOperationException($"Rate with level '{rateModel.Level}' already exists.");
        }

        var existingEntity = await _repository.GetByIdAsync(rateModel.Id);
        if (existingEntity is null)
        {
            _logger.LogWarning("Rate with ID {Id} not found for update", rateModel.Id);
            throw new KeyNotFoundException($"Rate with ID {rateModel.Id} not found.");
        }

        _mapper.Map(rateModel, existingEntity);
        existingEntity.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(existingEntity);
        await CacheEntityAsync(existingEntity);

        _logger.LogInformation("Rate with ID {Id} updated successfully", rateModel.Id);
    }

    public async Task DeleteRateAsync(Guid rateId)
    {
        _logger.LogInformation("Deleting rate with ID {Id}", rateId);

        var entity = await _repository.GetByIdAsync(rateId);
        if (entity is null)
        {
            _logger.LogWarning("Rate with ID {Id} not found for deletion", rateId);
            throw new KeyNotFoundException($"Rate with ID {rateId} not found.");
        }

        await _repository.DeleteAsync(rateId);
        await ClearEntityCacheAsync(rateId);
        await InvalidateEntityTagsCacheAsync();

        _logger.LogInformation("Rate with ID {Id} deleted successfully", rateId);
    }

    public async Task<PaginationResult<RateModel>> SearchAllAsync(Page page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        _logger.LogInformation("Retrieving all rate levels - Page {PageNumber}, Size {PageSize}", page.Number, page.Size);

        var (cacheKey, countCacheKey) = GetSearchCacheKeys(null, page);

        // Try getting from cache first using the base class method
        var cachedResult = await GetFromCacheAsync<RateModel>(cacheKey, countCacheKey, page);
        if (cachedResult is not null)
        {
            _logger.LogInformation("Returning {Count} cached rate levels", cachedResult.Items.Count);
            return cachedResult;
        }

        // Fetch from repository using EF Core directly
        var query = _repository.GetAll()
            .AsNoTracking()
            .OrderBy(r => r.Level);

        // Use the base class pagination method
        var result = await GetFromRepositoryAsync<RateModel>(cacheKey, countCacheKey, page);

        _logger.LogInformation("Found {Count} rate levels from repository", result.Items.Count);
        return result;
    }

    public async Task<RateModel> GetRateByIdAsync(Guid id)
    {
        _logger.LogInformation("Fetching rate by ID {Id}", id);

        var cachedResult = await TryGetFromCacheWithValidationAsync<RateModel>(
            id,
            Guid.Empty,
            entity => Guid.Empty,
            "Unauthorized");

        if (cachedResult is not null)
        {
            _logger.LogInformation("Returning cached rate with ID {Id}", id);
            return cachedResult;
        }

        // Fetch from repository using EF Core directly
        var entity = await _repository.GetAll()
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id);

        if (entity is null)
        {
            _logger.LogWarning("Rate with ID {Id} not found", id);
            throw new KeyNotFoundException($"Rate with ID '{id}' not found.");
        }

        await CacheEntityAsync(entity);
        _logger.LogInformation("Rate with ID {Id} retrieved from repository", id);

        return _mapper.Map<RateModel>(entity);
    }

    // Additional helper method to get all rates without pagination (if needed)
    public async Task<IReadOnlyList<RateModel>> GetAllRatesAsync()
    {
        _logger.LogInformation("Retrieving all rate levels");

        return await _repository.GetAll()
            .AsNoTracking()
            .OrderBy(r => r.Level)
            .Select(r => _mapper.Map<RateModel>(r))
            .ToListAsync();
    }
}