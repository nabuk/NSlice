using System;
using System.Collections.Generic;
using System.Linq;

namespace NSlice.Helpers
{
    static class EnumerableSliceCases
    {
        internal static IEnumerable<T> PPP<T>(IEnumerable<T> source, int from, int? to, int step)
        {
            if (to <= from)
                yield break;

            using (var enumerator = source.GetEnumerator())
            {
                var tmpIter = from;
                while (tmpIter > 0 && enumerator.MoveNext())
                    --tmpIter;
                if (tmpIter > 0)
                    yield break;

                if (to.HasValue)
                {
                    var toValue = to.Value - from;
                    if (step == 1)
                        while (toValue-- > 0 && enumerator.MoveNext())
                            yield return enumerator.Current;
                    else
                    {
                        while (enumerator.MoveNext())
                        {
                            yield return enumerator.Current;

                            if ((toValue -= step) <= 0)
                                yield break;

                            for (var i = 1; i < step; ++i)
                                if (!enumerator.MoveNext())
                                    yield break;
                        }
                    }
                }
                else
                {
                    if(step == 1)
                        while (enumerator.MoveNext())
                            yield return enumerator.Current;
                    else
                    {
                        while (enumerator.MoveNext())
                        {
                            yield return enumerator.Current;

                            for (var i = 1; i < step; ++i)
                                if (!enumerator.MoveNext())
                                    yield break;
                        }
                    }
                }
            }
        }

        internal static IEnumerable<T> PPN<T>(IEnumerable<T> source, int from, int? to, int step)
        {
            if (from <= to)
                yield break;
            int count;
            step = -step;
            var buffer = new DynamicBuffer<T>();
            using (var enumerator = source.GetEnumerator())
            {
                if (to.HasValue)
                {
                    var toValue = to.Value;
                    if (enumerator.Skip(toValue + 1) != toValue + 1)
                        yield break;
                    count = from - toValue;
                }
                else
                    count = from + 1;
                buffer.BufferUpToCount(enumerator, count);
            }
            count = buffer.length;
            --count;
            while (count >= 0)
            {
                yield return buffer.items[count];
                count -= step;
            }
        }

        internal static IEnumerable<T> NNP<T>(IEnumerable<T> source, int from, int to, int step)
        {
            if (from >= to)
                yield break;

            var buffer = new DynamicRotativeBuffer<T>();
            from = -from;
            to = -to;
            int count;
            using (var enumerator = source.GetEnumerator())
            {
                buffer.BufferUpToCount(enumerator, from);
                count = buffer.length;
                if (from == count)
                    count += buffer.RotateUntilEndWithCount(enumerator);
            }

            to = Math.Min(from, count) - to;
            int head = buffer.head;
            int i = 0;
            while (i < to)
            {
                i += step;
                yield return buffer.items[head];
                head = (head + step) % buffer.length;
            }
        }

        internal static IEnumerable<T> NNN<T>(IEnumerable<T> source, int from, int to, int step)
        {
            if (from <= to)
                yield break;

            from = -from;
            step = -step;
            to = -to;
            var buffer = new DynamicRotativeBuffer<T>();
            using (var enumerator = source.GetEnumerator())
            {
                buffer.BufferUpToCount(enumerator, to);
                if (buffer.length == to)
                    buffer.RotateUntilEndWithCount(enumerator);
            }

            var length = buffer.length;
            if (length < from)
                yield break;

            var head = buffer.head - from;
            var count = (Math.Min(length - from, to - from - 1) + step) / step;
            while (count-- > 0)
            {
                yield return buffer.items[(head + length) % length];
                head -= step;
            }
        }

        internal static IEnumerable<T> PNP<T>(IEnumerable<T> source, int from, int to, int step)
        {
            to = -to;
            using (var enumerator = source.GetEnumerator())
            {
                if (enumerator.Skip(from) != from)
                    yield break;

                var buffer = new DynamicRotativeBuffer<T>();
                if (step == 1)
                {
                    buffer.BufferUpToCount(enumerator, to);
                    if (buffer.length < to)
                        yield break;

                    var head = 0;
                    var sanitizedHead = 0;
                    while (enumerator.MoveNext())
                    {
                        sanitizedHead = head%to;
                        yield return buffer.items[sanitizedHead];
                        buffer.items[sanitizedHead] = enumerator.Current;
                        head++;
                    }
                }
                else
                {
                    if (!buffer.BufferWithStepUpToCount(enumerator, step, to))
                        yield break;

                    var head = 0;
                    var tmpIter = 0;
                    while (enumerator.MoveNext())
                    {
                        if (tmpIter == 0)
                            yield return buffer.items[head];

                        tmpIter = (tmpIter + 1) % step;
                        to = to % step;
                        
                        if (to == 0)
                        {
                            buffer.items[head] = enumerator.Current;
                            head = (head + 1) % buffer.length;
                        }
                        ++to;
                    }
                }
            }
        }

        internal static IEnumerable<T> PNN<T>(IEnumerable<T> source, int from, int to, int step)
        {
            to = -to;
            step = -step;
            int count;
            var buffer = new DynamicRotativeBuffer<T>();

            using (var enumerator = source.GetEnumerator())
            {
                var fromCount = from + 1;
                var toCount = to - 1;
                var minToBuffer = Math.Min(fromCount, toCount);
                buffer.BufferUpToCount(enumerator, minToBuffer);
                if (buffer.length == 0)
                    yield break;

                var reachedEnd = buffer.length < minToBuffer;
                if (!reachedEnd)
                {
                    if (fromCount > toCount)
                    {
                        var toSkip = fromCount - toCount;
                        reachedEnd = toSkip != buffer.RotateUpToCount(enumerator, toSkip);
                    }
                    else if (toCount > fromCount)
                    {
                        count = toCount - fromCount;
                        while (count > 0 && enumerator.MoveNext())
                            --count;
                        reachedEnd = count > 0;
                    }
                }
                count = buffer.length;
                if (!reachedEnd)
                    count -= enumerator.Skip(count);
            }

            if (count <= 0)
                yield break;

            var head = (buffer.head - 1 + buffer.length) % buffer.length;
            while (count > 0)
            {
                count -= step;
                yield return buffer.items[head];
                head = (head - step + buffer.length) % buffer.length;
            }
        }

        internal static IEnumerable<T> NPP<T>(IEnumerable<T> source, int from, int? to, int step)
        {
            from = -from;
            var buffer = new DynamicRotativeBuffer<T>();
            int count;

            var offset = 0;
            if (to.HasValue)
            {
                var toValue = to.Value;
                if (from >= toValue)
                {
                    using (var enumerator = source.GetEnumerator())
                    {
                        buffer.BufferUpToCount(enumerator, toValue);
                        if (buffer.length != toValue)
                        {
                            offset = 0;
                            count = buffer.length;
                        }
                        else
                        {
                            var toSkip = from - toValue;
                            if (toSkip != enumerator.Skip(toSkip))
                            {
                                offset = 0;
                                count = buffer.length;
                            }
                            else
                            {
                                offset = enumerator.Skip(toValue);
                                count = toValue - offset;
                            }
                        }
                    }
                }
                else
                {
                    using (var enumerator = source.GetEnumerator())
                    {
                        buffer.BufferUpToCount(enumerator, from);
                        count = buffer.length;
                        if (count != from)
                        {
                            offset = 0;
                        }
                        else
                        {
                            var toSkip = toValue - from;
                            if (toSkip != buffer.RotateUpToCount(enumerator, toSkip))
                            {
                                offset = 0;
                                count = buffer.length;
                            }
                            else
                            {
                                offset = enumerator.Skip(from);
                                count = from - offset;
                            }
                        }
                    }
                }
            }
            else
            {
                using (var enumerator = source.GetEnumerator())
                {
                    buffer.BufferUpToCount(enumerator, from);
                    if (buffer.length == from)
                        buffer.RotateUntilEndWithCount(enumerator);
                }

                count = buffer.length;
            }

            if (count <= 0)
                yield break;

            count = (count + step - 1) / step;
            buffer.head += offset;
            while (count-- > 0)
            {
                yield return buffer.items[buffer.head % buffer.length];
                buffer.head += step;
            }
        }

        internal static IEnumerable<T> NPN<T>(IEnumerable<T> source, int from, int? to, int step)
        {
            var buffer = new DynamicBuffer<T>();
            using (var enumerator = source.GetEnumerator())
            {
                if (to.HasValue && enumerator.Skip(to.Value + 1) != to.Value + 1)
                    yield break;
                buffer.Buffer(enumerator);
            }

            from += buffer.length;
            while (from >= 0)
            {
                yield return buffer.items[from];
                from += step;
            }
        }
    }
}