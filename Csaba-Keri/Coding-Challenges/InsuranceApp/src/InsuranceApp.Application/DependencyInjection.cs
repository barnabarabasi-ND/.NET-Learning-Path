using FluentValidation;
using InsuranceApp.Application.Clients;
using InsuranceApp.Application.Clients.Commands;
using InsuranceApp.Application.Clients.Queries;
using InsuranceApp.Application.Clients.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace InsuranceApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddScoped<IClientService, ClientService>();

        services.AddScoped<IValidator<CreateClientCommand>, CreateClientCommandValidator>();
        services.AddScoped<IValidator<UpdateClientCommand>, UpdateClientCommandValidator>();
        services.AddScoped<IValidator<SearchClientsQuery>, SearchClientsQueryValidator>();

        return services;
    }
}
