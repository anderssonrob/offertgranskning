namespace Offertgranskning.Api.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddOpenApi();
        
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