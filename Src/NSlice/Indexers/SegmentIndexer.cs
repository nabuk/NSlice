using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSlice.Indexers
{
    class SegmentIndexer<T> : IItemIndexer<T>
    {
        private readonly IList<T> _source;
        private readonly int _from;
        private readonly int _count;

        public SegmentIndexer(IList<T> source, int from, int count)
        {
            this._source = source;
            this._from = from;
            this._count = count;
        }

        public T GetItemAt(int index)
        {
            return _source[this._from + index];
        }

        public int Count
        {
            get { return this._count; }
        }
    }
}
