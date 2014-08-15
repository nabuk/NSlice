using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSlice.Indexers
{
    class DoubleDragIndexer<T> : IItemIndexer<DoubleItem<T>>
    {
        private readonly IList<T> _source;
        private readonly int _count;

        public DoubleDragIndexer(IList<T> source)
        {
            this._source = source;
            this._count = Math.Max(source.Count - 1, 0);
        }

        public DoubleItem<T> GetItemAt(int index)
        {
            return new DoubleItem<T>(_source[index], _source[index + 1]);
        }

        public int Count
        {
            get { return this._count; }
        }
    }
}
