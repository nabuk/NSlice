using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NSliceTests.Helpers
{
    public static class ErrorFormatter
    {
        public static string RenderElement(object item)
        {
            if (item == null)
                return "null";
            else if (item is IEnumerable)
                return string.Format("[{0}]", string.Join(", ", ((IEnumerable)item).Cast<object>().Select(RenderElement)));
            else
                return item.ToString();
        }

        public static object[] RenderElements(params object[] args)
        {
            return args.Select(RenderElement).ToArray();
        }

        public static string Format(string format, params object[] args)
        {
            return string.Format(format, RenderElements(args));
        }
    }
}
