using NSlice;
using NSliceTests.Helpers;
using NSliceTests.TestData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Extensions;

namespace NSliceTests.Tests.EnumerableTests
{
    public class EnumerableSliceDeleteTests
    {
        [Theory, ClassData(typeof(SliceTestCaseSource))]
        public void SliceDelete_FromEnumerableExtensions_ReturnsCorrectValues(int? from, int? to, int step, int length)
        {
            var source = Enumerable.Range(0, length);
            var sourceArray = source.ToArray();
            var sut = EnumerableExtensions.SliceDelete(source, from, to, step).ToArray();
            var expected = SliceDeleteExpectedResultCalculator.Calculate(from, to, step, length);

            Assert.True(
                expected.SequenceEqual(sut),
                ErrorFormatter.FormatSliceResultError(sourceArray, from, to, step, expected, sut));
        }

        [Fact]
        public void SliceDelete_FromEnumerableExtensions_GivenStepZero_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => EnumerableExtensions.SliceDelete(Enumerable.Empty<int>(), step: 0));
        }

        [Fact]
        public void SliceDelete_FromEnumerableExtensions_GivenNullSource_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => EnumerableExtensions.SliceDelete<int>(null));
        }
    }
}
