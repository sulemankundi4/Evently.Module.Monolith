using Evently.Modules.Events.Domain.Categories;
using Evently.Modules.Events.Domain.Events;
using Evently.Modules.Events.Domain.TicketTypes;
using Evently.Modules.Events.Infrastructure.Categories;
using Evently.Modules.Events.Infrastructure.Database;
using Evently.Modules.Events.Infrastructure.Events;
using Evently.Modules.Events.Infrastructure.TicketTypes;
using Evently.Modules.Events.Presentation.Categories;
using Evently.Modules.Events.Presentation.Events;
using Evently.Modules.Events.Presentation.TicketTypes;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IUnitOfWork = Evently.Modules.Events.Application.Abstractions.Data.IUnitOfWork;

namespace Evently.Modules.Events.Infrastructure;

public static class EventsModule
{
    public static void MapEndpoints(IEndpointRouteBuilder app)
    {
        EventEndpoints.MapEndpoints(app);
        TicketTypeEndpoints.MapEndpoints(app);
        CategoryEndpoints.MapEndpoints(app);
    }

    public static IServiceCollection AddEventsModule(this IServiceCollection services, IConfiguration configuration)
    {
        AddInfrastructure(services, configuration);
        return services;
    }

    private static void AddInfrastructure(IServiceCollection services, IConfiguration configuration1)
    {
        string databaseConnectionString = configuration1.GetConnectionString("EventsDatabase");

       

        services.AddDbContext<EventsDbContext>(options =>
        {
            options.UseNpgsql(databaseConnectionString, npgsqlOption =>
            {
                npgsqlOption.MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schemas.Events);
            }).UseSnakeCaseNamingConvention().AddInterceptors();
        });

        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<EventsDbContext>());

        services.AddScoped<IEventRepository, EventRepository>();

        services.AddScoped<ITicketTypeRepository, TicketTypeRepository>();

        services.AddScoped<ICategoryRepository, CategoryRepository>();
    }
}
