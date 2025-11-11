namespace AuditSystem.Host.Seed.Interfaces;

public interface ICompanyValidator
{
    Task ValidateAsync(Guid companyId, CancellationToken cancellationToken = default);
}