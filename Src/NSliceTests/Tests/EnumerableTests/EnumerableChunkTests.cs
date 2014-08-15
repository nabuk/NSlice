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
    public class EnumerableChunkTests : BaseChunkCaseTests
    {
        private const string chunkResultErrorFormat = "For Range(0, {0}).Chunk({1}) got unexpected result.";
        private const string chunkDisposeOnceErrorFormat = "For [collection of length = {0}].Chunk({1}) Dispose has been called {2} time(s).";
        private const string chunkExceptionsAreNotHandledErrorFormat = "For [collection of length = {0}].Chunk({1}) exception has been handled but it shouldn't be.";
        private const string chunkDoesntCallResetErrorFormat = "For [collection of length = {0}].Chunk({1}) Reset has been called {2} time(s).";

        [Fact]
        public void Chunk_FromEnumerableExtensions_GivenNullSource_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => EnumerableExtensions.Chunk<int>(null, 5));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-2)]
        public void Chunk_FromEnumerableExtensions_ThrowsArgumentException(int chunkSize)
        {
            Assert.Throws<ArgumentNullException>(() => EnumerableExtensions.Chunk<int>(Enumerable.Empty<int>(), chunkSize));
        }

        [Fact]
        public void Chunk_FromEnumerableExtensions_ReturnsCorrectValues()
        {
            this.RunChunkTestCases((count, chunkSize) =>
            {
                var source = Enumerable.Range(0, count);
                var sut = EnumerableExtensions.Chunk(source, chunkSize);

                LazyAssert.True(
                    CheckSut(sut, count, chunkSize),
                    () => string.Format(chunkResultErrorFormat, count, chunkSize));
            });
        }

        [Fact]
        public void Chunk_FromEnumerableExtensions_CallsDisposeOnce()
        {
            this.RunChunkTestCases((count, chunkSize) =>
            {
                var sut = new EnumerableMock<int>(Enumerable.Range(0, count));
                EnumerableExtensions.Chunk(sut, chunkSize).Sum(x => x[0]);

                if (sut.Enumerators.Count > 0)
                {
                    var disposeCallCount = sut.Enumerators.Single().DisposeCallCount;

                    LazyAssert.True(
                        disposeCallCount == 1,
                        () => string.Format(chunkDisposeOnceErrorFormat, count, chunkSize, disposeCallCount));
                }
            });
        }

        [Fact]
        public void Chunk_FromEnumerableExtensions_CallsDisposeWhenExceptionWasThrownFromMoveNext()
        {
            this.RunChunkTestCases((count, chunkSize) =>
            {
                var sut = new EnumerableMock<int>(Enumerable.Range(0, count));
                sut.EnumeratorCreated += e => { e.MoveNext = () => { throw new InvalidOperationException(); }; };

                try
                {
                    EnumerableExtensions.Chunk(sut, chunkSize).Sum(x => x[0]);
                }
                catch (InvalidOperationException) { }

                if (sut.Enumerators.Count > 0)
                {
                    var disposeCallCount = sut.Enumerators.Single().DisposeCallCount;

                    LazyAssert.True(
                        disposeCallCount == 1,
                        () => string.Format(chunkDisposeOnceErrorFormat, count, chunkSize, disposeCallCount));
                }
            });
        }

        [Fact]
        public void Chunk_FromEnumerableExtensions_CallsDisposeWhenExceptionWasThrownFromCurrent()
        {
            this.RunChunkTestCases((count, chunkSize) =>
            {
                var sut = new EnumerableMock<int>(Enumerable.Range(0, count));
                sut.EnumeratorCreated += e => { e.Current = () => { throw new InvalidOperationException(); }; };

                try
                {
                    EnumerableExtensions.Chunk(sut, chunkSize).Sum(x => x[0]);
                }
                catch (InvalidOperationException) { }

                if (sut.Enumerators.Count > 0)
                {
                    var disposeCallCount = sut.Enumerators.Single().DisposeCallCount;

                    LazyAssert.True(
                        disposeCallCount == 1,
                        () => string.Format(chunkDisposeOnceErrorFormat, count, chunkSize, disposeCallCount));
                }
            });
        }

        [Fact]
        public void Chunk_FromEnumerableExtensions_DoesntHandleExceptions()
        {
            this.RunChunkTestCases((count, chunkSize) =>
            {
                bool expected = false;
                var collection = new EnumerableMock<int>(Enumerable.Range(0, count));
                collection.EnumeratorCreated += e => { e.Current = () => { expected = true; throw new InvalidOperationException(); }; };
                bool sut = false;
                try
                {
                    EnumerableExtensions.Chunk(collection, chunkSize).Sum(x => x[0]);
                }
                catch (InvalidOperationException)
                {
                    sut = true;
                }

                LazyAssert.True(sut == expected, () => string.Format(chunkExceptionsAreNotHandledErrorFormat, count, chunkSize));
            });
        }

        [Fact]
        public void Chunk_FromEnumerableExtensions_DoesNotCallReset()
        {
            this.RunChunkTestCases((count, chunkSize) =>
            {
                var sut = new EnumerableMock<int>(Enumerable.Range(0, count));
                EnumerableExtensions.Chunk(sut, chunkSize).Sum(x => x[0]);

                if (sut.Enumerators.Count > 0)
                {
                    var resetCallCount = sut.Enumerators.Single().ResetCallCount;

                    LazyAssert.True(
                        resetCallCount == 0,
                        () => string.Format(chunkDoesntCallResetErrorFormat, count, chunkSize, resetCallCount));
                }
            });
        }

        [Fact]
        public void Chunk_FromEnumerableExtensions_GivenListAsSource_ReturnsCorrectValues()
        {
            this.RunChunkTestCases((count, chunkSize) =>
            {
                var source = Enumerable.Range(0, count).ToArray();
                var sut = EnumerableExtensions.Chunk(source, chunkSize);

                LazyAssert.True(
                    CheckSut(sut, count, chunkSize),
                    () => string.Format(chunkResultErrorFormat, count, chunkSize));
            });
        }

        [Fact]
        public void Chunk_FromEnumerableExtensions_GivenCollectionAsSource_ReturnsCorrectValues()
        {
            this.RunChunkTestCases((count, chunkSize) =>
            {
                var source = new Queue<int>(Enumerable.Range(0, count));
                var sut = EnumerableExtensions.Chunk(source, chunkSize);

                LazyAssert.True(
                    CheckSut(sut, count, chunkSize),
                    () => string.Format(chunkResultErrorFormat, count, chunkSize));
            });
        }
    }
}
