using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSlicePerformanceTests.Tests
{
    interface ITestCase
    {
        string Name { get; }
        ITest Test { get; }
        ITest Benchmark { get; }
    }
}
