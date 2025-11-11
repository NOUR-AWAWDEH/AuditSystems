using AuditSystem.DataAccess.Contexts;
using AuditSystem.Host.Seed.Exceptions;
using AuditSystem.Host.Seed.Interfaces;

namespace AuditSystem.Host.Seed.Services;
public sealed class CompanyValidator : ICompanyValidator
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<CompanyValidator> _logger;

    public CompanyValidator(ApplicationDbContext dbContext, ILogger<CompanyValidator> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task ValidateAsync(Guid companyId, CancellationToken cancellationToken = default)
    {
        var company = await _dbContext.Companies.FindAsync(new[] { companyId }, cancellationToken);
        
        if (company is null)
        {
            _logger.LogError("Company {CompanyId} not found in database", companyId);
            throw new SeedingException($"Company with ID {companyId} not found. Ensure migrations are applied.");
        }

        _logger.LogInformation("Company validation passed: {CompanyName}", company.Name);
    }
}