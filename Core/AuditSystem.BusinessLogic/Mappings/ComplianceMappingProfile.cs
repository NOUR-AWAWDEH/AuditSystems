using AuditSystem.Application.Features.Compliance.ComplianceChecklist.Create;
using AuditSystem.Application.Features.Compliance.ComplianceChecklist.Update;
using AuditSystem.Contract.Models.Compliance;
using AuditSystem.Domain.Entities.Compliance;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Mappings;

public sealed class ComplianceMappingProfile : Profile
{
    public ComplianceMappingProfile()
    {
        //Compliance
        CreateMap<ComplianceChecklistModel, ComplianceChecklist>().ReverseMap();
        CreateMap<CreateComplianceChecklistCommand, ComplianceChecklistModel>();
        CreateMap<UpdateComplianceChecklistCommand, ComplianceChecklistModel>().ReverseMap();
    }
}
