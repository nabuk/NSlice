using System;
using System.Linq;
using NSlice;
using NSliceTests.TestData;
using Xunit;
using Xunit.Extensions;

namespace NSliceTests.Tests.EnumerableTests
{
    public class EnumerableAtOrDefaultTests
    {
        [Theory, ClassData(typeof(AtOrDefaultTestCaseSource))]
        public void AtOrDefault_FromEnumerableExtensions_ReturnsCorrectValues(int index, int length)
        {
            var source = Enumerable.Range(0, length);
            var sut = EnumerableExtensions.AtOrDefault(source, index);
            if (index < 0)
                index = length + index;

            Assert.Equal(index, sut);
        }

        [Fact]
        public void AtOrDefault_FromEnumerableExtensions_GivenNullSource_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => EnumerableExtensions.AtOrDefault<int>(null, 0));
        }

        [Theory]
        [InlineData(10, 6)]
        [InlineData(-10, 6)]
        public void AtOrDefault_FromEnumerableExtensions_GivenIndexOurOfRange_ReturnsDefaultValue_WhenExecutedOnValueCollection(int index, int length)
        {
            var source = Enumerable.Range(0, length);
            Assert.Equal(default(int), EnumerableExtensions.AtOrDefault(source, index));
        }

        [Theory]
        [InlineData(10, 6)]
        [InlineData(-10, 6)]
        public void AtOrDefault_FromEnumerableExtensions_GivenIndexOurOfRange_ReturnsNull_WhenExecutedOnReferenceCollection(int index, int collectionLength)
        {
            var source = Enumerable.Range(0, collectionLength).Select(x => x.ToString());
            Assert.Equal(null, EnumerableExtensions.AtOrDefault(source, index));
        }
    }
}
