using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSliceTests.Helpers
{
    public class EnumeratorMock<T> : IEnumerator<T>
    {
        public readonly IEnumerator<T> enumerator;

        public EnumeratorMock(IEnumerator<T> enumerator)
        {
            this.enumerator = enumerator;

            this.MoveNext = this.enumerator.MoveNext;
            this.Current = () => this.enumerator.Current;
        }

        public Func<bool> MoveNext { get; set; }

        public Func<T> Current { get; set; }

        public int DisposeCallCount { get; private set; }

        public int ResetCallCount { get; private set; }

        #region IEnumerator
        T IEnumerator<T>.Current
        {
            get { return this.Current(); }
        }

        void IDisposable.Dispose()
        {
            ++this.DisposeCallCount;

            this.enumerator.Dispose();
        }

        bool IEnumerator.MoveNext()
        {
            return this.MoveNext();
        }

        void IEnumerator.Reset()
        {
            ++this.ResetCallCount;

            this.enumerator.Reset();
        }

        object IEnumerator.Current
        {
            get { return this.Current(); }
        }
        #endregion
    }
}
