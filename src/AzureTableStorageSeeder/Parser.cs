using System.Globalization;
using System.Text.Json;

namespace AzureTableStorageSeeder;

internal static class Parser
{
    public static object ParseNumber(this JsonElement jsonElement)
    {
        var text = jsonElement.GetRawText();

        if (text.Contains('.'))
        {
            return double.Parse(text);
        }

        return long.Parse(text);
    }

    public static object ParseString(this JsonElement jsonElement)
    {
        var text = jsonElement.GetString()!;

        if (Guid.TryParse(text, out var guid))
        {
            return guid;
        }

        if (DateTime.TryParse(text, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var dateTime))
        {
            return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
        }

        return text;
    }
}

