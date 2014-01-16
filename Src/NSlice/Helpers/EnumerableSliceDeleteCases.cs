﻿using System;
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
            throw new NotImplementedException();
        }

        internal static IEnumerable<T> NNN<T>(IEnumerable<T> source, int from, int to, int step)
        {
            throw new NotImplementedException();
        }

        internal static IEnumerable<T> PNP<T>(IEnumerable<T> source, int from, int to, int step)
        {
            throw new NotImplementedException();
        }

        internal static IEnumerable<T> PNN<T>(IEnumerable<T> source, int from, int to, int step)
        {
            throw new NotImplementedException();
        }

        internal static IEnumerable<T> NPP<T>(IEnumerable<T> source, int from, int? to, int step)
        {
            throw new NotImplementedException();
        }

        internal static IEnumerable<T> NPN<T>(IEnumerable<T> source, int from, int? to, int step)
        {
            throw new NotImplementedException();
        }
    }
}
