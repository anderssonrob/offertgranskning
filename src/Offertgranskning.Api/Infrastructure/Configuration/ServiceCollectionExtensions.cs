using Offertgranskning.Api.Infrastructure.Configuration.Slices;
using Offertgranskning.Api.Infrastructure.Middleware;

namespace Offertgranskning.Api.Infrastructure.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        services.AddOpenApi();
        
        services.RegisterSlices();
        


        
        return services;
    }
    
    public static IServiceCollection RegisterDomainServices(this IServiceCollection services)
    {
        return services;
    }

    public static IServiceCollection RegisterPersistenceServices(this IServiceCollection services)
    {
        return services;
    }}