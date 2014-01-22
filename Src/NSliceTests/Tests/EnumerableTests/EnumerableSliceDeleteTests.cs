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

namespace NSliceTests.Tests.EnumerableTests
{
    public class EnumerableSliceDeleteTests : BaseSliceCaseTests
    {
        [Fact]
        public void SliceDelete_FromEnumerableExtensions_ReturnsCorrectValues()
        {
            this.RunSliceTestCases((from, to, step, length) =>
            {
                var source = Enumerable.Range(0, length);
                var sut = EnumerableExtensions.SliceDelete(source, from, to, step).ToArray();
                var expected = SliceDeleteExpectedResultCalculator.Calculate(from, to, step, length);

                LazyAssert.True(
                    expected.SequenceEqual(sut),
                    () => ErrorFormatter.FormatSliceDeleteResultError(source, from, to, step, expected, sut));
            });
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

        [Fact]
        public void Slice_FromEnumerableExtensions_GivenListAsSource_ReturnsCorrectValues()
        {
            this.RunSliceTestCases((from, to, step, length) =>
            {
                var source = Enumerable.Range(0, length).ToArray();
                var sut = EnumerableExtensions.SliceDelete(source, from, to, step).ToArray();
                var expected = SliceDeleteExpectedResultCalculator.Calculate(from, to, step, length);

                LazyAssert.True(
                    expected.SequenceEqual(sut),
                    () => ErrorFormatter.FormatSliceDeleteResultError(source, from, to, step, expected, sut));
            });
        }

        [Fact]
        public void Slice_FromEnumerableExtensions_GivenCollectionAsSource_ReturnsCorrectValues()
        {
            this.RunSliceTestCases((from, to, step, length) =>
            {
                var source = new Queue<int>(Enumerable.Range(0, length));
                var sut = EnumerableExtensions.SliceDelete(source, from, to, step).ToArray();
                var expected = SliceDeleteExpectedResultCalculator.Calculate(from, to, step, length);

                LazyAssert.True(
                    expected.SequenceEqual(sut),
                    () => ErrorFormatter.FormatSliceDeleteResultError(source, from, to, step, expected, sut));
            });
        }
    }
}
