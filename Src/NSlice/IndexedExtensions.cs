using System;
using System.Collections.Generic;
using NSlice.Helpers;
using NSlice.Collections;
using NSlice.Indexers;

namespace NSlice
{
    /// <summary>
    /// Provides a set of static methods for objects that implement System.Collections.Generic.IList&lt;T&gt;.
    /// </summary>
    public static class IndexedExtensions
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
        public static IList<T> Slice<T>(
            this IList<T> source,
            int? from = null,
            int? to = null,
            int step = 1)
        {
            if (source == null) throw new ArgumentNullException("source");
            return ProxiedListCreator.GetSlice(source, from, to, step);
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
        public static IList<T> SliceDelete<T>(
            this IList<T> source,
            int? from = null,
            int? to = null,
            int step = 1)
        {
            if (source == null) throw new ArgumentNullException("source");

            return ProxiedListCreator.GetSliceDelete(source, from, to, step);
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
        public static T At<T>(this IList<T> source, int index)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (index < 0) index += source.Count;
            if (index < 0 || index >= source.Count)
                throw new IndexOutOfRangeException();
            return source[index];
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
        public static T AtOrDefault<T>(this IList<T> source, int index)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (index < 0) index += source.Count;
            if (index < 0 || index >= source.Count) return default(T);
            return source[index];
        }

        /// <summary>
        /// For each items returns also the previous one, e.g. for {0, 1, 2, 3} it returns {{0, 1}, {1, 2}, {2, 3}}.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">Source collection.</param>
        /// <exception cref="ArgumentNullException">source is null.</exception>
        /// <returns>Result of slice operation. Notice, that the result collection will have one item less than the source one.</returns>
        public static IList<DoubleItem<T>> Drag<T>(this IList<T> source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            return new ProxiedReadOnlyList<DoubleItem<T>>(new DoubleDragIndexer<T>(source));
        }

        /// <summary>
        /// For each items returns also numberOfItemsToDrag-1 previous ones, e.g. for {0, 1, 2, 3, 4} and numberOfItemsToDrag=3 it returns {{0, 1, 2}, {1, 2, 3}, {2, 3, 4}}.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">Source collection.</param>
        /// <exception cref="ArgumentNullException">source is null.</exception>
        /// <exception cref="ArgumentException">numberOfItemsToDrag is lower than 1.</exception>
        /// <returns>Result of slice operation. Notice, that the result collection will have numberOfItemsToDrag-1 items less than the source one.</returns>
        public static IList<IList<T>> Drag<T>(this IList<T> source, int numberOfItemsToDrag)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (numberOfItemsToDrag < 1)
                throw new ArgumentException("numberOfItemsToDrag cannot be lower than one.");

            return new ProxiedReadOnlyList<IList<T>>(new DragIndexer<T>(source, numberOfItemsToDrag));
        }

        /// <summary>
        /// Divides source collection into chunks, e.g. for {0, 1, 2, 3, 4} and chunkSize=2 it returns {{0, 1}, {2, 3}, {4}}.
        /// </summary>
        /// <typeparam name="T">The type of the elements of source.</typeparam>
        /// <param name="source">Source collection.</param>
        /// <param name="chunkSize"></param>
        /// <exception cref="ArgumentNullException">source is null.</exception>
        /// <exception cref="ArgumentException">chunkSize is lower than 1.</exception>
        /// <returns>Chunked collection.</returns>
        public static IList<IList<T>> Chunk<T>(this IList<T> source, int chunkSize)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (chunkSize < 1)
                throw new ArgumentException("chunkSize cannot be lower than one.");

            return new ProxiedReadOnlyList<IList<T>>(new ChunkIndexer<T>(source, chunkSize));
        }
    }
}
