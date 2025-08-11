using FluentValidation;
using Offertgranskning.Api.Infrastructure.Configuration.Slices;
using Offertgranskning.Api.Shared.Domain.Models;

using ValidationException = Offertgranskning.Api.Shared.Exceptions.ValidationException;

namespace Offertgranskning.Api.Features.Orders;

public sealed class PlaceOrder : Slice
{
    public override void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapPost("api/orders/place-order", Handler)
            .WithName("PlaceOrder")
            .WithTags("Orders");
    }

    public static async Task<IResult> Handler(PlaceOrderRequest request, CancellationToken cancellationToken)
    {
        var validator = new PlaceOrderRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid == false)
        {
            // TODO: in globalExceptionHandler.cs, should we really expost traceid?
            throw new ValidationException(validationResult);
        }
        
        var correlationId = Guid.NewGuid();
        
        // TODO: send request with correlation-id to message-broker
        
        var response = new PlaceOrderResponse("The order has been placed.", correlationId);
        
        return await Task.FromResult(Results.Ok(response));
    }

    public sealed record PlaceOrderRequest(Package Package, Customer Customer);
    public sealed record PlaceOrderResponse(string Message, Guid CorrelationId);

    public sealed class PlaceOrderRequestValidator : AbstractValidator<PlaceOrderRequest>
    {
        // TODO - add validation, depending on package, for IFormCollection
        
        public PlaceOrderRequestValidator()
        {
            RuleFor(x => x.Package)
                .IsInEnum()
                .WithMessage("Package must be Basic or Compare");

            RuleFor(x => x.Customer)
                .SetValidator(new CustomerValidator());
            
            

        }
    }
}

