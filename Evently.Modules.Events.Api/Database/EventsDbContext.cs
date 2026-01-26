using Microsoft.EntityFrameworkCore;

namespace Evently.Modules.Events.Api.Database;

public sealed class EventsDbContext(DbContextOptions<EventsDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.Events);
    }

    internal DbSet<Events.Event> Events { get; set; }
}
