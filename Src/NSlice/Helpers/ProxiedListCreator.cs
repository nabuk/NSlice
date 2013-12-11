using System.Collections.Generic;
using NSlice.Collections;

namespace NSlice.Helpers
{
    static class ProxiedListCreator
    {
        internal static IList<T> GetSlice<T>(IList<T> source, int? from, int? to, int? step)
        {
            var indexer = SliceIndexerCalculator.Calculate(from, to, step, source.Count);
            return new ProxiedList<T>(source, indexer);
        }

        internal static IList<T> GetSlice<T>(IList<T> source, int? from, int? to, int? step, int count)
        {
            var indexer = SliceIndexerCalculator.Calculate(from, to, step, count);
            return new ProxiedList<T>(source, indexer);
        }
    }
}