using System.Reflection;

namespace Offertgranskning.API.Infrastructure.Configuration.Endpoints;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterEndpoints(this IServiceCollection services)
    {
        var currentAssembly = Assembly.GetExecutingAssembly();

        var slices = currentAssembly.GetTypes().Where(t =>
            typeof(Endpoint).IsAssignableFrom(t) && 
            t != typeof(Endpoint) &&
            t is { IsPublic: true, IsAbstract: false });

        foreach (var slice in slices)
        {
            services.AddSingleton(typeof(Endpoint), slice);
        }
        
        return services; 
    }
}