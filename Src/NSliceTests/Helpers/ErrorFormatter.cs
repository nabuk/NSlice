using System.Collections.Generic;
using System.Linq;

namespace NSliceTests.Helpers
{
    public static class ErrorFormatter
    {
        private static string RenderElement<T>(T item)
        {
            return item == null ? "null" : item.ToString();
        }

        private static string RendertCollection<T>(IEnumerable<T> source)
        {
            return string.Format("[{0}]", string.Join(", ", source.Select(RenderElement)));
        }

        public static string FormatSliceResultError<T>(
            IEnumerable<T> source,
            int? from,
            int? to,
            int step,
            IEnumerable<T> expected,
            IEnumerable<T> got)
        {
            return string.Format(
                "For {0}.Slice({1}, {2}, {3}) got {4}, expected {5}",
                RendertCollection(source),
                RenderElement(from),
                RenderElement(to),
                RenderElement(step),
                RendertCollection(got),
                RendertCollection(expected));
        }

        public static string FormatSliceDeleteResultError<T>(
            IEnumerable<T> source,
            int? from,
            int? to,
            int step,
            IEnumerable<T> expected,
            IEnumerable<T> got)
        {
            return string.Format(
                "For {0}.SliceDelete({1}, {2}, {3}) got {4}, expected {5}",
                RendertCollection(source),
                RenderElement(from),
                RenderElement(to),
                RenderElement(step),
                RendertCollection(got),
                RendertCollection(expected));
        }

        public static string FormatSliceDisposeError(
            int? from,
            int? to,
            int step,
            int length,
            int disposeCallCount)
        {
            return string.Format("For [collection of length = {0}].Slice({1}, {2}, {3}) Dispose has been called {4} time(s).",
                RenderElement(length),
                RenderElement(from),
                RenderElement(to),
                RenderElement(step),
                RenderElement(disposeCallCount));
        }

        public static string FormatSliceDeleteDisposeError(
            int? from,
            int? to,
            int step,
            int length,
            int disposeCallCount)
        {
            return string.Format("For [collection of length = {0}].SliceDelete({1}, {2}, {3}) Dispose has been called {4} time(s).",
                RenderElement(length),
                RenderElement(from),
                RenderElement(to),
                RenderElement(step),
                RenderElement(disposeCallCount));
        }

        public static string FormatSliceResetError(
            int? from,
            int? to,
            int step,
            int length,
            int resetCallCount)
        {
            return string.Format("For [collection of length = {0}].Slice({1}, {2}, {3}) Reset has been called {4} time(s).",
                RenderElement(length),
                RenderElement(from),
                RenderElement(to),
                RenderElement(step),
                RenderElement(resetCallCount));
        }

        public static string FormatSliceDeleteResetError(
            int? from,
            int? to,
            int step,
            int length,
            int resetCallCount)
        {
            return string.Format("For [collection of length = {0}].SliceDelete({1}, {2}, {3}) Reset has been called {4} time(s).",
                RenderElement(length),
                RenderElement(from),
                RenderElement(to),
                RenderElement(step),
                RenderElement(resetCallCount));
        }
    }
}
