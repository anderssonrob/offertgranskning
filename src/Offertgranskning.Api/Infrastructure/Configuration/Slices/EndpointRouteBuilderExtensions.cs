namespace Offertgranskning.Api.Infrastructure.Configuration.Slices;

public static class EndpointRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapSliceEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {
        foreach (var slice in endpointRouteBuilder.ServiceProvider.GetServices<Slice>())
        {
            slice.AddEndpoint(endpointRouteBuilder);
        }

        return endpointRouteBuilder;
    }
}