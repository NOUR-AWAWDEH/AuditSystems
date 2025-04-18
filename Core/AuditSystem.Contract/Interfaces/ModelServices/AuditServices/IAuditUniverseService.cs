﻿using AuditSystem.Contract.Models.Audit;

namespace AuditSystem.Contract.Interfaces.ModelServices.AuditServices;

public interface IAuditUniverseService
{
    public Task<Guid> CreateAuditUniverseAsync(AuditUniverseModel auditUniverseModel);
    public Task UpdateAuditUniverseAsync(AuditUniverseModel auditUniverseModel);
    public Task DeleteAuditUniverseAsync(Guid auditUniverseId);
    public Task<AuditUniverseModel> GetAuditUniverseByIdAsync(Guid id);
}
