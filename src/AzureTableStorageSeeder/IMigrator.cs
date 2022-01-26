namespace AzureTableStorageSeeder;

public interface IMigrator
{
    public Task MigrateAsync(CancellationToken cancellationToken = default);
}