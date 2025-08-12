using FluentValidation.Results;

namespace Offertgranskning.API.Shared.Exceptions;

public sealed class ValidationException : Exception
{
    public IReadOnlyDictionary<string, string[]> Errors { get; }
    
    public ValidationException(ValidationResult validationResult) 
        : base("One or more validation errors occurred.")
    {
        Errors = validationResult.Errors
            .GroupBy(i => i.PropertyName)
            .ToDictionary(
                i => i.Key,
                i => i.Select(e => e.ErrorMessage).ToArray()
            );
    }
}