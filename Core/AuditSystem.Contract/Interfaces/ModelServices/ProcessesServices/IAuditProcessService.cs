﻿using AuditSystem.Contract.Models.Processes;

namespace AuditSystem.Contract.Interfaces.ModelServices.ProcessesServices;

public interface IAuditProcessService
{
    public Task<Guid> CreateAuditProcessAsync(AuditProcessModel auditProcessModel);
    public Task UpdateAuditProcessAsync(AuditProcessModel auditProcessModel);
    public Task DeleteAuditProcessAsync(Guid auditProcessId);
    public Task<AuditProcessModel> GetAuditProcessByIdAsync(Guid id);
}