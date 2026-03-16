using Microsoft.EntityFrameworkCore;
using WebApplication1.Utilities;

namespace WebApplication1.Data;

public class ApplicationContext : DbContext
{
    public DbSet<Actor> Actors { get; set; }

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {

    }

    protected async override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Actor>()
            .Property(p => p.AdditionalInformation)
            .HasConversion(new JsonDocumentConverter());
    }
}
