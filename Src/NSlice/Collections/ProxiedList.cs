using System;
using System.Collections;
using System.Collections.Generic;
using NSlice.Indexers;

namespace NSlice.Collections
{
    class ProxiedList<T> : IList<T>
    {
        private readonly IList<T> _source;
        private readonly int _from;
        private readonly int _step;
        private readonly int _count;

        public ProxiedList(IList<T> source, StepIndexer indexer)
        {
            _source = source;
            _from = indexer.from;
            _step = indexer.step;
            _count = indexer.count;
        }

        #region IEnumerable<T>
        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < _count; ++i)
                yield return _source[_from + (i * _step)];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region IList<T>
        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= _count)
                    throw new IndexOutOfRangeException();
                return _source[_from + (index * _step)];
            }
            set { throw new NotSupportedException(); }
        }

        public int Count { get { return _count; } }

        
        public bool IsReadOnly
        {
            get { return true; }
        }

        public bool Contains(T item)
        {
            return IndexOf(item) >= 0;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentNullException("array");
            for (var index = 0; index < _count; ++index)
                array[index + arrayIndex] = _source[_from + (index * _step)];
        }

        public int IndexOf(T item)
        {
            if ((object)item == null)
            {
                for (var index = 0; index < _count; ++index)
                    if ((object)_source[_from + (index * _step)] == null)
                        return index;
                return -1;
            }

            var equalityComparer = EqualityComparer<T>.Default;
            for (var index = 0; index < _count; ++index)
                if (equalityComparer.Equals(_source[_from + (index * _step)], item))
                    return index;
            return -1;
        }
        #endregion

        #region IList<T> not supported
        public void Add(T item)
        {
            throw new NotSupportedException();
        }

        public void Clear()
        {
            throw new NotSupportedException();
        }

        public bool Remove(T item)
        {
            throw new NotSupportedException();
        }

        public void Insert(int index, T item)
        {
            throw new NotSupportedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }
        #endregion
    }
}
