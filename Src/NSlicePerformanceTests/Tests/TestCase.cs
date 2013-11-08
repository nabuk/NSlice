namespace NSlicePerformanceTests.Tests
{
    class TestCase : ITestCase
    {
        private readonly string _name;
        private readonly ITest _test;
        private readonly ITest _benchmark;

        public TestCase(string name, ITest test, ITest benchmark = null)
        {
            _name = name;
            _test = test;
            _benchmark = benchmark;
        }

        public string Name
        {
            get { return _name; }
        }

        public ITest Test
        {
            get { return _test; }
        }

        public ITest Benchmark
        {
            get { return _benchmark; }
        }
    }
}
