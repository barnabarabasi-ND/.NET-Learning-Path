using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderProcessingDiDemo.Application.Abstractions.Persistence;
using OrderProcessingDiDemo.Infrastructure.Persistence;

namespace OrderProcessingDiDemo.Infrastructure.DependencyInjection;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbContext<OrderDbContext>(options =>
            options.UseSqlite(
                configuration.GetConnectionString("Orders")
            )
        );

        services.AddScoped<IOrderRepository, OrderRepository>();

        return services;
    }
}
