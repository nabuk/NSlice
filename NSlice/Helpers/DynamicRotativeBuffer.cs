using System.Collections.Generic;

namespace NSlice.Helpers
{
    class DynamicRotativeBuffer<TElement> : DynamicBuffer<TElement>
    {
        internal int head;

        internal int RotateUntilEndWithCount(IEnumerator<TElement> enumerator)
        {
            var count = 0;
            while (enumerator.MoveNext())
            {
                items[count++ % length] = enumerator.Current;
            }
            head = count % length;
            return count;
        }

        internal void RotateUntilEndWithoutCount(IEnumerator<TElement> enumerator)
        {
            while (enumerator.MoveNext())
            {
                items[head++] = enumerator.Current;
                head %= length;
            }
        }

        internal int RotateUpToCount(IEnumerator<TElement> enumerator, int count)
        {
            var tmp = count;
            while (tmp > 0 && enumerator.MoveNext())
            {
                --tmp;
                items[head++] = enumerator.Current;
                head %= length;
            }

            return count - tmp;
        }
    }
}
