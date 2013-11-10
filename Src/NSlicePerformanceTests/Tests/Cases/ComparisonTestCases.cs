using System;
using System.Collections.Generic;
using System.Linq;
using NSlice;

namespace NSlicePerformanceTests.Tests.Cases
{
    class ComparisonTestCases
    {
        private readonly TestSettings _testSettings;
        private readonly IEnumerable<int> _collection;

        public ComparisonTestCases(TestSettings testSettings)
        {
            _testSettings = testSettings;
            _collection = Enumerable.Range(0, _testSettings.TestCollectionSize);
        }

        private ITestCase PrepareTest(
            string name,
            string testCode,
            Action testAction,
            string benchmarkTestCode,
            Action benchmarkTestAction)
        {
            return new TestCase(
                name,
                new Test(string.Empty, testCode, testAction)
                {
                    IterationsPerSample = _testSettings.IterationsPerSample,
                    SampleCount = _testSettings.SampleCount
                },
                new Test(string.Empty, benchmarkTestCode, benchmarkTestAction)
                {
                    IterationsPerSample = _testSettings.IterationsPerSample,
                    SampleCount = _testSettings.SampleCount
                });
        }

        public ITestCase GetItemsAtOddIndices
        {
            get
            {
                return PrepareTest(
                    "Get items at odd indices",
                    ".Slice(1, step: 2).Last()", () => _collection.Slice(1, step: 2).Last(),
                    ".Where((x, i) => i % 2 == 1).Last()", () => _collection.Where((x, i) => i % 2 == 1).Last());
            }
        }

        public ITestCase Reverse
        {
            get
            {
                return PrepareTest(
                    "Reverse",
                    ".Slice(step: -1).First()", () => _collection.Slice(step: -1).First(),
                    ".Reverse().First()", () => _collection.Reverse().First());
            }
        }

        public ITestCase ReverseAndEvenIndices
        {
            get
            {
                return PrepareTest(
                    "Reverse and get at even indices",
                    ".Slice(step: -2).Last()", () => _collection.Slice(step: -2).Last(),
                    ".Reverse().Where((x, i) => i % 2 == 0).Last()", () => _collection.Reverse().Where((x, i) => i % 2 == 0).Last());
            }
        }

        public ITestCase MiddleTerce
        {
            get
            {
                int terce = _testSettings.TestCollectionSize / 3;
                return PrepareTest(
                    "Get middle terce",
                    ".Slice(terce, terce*2).Last()", () => _collection.Slice(terce, terce * 2).Last(),
                    ".Skip(terce).Take(terce).Last()", () => _collection.Skip(terce).Take(terce).Last());
            }
        }

        public ITestCase MiddleTerceReversed
        {
            get
            {
                int terce = _testSettings.TestCollectionSize / 3;
                return PrepareTest(
                    "Get middle terce reversed",
                    ".Slice(-terce, terce, -1).First()", () => _collection.Slice(-terce, terce, -1).First(),
                    ".Skip(terce).Take(terce).Reverse().First()", () => _collection.Skip(terce).Take(terce).Reverse().First());
            }
        }
    }
}
