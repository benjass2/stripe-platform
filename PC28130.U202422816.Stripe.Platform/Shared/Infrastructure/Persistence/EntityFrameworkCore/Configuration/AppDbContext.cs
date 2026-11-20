using Microsoft.EntityFrameworkCore;
using PC28130.U202422816.Stripe.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using PC28130.U202422816.Stripe.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors;

namespace PC28130.U202422816.Stripe.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

/// <summary>
/// Database context for the Stripe platform.
/// </summary>
/// <remarks>
/// Author: Benjamin Solorzano.
/// </remarks>
public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    /// <inheritdoc />
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.AddInterceptors(new AuditableEntityInterceptor());
        base.OnConfiguring(builder);
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.UseSnakeCaseNamingConvention();
    }
}