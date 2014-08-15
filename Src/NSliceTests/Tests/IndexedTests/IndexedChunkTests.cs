using NSlice;
using NSliceTests.Helpers;
using NSliceTests.Tests.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Extensions;

namespace NSliceTests.Tests.IndexedTests
{
    public class IndexedChunkTests : BaseChunkCaseTests
    {
        [Fact]
        public void Chunk_FromIndexedExtensions_GivenNullSource_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => IndexedExtensions.Chunk<int>(null, 5));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-2)]
        public void Chunk_FromIndexedExtensions_ThrowsArgumentException(int chunkSize)
        {
            Assert.Throws<ArgumentException>(() => IndexedExtensions.Chunk<int>(Enumerable.Empty<int>().ToArray(), chunkSize));
        }

        [Fact]
        public void Chunk_FromIndexedExtensions_ReturnsCorrectValues()
        {
            this.RunChunkTestCases((count, chunkSize) =>
            {
                var source = Enumerable.Range(0, count).ToArray();
                var sut = IndexedExtensions.Chunk(source, chunkSize);

                LazyAssert.True(
                    CheckSut(sut, count, chunkSize),
                    () => string.Format(chunkResultErrorFormat, count, chunkSize));
            });
        }
    }
}
