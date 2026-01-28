using Evently.Modules.Events.Api.Database;
using Evently.Modules.Events.Application.Abstractions.Data;
using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.Infrastructure.Data;
using Evently.Modules.Events.Infrastructure.Database;
using Evently.Modules.Events.Infrastructure.Events;
using Evently.Modules.Events.Presentation.Events;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;

namespace Evently.Modules.Events.Infrastructure;

public static class EventsModule
{
    public static void MapEndpoints(IEndpointRouteBuilder app)
    {
        EventEndpoints.MapEndpoints(app);
    }

    public static IServiceCollection AddEventsModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(Application.AssemblyReference.Assembly);
        });

        AddInfrastructure(services, configuration);
        return services;
    }

    private static void AddInfrastructure(IServiceCollection serviceCollection, IConfiguration configuration1)
    {
        string databaseConnectionString = configuration1.GetConnectionString("EventsDatabase");

        NpgsqlDataSource npgsqlDataSource = new NpgsqlDataSourceBuilder(databaseConnectionString).Build();
        serviceCollection.TryAddSingleton(npgsqlDataSource);


        serviceCollection.AddScoped<IDbConnectionFactory, DbConnectionFactory>();

        serviceCollection.AddDbContext<EventsDbContext>(options =>
        {
            options.UseNpgsql(databaseConnectionString, npgsqlOption =>
            {
                npgsqlOption.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Events);
            }).UseSnakeCaseNamingConvention();
        });

        serviceCollection.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<EventsDbContext>());
        serviceCollection.AddScoped<IEventRepository, EventRepository>();
    }
}
