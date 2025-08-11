using System.Reflection;

namespace Offertgranskning.Api.Infrastructure.Configuration.Slices;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterSlices(this IServiceCollection services)
    {
        var currentAssembly = Assembly.GetExecutingAssembly();

        var slices = currentAssembly.GetTypes().Where(t =>
            typeof(Slice).IsAssignableFrom(t) && 
            t != typeof(Slice) &&
            t is { IsPublic: true, IsAbstract: false });

        foreach (var slice in slices)
        {
            services.AddSingleton(typeof(Slice), slice);
        }
        
        return services; 
    }
}