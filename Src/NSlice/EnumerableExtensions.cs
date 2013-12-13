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
    }
}
