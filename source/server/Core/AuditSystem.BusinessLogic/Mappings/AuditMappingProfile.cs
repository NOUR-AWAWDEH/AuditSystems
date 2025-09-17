using AuditSystem.Application.Features.Audit.AuditProgram.Create;
using AuditSystem.Application.Features.Audit.AuditProgram.Update;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Domain.Entities.Audit;
using AutoMapper;


namespace AuditSystem.BusinessLogic.Mappings;

public sealed class AuditMappingProfile : Profile
{
    public AuditMappingProfile()
    {
        // Audit Program
        CreateMap<AuditProgramModel, AuditProgram>().ReverseMap();
        CreateMap<CreateAuditProgramCommand, AuditProgramModel>();
        CreateMap<UpdateAuditProgramCommand, AuditProgramModel>();
    }
}
