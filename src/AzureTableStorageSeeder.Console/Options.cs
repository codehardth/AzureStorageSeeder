#nullable disable

using CommandLine;

namespace AzureTableStorageSeeder.Console;

internal class Options
{
    [Option('s', "connectionString", Required = true, HelpText = "Azure Storage connection string.")]
    public string ConnectionString { get; set; }
    
    [Option('d', "directory", Required = true, HelpText = "Seed files directory.")]
    public string Directory { get; set; }
}