using System;
using System.Linq;
using NSlice;
using NSliceTests.TestData;
using Xunit;
using Xunit.Extensions;

namespace NSliceTests.Tests.EnumerableTests
{
    public class EnumerableAtTests
    {
        [Theory, ClassData(typeof(AtOrDefaultTestCaseSource))]
        public void At_FromEnumerableExtensions_ReturnsCorrectValues(int index, int length)
        {
            var source = Enumerable.Range(0, length);
            var sut = EnumerableExtensions.At(source, index);
            if (index < 0)
                index = length + index;

            Assert.Equal(index, sut);
        }

        [Fact]
        public void At_FromEnumerableExtensions_GivenNullSource_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => EnumerableExtensions.At<int>(null, 0));
        }

        [Theory]
        [InlineData(10, 6)]
        [InlineData(-10, 6)]
        public void At_FromEnumerableExtensions_GivenIndexOurOfRange_ThrowsIndexOurOfRangeException(int index, int length)
        {
            var source = Enumerable.Range(0, length);
            Assert.Throws<IndexOutOfRangeException>(() => EnumerableExtensions.At(source, index));
        }
    }
}
