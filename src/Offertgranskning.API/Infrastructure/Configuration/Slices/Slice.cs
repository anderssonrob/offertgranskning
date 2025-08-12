namespace Offertgranskning.API.Infrastructure.Configuration.Slices;

public abstract class Slice
{
    public abstract void AddEndpoint(IEndpointRouteBuilder endpointRouteBuilder);
}