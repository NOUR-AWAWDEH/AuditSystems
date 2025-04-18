﻿using AuditSystem.Contract.Models.Audit;

namespace AuditSystem.Contract.Interfaces.ModelServices.AuditServices;

public interface IAuditPlanSummaryService
{
    public Task<Guid> CreateAuditPlanSummaryAsync(AuditPlanSummaryModel auditPlanSummaryModel);
    public Task UpdateAuditPlanSummaryAsync(AuditPlanSummaryModel auditPlanSummaryModel);
    public Task DeleteAuditPlanSummaryAsync(Guid auditPlanSummaryId);
    public Task<AuditPlanSummaryModel> GetAuditPlanSummaryByIdAsync(Guid id);
}
