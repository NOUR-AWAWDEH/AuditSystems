using AuditSystem.Application.Features.Audit.AuditProgram.Create;
using AuditSystem.Application.Features.Audit.AuditProgram.Update;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Contract.Models.Users;
using AuditSystem.Domain.Entities.Audit;
using AuditSystem.Domain.Entities.Users;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Domain.Entities.Organization;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Mappings;

public sealed class AuditMappingProfile : Profile
{
    public AuditMappingProfile()
    {
        CreateMap<AuditProgram, AuditProgramModel>()
            .ForMember(dest => dest.ActionOwner,
                opt => opt.MapFrom(src => src.ActionOwner));

        CreateMap<AuditProgramModel, AuditProgram>()
            .ForMember(dest => dest.ActionOwner, opt => opt.Ignore());

        CreateMap<CreateAuditProgramCommand, AuditProgramModel>();
        CreateMap<UpdateAuditProgramCommand, AuditProgramModel>();

        CreateMap<AppUser, UserModel>()
            .ForCtorParam("Role", opt => opt.MapFrom(src => "ActionOwner"));
        CreateMap<JobTitle, JobTitleModel>();
    }
}
