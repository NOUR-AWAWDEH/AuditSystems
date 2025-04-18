﻿using FluentValidation;

namespace AuditSystem.Application.Features.Organization.Department.Create;

public sealed class CreateDepartmentValidator : AbstractValidator<CreateDepartmentCommand>
{
    public CreateDepartmentValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Department Name is required.")
            .MinimumLength(2)
            .WithMessage("Department Name must be at least 2 characters long.")
            .MaximumLength(200)
            .WithMessage("Department Name must not exceed 200 characters.");

    }
}
