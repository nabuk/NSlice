using System;
using System.Collections.Generic;
using NSlice.Helpers;

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
            return ProxiedListCreator.Create(source, from, to, step);
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
    }
}
