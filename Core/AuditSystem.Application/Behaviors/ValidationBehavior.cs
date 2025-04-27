using FluentValidation;
using MediatR;

namespace AuditSystem.Application.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) :
    IPipelineBehavior<TRequest, ValidationResultResponse<TResponse>>
    where TRequest : IRequest<ValidationResultResponse<TResponse>>
{
    public async Task<ValidationResultResponse<TResponse>> Handle(TRequest request,
        RequestHandlerDelegate<ValidationResultResponse<TResponse>> next,
        CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);
        var validationFailures = await Task.WhenAll(validators.Select(validator => validator.ValidateAsync(context)));
        var errors = validationFailures
            .Where(validationResult => !validationResult.IsValid)
            .SelectMany(validationResult => validationResult.Errors)
            .Select(error => error.ErrorMessage)
            .ToList();

        if (errors.Count != 0)
        {
            return new ValidationResultResponse<TResponse>
            {
                IsValid = false,
                Errors = errors
            };
        }

        var response = await next();

        return new ValidationResultResponse<TResponse>
        {
            IsValid = true,
            Response = response.Response
        };
    }
}