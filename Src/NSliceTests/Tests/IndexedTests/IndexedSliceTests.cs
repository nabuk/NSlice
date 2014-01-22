using System;
using System.Linq;
using NSlice;
using NSliceTests.Helpers;
using NSliceTests.TestData;
using Xunit;
using Xunit.Extensions;
using NSliceTests.Tests.Base;

namespace NSliceTests.Tests.IndexedTests
{
    public class IndexedSliceTests : BaseSliceCaseTests
    {
        [Fact]
        public void Slice_FromIndexedExtensions_ReturnsCorrectValues()
        {
            this.RunSliceTestCases((from, to, step, length) =>
            {
                var source = Enumerable.Range(0, length).ToArray();
                var sut = IndexedExtensions.Slice(source, from, to, step).ToArray();
                var expected = SliceExpectedResultCalculator.Calculate(from, to, step, length);

                Assert.True(
                    expected.SequenceEqual(sut),
                    ErrorFormatter.FormatSliceResultError(source, from, to, step, expected, sut));
            });
        }

        [Fact]
        public void Slice_FromIndexedExtensions_GivenStepZero_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => IndexedExtensions.Slice(Enumerable.Empty<int>().ToArray(), step: 0));
        }

        [Fact]
        public void Slice_FromIndexedExtensions_GivenNullSource_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => IndexedExtensions.Slice<int>(null));
        }
    }
}