using FluentValidation;

namespace AuditSystem.Application.Features.Audit.AuditUniverse.Create
{
    public sealed class CreateAuditUniverseValidator : AbstractValidator<CreateAuditUniverseCommand>
    {
        public CreateAuditUniverseValidator()
        {
            RuleFor(x => x.BusinessObjective)
                .NotEmpty()
                .WithMessage("Business objective is required")
                .MaximumLength(500)
                .WithMessage("Business objective cannot exceed 500 characters");

            RuleFor(x => x.IndustryUpdate)
                .MaximumLength(1000)
                .WithMessage("Industry update cannot exceed 1000 characters");

            RuleFor(x => x.CompanyUpdate)
                .MaximumLength(1000)
                .WithMessage("Company update cannot exceed 1000 characters");

            RuleFor(x => x.DomainId)
                .NotEmpty()
                .WithMessage("Domain ID is required");

            RuleFor(x => x.IsFinancialQuantifiable)
                .NotNull()
                .WithMessage("Financial quantifiability must be specified");

            RuleFor(x => x.IsSpecialProject)
                .NotNull()
                .WithMessage("Special project status must be specified");

            RuleFor(x => x.SpecialProjectId)
                .NotEmpty()
                .When(x => x.IsSpecialProject)
                .WithMessage("Special project ID is required when the project is marked as special");
        }
    }
}