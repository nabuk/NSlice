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
            int? step,
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
    }
}
