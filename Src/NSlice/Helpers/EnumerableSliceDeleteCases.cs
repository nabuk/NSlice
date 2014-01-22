using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSlice.Helpers
{
    static class EnumerableSliceDeleteCases
    {
        internal static IEnumerable<T> PPP<T>(IEnumerable<T> source, int from, int? to, int step)
        {
            using (var enumerator = source.GetEnumerator())
            {
                for (var i = 0; i < from; ++i)
                    if (enumerator.MoveNext())
                        yield return enumerator.Current;
                    else
                        yield break;

                if (to.HasValue)
                {
                    var toValue = to.Value;

                    if (step == 1)
                    {
                        while (from++ < toValue)
                            if (!enumerator.MoveNext())
                                yield break;
                    }
                    else
                    {
                        int reminder;
                        var iterations = Math.DivRem(toValue - from, step, out reminder);

                        for (var i = 0; i < iterations; ++i)
                        {
                            if (!enumerator.MoveNext())
                                yield break;

                            for (var j = 1; j < step; ++j)
                                if (enumerator.MoveNext())
                                    yield return enumerator.Current;
                                else
                                    yield break;
                        }

                        if (reminder > 0)
                        {
                            if (!enumerator.MoveNext())
                                yield break;

                            for (var j = 1; j < reminder; ++j)
                                if (enumerator.MoveNext())
                                    yield return enumerator.Current;
                                else
                                    yield break;
                        }
                    }

                    while (enumerator.MoveNext())
                        yield return enumerator.Current;
                }
                else
                {
                    if (step == 1)
                        yield break;

                    while (enumerator.MoveNext())
                    {
                        for (var i = 1; i < step; ++i)
                            if (enumerator.MoveNext())
                                yield return enumerator.Current;
                            else
                                yield break;
                    }
                }
            }
        }

        internal static IEnumerable<T> PPN<T>(IEnumerable<T> source, int from, int? to, int step)
        {
            step = -step;

            using (var enumerator = source.GetEnumerator())
            {
                if (to.HasValue)
                {
                    var toValue = to.Value;

                    for (var i = 0; i <= toValue; ++i)
                        if (enumerator.MoveNext())
                            yield return enumerator.Current;
                        else
                            yield break;

                    var fromToDiff = from - toValue;

                    if (step == 1)
                    {
                        for (int i = 0, b = fromToDiff; i < b; ++i)
                            if (!enumerator.MoveNext())
                                yield break;
                    }
                    else
                    {
                        var buffer = new DynamicBuffer<T>();
                        buffer.BufferUpToCount(enumerator, fromToDiff);

                        int reminder;
                        var iterations = Math.DivRem(buffer.length, step, out reminder);

                        var i = 0;
                        for (var decReminder = reminder - 1; i < decReminder; ++i)
                            yield return buffer.items[i];

                        if (reminder > 0)
                            ++i;

                        for (int j = 0, decStep = step - 1; j < iterations; ++j, ++i)
                            for (var b = i + decStep; i < b; ++i)
                                yield return buffer.items[i];

                        if (buffer.length < fromToDiff)
                            yield break;
                    }

                    while (enumerator.MoveNext())
                        yield return enumerator.Current;
                }
                else
                {
                    if (step == 1)
                    {
                        while (from-- >= 0)
                            if (!enumerator.MoveNext())
                                yield break;
                    }
                    else
                    {
                        var buffer = new DynamicBuffer<T>();
                        buffer.BufferUpToCount(enumerator, from + 1);

                        int reminder;
                        var iterations = Math.DivRem(buffer.length, step, out reminder);

                        var i = 0;
                        for (var decReminder = reminder - 1; i < decReminder; ++i)
                            yield return buffer.items[i];

                        if (reminder > 0)
                            ++i;

                        for (int j = 0, decStep = step - 1; j < iterations; ++j, ++i)
                            for (var b = i + decStep; i < b; ++i)
                                yield return buffer.items[i];

                        if (buffer.length < from + 1)
                            yield break;
                    }

                    while (enumerator.MoveNext())
                        yield return enumerator.Current;
                }
            }
        }

        internal static IEnumerable<T> NNP<T>(IEnumerable<T> source, int from, int to, int step)
        {
            from = -from;
            to = -to;

            using (var enumerator = source.GetEnumerator())
            {
                if (to >= from)
                {
                    while (enumerator.MoveNext())
                        yield return enumerator.Current;

                    yield break;
                }

                var head = 0;
                var buffer = new DynamicBuffer<T>();
                buffer.BufferUpToCount(enumerator, from);

                if (buffer.length < from)
                {
                    from = buffer.length;

                    if (to >= from)
                    {
                        for (var i = 0; i < from; ++i)
                            yield return buffer.items[i];

                        yield break;
                    }
                }
                else
                {
                    while (enumerator.MoveNext())
                    {
                        yield return buffer.items[head];
                        buffer.items[head] = enumerator.Current;
                        head = (head + 1) % from;
                    }
                }

                if (step == 1)
                {
                    head = (head - to + from) % from;
                    for (var i = 0; i < to; ++i)
                    {
                        yield return buffer.items[head];
                        head = (head + 1) % from;
                    }
                }
                else
                {
                    int rem;
                    var iterations = Math.DivRem(from - to, step, out rem);

                    for (var i = 0; i < iterations; ++i)
                    {
                        head = (head + 1) % from;

                        for (int j = 1; j < step; ++j)
                        {
                            yield return buffer.items[head];
                            head = (head + 1) % from;
                        }
                    }

                    if (rem > 0)
                    {
                        head = (head + 1) % from;

                        for (var j = 1; j < rem; ++j)
                        {
                            yield return buffer.items[head];
                            head = (head + 1) % from;
                        }
                    }

                    for (var i = 0; i < to; ++i)
                    {
                        yield return buffer.items[head];
                        head = (head + 1) % from;
                    }
                }
            }
        }

        internal static IEnumerable<T> NNN<T>(IEnumerable<T> source, int from, int to, int step)
        {
            from = -from;
            to = -to;
            step = -step;

            using (var enumerator = source.GetEnumerator())
            {
                if (to <= from)
                {
                    while (enumerator.MoveNext())
                        yield return enumerator.Current;

                    yield break;
                }

                var head = 0;
                var sliceLength = to - from;
                var buffer = new DynamicBuffer<T>();

                if (step == 1)
                {
                    var toBuffer = sliceLength + from - 1;
                    buffer.BufferUpToCount(enumerator, toBuffer);
                    if (buffer.length < toBuffer)
                    {
                        sliceLength = buffer.length - from + 1;
                        if (sliceLength > 0)
                            head += sliceLength;

                        for (; head < buffer.length; ++head)
                            yield return buffer.items[head];
                    }
                    else
                    {
                        while (enumerator.MoveNext())
                        {
                            yield return buffer.items[head];
                            buffer.items[head] = enumerator.Current;
                            head = (head + 1) % toBuffer;
                        }

                        head = (head + sliceLength) % toBuffer;
                        for (var i = 1; i < from; ++i)
                        {
                            yield return buffer.items[head];
                            head = (head + 1) % toBuffer;
                        }
                    }
                }
                else
                {
                    var parts = (sliceLength - 1) / step;
                    var toBuffer = parts * step + from;
                
                    buffer.BufferUpToCount(enumerator, toBuffer);
                    if (buffer.length < toBuffer)
                    {
                        sliceLength = buffer.length - from + 1;
                        if (sliceLength <= 0)
                            for (int i = 0; i < buffer.length; ++i)
                                yield return buffer.items[i];
                        else
                        {
                            parts = (sliceLength - 1) / step;
                            toBuffer = parts * step + 1;
                            for (int toReturn = sliceLength - toBuffer; head < toReturn; ++head)
                                yield return buffer.items[head];

                            ++head;
                            for (var i = 0; i < parts; ++i, ++head)
                                for (var j = 1; j < step; ++j)
                                    yield return buffer.items[head++];

                            for (; head < buffer.length; ++head)
                                yield return buffer.items[head];
                        }
                    }
                    else
                    {
                        while (enumerator.MoveNext())
                        {
                            yield return buffer.items[head];
                            buffer.items[head] = enumerator.Current;
                            head = (head + 1) % toBuffer;
                        }

                        head = (head + 1) % toBuffer;
                        for (var i = 0; i < parts; ++i)
                        {
                            for (var j = 1; j < step; ++j)
                            {
                                yield return buffer.items[head];
                                head = (head + 1) % toBuffer;
                            }

                            head = (head + 1) % toBuffer;
                        }

                        for (var i = 1; i < from; ++i)
                        {
                            yield return buffer.items[head];
                            head = (head + 1) % toBuffer;
                        }
                    }
                }
            }
        }

        internal static IEnumerable<T> PNP<T>(IEnumerable<T> source, int from, int to, int step)
        {
            to = -to;
            var head = 0;
            DynamicBuffer<T> buffer;

            using (var enumerator = source.GetEnumerator())
            {
                for (var i = 0; i < from; ++i)
                    if (enumerator.MoveNext())
                        yield return enumerator.Current;
                    else
                        yield break;

                buffer = new DynamicBuffer<T>();
                buffer.BufferUpToCount(enumerator, to);
                if (buffer.length < to)
                {
                    for (var i = 0; i < buffer.length; ++i)
                        yield return buffer.items[i];
                    yield break;
                }

                if (step == 1)
                {
                    while (enumerator.MoveNext())
                    {
                        buffer.items[head] = enumerator.Current;
                        head = (head + 1) % to;
                    }
                }
                else
                {
                    bool continuation = true;
                    while (continuation)
                    {
                        if (!enumerator.MoveNext())
                            break;

                        buffer.items[head] = enumerator.Current;
                        head = (head + 1) % to;

                        for (var i = 1; i < step; ++i)
                        {
                            if (enumerator.MoveNext())
                            {
                                yield return buffer.items[head];
                                buffer.items[head] = enumerator.Current;
                                head = (head + 1) % to;
                            }
                            else
                            {
                                continuation = false;
                                break;
                            }
                        }
                    }
                }
            }

            for (var i = 0; i < to; ++i)
            {
                yield return buffer.items[head];
                head = (head + 1) % to;
            }
        }

        internal static IEnumerable<T> PNN<T>(IEnumerable<T> source, int from, int to, int step)
        {
            to = -to;
            step = -step;

            var fromCount = from + 1;
            var toCount = to - 1;

            using (var enumerator = source.GetEnumerator())
            {
                if (to == 1)
                {
                    while (enumerator.MoveNext())
                        yield return enumerator.Current;
                    
                    yield break;
                }

                var buffer = new DynamicBuffer<T>();
                var head = 0;

                buffer.BufferUpToCount(enumerator, toCount);

                if (fromCount > toCount)
                {
                    if (buffer.length < toCount)
                    {
                        if (step != 1)
                        {
                            int rem;
                            var iterations = Math.DivRem(buffer.length, step, out rem);

                            for (var b = rem - 1; head < b; ++head)
                                yield return buffer.items[head];

                            if (rem > 0)
                                ++head;

                            for (int i = 0, decStep = step - 1; i < iterations; ++i, ++head)
                                for (var b = head + decStep; head < b; ++head)
                                    yield return buffer.items[head];
                        }

                        yield break;
                    }

                    for (int i = 0, b = fromCount - toCount; i < b; ++i)
                        if (enumerator.MoveNext())
                        {
                            yield return buffer.items[head];
                            buffer.items[head] = enumerator.Current;
                            head = (head + 1) % toCount;
                        }
                        else
                        {
                            if (step != 1)
                            {
                                int rem;
                                var iterations = Math.DivRem(toCount, step, out rem);

                                for (var j = 1; j < rem; ++j)
                                {
                                    yield return buffer.items[head];
                                    head = (head + 1) % toCount;
                                }

                                if (rem > 0)
                                    head = (head + 1) % toCount;

                                for (var j = 0; j < iterations; ++j)
                                {
                                    for (var k = 1; k < step; ++k)
                                    {
                                        yield return buffer.items[head];
                                        head = (head + 1) % toCount;
                                    }
                                    head = (head + 1) % toCount;
                                }
                            }

                            yield break;
                        }

                    for (var i = 0; i < toCount; ++i)
                    {
                        if (enumerator.MoveNext())
                        {
                            yield return buffer.items[head];
                            buffer.items[head] = enumerator.Current;
                            head = (head + 1) % toCount;
                        }
                        else
                        {
                            if (step != 1)
                            {
                                var orgHead = head;
                                int rem;
                                var iterations = Math.DivRem(toCount - i, step, out rem);

                                for (var j = 1; j < rem; ++j)
                                {
                                    yield return buffer.items[head];
                                    head = (head + 1) % toCount;
                                }

                                if (rem > 0)
                                    head = (head + 1) % toCount;

                                for (var j = 0; j < iterations; ++j)
                                {
                                    for (var k = 1; k < step; ++k)
                                    {
                                        yield return buffer.items[head];
                                        head = (head + 1) % toCount;
                                    }
                                    head = (head + 1) % toCount;
                                }

                                head = orgHead;
                            }

                            head = (head - i + toCount) % toCount;

                            for (; i > 0; --i)
                            {
                                yield return buffer.items[head];
                                head = (head + 1) % toCount;
                            }

                            yield break;
                        }
                    }

                    for (var i = 0; i < toCount; ++i)
                    {
                        yield return buffer.items[head];
                        head = (head + 1) % toCount;
                    }

                    while (enumerator.MoveNext())
                        yield return enumerator.Current;
                }
                else
                {
                    if (buffer.length < toCount)
                    {
                        if (step != 1)
                        {
                            int rem;
                            var iterations = Math.DivRem(Math.Min(fromCount, buffer.length), step, out rem);

                            for (var b = rem - 1; head < b; ++head)
                                yield return buffer.items[head];

                            if (rem > 0)
                                ++head;

                            for (int i = 0, decStep = step - 1; i < iterations; ++i, ++head)
                                for (var b = head + decStep; head < b; ++head)
                                    yield return buffer.items[head];
                        }

                        for (var i = fromCount; i < buffer.length; ++i)
                            yield return buffer.items[i];
                            
                        yield break;
                    }

                    for (; head < fromCount; ++head)
                    {
                        if (enumerator.MoveNext())
                        {
                            yield return buffer.items[head];
                            buffer.items[head] = enumerator.Current;
                        }
                        else
                        {
                            var buffered = head;

                            if (step != 1)
                            {
                                int rem;
                                var iterations = Math.DivRem(fromCount - head, step, out rem);

                                for (var i = 1; i < rem; ++i)
                                {
                                    yield return buffer.items[head];
                                    head = (head + 1) % toCount;
                                }

                                if (rem > 0)
                                    head = (head + 1) % toCount;

                                for (var i = 0; i < iterations; ++i)
                                {
                                    for (var j = 1; j < step; ++j)
                                    {
                                        yield return buffer.items[head];
                                        head = (head + 1) % toCount;
                                    }
                                    head = (head + 1) % toCount;
                                }
                            }

                            for (head = fromCount; head < toCount; ++head)
                                yield return buffer.items[head];

                            for (head = 0; head < buffered; ++head)
                                yield return buffer.items[head];

                            yield break;
                        }
                    }

                    for (head = fromCount; head < toCount; ++head)
                        yield return buffer.items[head];

                    for (head = 0; head < fromCount; ++head)
                        yield return buffer.items[head];

                    while (enumerator.MoveNext())
                        yield return enumerator.Current;
                }
            }
        }

        internal static IEnumerable<T> NPP<T>(IEnumerable<T> source, int from, int? to, int step)
        {
            from = -from;
            var head = 0;

            using (var enumerator = source.GetEnumerator())
            {
                var buffer = new DynamicBuffer<T>();

                if (to.HasValue)
                {
                    var toValue = to.Value;

                    buffer.BufferUpToCount(enumerator, from);

                    if (buffer.length < from)
                    {
                        if (step > 1)
                        {
                            int rem;
                            var iterations = Math.DivRem(Math.Min(toValue, buffer.length), step, out rem);

                            for (int i = 0, decStep = step - 1; i < iterations; ++i)
                            {
                                ++head;
                                for (var b = head + decStep; head < b; ++head)
                                    yield return buffer.items[head];
                            }

                            ++head;

                            for (var b = head + rem - 1; head < b; ++head)
                                yield return buffer.items[head];
                        }

                        for (head = toValue; head < buffer.length; ++head)
                            yield return buffer.items[head];

                        yield break;
                    }

                    if (from > toValue)
                    {
                        for (var i = 0; i < toValue; ++i)
                            if (enumerator.MoveNext())
                            {
                                yield return buffer.items[i];
                                buffer.items[i] = enumerator.Current;
                            }
                            else
                            {
                                if (step > 1)
                                {
                                    int rem;
                                    var iterations = Math.DivRem(toValue - i, step, out rem);
                                    head = i;
                                    for (int j = 0, decStep = step - 1; j < iterations; ++j)
                                    {
                                        ++head;
                                        for (var b = head + decStep; head < b; ++head)
                                            yield return buffer.items[head];
                                    }

                                    ++head;

                                    for (var b = head + rem - 1; head < b; ++head)
                                        yield return buffer.items[head];
                                }

                                for (head = toValue; head < from; ++head)
                                    yield return buffer.items[head];

                                for (head = 0; head < i; ++head)
                                    yield return buffer.items[head];

                                yield break;
                            }

                        for (head = toValue; head < from; ++head)
                            yield return buffer.items[head];

                        for (head = 0; head < toValue; ++head)
                            yield return buffer.items[head];

                        while (enumerator.MoveNext())
                            yield return enumerator.Current;
                    }
                    else
                    {
                        for (var i = from; i < toValue; ++i)
                            if (enumerator.MoveNext())
                            {
                                yield return buffer.items[head];
                                buffer.items[head] = enumerator.Current;
                                head = (head + 1) % from;
                            }
                            else
                            {
                                if (step > 1)
                                {
                                    int rem;
                                    var iterations = Math.DivRem(from, step, out rem);

                                    for (var j = 0; j < iterations; ++j)
                                    {
                                        head = (head + 1) % from;
                                        for (int k = 1; k < step; ++k)
                                        {
                                            yield return buffer.items[head];
                                            head = (head + 1) % from;
                                        }
                                    }

                                    head = (head + 1) % from;

                                    for (var j = 1; j < rem; ++j)
                                    {
                                        yield return buffer.items[head];
                                        head = (head + 1) % from;
                                    }
                                }

                                yield break;
                            }

                        for (var i = 0; i < from; ++i)
                            if (enumerator.MoveNext())
                            {
                                yield return buffer.items[head];
                                buffer.items[head] = enumerator.Current;
                                head = (head + 1) % from;
                            }
                            else
                            {
                                if (step > 1)
                                {
                                    int rem;
                                    var iterations = Math.DivRem(from - i, step, out rem);
                                    var orgHead = head;

                                    for (var j = 0; j < iterations; ++j)
                                    {
                                        head = (head + 1) % from;
                                        for (int k = 1; k < step; ++k)
                                        {
                                            yield return buffer.items[head];
                                            head = (head + 1) % from;
                                        }
                                    }

                                    head = (head + 1) % from;

                                    for (var j = 1; j < rem; ++j)
                                    {
                                        yield return buffer.items[head];
                                        head = (head + 1) % from;
                                    }

                                    head = orgHead;
                                }

                                head = (head - i + from) % from;
                                for (; i > 0; --i)
                                {
                                    yield return buffer.items[head];
                                    head = (head + 1) % from;
                                }
                                yield break;
                            }

                        for (var i = 0; i < from; ++i)
                        {
                            yield return buffer.items[head];
                            head = (head + 1) % from;
                        }

                        while (enumerator.MoveNext())
                            yield return enumerator.Current;
                    }
                }
                else
                {
                    buffer.BufferUpToCount(enumerator, from);

                    if (step == 1)
                    {
                        if (buffer.length < from)
                            yield break;

                        while (enumerator.MoveNext())
                        {
                            yield return buffer.items[head];
                            buffer.items[head] = enumerator.Current;
                            head = (head + 1) % from;
                        }
                    }
                    else
                    {
                        int rem, iterations;

                        if (buffer.length < from)
                        {
                            iterations = Math.DivRem(buffer.length, step, out rem);

                            for (int i = 0, decStep = step - 1; i < iterations; ++i)
                            {
                                ++head;
                                for (var b = head + decStep; head < b; ++head)
                                    yield return buffer.items[head];
                            }

                            ++head;

                            for (var b = head + rem - 1; head < b; ++head)
                                yield return buffer.items[head];
                        }
                        else
                        {
                            while (enumerator.MoveNext())
                            {
                                yield return buffer.items[head];
                                buffer.items[head] = enumerator.Current;
                                head = (head + 1) % from;
                            }

                            iterations = Math.DivRem(buffer.length, step, out rem);

                            for (var i = 0; i < iterations; ++i)
                            {
                                head = (head + 1) % from;
                                for (var j = 1; j < step; ++j)
                                {
                                    yield return buffer.items[head];
                                    head = (head + 1) % from;
                                }
                            }

                            head = (head + 1) % from;

                            for (var i = 1; i < rem; ++i)
                            {
                                yield return buffer.items[head];
                                head = (head + 1) % from;
                            }
                        }
                    }
                }
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
                    for (var i = 0; i <= toValue; ++i)
                        if (enumerator.MoveNext())
                            yield return enumerator.Current;
                        else
                            yield break;
                }

                buffer.Buffer(enumerator);
            }

            from = -from;
            step = -step;

            var sliceLength = Math.Max(buffer.length - from + 1, 0);

            if (step != 1)
            {
                int rem;
                var iterations = Math.DivRem(sliceLength, step, out rem);

                var head = 0;
                for (var b = rem - 1; head < b; ++head)
                    yield return buffer.items[head];

                if (rem > 0)
                    ++head;

                for (int i = 0, decStep = step - 1; i < iterations; ++i, ++head)
                    for (var b = head + decStep; head < b; ++head)
                        yield return buffer.items[head];
            }

            for (var i = sliceLength; i < buffer.length; ++i)
                yield return buffer.items[i];
        }
    }
}
