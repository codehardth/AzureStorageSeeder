using Azure.Data.Tables;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureTableStorageSeeder;

public sealed class Migrator : IMigrator
{
    private readonly MigrationOptions options;
    private readonly ILogger<Migrator>? logger;

    public Migrator(MigrationOptions options, ILogger<Migrator>? logger = null)
    {
        this.options = options;
        this.logger = logger;
    }

    public async Task MigrateAsync(CancellationToken cancellationToken = default)
    {
        var files = Directory.EnumerateFiles(options.Directory);

        if (!files.Any())
        {
            return;
        }

        foreach (var file in files)
        {
            var fileInfo = new FileInfo(file);
            var fileName = fileInfo.Name;

            if (!fileName.EndsWith(".json"))
            {
                continue;
            }

            var tableName = fileName.Replace(".json", "");

            logger?.LogInformation($"Starting migrate data for {tableName}.");

            var table = new TableClient(options.connectionString, tableName);

            await table.CreateIfNotExistsAsync(cancellationToken);

            var entities = GetTableEntities(file);

            foreach (var entity in entities)
            {
                try
                {
                    await table.UpsertEntityAsync(entity, options.Mode, cancellationToken);

                    logger?.LogInformation($"PartitionKey {entity.PartitionKey} RowKey {entity.RowKey} inserted.");
                }
                catch (Azure.RequestFailedException ex)
                {
                    logger?.LogError(ex.Message);

                    throw;
                }
            }
        }
    }

    private static IEnumerable<TableEntity> GetTableEntities(string file)
    {
        var content = File.ReadAllText(file);

        var values = Flatten(content);

        if (values == null || !values.Any())
        {
            yield break;
        }

        foreach (var value in values)
        {
            var entity = new TableEntity();

            foreach (var key in value.Keys)
            {
                entity.Add(key, value[key]);
            }

            yield return entity;
        }

        static Dictionary<string, object>[]? Flatten(string json)
        {
            var values = JsonConvert.DeserializeObject<Dictionary<string, object>[]>(json);

            return values;
        }
    }
}