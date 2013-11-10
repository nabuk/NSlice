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

            var buffer = new DynamicBuffer<T>();
            using (var enumerator = source.GetEnumerator())
            {
                step = -step;

                if (to.HasValue)
                {
                    var toValue = to.Value;
                    var tmpIter = toValue;
                    while (tmpIter >= 0 && enumerator.MoveNext())
                        --tmpIter;
                    if (tmpIter >= 0)
                        yield break;

                    if (step == 1)
                    {
                        buffer.BufferUpToCount(enumerator, from - toValue);
                    }
                    else
                    {
                        var offset = (from - toValue - 1) % step;
                        tmpIter = offset;
                        while (tmpIter > 0 && enumerator.MoveNext())
                            --tmpIter;
                        if (tmpIter > 0)
                            yield break;

                        buffer.BufferWithStepUpToCount(enumerator, step, from - toValue - offset);
                    }
                }
                else
                {
                    if (step == 1)
                    {
                        buffer.BufferUpToCount(enumerator, from + 1);
                    }
                    else
                    {
                        var offset = from % step;
                        var tmpIter = offset;
                        while (tmpIter > 0 && enumerator.MoveNext())
                            --tmpIter;
                        if (tmpIter > 0)
                            yield break;

                        buffer.BufferWithStepUpToCount(enumerator, step, from + 1 - offset);
                    }
                }
            }

            var i = buffer.length;
            while (i > 0)
                yield return buffer.items[--i];
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
                if (buffer.length == from)
                    count += buffer.RotateUntilEndWithCount(enumerator);
            }

            if (step == 1)
            {
                var length = buffer.length;
                if (length <= to)
                    yield break;

                count = length - to;
                while (count-- > 0)
                {
                    yield return buffer.items[buffer.head % length];
                    buffer.head++;
                }
            }
            else
            {
                var length = buffer.length;
                if (length <= to)
                    yield break;
                var offset = 0;
                if (from > count)
                    offset = (step - ((from - count) % step)) % step;

                count = (length - to - offset + step - 1) / step;
                buffer.head += offset;
                while (count-- > 0)
                {
                    yield return buffer.items[buffer.head % length];
                    buffer.head += step;
                }
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
                var tmpIter = from;
                while (tmpIter > 0 && enumerator.MoveNext())
                    --tmpIter;
                if (tmpIter > 0)
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
                    tmpIter = 0;
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

            if (step == 1)
            {
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
                        while (count > 0 && enumerator.MoveNext())
                            --count;
                }
            }
            else
            {
                count = PNNS(source, buffer, from, to, step);
            }

            if (count <= 0)
                yield break;

            var head = (buffer.length + buffer.head - 1) % buffer.length;
            while (count-- > 0)
            {
                yield return buffer.items[(head + buffer.length) % buffer.length];
                head--;
            }
        }

        private static int PNNS<T>(IEnumerable<T> source, DynamicRotativeBuffer<T> buffer, int from, int to, int step)
        {
            var offset = from % step;
            var fromCount = from + 1;
            var toCount = to - 1;
            var bufferSpread = fromCount - offset;
            using (var enumerator = source.GetEnumerator())
            {
                if (enumerator.Skip(offset) != offset)
                    return 0;

                bufferSpread = Math.Min(toCount, bufferSpread);
                if (bufferSpread == 0)
                    return 0;
                if (!buffer.BufferWithStepUpToCount(enumerator, step, bufferSpread))
                    return buffer.length;

                int tmpIter;
                if (fromCount > toCount)
                {
                    tmpIter = offset + bufferSpread;
                    while (tmpIter < fromCount && enumerator.MoveNext())
                    {
                        if (tmpIter%step == offset)
                        {
                            buffer.head = buffer.head%buffer.length;
                            buffer.items[buffer.head] = enumerator.Current;
                            buffer.head++;
                        }
                        tmpIter++;
                    }
                    if (tmpIter < fromCount)
                    {
                        tmpIter = (tmpIter - bufferSpread - offset)%step;
                        var max = (toCount + step - 1) / step;
                        var minCount = (step - (toCount % step)) % step;

                        return tmpIter > 0 && tmpIter <= minCount
                            ? max - 1
                            : max;
                    }
                }
                else
                {
                    tmpIter = toCount - fromCount;
                    if (enumerator.Skip(tmpIter) != tmpIter)
                        return buffer.length;
                }

                bufferSpread = Math.Min(toCount, fromCount);
                tmpIter = enumerator.Skip(bufferSpread);
                return (step - 1 + bufferSpread - tmpIter)/step;
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
                int length;
                int tmp;
                if (from >= toValue)
                {
                    using (var enumerator = source.GetEnumerator())
                    {
                        buffer.BufferUpToCount(enumerator, toValue);
                        var reachedEnd = buffer.length != toValue;
                        if (reachedEnd)
                        {
                            offset = (step - ((from - buffer.length)%step))%step;
                            length = buffer.length - offset;
                        }
                        else
                        {
                            var toSkip = from - toValue;
                            tmp = enumerator.Skip(toSkip);
                            reachedEnd = tmp != toSkip;

                            if (reachedEnd)
                            {
                                offset = (step - ((from - buffer.length - tmp) % step)) % step;
                                length = buffer.length - offset;
                            }
                            else
                            {
                                tmp = enumerator.Skip(toValue);
                                length = toValue - tmp;
                                offset = tmp;
                            }
                        }
                    }
                    count = length;
                }
                else
                {
                    using (var enumerator = source.GetEnumerator())
                    {
                        buffer.BufferUpToCount(enumerator, from);
                        count = buffer.length;
                        var reachedEnd = count != from;
                        if (reachedEnd)
                        {
                            offset = (step - ((from - count) % step)) % step;
                            length = count - offset;
                        }
                        else
                        {
                            var toSkip = toValue - from;
                            tmp = buffer.RotateUpToCount(enumerator, toSkip);
                            reachedEnd = toSkip != tmp;

                            if (reachedEnd)
                            {
                                offset = 0;
                                length = buffer.length;
                            }
                            else
                            {
                                tmp = enumerator.Skip(from);
                                length = from - tmp;
                                offset = tmp;
                            }
                        }
                    }

                    count = length;
                }
            }
            else
            {
                using (var enumerator = source.GetEnumerator())
                {
                    buffer.BufferUpToCount(enumerator, from);
                    if (buffer.length == from)
                        buffer.RotateUntilEndWithCount(enumerator);
                    else
                    {
                        offset = (step - ((from - buffer.length) % step)) % step;
                    }
                }

                count = buffer.length - offset;
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
                if (to.HasValue)
                {
                    var toValue = to.Value;
                    while (toValue >= 0)
                    {
                        if (enumerator.MoveNext())
                            --toValue;
                        else
                            yield break;
                    }
                }
                buffer.Buffer(enumerator);
            }

            from = buffer.length + from;
            while (from >= 0)
            {
                yield return buffer.items[from];
                from += step;
            }
        }
    }
}