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
            var sut = EnumerableExtensions.SliceDelete(source, from, to, step).ToArray();
            var expected = SliceDeleteExpectedResultCalculator.Calculate(from, to, step, length);

            LazyAssert.True(
                expected.SequenceEqual(sut),
                () => ErrorFormatter.FormatSliceDeleteResultError(source, from, to, step, expected, sut));
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

        [Theory, ClassData(typeof(SliceTestCaseSource))]
        public void Slice_FromEnumerableExtensions_GivenListAsSource_ReturnsCorrectValues(int? from, int? to, int step, int length)
        {
            var source = Enumerable.Range(0, length).ToArray();
            var sut = EnumerableExtensions.SliceDelete(source, from, to, step).ToArray();
            var expected = SliceDeleteExpectedResultCalculator.Calculate(from, to, step, length);

            LazyAssert.True(
                expected.SequenceEqual(sut),
                () => ErrorFormatter.FormatSliceDeleteResultError(source, from, to, step, expected, sut));
        }

        [Theory, ClassData(typeof(SliceTestCaseSource))]
        public void Slice_FromEnumerableExtensions_GivenCollectionAsSource_ReturnsCorrectValues(int? from, int? to, int step, int length)
        {
            var source = new Queue<int>(Enumerable.Range(0, length));
            var sut = EnumerableExtensions.SliceDelete(source, from, to, step).ToArray();
            var expected = SliceDeleteExpectedResultCalculator.Calculate(from, to, step, length);

            LazyAssert.True(
                expected.SequenceEqual(sut),
                () => ErrorFormatter.FormatSliceDeleteResultError(source, from, to, step, expected, sut));
        }
    }
}
