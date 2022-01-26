# AzureStorageSeeder
A tool to apply seed data from JSON files to Azure Table Storage.

### Prerequisite
- [.NET 6 Runtime](https://dotnet.microsoft.com/download/dotnet/6.0)

### Supported OS
- Windows
- Linux
- MacOS

### How to use
simply run the tool by

```
dotnet ./AzureTableStorageSeeder.Console.dll --directory "~/some/working/directory" --connectionString "UseDevelopmentStorage=true;"
```

above command will result in searching all JSON files in given directory and apply each file to Azure Table Storage with file name as a table name.

### Limitation
JSON file must be array of simple object (since Azure Table Storage only supports simple data type) more information [here](https://docs.microsoft.com/en-us/rest/api/storageservices/Understanding-the-Table-Service-Data-Model#property-types).

### Supported command line arguments

| Name    | Required | Default Value | Description                               |
|---------|----------|---------------|-------------------------------------------|
| directory     | yes      | -             | Seed files directory. |
| connectionString | yes       | -             | Azure Storage connection string.                 |