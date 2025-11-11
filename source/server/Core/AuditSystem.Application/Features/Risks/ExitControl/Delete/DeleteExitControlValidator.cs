namespace AuditSystem.Application.Features.Risks.ExitControl.Delete;

using FluentValidation;

public sealed class DeleteExitControlValidator : AbstractValidator<DeleteExitControlCommand>
{
    public DeleteExitControlValidator()
    {
        RuleFor(x => x.Id)
          .NotEmpty()
          .WithMessage("Exit Control Id is required")
          .Must(IsValidGuid)
          .WithMessage("Invalid Exit Control Id format");
        
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
