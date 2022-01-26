using AzureTableStorageSeeder;
using AzureTableStorageSeeder.Console;
using CommandLine;
using Microsoft.Extensions.Logging;

public class Program
{
    public static async Task Main(string[] args)
    {
        await Parser.Default
        .ParseArguments<Options>(args)
        .WithParsedAsync(MigrateAsync);
    }

    private static Task MigrateAsync(Options options)
    {
        using var logger = LoggerFactory.Create(builder => builder.AddConsole());

        var migrateOptions = new MigrationOptions(options.ConnectionString, options.Directory, options.Mode);

        var migrator = new Migrator(migrateOptions, logger.CreateLogger<Migrator>());

        return migrator.MigrateAsync();
    }
}