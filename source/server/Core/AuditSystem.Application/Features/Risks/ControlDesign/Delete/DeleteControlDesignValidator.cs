namespace AuditSystem.Application.Features.Risks.ControlDesign.Delete;

using FluentValidation;

public sealed class DeleteControlDesignValidator : AbstractValidator<DeleteControlDesignCommand>
{
    public DeleteControlDesignValidator()
    {
        RuleFor(x => x.Id)
          .NotEmpty()
          .WithMessage("Control Design Id is required")
          .Must(IsValidGuid)
          .WithMessage("Invalid Control Design Id format");
        
        RuleFor(x => x.UserId)
          .NotEmpty()
          .WithMessage("User Id is required")
          .Must(IsValidGuid)
          .WithMessage("Invalid User Id format");
    }
    
    private static bool IsValidGuid(Guid id)
    {
      return id != Guid.Empty;
    }
}
