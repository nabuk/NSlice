using NSlice;
using NSliceTests.Helpers;
using NSliceTests.Tests.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Extensions;

namespace NSliceTests.Tests.EnumerableTests
{
    public class EnumerableDragTests : BaseDragCaseTests
    {
        private const string dragNoArgumentResultErrorFormat = "For Range(0, {0}).Drag() got unexpected result.";
        private const string dragNoArgumentDisposeOnceErrorFormat = "For [collection of length = {0}].Drag() Dispose has been called {1} time(s).";

        [Fact]
        public void Drag_NoArgumentVersion_FromEnumerableExtensions_ReturnsCorrectValues()
        {
            this.RunNoArgumentDragTestCases((count, numberOfItemsToDrag) =>
            {
                var source = Enumerable.Range(0, count);
                var sut = EnumerableExtensions.Drag(source);

                LazyAssert.True(
                    CheckSut(sut.Select(x => new[] { x.First, x.Second }), count, numberOfItemsToDrag),
                    () => string.Format(dragNoArgumentResultErrorFormat, count));
            });
        }

        [Fact]
        public void Drag_NoArgumentVersion_FromEnumerableExtensions_CallsDisposeOnce()
        {
            this.RunNoArgumentDragTestCases((count, numberOfItemsToDrag) =>
            {
                var sut = new EnumerableMock<int>(Enumerable.Range(0, count));
                EnumerableExtensions.Drag(sut).Sum(x => x.First);

                if (sut.Enumerators.Count > 0)
                {
                    var disposeCallCount = sut.Enumerators.Single().DisposeCallCount;

                    LazyAssert.True(
                        disposeCallCount == 1,
                        () => string.Format(dragNoArgumentDisposeOnceErrorFormat, count, disposeCallCount));
                }
            });
        }

        [Fact]
        public void Drag_NoArgumentVersion_FromEnumerableExtensions_CallsDisposeWhenExceptionWasThrownFromMoveNext()
        {
            this.RunNoArgumentDragTestCases((count, numberOfItemsToDrag) =>
            {
                var sut = new EnumerableMock<int>(Enumerable.Range(0, count));
                sut.EnumeratorCreated += e => { e.MoveNext = () => { throw new InvalidOperationException(); }; };

                try
                {
                    EnumerableExtensions.Drag(sut).Sum(x => x.First);
                }
                catch (InvalidOperationException) { }

                if (sut.Enumerators.Count > 0)
                {
                    var disposeCallCount = sut.Enumerators.Single().DisposeCallCount;

                    LazyAssert.True(
                        disposeCallCount == 1,
                        () => string.Format(dragNoArgumentDisposeOnceErrorFormat, count, disposeCallCount));
                }
            });

        }

        [Fact]
        public void Drag_NoArgumentVersion_FromEnumerableExtensions_CallsDisposeWhenExceptionWasThrownFromCurrent() { }

        [Fact]
        public void Drag_NoArgumentVersion_FromEnumerableExtensions_DoesntHandleExceptions() { }

        [Fact]
        public void Drag_NoArgumentVersion_FromEnumerableExtensions_DoesNotCallReset() { }

        [Fact]
        public void Drag_NoArgumentVersion_FromEnumerableExtensions_GivenListAsSource_ReturnsCorrectValues() { }

        [Fact]
        public void Drag_NoArgumentVersion_FromEnumerableExtensions_GivenCollectionAsSource_ReturnsCorrectValues() { }



        [Fact]
        public void Drag_NumberOfItemsToDragVersion_FromEnumerableExtensions_ReturnsCorrectValues()
        {
            this.RunDragTestCasesWithNumberOfItemsToDrag((count, numberOfItemsToDrag) =>
            {
                var source = Enumerable.Range(0, count);
                var sut = EnumerableExtensions.Drag(source, numberOfItemsToDrag);

                LazyAssert.True(
                    CheckSut(sut, count, numberOfItemsToDrag),
                    () => string.Format("For Range(0, {0}).Drag({1}) got unexpected result.", count, numberOfItemsToDrag));
            });

        }

        [Fact]
        public void Drag_NumberOfItemsToDragVersion_FromEnumerableExtensions_CallsDisposeOnce() { }

        [Fact]
        public void Drag_NumberOfItemsToDragVersion_FromEnumerableExtensions_CallsDisposeWhenExceptionWasThrownFromMoveNext() { }

        [Fact]
        public void Drag_NumberOfItemsToDragVersion_FromEnumerableExtensions_CallsDisposeWhenExceptionWasThrownFromCurrent() { }

        [Fact]
        public void Drag_NumberOfItemsToDragVersion_FromEnumerableExtensions_DoesntHandleExceptions() { }

        [Fact]
        public void Drag_NumberOfItemsToDragVersion_FromEnumerableExtensions_DoesNotCallReset() { }

        [Fact]
        public void Drag_NumberOfItemsToDragVersion_FromEnumerableExtensions_GivenListAsSource_ReturnsCorrectValues() { }

        [Fact]
        public void Drag_NumberOfItemsToDragVersion_FromEnumerableExtensions_GivenCollectionAsSource_ReturnsCorrectValues() { }



        [Fact]
        public void Drag_NoArgumentVersion_FromEnumerableExtensions_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => EnumerableExtensions.Drag<int>(null));
        }

        [Fact]
        public void Drag_NumberOfItemsToDragVersion_FromEnumerableExtensions_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => EnumerableExtensions.Drag<int>(null, 5));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-2)]
        public void Drag_NumberOfItemsToDragVersion_FromEnumerableExtensions_ThrowsArgumentException(int numberOfItemsToDrag)
        {
            Assert.Throws<ArgumentNullException>(() => EnumerableExtensions.Drag<int>(Enumerable.Empty<int>(), numberOfItemsToDrag));
        }
    }
}
