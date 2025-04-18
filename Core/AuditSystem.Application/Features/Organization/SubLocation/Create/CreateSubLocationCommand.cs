﻿using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Organization.SubLocation.Create;

public sealed record class CreateSubLocationCommand : ICommand<Result<CreateSubLocationCommandResponse>>
{
    public required string Name { get; set; } = string.Empty;
    public required Guid LocationId { get; set; }
}