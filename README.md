# AzureStorageSeeder
A tool to apply seed data from JSON files to Azure Table Storage.

[![nuget](https://img.shields.io/nuget/v/AzureTableStorageSeeder.Tool.svg)](https://www.nuget.org/packages/AzureTableStorageSeeder.Tool/1.0.1/)

### Prerequisites
- [.NET 6 Runtime](https://dotnet.microsoft.com/download/dotnet/6.0)

### Supported OS
- Windows
- Linux
- MacOS

### How to use
simply run the tool by

```
azureseeder --directory "~/some/working/directory" --connectionString "UseDevelopmentStorage=true;"
```

above command will result in searching all JSON files in given directory and apply each file to Azure Table Storage with file name as a table name.

### Limitations
- Partition Key and Row Key **must** be a String.
- JSON file must be array of simple object (since Azure Table Storage only supports simple data type) more information [here](https://docs.microsoft.com/en-us/rest/api/storageservices/Understanding-the-Table-Service-Data-Model#property-types).

### Supported command line arguments

| Name    | Required | Default Value | Description                               |
|---------|----------|---------------|-------------------------------------------|
| directory     | yes      | -             | Seed files directory. |
| connectionString | yes       | -             | Azure Storage connection string.|
| mode | no       | Replace             | Update mode strategy, supported values are (Replace, Merge).|