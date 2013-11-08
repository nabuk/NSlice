using System;
using System.Linq;
using NSlice;
using NSliceTests.TestData;
using Xunit;
using Xunit.Extensions;

namespace NSliceTests.Tests.StringTests
{
    public class StringAtOrDefaultTests
    {
        [Theory, ClassData(typeof(AtOrDefaultTestCaseSource))]
        public void AtOrDefault_FromStringExtensions_ReturnsCorrectValues(int index, int length)
        {
            var source = new string(Enumerable.Range(0, length).Select(i => (char) ('a' + i)).ToArray());
            var sut = StringExtensions.AtOrDefault(source, index);
            if (index < 0)
                index = length + index;

            Assert.Equal((char)('a' + index), sut);
        }

        [Fact]
        public void AtOrDefault_FromStringExtensions_GivenNullSource_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => StringExtensions.AtOrDefault(null, 0));
        }

        [Theory]
        [InlineData(10, 6)]
        [InlineData(-10, 6)]
        public void AtOrDefault_FromStringExtensions_GivenIndexOurOfRange_ReturnsDefaultValue(int index, int length)
        {
            var source = new string(Enumerable.Range(0, length).Select(i => (char)('a' + i)).ToArray());
            Assert.Null(StringExtensions.AtOrDefault(source, index));
        }
    }
}
