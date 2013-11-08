using System;
using System.Collections.Generic;

namespace NSlice.Helpers
{
    class DynamicBuffer<TElement>
    {
        internal TElement[] items;
        internal int length;

        internal DynamicBuffer()
        {
            items = new TElement[4];
        }

        internal void Buffer(IEnumerator<TElement> enumerator)
        {
            while (enumerator.MoveNext())
            {
                //IncreaseBufferIfNecessary
                if (items.Length == length)
                {
                    var newArray = new TElement[checked(length * 2)];
                    Array.Copy(items, 0, newArray, 0, length);
                    items = newArray;
                }

                items[length++] = enumerator.Current;
            }
        }

        internal void BufferWithStep(IEnumerator<TElement> enumerator, int step)
        {
            int tmpIter;
            while (enumerator.MoveNext())
            {
                //IncreaseBufferIfNecessary
                if (items.Length == length)
                {
                    var newArray = new TElement[checked(length * 2)];
                    Array.Copy(items, 0, newArray, 0, length);
                    items = newArray;
                }

                items[length++] = enumerator.Current;

                tmpIter = step;
                while(tmpIter-- > 1)
                    if (!enumerator.MoveNext())
                        return;
            }
        }

        internal void BufferUpToCount(IEnumerator<TElement> enumerator, int count)
        {
            while (count-- > 0 && enumerator.MoveNext())
            {
                //IncreaseBufferIfNecessary
                if (items.Length == length)
                {
                    var newArray = new TElement[checked(length * 2)];
                    Array.Copy(items, 0, newArray, 0, length);
                    items = newArray;
                }

                items[length++] = enumerator.Current;
            }
        }

        internal bool BufferWithStepUpToCount(IEnumerator<TElement> enumerator, int step, int count)
        {
            --step;
            int tmpIter;
            int tmpStep;
            while (count > 0 && enumerator.MoveNext())
            {
                --count;

                //IncreaseBufferIfNecessary
                if (items.Length == length)
                {
                    var newArray = new TElement[checked(length * 2)];
                    Array.Copy(items, 0, newArray, 0, length);
                    items = newArray;
                }

                items[length++] = enumerator.Current;

                tmpStep = tmpIter = Math.Min(step, count);
                while (tmpIter > 0)
                {
                    if (!enumerator.MoveNext())
                        return false;
                    --tmpIter;
                }
                    
                count -= tmpStep;
            }

            return count == 0;
        }
    }
}
