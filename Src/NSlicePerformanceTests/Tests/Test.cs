using System;
using NSlicePerformanceTests.Clock;

namespace NSlicePerformanceTests.Tests
{
    class Test : ITest
    {
        private readonly string _name;
        private readonly string _code;
        private readonly Action _actionToTest;
        private double? _executionTime;

        private const int IterationsPerSampleDefault = 1;
        private const int SampleCountDefault = 5;

        public Test(
            string name,
            string code,
            Action actionToTest)
        {
            _name = name;
            _code = code;
            _actionToTest = actionToTest;
        }

        public string Name
        {
            get { return _name; }
        }

        public string Code
        {
            get { return _code; }
        }

        public double ExecutionTime
        {
            get { return _executionTime ?? (_executionTime = Performance.BenchmarkTime(_actionToTest, IterationsPerSample, SampleCount)).Value; }
        }

        private int _iterationsPerSample;
        public int IterationsPerSample
        {
            get
            {
                return _iterationsPerSample == default(int)
                           ? IterationsPerSampleDefault
                           : _iterationsPerSample;
            }
            set { _iterationsPerSample = value; }
        }

        private int _sampleCount;
        public int SampleCount
        {
            get
            {
                return _sampleCount == default(int)
                           ? SampleCountDefault
                           : _sampleCount;
            }
            set { _sampleCount = value; }
        }
    }
}
