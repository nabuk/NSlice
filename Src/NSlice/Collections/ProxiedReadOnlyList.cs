using NSlice.Indexers;
using System;
using System.Collections;
using System.Collections.Generic;

namespace NSlice.Collections
{
    class ProxiedReadOnlyList<T> : IList<T>
    {
        private readonly IItemIndexer<T> _itemIndexer;
        private readonly int _count;

        public ProxiedReadOnlyList(IItemIndexer<T> itemIndexer)
        {
            this._itemIndexer = itemIndexer;
            this._count = itemIndexer.Count;
        }

        #region IEnumerable<T>
        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < this._count; ++i)
                yield return this._itemIndexer.GetItemAt(i);
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
                if (index < 0 || index >= this._count)
                    throw new IndexOutOfRangeException();
                return this._itemIndexer.GetItemAt(index);
            }
            set { throw new NotSupportedException(); }
        }

        public int Count { get { return this._count; } }


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
            for (var index = 0; index < this._count; ++index)
                array[index + arrayIndex] = this._itemIndexer.GetItemAt(index);
        }

        public int IndexOf(T item)
        {
            if ((object)item == null)
            {
                for (var index = 0; index < this._count; ++index)
                    if ((object)this._itemIndexer.GetItemAt(index) == null)
                        return index;
                return -1;
            }

            var equalityComparer = EqualityComparer<T>.Default;
            for (var index = 0; index < this._count; ++index)
                if (equalityComparer.Equals(this._itemIndexer.GetItemAt(index), item))
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
