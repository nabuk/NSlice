using System;
using System.Linq;
using System.Text;
using NSlice.Helpers;
using NSlice.Indexers;
using NSlice.Collections;

namespace NSlice
{
    /// <summary>
    /// Provides a set of static methods for System.String.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Performs slice on passed string.
        /// </summary>
        /// <param name="source">Source string.</param>
        /// <param name="from">First character index.</param>
        /// <param name="to">Exclusive boundary.</param>
        /// <param name="step">Increment index by.</param>
        /// <exception cref="ArgumentNullException">source is null.</exception>
        /// <exception cref="ArgumentException">step is equal to 0.</exception>
        /// <returns>Result of slice operation.</returns>
        public static string Slice(
            this string source,
            int? from = null,
            int? to = null,
            int step = 1)
        {
            if (source == null) throw new ArgumentNullException("source");

            var indexer = new SliceItemIndexer<int>(new ProxiedReadOnlyList<int>(new RangeIndexer(0, source.Length)), from, to, step);
            return Enumerable.Range(0, indexer.Count)
                             .Aggregate(new StringBuilder(), (builder, i) => builder.Append(source[indexer.GetItemAt(i)]))
                             .ToString();
        }

        /// <summary>
        /// Performs deletion of specified slice.
        /// </summary>
        /// <param name="source">Source string.</param>
        /// <param name="from">First character index to delete.</param>
        /// <param name="to">Exclusive boundary.</param>
        /// <param name="step">Increment index by.</param>
        /// <exception cref="ArgumentNullException">source is null.</exception>
        /// <exception cref="ArgumentException">step is equal to 0.</exception>
        /// <returns>Result of slice deletion.</returns>
        public static string SliceDelete(
            this string source,
            int? from = null,
            int? to = null,
            int step = 1)
        {
            if (source == null) throw new ArgumentNullException("source");

            var indexer = new SliceDeleteItemIndexer<int>(new ProxiedReadOnlyList<int>(new RangeIndexer(0, source.Length)), from, to, step);
            return Enumerable.Range(0, indexer.Count)
                             .Aggregate(new StringBuilder(), (builder, i) => builder.Append(source[indexer.GetItemAt(i)]))
                             .ToString();
        }

        /// <summary>
        /// Returns the character at a specified index in a string.
        /// Index can be negative, e.g. '-1' means last, '-2' means one before last and so on.
        /// </summary>
        /// <param name="source">Source string.</param>
        /// <param name="index">Index of character to retrieve.</param>
        /// <exception cref="ArgumentNullException">source is null.</exception>
        /// <exception cref="IndexOutOfRangeException">index is out of source string's range.</exception>
        /// <returns>The character at the specified position in the source string.</returns>
        public static char At(this string source, int index)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (index < 0) index += source.Length;
            return source[index];
        }

        /// <summary>
        /// Returns the character at a specified index in a string or null if the index is out of range.
        /// Index can be negative, e.g. '-1' means last, '-2' means one before last and so on.
        /// </summary>
        /// <param name="source">Source string.</param>
        /// <param name="index">Index of character to retrieve.</param>
        /// <exception cref="ArgumentNullException">source is null.</exception>
        /// <returns>The character at the specified position in the source string.</returns>
        public static char? AtOrDefault(this string source, int index)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (index < 0) index += source.Length;
            if (index < 0 || index >= source.Length) return null;
            return source[index];
        }
    }
}
