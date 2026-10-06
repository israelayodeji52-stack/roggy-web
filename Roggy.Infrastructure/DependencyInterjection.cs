using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Roggy.Application.Abstractions.Authentication;
using Roggy.Application.Abstractions.Persistence;
using Roggy.Infrastructure.Authentication;
using Roggy.Infrastructure.Data;

namespace Roggy.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("RoggyDatabase")
            ?? throw new InvalidOperationException(
                "RoggyDatabase connection string was not found.");

        services.AddDbContext<RoggyDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        services.AddScoped<IApplicationDbContext>(
            provider => provider.GetRequiredService<RoggyDbContext>());

        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        return services;
    }
}