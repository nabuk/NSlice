using NSlice.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSlice.Indexers
{
    class DragIndexer<T> : IItemIndexer<IList<T>>
    {
        private readonly IList<T> _source;
        private readonly int _count;
        private readonly int _numberOfItemsToDrag;

        public DragIndexer(IList<T> source, int numberOfItemsToDrag)
        {
            this._source = source;
            this._numberOfItemsToDrag = numberOfItemsToDrag;
            this._count = Math.Max(source.Count - numberOfItemsToDrag + 1, 0);
        }

        public IList<T> GetItemAt(int index)
        {
            return new ProxiedReadOnlyList<T>(new SegmentIndexer<T>(_source, index, _numberOfItemsToDrag));
        }

        public int Count
        {
            get { return this._count; }
        }
    }
}
