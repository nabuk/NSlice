using System;
using System.Linq;
using NSlice;
using NSliceTests.TestData;
using Xunit;
using Xunit.Extensions;

namespace NSliceTests.Tests.StringTests
{
    public class StringAtTests
    {
        [Theory, ClassData(typeof(AtOrDefaultTestCaseSource))]
        public void At_FromStringExtensions_ReturnsCorrectValues(int index, int length)
        {
            var source = new string(Enumerable.Range(0, length).Select(i => (char)('a' + i)).ToArray());
            var sut = StringExtensions.At(source, index);
            if (index < 0)
                index = length + index;

            Assert.Equal((char)('a' + index), sut);
        }

        [Fact]
        public void At_FromStringExtensions_GivenNullSource_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => StringExtensions.At(null, 0));
        }

        [Theory]
        [InlineData(10, 6)]
        [InlineData(-10, 6)]
        public void At_FromStringExtensions_GivenIndexOurOfRange_ThrowsIndexOurOfRangeException(int index, int length)
        {
            var source = new string(Enumerable.Range(0, length).Select(i => (char)('a' + i)).ToArray());
            Assert.Throws<IndexOutOfRangeException>(() => StringExtensions.At(source, index));
        }
    }
}
