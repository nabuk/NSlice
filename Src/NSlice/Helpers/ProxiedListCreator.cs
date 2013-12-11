using System.Collections.Generic;
using NSlice.Collections;
using System;
using NSlice.Indexers;

namespace NSlice.Helpers
{
    static class ProxiedListCreator
    {
        private static Func<int, T> PrepareSliceItemCallback<T>(IList<T> source, StepIndexer indexer)
        {
            int from = indexer.from;
            int step = indexer.step;

            return index => source[from + index * step];
        }

        internal static IList<T> GetSlice<T>(IList<T> source, int? from, int? to, int? step)
        {
            var indexer = SliceIndexerCalculator.Calculate(from, to, step, source.Count);
            var itemCallback = PrepareSliceItemCallback(source, indexer);

            return new ProxiedReadOnlyList<T>(itemCallback, indexer.count);
        }

        internal static IList<T> GetSlice<T>(IList<T> source, int? from, int? to, int? step, int count)
        {
            var indexer = SliceIndexerCalculator.Calculate(from, to, step, count);
            var itemCallback = PrepareSliceItemCallback(source, indexer);

            return new ProxiedReadOnlyList<T>(itemCallback, indexer.count);
        }
    }
}