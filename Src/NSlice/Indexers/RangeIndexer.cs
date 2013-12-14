using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSlice.Indexers
{
    class RangeIndexer : IItemIndexer<int>
    {
        private readonly int _start;
        private readonly int _count;

        public RangeIndexer(int start, int count)
        {
            this._start = start;
            this._count = count;
        }

        public int GetItemAt(int index)
        {
            return this._start + index;
        }

        public int Count
        {
            get { return this._count; }
        }
    }
}
