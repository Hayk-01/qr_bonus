using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace QRBonus.DAL;
public abstract class DesignTimeDbContextFactory<TContext>
   : IDesignTimeDbContextFactory<TContext> where TContext : DbContext
{
    public TContext CreateDbContext(string[] args)
    {
        return Create(Directory.GetCurrentDirectory(),
            Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
    }

    protected abstract TContext CreateNewInstance(DbContextOptions<TContext> options);

    private TContext Create(string basePath, string env)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("dbappsettings.json", false, true)
            .AddEnvironmentVariables();

        var config = builder.Build();

        var connectionString = config.GetConnectionString("QRBonus");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("Could not found a connection string named 'DefaultConnection'");

        return Create(connectionString);
    }

    private TContext Create(string connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentException($"{nameof(connectionString)} is null or empty", nameof(connectionString));

        var optionsBuilder = new DbContextOptionsBuilder<TContext>();

        optionsBuilder.UseNpgsql(connectionString, x =>
        {
            x.MigrationsHistoryTable("ef_migration_history");
        })
        .UseSnakeCaseNamingConvention();

        var options = optionsBuilder.Options;
        return CreateNewInstance(options);
    }
}
