using FluentValidation;

namespace AuditSystem.Application.Features.Audit.AuditUniverse.Update;

public sealed class UpdateAuditUniverseValidator : AbstractValidator<UpdateAuditUniverseCommand>
{
    public UpdateAuditUniverseValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Audit Universe Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("Audit Universe Id must be a valid GUID.");

        RuleFor(x => x.BusinessObjective)
            .NotEmpty()
            .WithMessage("Business Objective is required.")
            .MinimumLength(2)
            .WithMessage("Business Objective must be at least 2 characters long.")
            .MaximumLength(200)
            .WithMessage("Business Objective must not exceed 200 characters.");

        RuleFor(x => x.IndustryUpdate)
            .NotEmpty()
            .WithMessage("Industry Update is required.")
            .MinimumLength(2)
            .WithMessage("Industry Update must be at least 2 characters long.")
            .MaximumLength(200)
            .WithMessage("Industry Update must not exceed 200 characters.");

        RuleFor(x => x.CompanyUpdate)
            .NotEmpty()
            .WithMessage("Company Update is required.")
            .MinimumLength(2)
            .WithMessage("Company Update must be at least 2 characters long.")
            .MaximumLength(200)
            .WithMessage("Company Update must not exceed 200 characters.");

        RuleFor(x => x.DomainId)
            .NotEmpty()
            .WithMessage("Domain Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("Domain Id must be a valid GUID.");

        RuleFor(x => x.IsFinancialQuantifiable)
            .NotNull()
            .WithMessage("Is Financial Quantifiable is required.")
            .Must(x => x == true || x == false)
            .WithMessage("Is Financial Quantifiable must be either true or false.");

        RuleFor(x => x.IsSpecialProject)
            .NotNull()
            .WithMessage("Is Special Project is required.")
            .Must(x => x == true || x == false)
            .WithMessage("Is Special Project must be either true or false.");

        RuleFor(x => x.SpecialProjectId)
            .NotEmpty()
            .WithMessage("Special Project Id is required.")
            .Must(x => x != Guid.Empty)
            .WithMessage("Special Project Id must be a valid GUID.");
    }
}