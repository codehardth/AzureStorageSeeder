using Azure.Data.Tables;

namespace AzureTableStorageSeeder;

public record MigrationOptions(string ConnectionString, string Directory, TableUpdateMode Mode);