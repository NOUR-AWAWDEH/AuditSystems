using AuditSystem.Application.Features.Tasks.Create;
using AuditSystem.Application.Features.Tasks.Update;
using AuditSystem.Contract.Models.Tasks;
using AuditSystem.Domain.Entities.Tasks;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Mappings;

public sealed class TaskManagementMappingProfile : Profile
{
    public TaskManagementMappingProfile()
    {
        //TaskManagement
        CreateMap<TaskManagementModel, TaskManagement>().ReverseMap();
        CreateMap<CreateTaskManagementCommand, TaskManagementModel>();
        CreateMap<UpdateTaskManagementCommand, TaskManagementModel>();
    }
}
