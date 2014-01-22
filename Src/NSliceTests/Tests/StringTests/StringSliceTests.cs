using System;
using System.Linq;
using NSlice;
using NSliceTests.Helpers;
using NSliceTests.TestData;
using Xunit;
using Xunit.Extensions;
using NSliceTests.Tests.Base;

namespace NSliceTests.Tests.StringTests
{
    public class StringSliceTests : BaseSliceCaseTests
    {
        [Fact]
        public void Slice_FromStringExtensions_ReturnsCorrectValues()
        {
            this.RunSliceTestCases((from, to, step, length) =>
            {
                var source = new string(Enumerable.Range(0, length).Select(i => (char)('a' + i)).ToArray());
                var sut = StringExtensions.Slice(source, from, to, step);
                var expectedIndexList = SliceExpectedResultCalculator.Calculate(from, to, step, length);
                var expected = new string(expectedIndexList.Select(i => (char)('a' + i)).ToArray());

                Assert.Equal(expected, sut);
            });
        }

        [Fact]
        public void Slice_FromStringExtensions_GivenStepZero_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => StringExtensions.Slice(string.Empty, step: 0));
        }

        [Fact]
        public void Slice_FromStringExtensions_GivenNullSource_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => StringExtensions.Slice(null));
        }
    }
}