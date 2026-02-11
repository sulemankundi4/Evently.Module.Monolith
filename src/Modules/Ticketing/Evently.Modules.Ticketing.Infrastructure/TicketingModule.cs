using Evently.Common.Application.Data;
using Evently.Common.Infrastructure.Interceptors;
using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Ticketing.Application.Carts;
using Evently.Modules.Ticketing.Domain.Customers;
using Evently.Modules.Ticketing.Infrastructure.Customers;
using Evently.Modules.Ticketing.Infrastructure.Database;
using Evently.Modules.Ticketing.Presentation.Consumers;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Modules.Ticketing.Infrastructure;

public static class TicketingModule
{
    public static IServiceCollection AddTicketingModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);

        services.AddEndpoints(Presentation.AssemblyReference.Assembly);

        return services;
    }

    public static void ConfigureConsumers(IRegistrationConfigurator registrationConfigurator)
    {
        registrationConfigurator.AddConsumer<UserRegisteredIntegrationEventConsumer>();
    }

    private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        string databaseConnectionString = configuration.GetConnectionString("EventsDatabase");

        services.AddDbContext<TicketingDbContext>((sp, options) =>
        {
            options.UseNpgsql(databaseConnectionString, npgsqlOption =>
            {
                npgsqlOption.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Ticketing);
            }).UseSnakeCaseNamingConvention().AddInterceptors(sp.GetRequiredService<PublishDomainEventsInterceptor>());
        });

        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<TicketingDbContext>());

        services.AddSingleton<CartService>();

        services.AddScoped<ICustomerRepository, CustomerRepository>();
    }
}
