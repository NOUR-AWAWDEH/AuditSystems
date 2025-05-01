using AuditSystem.Application.Features.SupportingDocs.Create;
using AuditSystem.Application.Features.SupportingDocs.Update;
using AuditSystem.Contract.Models.SupportingDocs;
using AuditSystem.Domain.Entities.SupportingDocs;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Mappings;

public sealed class SupportingDocsMappingProfile : Profile
{
    public SupportingDocsMappingProfile()
    {
        //SupportingDoc
        CreateMap<SupportingDocModel, SupportingDoc>().ReverseMap();
        CreateMap<CreateSupportingDocCommand, SupportingDocModel>();
        CreateMap<UpdateSupportingDocCommand, SupportingDocModel>();
    }
}