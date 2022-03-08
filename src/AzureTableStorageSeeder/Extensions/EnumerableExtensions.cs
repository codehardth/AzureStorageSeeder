namespace AzureTableStorageSeeder.Extensions;

/// <summary>
/// Backport of Enumerable.Chunk from .NET 6
/// </summary>
public static class EnumerableExtensions
{
#nullable enable
    public static IEnumerable<TSource[]> Chunk<TSource>(
        this IEnumerable<TSource> source,
        int size)
    {
        if (source == null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (size < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(size));
        }

        return ChunkIterator(source, size);
    }


#nullable disable
    private static IEnumerable<TSource[]> ChunkIterator<TSource>(
        IEnumerable<TSource> source,
        int size)
    {
        using var e = source.GetEnumerator();

        while (e.MoveNext())
        {
            var array = new TSource[size];

            array[0] = e.Current;

            int newSize;

            for (newSize = 1; newSize < array.Length && e.MoveNext(); ++newSize)
            {
                array[newSize] = e.Current;
            }

            if (newSize == array.Length)
            {
                yield return array;
            }
            else
            {
                Array.Resize(ref array, newSize);

                yield return array;

                goto label_10;
            }
        }

        label_10: ;
    }
}