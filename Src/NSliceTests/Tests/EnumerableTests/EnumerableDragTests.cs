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
        private const string dragNoArgumentDisposeOnceErrorFormat = "For [collection of length = {0}].Drag() Dispose has been called {1} time(s).";
        private const string dragNoArgumentExceptionsAreNotHandledErrorFormat = "For [collection of length = {0}].Drag() exception has been handled but it shouldn't be.";
        private const string dragNoArgumentDoesntCallResetErrorFormat = "For [collection of length = {0}].Drag() Reset has been called {1} time(s).";

        private const string dragWithDragCountArgumentDisposeOnceErrorFormat = "For [collection of length = {0}].Drag({1}) Dispose has been called {2} time(s).";
        private const string dragWithDragCountArgumentExceptionsAreNotHandledErrorFormat = "For [collection of length = {0}].Drag({1}) exception has been handled but it shouldn't be.";
        private const string dragWithDragCountArgumentDoesntCallResetErrorFormat = "For [collection of length = {0}].Drag({1}) Reset has been called {2} time(s).";

        [Fact]
        public void Drag_NoArgumentVersion_FromEnumerableExtensions_GivenNullSource_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => EnumerableExtensions.Drag<int>(null));
        }

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
        public void Drag_NoArgumentVersion_FromEnumerableExtensions_CallsDisposeWhenExceptionWasThrownFromCurrent()
        {
            this.RunNoArgumentDragTestCases((count, numberOfItemsToDrag) =>
            {
                var sut = new EnumerableMock<int>(Enumerable.Range(0, count));
                sut.EnumeratorCreated += e => { e.Current = () => { throw new InvalidOperationException(); }; };

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
        public void Drag_NoArgumentVersion_FromEnumerableExtensions_DoesntHandleExceptions()
        {
            this.RunNoArgumentDragTestCases((count, numberOfItemsToDrag) =>
            {
                bool expected = false;
                var collection = new EnumerableMock<int>(Enumerable.Range(0, count));
                collection.EnumeratorCreated += e => { e.Current = () => { expected = true; throw new InvalidOperationException(); }; };
                bool sut = false;
                try
                {
                    EnumerableExtensions.Drag(collection).Sum(x => x.First);
                }
                catch (InvalidOperationException)
                {
                    sut = true;
                }

                LazyAssert.True(sut == expected, () => string.Format(dragNoArgumentExceptionsAreNotHandledErrorFormat, count));
            });
        }

        [Fact]
        public void Drag_NoArgumentVersion_FromEnumerableExtensions_DoesNotCallReset()
        {
            this.RunNoArgumentDragTestCases((count, numberOfItemsToDrag) =>
            {
                var sut = new EnumerableMock<int>(Enumerable.Range(0, count));
                EnumerableExtensions.Drag(sut).Sum(x => x.First);

                if (sut.Enumerators.Count > 0)
                {
                    var resetCallCount = sut.Enumerators.Single().ResetCallCount;

                    LazyAssert.True(
                        resetCallCount == 0,
                        () => string.Format(dragNoArgumentDoesntCallResetErrorFormat, count, resetCallCount));
                }
            });
        }

        [Fact]
        public void Drag_NoArgumentVersion_FromEnumerableExtensions_GivenListAsSource_ReturnsCorrectValues()
        {
            this.RunNoArgumentDragTestCases((count, numberOfItemsToDrag) =>
            {
                var source = Enumerable.Range(0, count).ToArray();
                var sut = EnumerableExtensions.Drag(source);

                LazyAssert.True(
                    CheckSut(sut.Select(x => new[] { x.First, x.Second }), count, numberOfItemsToDrag),
                    () => string.Format(dragNoArgumentResultErrorFormat, count));
            });
        }

        [Fact]
        public void Drag_NoArgumentVersion_FromEnumerableExtensions_GivenCollectionAsSource_ReturnsCorrectValues()
        {
            this.RunNoArgumentDragTestCases((count, numberOfItemsToDrag) =>
            {
                var source = new Queue<int>(Enumerable.Range(0, count));
                var sut = EnumerableExtensions.Drag(source);

                LazyAssert.True(
                    CheckSut(sut.Select(x => new[] { x.First, x.Second }), count, numberOfItemsToDrag),
                    () => string.Format(dragNoArgumentResultErrorFormat, count));
            });
        }

        [Fact]
        public void Drag_NumberOfItemsToDragVersion_FromEnumerableExtensions_GivenNullSource_ThrowsArgumentNullException()
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

        [Fact]
        public void Drag_NumberOfItemsToDragVersion_FromEnumerableExtensions_ReturnsCorrectValues()
        {
            this.RunDragTestCasesWithNumberOfItemsToDrag((count, numberOfItemsToDrag) =>
            {
                var source = Enumerable.Range(0, count);
                var sut = EnumerableExtensions.Drag(source, numberOfItemsToDrag);

                LazyAssert.True(
                    CheckSut(sut, count, numberOfItemsToDrag),
                    () => string.Format(dragWithDragCountArgumentResultErrorFormat, count, numberOfItemsToDrag));
            });
        }

        [Fact]
        public void Drag_NumberOfItemsToDragVersion_FromEnumerableExtensions_CallsDisposeOnce()
        {
            this.RunDragTestCasesWithNumberOfItemsToDrag((count, numberOfItemsToDrag) =>
            {
                var sut = new EnumerableMock<int>(Enumerable.Range(0, count));
                EnumerableExtensions.Drag(sut, numberOfItemsToDrag).Sum(x => x[0]);

                if (sut.Enumerators.Count > 0)
                {
                    var disposeCallCount = sut.Enumerators.Single().DisposeCallCount;

                    LazyAssert.True(
                        disposeCallCount == 1,
                        () => string.Format(dragWithDragCountArgumentDisposeOnceErrorFormat, count, numberOfItemsToDrag, disposeCallCount));
                }
            });
        }

        [Fact]
        public void Drag_NumberOfItemsToDragVersion_FromEnumerableExtensions_CallsDisposeWhenExceptionWasThrownFromMoveNext()
        {
            this.RunDragTestCasesWithNumberOfItemsToDrag((count, numberOfItemsToDrag) =>
            {
                var sut = new EnumerableMock<int>(Enumerable.Range(0, count));
                sut.EnumeratorCreated += e => { e.MoveNext = () => { throw new InvalidOperationException(); }; };

                try
                {
                    EnumerableExtensions.Drag(sut, numberOfItemsToDrag).Sum(x => x[0]);
                }
                catch (InvalidOperationException) { }

                if (sut.Enumerators.Count > 0)
                {
                    var disposeCallCount = sut.Enumerators.Single().DisposeCallCount;

                    LazyAssert.True(
                        disposeCallCount == 1,
                        () => string.Format(dragWithDragCountArgumentDisposeOnceErrorFormat, count, numberOfItemsToDrag, disposeCallCount));
                }
            });
        }

        [Fact]
        public void Drag_NumberOfItemsToDragVersion_FromEnumerableExtensions_CallsDisposeWhenExceptionWasThrownFromCurrent()
        {
            this.RunDragTestCasesWithNumberOfItemsToDrag((count, numberOfItemsToDrag) =>
            {
                var sut = new EnumerableMock<int>(Enumerable.Range(0, count));
                sut.EnumeratorCreated += e => { e.Current = () => { throw new InvalidOperationException(); }; };

                try
                {
                    EnumerableExtensions.Drag(sut, numberOfItemsToDrag).Sum(x => x[0]);
                }
                catch (InvalidOperationException) { }

                if (sut.Enumerators.Count > 0)
                {
                    var disposeCallCount = sut.Enumerators.Single().DisposeCallCount;

                    LazyAssert.True(
                        disposeCallCount == 1,
                        () => string.Format(dragWithDragCountArgumentDisposeOnceErrorFormat, count, numberOfItemsToDrag, disposeCallCount));
                }
            });
        }

        [Fact]
        public void Drag_NumberOfItemsToDragVersion_FromEnumerableExtensions_DoesntHandleExceptions()
        {
            this.RunDragTestCasesWithNumberOfItemsToDrag((count, numberOfItemsToDrag) =>
            {
                bool expected = false;
                var collection = new EnumerableMock<int>(Enumerable.Range(0, count));
                collection.EnumeratorCreated += e => { e.Current = () => { expected = true; throw new InvalidOperationException(); }; };
                bool sut = false;
                try
                {
                    EnumerableExtensions.Drag(collection, numberOfItemsToDrag).Sum(x => x[0]);
                }
                catch (InvalidOperationException)
                {
                    sut = true;
                }

                LazyAssert.True(sut == expected, () => string.Format(dragWithDragCountArgumentExceptionsAreNotHandledErrorFormat, count, numberOfItemsToDrag));
            });
        }

        [Fact]
        public void Drag_NumberOfItemsToDragVersion_FromEnumerableExtensions_DoesNotCallReset()
        {
            this.RunDragTestCasesWithNumberOfItemsToDrag((count, numberOfItemsToDrag) =>
            {
                var sut = new EnumerableMock<int>(Enumerable.Range(0, count));
                EnumerableExtensions.Drag(sut, numberOfItemsToDrag).Sum(x => x[0]);

                if (sut.Enumerators.Count > 0)
                {
                    var resetCallCount = sut.Enumerators.Single().ResetCallCount;

                    LazyAssert.True(
                        resetCallCount == 0,
                        () => string.Format(dragWithDragCountArgumentDoesntCallResetErrorFormat, count, numberOfItemsToDrag, resetCallCount));
                }
            });
        }

        [Fact]
        public void Drag_NumberOfItemsToDragVersion_FromEnumerableExtensions_GivenListAsSource_ReturnsCorrectValues()
        {
            this.RunDragTestCasesWithNumberOfItemsToDrag((count, numberOfItemsToDrag) =>
            {
                var source = Enumerable.Range(0, count).ToArray();
                var sut = EnumerableExtensions.Drag(source, numberOfItemsToDrag);

                LazyAssert.True(
                    CheckSut(sut, count, numberOfItemsToDrag),
                    () => string.Format(dragWithDragCountArgumentResultErrorFormat, count, numberOfItemsToDrag));
            });
        }

        [Fact]
        public void Drag_NumberOfItemsToDragVersion_FromEnumerableExtensions_GivenCollectionAsSource_ReturnsCorrectValues()
        {
            this.RunDragTestCasesWithNumberOfItemsToDrag((count, numberOfItemsToDrag) =>
            {
                var source = new Queue<int>(Enumerable.Range(0, count));
                var sut = EnumerableExtensions.Drag(source, numberOfItemsToDrag);

                LazyAssert.True(
                    CheckSut(sut, count, numberOfItemsToDrag),
                    () => string.Format(dragWithDragCountArgumentResultErrorFormat, count, numberOfItemsToDrag));
            });
        }
    }
}
