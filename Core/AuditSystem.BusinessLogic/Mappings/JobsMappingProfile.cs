using AuditSystem.Application.Features.Jobs.AuditJob.Create;
using AuditSystem.Application.Features.Jobs.AuditJob.Update;
using AuditSystem.Application.Features.Jobs.JobPrioritization.Create;
using AuditSystem.Application.Features.Jobs.JobPrioritization.Update;
using AuditSystem.Application.Features.Jobs.JobScheduling.Create;
using AuditSystem.Application.Features.Jobs.JobScheduling.Update;
using AuditSystem.Contract.Models.Jobs;
using AuditSystem.Domain.Entities.Jobs;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Mappings;

public sealed class JobsMappingProfile : Profile
{
    public JobsMappingProfile()
    {
        //AuditJobs
        CreateMap<AuditJobModel, AuditJob>().ReverseMap();
        CreateMap<CreateAuditJobCommand, AuditJobModel>();
        CreateMap<UpdateAuditJobCommand, AuditJobModel>();

        //JobPrioritization
        CreateMap<JobPrioritizationModel, JobPrioritization>().ReverseMap();
        CreateMap<CreateJobPrioritizationCommand, JobPrioritizationModel>();
        CreateMap<UpdateJobPrioritizationCommand, JobPrioritizationModel>();

        //JobScheduling
        CreateMap<JobSchedulingModel, JobScheduling>().ReverseMap();
        CreateMap<CreateJobSchedulingCommand, JobSchedulingModel>();
        CreateMap<UpdateJobSchedulingCommand, JobSchedulingModel>();
    }
}
