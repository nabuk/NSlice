using System;
using System.Linq;
using NSlice;
using NSliceTests.TestData;
using Xunit;
using Xunit.Extensions;

namespace NSliceTests.Tests.IndexedTests
{
    public class IndexedAtOrDefaultTests
    {
        [Theory, ClassData(typeof(AtOrDefaultTestCaseSource))]
        public void AtOrDefault_FromIndexedExtensions_ReturnsCorrectValues(int index, int length)
        {
            var source = Enumerable.Range(0, length).ToArray();
            var sut = IndexedExtensions.AtOrDefault(source, index);
            if (index < 0)
                index = length + index;

            Assert.Equal(index, sut);
        }

        [Fact]
        public void AtOrDefault_FromIndexedExtensions_GivenNullSource_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => IndexedExtensions.AtOrDefault<int>(null, 0));
        }

        [Theory]
        [InlineData(10, 6)]
        [InlineData(-10, 6)]
        public void AtOrDefault_FromIndexedExtensions_GivenIndexOurOfRange_ReturnsDefaultValue_WhenExecutedOnValueCollection(int index, int length)
        {
            var source = Enumerable.Range(0, length).ToArray();
            Assert.Equal(default(int), IndexedExtensions.AtOrDefault(source, index));
        }

        [Theory]
        [InlineData(10, 6)]
        [InlineData(-10, 6)]
        public void AtOrDefault_FromIndexedExtensions_GivenIndexOurOfRange_ReturnsNull_WhenExecutedOnReferenceCollection(int index, int collectionLength)
        {
            var source = Enumerable.Range(0, collectionLength).Select(x => x.ToString()).ToArray();
            Assert.Equal(null, IndexedExtensions.AtOrDefault(source, index));
        }
    }
}
