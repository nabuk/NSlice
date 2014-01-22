using NSlice;
using NSliceTests.Helpers;
using NSliceTests.TestData;
using NSliceTests.Tests.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Extensions;

namespace NSliceTests.Tests.IndexedTests
{
    public class IndexedSliceDeleteTests : BaseSliceCaseTests
    {
        [Fact]
        public void SliceDelete_FromIndexedExtensions_ReturnsCorrectValues()
        {
            this.RunSliceTestCases((from, to, step, length) =>
            {
                var source = Enumerable.Range(0, length).ToArray();
                var sut = IndexedExtensions.SliceDelete(source, from, to, step).ToArray();
                var expected = SliceDeleteExpectedResultCalculator.Calculate(from, to, step, length);

                Assert.True(
                    expected.SequenceEqual(sut),
                    ErrorFormatter.FormatSliceDeleteResultError(source, from, to, step, expected, sut));
            });
        }

        [Fact]
        public void SliceDelete_FromIndexedExtensions_GivenStepZero_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => IndexedExtensions.SliceDelete(Enumerable.Empty<int>().ToArray(), step: 0));
        }

        [Fact]
        public void SliceDelete_FromIndexedExtensions_GivenNullSource_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => IndexedExtensions.SliceDelete<int>(null));
        }
    }
}
