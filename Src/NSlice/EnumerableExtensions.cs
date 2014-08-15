using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NSlice.Helpers;

namespace NSlice
{
    /// <summary>
    /// Provides a set of static methods for objects that implement System.Collections.Generic.IEnumerable&lt;T&gt;.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Performs slice on passed collection.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">Source collection.</param>
        /// <param name="from">First item index.</param>
        /// <param name="to">Exclusive boundary.</param>
        /// <param name="step">Increment index by.</param>
        /// <exception cref="ArgumentNullException">source is null.</exception>
        /// <exception cref="ArgumentException">step is equal to 0.</exception>
        /// <returns>Result of slice operation.</returns>
        public static IEnumerable<T> Slice<T>(
            this IEnumerable<T> source,
            int? from = null,
            int? to = null,
            int step = 1)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            {
                var sourceList = source as IList<T>;
                if (sourceList != null)
                    return ProxiedListCreator.GetSlice(sourceList, from, to, step);
            }
            
            {
                var sourceCollection = source as ICollection;
                if (sourceCollection != null)
                {
                    var count = sourceCollection.Count;
                    var indexer = SlicePropertiesCalculator.Calculate(from, to, step, count);
                    from = indexer.from;
                    step = indexer.step;
                    count = indexer.count;
                    to = indexer.from + (step*count);
                    if (to < 0)
                        to = null;
                    if (count == 0)
                        return Enumerable.Empty<T>();
                }
            }


            if (step == 0)
                throw new ArgumentException("Step cannot be zero.");
            var fromValue = from ?? (step > 0 ? 0 : -1);
            var toIsPositiveOrNull = !(to < 0);

            if (step > 0)
            {
                if (fromValue >= 0)
                {
                    if (toIsPositiveOrNull)
                    {
                        return EnumerableSliceCases.PPP(source, fromValue, to, step);
                    }
                    else
                    {
                        return EnumerableSliceCases.PNP(source, fromValue, to.Value, step);
                    }
                }
                else
                {
                    if (toIsPositiveOrNull)
                    {
                        return EnumerableSliceCases.NPP(source, fromValue, to, step);
                    }
                    else
                    {
                        return EnumerableSliceCases.NNP(source, fromValue, to.Value, step);
                    }
                }
            }
            else
            {
                if (fromValue >= 0)
                {
                    if (toIsPositiveOrNull)
                    {
                        return EnumerableSliceCases.PPN(source, fromValue, to, step);
                    }
                    else
                    {
                        return EnumerableSliceCases.PNN(source, fromValue, to.Value, step);
                    }
                }
                else
                {
                    if (toIsPositiveOrNull)
                    {
                        return EnumerableSliceCases.NPN(source, fromValue, to, step);
                    }
                    else
                    {
                        return EnumerableSliceCases.NNN(source, fromValue, to.Value, step);
                    }
                }
            }
        }

        /// <summary>
        /// Performs deletion of specified slice.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">Source collection.</param>
        /// <param name="from">First item index to delete.</param>
        /// <param name="to">Exclusive boundary.</param>
        /// <param name="step">Increment index by.</param>
        /// <exception cref="ArgumentNullException">source is null.</exception>
        /// <exception cref="ArgumentException">step is equal to 0.</exception>
        /// <returns>Result of slice deletion.</returns>
        public static IEnumerable<T> SliceDelete<T>(
            this IEnumerable<T> source,
            int? from = null,
            int? to = null,
            int step = 1)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            {
                var sourceList = source as IList<T>;
                if (sourceList != null)
                    return ProxiedListCreator.GetSliceDelete(sourceList, from, to, step);
            }

            {
                var sourceCollection = source as ICollection;
                if (sourceCollection != null)
                {
                    var count = sourceCollection.Count;
                    var indexer = SlicePropertiesCalculator.Calculate(from, to, step, count);
                    from = indexer.from;
                    step = indexer.step;
                    count = indexer.count;
                    to = indexer.from + (step * count);
                    if (to < 0)
                        to = null;
                    if (count == 0)
                        return source;
                }
            }


            if (step == 0)
                throw new ArgumentException("Step cannot be zero.");
            var fromValue = from ?? (step > 0 ? 0 : -1);
            var toIsPositiveOrNull = !(to < 0);

            if (step > 0)
            {
                if (fromValue >= 0)
                {
                    if (toIsPositiveOrNull)
                    {
                        return EnumerableSliceDeleteCases.PPP(source, fromValue, to, step);
                    }
                    else
                    {
                        return EnumerableSliceDeleteCases.PNP(source, fromValue, to.Value, step);
                    }
                }
                else
                {
                    if (toIsPositiveOrNull)
                    {
                        return EnumerableSliceDeleteCases.NPP(source, fromValue, to, step);
                    }
                    else
                    {
                        return EnumerableSliceDeleteCases.NNP(source, fromValue, to.Value, step);
                    }
                }
            }
            else
            {
                if (fromValue >= 0)
                {
                    if (toIsPositiveOrNull)
                    {
                        return EnumerableSliceDeleteCases.PPN(source, fromValue, to, step);
                    }
                    else
                    {
                        return EnumerableSliceDeleteCases.PNN(source, fromValue, to.Value, step);
                    }
                }
                else
                {
                    if (toIsPositiveOrNull)
                    {
                        return EnumerableSliceDeleteCases.NPN(source, fromValue, to, step);
                    }
                    else
                    {
                        return EnumerableSliceDeleteCases.NNN(source, fromValue, to.Value, step);
                    }
                }
            }
        }

        /// <summary>
        /// Returns the element at a specified index in a sequence.
        /// Index can be negative, e.g. '-1' means last, '-2' means one before last and so on.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">Source collection.</param>
        /// <param name="index">Index of element to retrieve.</param>
        /// <exception cref="ArgumentNullException">source is null.</exception>
        /// <exception cref="IndexOutOfRangeException">index is out of source collection's range.</exception>
        /// <returns>The element at the specified position in the source sequence.</returns>
        public static T At<T>(this IEnumerable<T> source, int index)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            {
                var sourceList = source as IList<T>;
                if (sourceList != null)
                    return IndexedExtensions.At<T>(sourceList, index);
            }

            {
                var sourceCollection = source as ICollection;
                if (sourceCollection != null)
                {
                    var count = sourceCollection.Count;
                    if (index >= count)
                        throw new IndexOutOfRangeException();
                    else
                    {
                        if (index < 0)
                        {
                            index += count;
                            if (index < 0 || index >= count)
                                throw new IndexOutOfRangeException();
                        }
                    }
                }
            }

            using (var enumerator = source.GetEnumerator())
            {
                if (index >= 0)
                {
                    while (index >= 0 && enumerator.MoveNext())
                        --index;
                    if (index >= 0)
                        throw new IndexOutOfRangeException();
                    return enumerator.Current;
                }
                index = -index;
                var buffer = new DynamicRotativeBuffer<T>();
                buffer.BufferUpToCount(enumerator, index);
                if (buffer.length == index)
                {
                    buffer.RotateUntilEndWithoutCount(enumerator);
                    return buffer.items[buffer.head - index + buffer.length];
                }
                throw new IndexOutOfRangeException();
            }
        }

        /// <summary>
        /// Returns the element at a specified index in a sequence or a default value if the index is out of range.
        /// Index can be negative, e.g. '-1' means last, '-2' means one before last and so on.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">Source collection.</param>
        /// <param name="index">Index of element to retrieve.</param>
        /// <exception cref="ArgumentNullException">source is null.</exception>
        /// <returns>The element at the specified position in the source sequence.</returns>
        public static T AtOrDefault<T>(this IEnumerable<T> source, int index)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            {
                var sourceList = source as IList<T>;
                if (sourceList != null)
                    return IndexedExtensions.AtOrDefault<T>(sourceList, index);
            }

            {
                var sourceCollection = source as ICollection;
                if (sourceCollection != null)
                {
                    var count = sourceCollection.Count;
                    if (index >= count)
                        return default(T);
                    else
                    {
                        if (index < 0)
                        {
                            index += count;
                            if (index < 0 || index >= count)
                                return default(T);
                        }
                    }
                }
            }

            using (var enumerator = source.GetEnumerator())
            {
                if (index >= 0)
                {
                    while (index >= 0 && enumerator.MoveNext())
                        --index;
                    return index >= 0 ? default(T) : enumerator.Current;
                }
                index = -index;
                var buffer = new DynamicRotativeBuffer<T>();
                buffer.BufferUpToCount(enumerator, index);
                if (buffer.length == index)
                {
                    buffer.RotateUntilEndWithoutCount(enumerator);
                    return buffer.items[buffer.head - index + buffer.length];
                }
                return default(T);
            }
        }

        /// <summary>
        /// For each items returns also the previous one, e.g. for {0, 1, 2, 3} it returns {{0, 1}, {1, 2}, {2, 3}}.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">Source collection.</param>
        /// <returns>Result of slice operation. Notice, that the result collection will have one item less than the source one.</returns>
        public static IEnumerable<DoubleItem<T>> Drag<T>(this IEnumerable<T> source)
        {
            throw new NotImplementedException();

            if (source == null)
                throw new ArgumentNullException("source");

            using (var enumerator = source.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                    yield break;

                T prev = enumerator.Current;
                T curr;

                while (enumerator.MoveNext())
                {
                    curr = enumerator.Current;
                    yield return new DoubleItem<T>(prev, curr);
                    prev = curr;
                }
            }
        }

        /// <summary>
        /// For each items returns also numberOfItemsToDrag-1 previous ones, e.g. for {0, 1, 2, 3, 4} and numberOfItemsToDrag=3 it returns {{0, 1, 2}, {1, 2, 3}, {2, 3, 4}}.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">Source collection.</param>
        /// <returns>Result of slice operation. Notice, that the result collection will have numberOfItemsToDrag-1 items less than the source one.</returns>
        public static IEnumerable<IList<T>> Drag<T>(this IEnumerable<T> source, int numberOfItemsToDrag)
        {
            throw new NotImplementedException();

            if (source == null)
                throw new ArgumentNullException("source");

            if (numberOfItemsToDrag < 1)
                throw new ArgumentException("numberOfItemsToDrag cannot be lower than one.");

            using (var enumerator = source.GetEnumerator())
            {
                if (numberOfItemsToDrag == 1)
                    while (enumerator.MoveNext())
                        yield return new[] { enumerator.Current };
                else
                {
                    if (!enumerator.MoveNext())
                        yield break;

                    var dragMinusOne = numberOfItemsToDrag - 1;
                    var bufferSize = Math.Max(numberOfItemsToDrag * 2, 1024);
                    var buffer = new T[bufferSize];
                    buffer[0] = enumerator.Current;

                    for (int i = 1; i < dragMinusOne; ++i)
                    {
                        if (!enumerator.MoveNext())
                            yield break;

                        buffer[i] = enumerator.Current;
                    }

                    if (!enumerator.MoveNext())
                        yield break;
                    buffer[dragMinusOne] = enumerator.Current;
                    yield return ProxiedListCreator.GetSegment(buffer, 0, numberOfItemsToDrag);

                    while (true)
                    {
                        for (int i = 1; i < bufferSize - numberOfItemsToDrag; ++i)
                        {
                            if (!enumerator.MoveNext())
                                yield break;
                            buffer[dragMinusOne + i] = enumerator.Current;
                            yield return ProxiedListCreator.GetSegment(buffer, i, numberOfItemsToDrag);
                        }

                        if (!enumerator.MoveNext())
                            yield break;

                        var newBuffer = new T[bufferSize];
                        Array.Copy(buffer, bufferSize - dragMinusOne, newBuffer, 0, dragMinusOne);
                        buffer = newBuffer;
                        buffer[dragMinusOne] = enumerator.Current;
                        yield return ProxiedListCreator.GetSegment(buffer, 0, numberOfItemsToDrag);
                    }
                }
            }
        }

        /// <summary>
        /// Divides source collection into chunks, e.g. for {0, 1, 2, 3, 4} and chunkSize=2 it returns {{0, 1}, {2, 3}, {4}}.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">Source collection.</param>
        /// <param name="chunkSize"></param>
        /// <returns>Chunked collection.</returns>
        public static IEnumerable<IList<T>> Chunk<T>(this IEnumerable<T> source, int chunkSize)
        {
            throw new NotImplementedException();

            if (source == null)
                throw new ArgumentNullException("source");

            if (chunkSize < 1)
                throw new ArgumentException("chunkSize cannot be lower than one.");

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
