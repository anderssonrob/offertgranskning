namespace Offertgranskning.API.Infrastructure.Configuration.Endpoints;

public abstract class Endpoint
{
    public abstract void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder);
}