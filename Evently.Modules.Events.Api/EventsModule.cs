using Evently.Modules.Events.Api.Events;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Evently.Modules.Events.Api;

public static class EventsModule
{
    public static void MapEndpoints(IEndpointRouteBuilder app)
    {
        CreateEvent.MapEndpoint(app);
        GetEvent.MapEndPoint(app);
    }

    public static IServiceCollection AddEventsModule(this IServiceCollection services, IConfiguration configuration)
    {
        // Register module services here in the future
        return services;
    }

}
