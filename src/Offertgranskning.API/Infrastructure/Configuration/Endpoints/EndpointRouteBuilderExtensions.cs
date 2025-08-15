namespace Offertgranskning.API.Infrastructure.Configuration.Endpoints;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        foreach (var slice in endpointRouteBuilder.ServiceProvider.GetServices<Endpoint>())
        {
            slice.AddEndpoint(endpointRouteBuilder);
        }

        return endpointRouteBuilder;
    }
}