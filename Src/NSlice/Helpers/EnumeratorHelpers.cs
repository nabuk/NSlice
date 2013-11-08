using System.Collections.Generic;

namespace NSlice.Helpers
{
    static class EnumeratorHelpers
    {
        internal static int Skip<T>(this IEnumerator<T> enumerator, int count)
        {
            var tmpIter = count;
            while (tmpIter > 0 && enumerator.MoveNext())
                --tmpIter;
            return count - tmpIter;
        }
    }
}
