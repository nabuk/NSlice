using NSlice.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSlice.Indexers
{
    class SliceItemIndexer<T> : IItemIndexer<T>
    {
        private readonly IList<T> _source;
        private readonly int _from;
        private readonly int _step;
        private readonly int _count;

        public SliceItemIndexer(IList<T> source, int? from, int? to, int step)
        {
            this._source = source;

            var properties = SlicePropertiesCalculator.Calculate(from, to, step, source.Count);
            this._from = properties.from;
            this._step = properties.step;
            this._count = properties.count;
        }

        public T GetItemAt(int index)
        {
            return _source[this._from + index * this._step];
        }

        public int Count
        {
            get { return this._count; }
        }
    }
}
