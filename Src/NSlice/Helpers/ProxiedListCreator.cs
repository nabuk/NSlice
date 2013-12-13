using System.Collections.Generic;
using NSlice.Collections;
using System;
using NSlice.Indexers;

namespace NSlice.Helpers
{
    static class ProxiedListCreator
    {
        internal static IList<T> GetSlice<T>(IList<T> source, int? from, int? to, int step)
        {
            var indexer = new SliceItemIndexer<T>(source, from, to, step);
            return new ProxiedReadOnlyList<T>(indexer);
        }

        internal static IList<T> GetSliceDelete<T>(IList<T> source, int? from, int? to, int step)
        {
            var indexer = new SliceDeleteItemIndexer<T>(source, from, to, step);
            return new ProxiedReadOnlyList<T>(indexer);
        }
    }
}