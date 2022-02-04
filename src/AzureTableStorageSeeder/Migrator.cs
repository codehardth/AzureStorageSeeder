using Azure.Data.Tables;
using Microsoft.Extensions.Logging;
using System.Text.Json;

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

        var transactionType = options.Mode switch
        {
            TableUpdateMode.Merge => TableTransactionActionType.UpsertMerge,
            TableUpdateMode.Replace => TableTransactionActionType.UpsertReplace,
            _ => throw new NotSupportedException(),
        };

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

            var client = new TableClient(options.ConnectionString, tableName);

            await client.CreateIfNotExistsAsync(cancellationToken);

            var entities = GetTableEntities(file);

            var batch = entities.Select(entity => new TableTransactionAction(transactionType, entity));

            await client.SubmitTransactionAsync(batch, cancellationToken);

            logger?.LogInformation($"{tableName} migrated succesfully.");
        }
    }

    private static IEnumerable<TableEntity> GetTableEntities(string file)
    {
        var content = File.ReadAllText(file);

        var pairs = Flatten(content);

        if (pairs == null || !pairs.Any())
        {
            yield break;
        }

        foreach (var pair in pairs)
        {
            var entity = new TableEntity();

            foreach (var key in pair.Keys)
            {
                var value = GetJsonElementValue(key, pair[key]);

                if (value == null)
                {
                    continue;
                }

                entity.Add(key, value);
            }

            yield return entity;
        }

        static Dictionary<string, JsonElement>[]? Flatten(string json)
        {
            var values = JsonSerializer.Deserialize<Dictionary<string, JsonElement>[]>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            });

            return values;
        }

        static object? GetJsonElementValue(string key, JsonElement jsonElement)
        {
            var valueKind = jsonElement.ValueKind;

            return valueKind switch
            {
                JsonValueKind.String => jsonElement.ParseString(key),
                JsonValueKind.Number => jsonElement.ParseNumber(),
                JsonValueKind.True => jsonElement.GetBoolean(),
                JsonValueKind.False => jsonElement.GetBoolean(),
                JsonValueKind.Null => null,
                JsonValueKind.Undefined => null,
                _ => throw new NotSupportedException($"'{valueKind}' is not supported in this context."),
            };
        }
    }
}