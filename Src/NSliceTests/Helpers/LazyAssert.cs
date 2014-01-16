using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit.Sdk;

namespace NSliceTests.Helpers
{
    public static class LazyAssert
    {
        public static void True(bool condition, Func<string> userMessageCallback)
        {
            if (!condition)
                throw new TrueException(userMessageCallback());
        }
    }
}
