using Azure.Data.Tables;

namespace AzureTableStorageSeeder;

public record MigrationOptions(string connectionString, string Directory, TableUpdateMode Mode);