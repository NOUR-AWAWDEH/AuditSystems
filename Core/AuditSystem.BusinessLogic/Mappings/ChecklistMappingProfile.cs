using AuditSystem.Application.Features.Checklists.Checklist.Create;
using AuditSystem.Application.Features.Checklists.Checklist.Update;
using AuditSystem.Application.Features.Checklists.Remark.Create;
using AuditSystem.Application.Features.Checklists.Remark.Update;
using AuditSystem.Contract.Models.Checklists;
using AuditSystem.Domain.Entities.Checklists;

using AutoMapper;

namespace AuditSystem.BusinessLogic.Mappings
{
    public sealed class ChecklistMappingProfile : Profile
    {
        public ChecklistMappingProfile()
        {
            //Checklist
            CreateMap<ChecklistModel, Checklist>().ReverseMap();
            CreateMap<CreateChecklistCommand, ChecklistModel>();
            CreateMap<UpdateChecklistCommand, ChecklistModel>();

            //Remark
            CreateMap<RemarkModel, Remark>().ReverseMap();
            CreateMap<CreateRemarkCommand, RemarkModel>();
            CreateMap<UpdateRemarkCommand, RemarkModel>();
        }
    }
}