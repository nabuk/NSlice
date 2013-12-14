using System;
using System.Linq;
using NSlice;
using NSliceTests.TestData;
using Xunit;
using Xunit.Extensions;

namespace NSliceTests.Tests.IndexedTests
{
    public class IndexedAtTests
    {
        [Theory, ClassData(typeof(AtOrDefaultTestCaseSource))]
        public void At_FromIndexedExtensions_ReturnsCorrectValues(int index, int length)
        {
            var source = Enumerable.Range(0, length).ToArray();
            var sut = IndexedExtensions.At(source, index);
            if (index < 0)
                index = length + index;

            Assert.Equal(index, sut);
        }

        [Fact]
        public void At_FromIndexedExtensions_GivenNullSource_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => IndexedExtensions.At<int>(null, 0));
        }

        [Theory]
        [InlineData(10, 6)]
        [InlineData(-10, 6)]
        public void At_FromIndexedExtensions_GivenIndexOurOfRange_ThrowsIndexOurOfRangeException(int index, int length)
        {
            var source = Enumerable.Range(0, length).ToArray();
            Assert.Throws<IndexOutOfRangeException>(() => IndexedExtensions.At(source, index));
        }
    }
}
