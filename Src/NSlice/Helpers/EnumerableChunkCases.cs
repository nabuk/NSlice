using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSlice.Helpers
{
    static class EnumerableChunkCases
    {
        internal static IEnumerable<IList<T>> Chunk<T>(this IEnumerable<T> source, int chunkSize)
        {
            var result = new T[chunkSize];
            var count = 0;

            foreach (var item in source)
            {
                result[count++] = item;
                if (count == chunkSize)
                {
                    yield return result;
                    result = new T[chunkSize];
                    count = 0;
                }
            }

            if (count > 0)
                yield return ProxiedListCreator.GetSegment(result, 0, count);
        }
    }
}
