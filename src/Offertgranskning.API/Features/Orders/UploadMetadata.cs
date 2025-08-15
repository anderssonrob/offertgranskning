using FluentValidation;
using Offertgranskning.API.Shared.Domain.Models;

using Endpoint = Offertgranskning.API.Infrastructure.Configuration.Endpoints.Endpoint;
using ValidationException = Offertgranskning.API.Shared.Exceptions.ValidationException;

namespace Offertgranskning.API.Features.Orders;

public sealed class UploadMetadata : Endpoint
{
    public override void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost("api/orders/upload-metadata", Handler)
            .WithName("UploadMetadata")
            .WithTags("Orders");
    }

    public static async Task<IResult> Handler(UploadMetadataRequest request, CancellationToken cancellationToken)
    {
        var validator = new UploadMetadataRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid == false)
        {
            // TODO: in globalExceptionHandler.cs, should we really expost traceid?
            throw new ValidationException(validationResult);
        }
        
        var correlationId = Guid.NewGuid();
        
        // TODO: send request with correlation-id to message-broker
        
        var response = new UploadMetadataResponse("The metadata has been uploaded.", correlationId);
        
        return await Task.FromResult(Results.Ok(response));
    }

    public sealed record UploadMetadataRequest(Package Package, Customer Customer);
    public sealed record UploadMetadataResponse(string Message, Guid CorrelationId);

    public sealed class UploadMetadataRequestValidator : AbstractValidator<UploadMetadataRequest>
    {
        public UploadMetadataRequestValidator()
        {
            RuleFor(x => x.Package)
                .IsInEnum()
                .WithMessage("Package must be Basic or Compare");

            RuleFor(x => x.Customer)
                .NotNull()
                .WithMessage("Customer is required")
                .SetValidator(new CustomerValidator());
        }
    }
}

