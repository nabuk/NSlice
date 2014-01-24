using NSlice;
using NSliceTests.Helpers;
using NSliceTests.TestData;
using NSliceTests.Tests.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Extensions;

namespace NSliceTests.Tests.EnumerableTests
{
    public class EnumerableSliceDeleteTests : BaseSliceCaseTests
    {
        private const string sliceDeleteResultErrorFormat = "For {0}.SliceDelete({1}, {2}, {3}) got {4}, expected {5}";
        private const string sliceDeleteDisposeOnceErrorFormat = "For [collection of length = {0}].SliceDelete({1}, {2}, {3}) Dispose has been called {4} time(s).";
        private const string sliceDeleteExceptionsAreNotHandledErrorFormat = "For [collection of length = {0}].SliceDelete({1}, {2}, {3}) exception has been handled but it shouldn't be.";
        private const string sliceDeleteDoesntCallResetErrorFormat = "For [collection of length = {0}].SliceDelete({1}, {2}, {3}) Reset has been called {4} time(s).";

        [Fact]
        public void SliceDelete_FromEnumerableExtensions_ReturnsCorrectValues()
        {
            this.RunSliceTestCases((from, to, step, length) =>
            {
                var source = Enumerable.Range(0, length);
                var sut = EnumerableExtensions.SliceDelete(source, from, to, step).ToArray();
                var expected = SliceDeleteExpectedResultCalculator.Calculate(from, to, step, length);

                LazyAssert.True(
                    expected.SequenceEqual(sut),
                    () => ErrorFormatter.Format(sliceDeleteResultErrorFormat, source, from, to, step, expected, sut));
            });
        }

        [Fact]
        public void SliceDelete_FromEnumerableExtensions_CallsDisposeOnce()
        {
            this.RunSliceTestCases((from, to, step, length) =>
            {
                var sut = new EnumerableMock<int>(Enumerable.Range(0, length));
                EnumerableExtensions.SliceDelete(sut, from, to, step).Sum();

                if (sut.Enumerators.Count > 0)
                {
                    var disposeCallCount = sut.Enumerators.Single().DisposeCallCount;

                    LazyAssert.True(
                        disposeCallCount == 1,
                        () => ErrorFormatter.Format(sliceDeleteDisposeOnceErrorFormat, from, to, step, length, disposeCallCount));
                }
            });
        }

        [Fact]
        public void SliceDelete_FromEnumerableExtensions_CallsDisposeWhenExceptionWasThrownFromMoveNext()
        {
            this.RunSliceTestCases((from, to, step, length) =>
            {
                var sut = new EnumerableMock<int>(Enumerable.Range(0, length));
                sut.EnumeratorCreated += e => { e.MoveNext = () => { throw new InvalidOperationException(); }; };

                try
                {
                    EnumerableExtensions.SliceDelete(sut, from, to, step).Sum();
                }
                catch (InvalidOperationException) { }

                if (sut.Enumerators.Count > 0)
                {
                    var disposeCallCount = sut.Enumerators.Single().DisposeCallCount;

                    LazyAssert.True(
                        disposeCallCount == 1,
                        () => ErrorFormatter.Format(sliceDeleteDisposeOnceErrorFormat, from, to, step, length, disposeCallCount));
                }
            });
        }

        [Fact]
        public void SliceDelete_FromEnumerableExtensions_CallsDisposeWhenExceptionWasThrownFromCurrent()
        {
            this.RunSliceTestCases((from, to, step, length) =>
            {
                var sut = new EnumerableMock<int>(Enumerable.Range(0, length));
                sut.EnumeratorCreated += e => { e.Current = () => { throw new InvalidOperationException(); }; };

                try
                {
                    EnumerableExtensions.SliceDelete(sut, from, to, step).Sum();
                }
                catch (InvalidOperationException) { }

                if (sut.Enumerators.Count > 0)
                {
                    var disposeCallCount = sut.Enumerators.Single().DisposeCallCount;

                    LazyAssert.True(
                        disposeCallCount == 1,
                        () => ErrorFormatter.Format(sliceDeleteDisposeOnceErrorFormat, from, to, step, length, disposeCallCount));
                }
            });
        }

        [Fact]
        public void SliceDelete_FromEnumerableExtensions_DoesntHandleExceptions()
        {
            this.RunSliceTestCases((from, to, step, length) =>
            {
                bool expected = false;
                var collection = new EnumerableMock<int>(Enumerable.Range(0, length));
                collection.EnumeratorCreated += e => { e.Current = () => { expected = true; throw new InvalidOperationException(); }; };
                bool sut = false;
                try
                {
                    EnumerableExtensions.SliceDelete(collection, from, to, step).Sum();
                }
                catch (InvalidOperationException)
                {
                    sut = true;
                }

                LazyAssert.True(sut == expected, () => ErrorFormatter.Format(sliceDeleteExceptionsAreNotHandledErrorFormat, from, to, step, length));
            });
        }

        [Fact]
        public void SliceDelete_FromEnumerableExtensions_DoesNotCallReset()
        {
            this.RunSliceTestCases((from, to, step, length) =>
            {
                var sut = new EnumerableMock<int>(Enumerable.Range(0, length));
                EnumerableExtensions.SliceDelete(sut, from, to, step).Sum();

                if (sut.Enumerators.Count > 0)
                {
                    var resetCallCount = sut.Enumerators.Single().ResetCallCount;

                    LazyAssert.True(
                        resetCallCount == 0,
                        () => ErrorFormatter.Format(sliceDeleteDoesntCallResetErrorFormat, from, to, step, length, resetCallCount));
                }
            });
        }

        [Fact]
        public void SliceDelete_FromEnumerableExtensions_GivenStepZero_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => EnumerableExtensions.SliceDelete(Enumerable.Empty<int>(), step: 0));
        }

        [Fact]
        public void SliceDelete_FromEnumerableExtensions_GivenNullSource_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => EnumerableExtensions.SliceDelete<int>(null));
        }

        [Fact]
        public void Slice_FromEnumerableExtensions_GivenListAsSource_ReturnsCorrectValues()
        {
            this.RunSliceTestCases((from, to, step, length) =>
            {
                var source = Enumerable.Range(0, length).ToArray();
                var sut = EnumerableExtensions.SliceDelete(source, from, to, step).ToArray();
                var expected = SliceDeleteExpectedResultCalculator.Calculate(from, to, step, length);

                LazyAssert.True(
                    expected.SequenceEqual(sut),
                    () => ErrorFormatter.Format(sliceDeleteResultErrorFormat, source, from, to, step, expected, sut));
            });
        }

        [Fact]
        public void Slice_FromEnumerableExtensions_GivenCollectionAsSource_ReturnsCorrectValues()
        {
            this.RunSliceTestCases((from, to, step, length) =>
            {
                var source = new Queue<int>(Enumerable.Range(0, length));
                var sut = EnumerableExtensions.SliceDelete(source, from, to, step).ToArray();
                var expected = SliceDeleteExpectedResultCalculator.Calculate(from, to, step, length);

                LazyAssert.True(
                    expected.SequenceEqual(sut),
                    () => ErrorFormatter.Format(sliceDeleteResultErrorFormat, source, from, to, step, expected, sut));
            });
        }
    }
}
