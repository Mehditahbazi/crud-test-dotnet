using Mc2.CrudTest.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mc2.CrudTest.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection RegisterPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        var z = configuration.GetConnectionString("Local");
        services.AddDbContext<CustomerDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("Local")));
        return services;
    }
}
