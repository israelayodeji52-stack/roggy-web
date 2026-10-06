using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Roggy.Infrastructure.Data;

public class RoggyDbContextFactory : IDesignTimeDbContextFactory<RoggyDbContext>
{
    public RoggyDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<RoggyDbContext>();

        var connectionString =
            "Host=localhost;Port=5432;Database=roggy;Username=postgres;Password=postgres";

        optionsBuilder.UseNpgsql(connectionString);

        return new RoggyDbContext(optionsBuilder.Options);
    }
}