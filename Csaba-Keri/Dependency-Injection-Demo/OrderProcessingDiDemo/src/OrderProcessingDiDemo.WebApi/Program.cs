using OrderProcessingDiDemo.Application.Abstractions.Time;
using OrderProcessingDiDemo.Application.Orders;
using OrderProcessingDiDemo.Application.Orders.Validation;
using OrderProcessingDiDemo.Infrastructure.DependencyInjection;
using OrderProcessingDiDemo.Infrastructure.Persistence;
using OrderProcessingDiDemo.Infrastructure.Time;

namespace OrderProcessingDiDemo.WebApi;

internal static class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddInfrastructure(builder.Configuration);

        builder.Services.AddSingleton<IClock, Clock>();

        builder.Services.AddTransient<IOrderValidator, OrderValidator>();

        builder.Services.AddScoped<IOrderService, OrderService>();

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();

        using (var scope = app.Services.CreateScope())
        {
            scope.ServiceProvider
                .GetRequiredService<OrderDbContext>()
                .Database.EnsureCreated();
        }

        app.MapControllers();

        app.Run();
    }
}
