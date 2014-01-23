using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSliceTests.Helpers
{
    public class EnumeratorMock<T> : IEnumerator<T>
    {
        private readonly IEnumerator<T> source;

        public EnumeratorMock(IEnumerator<T> source)
        {
            this.source = source;
        }

        public int DisposeCallCount { get; private set; }

        public int ResetCallCount { get; private set; }

        #region IEnumerator
        public T Current
        {
            get { return this.source.Current; }
        }

        public void Dispose()
        {
            ++this.DisposeCallCount;

            this.source.Dispose();
        }

        object System.Collections.IEnumerator.Current
        {
            get { return this.Current; }
        }

        public bool MoveNext()
        {
            return this.source.MoveNext();
        }

        public void Reset()
        {
            ++this.ResetCallCount;

            this.source.Reset();
        }
        #endregion
    }
}
