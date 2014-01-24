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
    public class StringSliceDeleteTests : BaseSliceCaseTests
    {
        [Fact]
        public void SliceDelete_FromStringExtensions_ReturnsCorrectValues()
        {
            this.RunSliceTestCases((from, to, step, length) =>
            {
                var source = new string(Enumerable.Range(0, length).Select(i => (char)('a' + i)).ToArray());
                var sut = StringExtensions.SliceDelete(source, from, to, step);
                var expectedIndexList = SliceDeleteExpectedResultCalculator.Calculate(from, to, step, length);
                var expected = new string(expectedIndexList.Select(i => (char)('a' + i)).ToArray());

                Assert.Equal(expected, sut);
            });
        }

        [Fact]
        public void SliceDelete_FromStringExtensions_GivenStepZero_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => StringExtensions.SliceDelete(string.Empty, step: 0));
        }

        [Fact]
        public void SliceDelete_FromStringExtensions_GivenNullSource_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => StringExtensions.SliceDelete(null));
        }
    }
}