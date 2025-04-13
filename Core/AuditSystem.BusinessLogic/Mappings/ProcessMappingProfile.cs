using AuditSystem.Application.Features.Processes.AuditProcess.Create;
using AuditSystem.Application.Features.Processes.AuditProcess.Update;
using AuditSystem.Application.Features.Processes.SubProcess.Create;
using AuditSystem.Application.Features.Processes.SubProcess.Update;
using AuditSystem.Contract.Models.Processes;
using AuditSystem.Domain.Entities.Processes;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Mappings;

public sealed class ProcessMappingProfile : Profile
{
    public ProcessMappingProfile()
    {
        //AuditProcess
        CreateMap<AuditProcessModel, AuditProcess>().ReverseMap();
        CreateMap<CreateAuditProcessCommand, AuditProcessModel>();
        CreateMap<UpdateAuditProcessCommand, AuditProcessModel>();

        //SubProcess
        CreateMap<SubProcessModel, SubProcess>().ReverseMap();
        CreateMap<CreateSubProcessCommand, SubProcessModel>();
        CreateMap<UpdateSubProcessCommand, SubProcessModel>();
    }
}
