using System;
using System.Collections.Generic;
using System.Linq;
using NSlice;
using NSliceTests.Helpers;
using NSliceTests.TestData;
using Xunit;
using Xunit.Extensions;
using NSliceTests.Tests.Base;

namespace NSliceTests.Tests.EnumerableTests
{
    public class EnumerableSliceTests : BaseSliceCaseTests
    {
        [Fact]
        public void Slice_FromEnumerableExtensions_ReturnsCorrectValues()
        {
            this.RunSliceTestCases((from, to, step, length) =>
            {
                var source = Enumerable.Range(0, length);
                var sut = EnumerableExtensions.Slice(source, from, to, step).ToArray();
                var expected = SliceExpectedResultCalculator.Calculate(from, to, step, length);

                LazyAssert.True(
                    expected.SequenceEqual(sut),
                    () => ErrorFormatter.FormatSliceResultError(source, from, to, step, expected, sut));
            });
        }

        [Fact]
        public void Slice_FromEnumerableExtensions_GivenStepZero_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => EnumerableExtensions.Slice(Enumerable.Empty<int>(), step: 0));
        }

        [Fact]
        public void Slice_FromEnumerableExtensions_GivenNullSource_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => EnumerableExtensions.Slice<int>(null));
        }


        [Fact]
        public void Slice_FromEnumerableExtensions_GivenListAsSource_ReturnsCorrectValues()
        {
            this.RunSliceTestCases((from, to, step, length) =>
            {
                var source = Enumerable.Range(0, length).ToArray();
                var sut = EnumerableExtensions.Slice(source, from, to, step).ToArray();
                var expected = SliceExpectedResultCalculator.Calculate(from, to, step, length);

                LazyAssert.True(
                    expected.SequenceEqual(sut),
                    () => ErrorFormatter.FormatSliceResultError(source, from, to, step, expected, sut));
            });
        }

        [Fact]
        public void Slice_FromEnumerableExtensions_GivenCollectionAsSource_ReturnsCorrectValues()
        {
            this.RunSliceTestCases((from, to, step, length) =>
            {
                var source = new Queue<int>(Enumerable.Range(0, length));
                var sut = EnumerableExtensions.Slice(source, from, to, step).ToArray();
                var expected = SliceExpectedResultCalculator.Calculate(from, to, step, length);

                LazyAssert.True(
                    expected.SequenceEqual(sut),
                    () => ErrorFormatter.FormatSliceResultError(source, from, to, step, expected, sut));
            });
        }
    }
}