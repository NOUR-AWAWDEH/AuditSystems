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
        CreateMap<AuditProgram, AuditProgramModel>()
            .ForMember(dest => dest.ProcessOwner,
                opt => opt.MapFrom(src => src.ProcessOwner));

        CreateMap<AuditProgramModel, AuditProgram>()
            .ForMember(dest => dest.ProcessOwner, opt => opt.Ignore());

        CreateMap<CreateAuditProgramCommand, AuditProgramModel>();
        CreateMap<UpdateAuditProgramCommand, AuditProgramModel>();
    }
}