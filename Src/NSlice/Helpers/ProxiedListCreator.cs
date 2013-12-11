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

        private static Func<int, T> PrepareSliceDeleteItemCallback<T>(IList<T> source, StepIndexer indexer)
        {
            int from = indexer.from;
            int step = indexer.step;
            int count = indexer.count;
            int boundary = from + (count - 1) * (step - 1);

            return index =>
                {
                    if (index >= from)
                    {
                        if (index >= boundary)
                            index += count;
                        else
                        {
                            int rem;
                            var div = Math.DivRem(index - from, step - 1, out rem);
                            index = div * step + rem + from + 1;
                        }
                    }

                    return source[index];
                };
        }

        internal static IList<T> GetSlice<T>(IList<T> source, int? from, int? to, int step)
        {
            var indexer = SliceIndexerCalculator.Calculate(from, to, step, source.Count);
            var itemCallback = PrepareSliceItemCallback(source, indexer);

            return new ProxiedReadOnlyList<T>(itemCallback, indexer.count);
        }

        internal static IList<T> GetSliceDelete<T>(IList<T> source, int? from, int? to, int step)
        {
            var indexer = SliceIndexerCalculator.Calculate(from, to, step, source.Count);
            indexer = SliceIndexerCalculator.Abs(indexer);
            var itemCallback = PrepareSliceDeleteItemCallback(source, indexer);
            int count = source.Count;
            if (count - 1 >= indexer.from)
            {
                count -= Math.Min(indexer.count, (count - indexer.from + indexer.step - 1) / indexer.step);
            }
            
            return new ProxiedReadOnlyList<T>(itemCallback, count);
        }
    }
}