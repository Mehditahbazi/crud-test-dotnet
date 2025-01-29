using Microsoft.Extensions.DependencyInjection;

namespace Mc2.CrudTest.Application;

public static class ConfigureServices
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ConfigureServices).Assembly);
        });
        return services;
    }
}
