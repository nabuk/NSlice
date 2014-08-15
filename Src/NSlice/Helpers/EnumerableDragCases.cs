using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSlice.Helpers
{
    static class EnumerableDragCases
    {
        internal static IEnumerable<DoubleItem<T>> Drag<T>(this IEnumerable<T> source)
        {
            using (var enumerator = source.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                    yield break;

                T prev = enumerator.Current;
                T curr;

                while (enumerator.MoveNext())
                {
                    curr = enumerator.Current;
                    yield return new DoubleItem<T>(prev, curr);
                    prev = curr;
                }
            }
        }

        internal static IEnumerable<IList<T>> Drag<T>(this IEnumerable<T> source, int numberOfItemsToDrag)
        {
            using (var enumerator = source.GetEnumerator())
            {
                if (numberOfItemsToDrag == 1)
                    while (enumerator.MoveNext())
                        yield return new[] { enumerator.Current };
                else
                {
                    if (!enumerator.MoveNext())
                        yield break;

                    var dragMinusOne = numberOfItemsToDrag - 1;
                    var bufferSize = Math.Max(numberOfItemsToDrag * 2, 1024);
                    var buffer = new T[bufferSize];
                    buffer[0] = enumerator.Current;

                    for (int i = 1; i < dragMinusOne; ++i)
                    {
                        if (!enumerator.MoveNext())
                            yield break;

                        buffer[i] = enumerator.Current;
                    }

                    if (!enumerator.MoveNext())
                        yield break;
                    buffer[dragMinusOne] = enumerator.Current;
                    yield return ProxiedListCreator.GetSegment(buffer, 0, numberOfItemsToDrag);

                    while (true)
                    {
                        for (int i = 1; i < bufferSize - numberOfItemsToDrag; ++i)
                        {
                            if (!enumerator.MoveNext())
                                yield break;
                            buffer[dragMinusOne + i] = enumerator.Current;
                            yield return ProxiedListCreator.GetSegment(buffer, i, numberOfItemsToDrag);
                        }

                        if (!enumerator.MoveNext())
                            yield break;

                        var newBuffer = new T[bufferSize];
                        Array.Copy(buffer, bufferSize - dragMinusOne, newBuffer, 0, dragMinusOne);
                        buffer = newBuffer;
                        buffer[dragMinusOne] = enumerator.Current;
                        yield return ProxiedListCreator.GetSegment(buffer, 0, numberOfItemsToDrag);
                    }
                }
            }
        }
    }
}
