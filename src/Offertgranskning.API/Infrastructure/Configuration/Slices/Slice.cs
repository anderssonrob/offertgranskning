namespace Offertgranskning.API.Infrastructure.Configuration.Slices;

public abstract class Slice // TODO - rename to Endpoint
{
    public abstract void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder);
}