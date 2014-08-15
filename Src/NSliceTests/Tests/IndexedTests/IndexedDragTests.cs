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
    public class IndexedDragTests : BaseDragCaseTests
    {
        [Fact]
        public void Drag_NoArgumentVersion_FromIndexedExtensions_GivenNullSource_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => IndexedExtensions.Drag<int>(null));
        }

        [Fact]
        public void Drag_NoArgumentVersion_FromIndexedExtensions_ReturnsCorrectValues()
        {
            this.RunNoArgumentDragTestCases((count, numberOfItemsToDrag) =>
            {
                var source = Enumerable.Range(0, count).ToArray();
                var sut = IndexedExtensions.Drag(source);

                LazyAssert.True(
                    CheckSut(sut.Select(x => new[] { x.First, x.Second }), count, numberOfItemsToDrag),
                    () => string.Format(dragNoArgumentResultErrorFormat, count));
            });
        }

        [Fact]
        public void Drag_NumberOfItemsToDragVersion_FromIndexedExtensions_GivenNullSource_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => IndexedExtensions.Drag<int>(null, 5));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-2)]
        public void Drag_NumberOfItemsToDragVersion_FromIndexedExtensions_ThrowsArgumentException(int numberOfItemsToDrag)
        {
            Assert.Throws<ArgumentException>(() => IndexedExtensions.Drag<int>(Enumerable.Empty<int>().ToArray(), numberOfItemsToDrag));
        }

        [Fact]
        public void Drag_NumberOfItemsToDragVersion_FromIndexedExtensions_ReturnsCorrectValues()
        {
            this.RunDragTestCasesWithNumberOfItemsToDrag((count, numberOfItemsToDrag) =>
            {
                var source = Enumerable.Range(0, count).ToArray();
                var sut = IndexedExtensions.Drag(source, numberOfItemsToDrag);

                LazyAssert.True(
                    CheckSut(sut, count, numberOfItemsToDrag),
                    () => string.Format(dragWithDragCountArgumentResultErrorFormat, count, numberOfItemsToDrag));
            });
        }
    }
}
