#nullable disable

using Azure.Data.Tables;
using CommandLine;

namespace AzureTableStorageSeeder.Tool;

internal class Options
{
    [Option('s', "connectionString", Required = true, HelpText = "Azure Storage connection string.")]
    public string ConnectionString { get; set; }

    [Option('d', "directory", Required = true, HelpText = "Seed files directory.")]
    public string Directory { get; set; }

    [Option('m', "mode", Required = false, HelpText = "Update mode strategy, supported values are (Replace, Merge).", Default = TableUpdateMode.Replace)]
    public TableUpdateMode Mode { get; set; }
}